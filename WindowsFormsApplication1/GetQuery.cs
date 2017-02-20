using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCC
{
    public static class GetQuery
    {

        public static string AgentSummary(string sAgentName)
        {

            string sStart = string.Empty;
            string sEndDate = string.Empty;
            if (GM.GetDateTime().Day < 25)
            {
                if (GM.GetDateTime().Month == 1)
                    sStart = (GM.GetDateTime().Year - 1) + "-12-25";
                else
                    sStart = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month - 1) + "-25";

                sEndDate = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-24";
            }
            else
            {
                sStart = GM.GetDateTime().Year + "-" + GM.GetDateTime().Month + "-25";

                if (GM.GetDateTime().Month == 12)
                    sEndDate = (GM.GetDateTime().Year + 1) + "-01-24";
                else
                    sEndDate = GM.GetDateTime().Year + "-" + (GM.GetDateTime().Month + 1) + "-24";
            }

            string sSQLText = "DECLARE @FromDate DATE = (Select convert(DATE,'" + sStart + "')) DECLARE @ToDate DATE = (Select convert(DATE,'" + sEndDate + "')) ";                           

            sSQLText +=     @" ;WITH CTE AS ( SELECT   D1.PROJECT_NAME,D2.DATECALLED ,D2.AGENTNAME ,D3.Fullname AgentFullName ,NO_OF_CONTACTS_VALIDATED AchievedContacts ,LastSeen LastSeen ,POINTS POINTS ,ISNULL(SELF_TARGET, 0) SELF_TARGET,
                            ROW_NUMBER() OVER (ORDER BY DateCalled DESC ) ROW_NUM 
                            FROM DASHBOARD D1 
                            INNER JOIN DAILY_AGENT_PERFORMANCE_V1 D2 ON D2.DASHBOARD_ID = D1.ID AND D2.AGENTNAME = '" + sAgentName + "' INNER JOIN Timesheet..Users D3 ON D3.UserName = D2.AGENTNAME AND D2.FLAG = '" + GV.sAccessTo + "'";

            sSQLText +=     @"WHERE D2.DATECALLED BETWEEN @FromDate AND @ToDate
                            and D3.ACTIVE = 'Y' group by D1.PROJECT_NAME , D2.DATECALLED , D2.AGENTNAME , D3.Fullname , NO_OF_CONTACTS_VALIDATED , LastSeen ,POINTS , SELF_TARGET), 
                            CTECONSOLIDATE AS ( SELECT   AgentFullName ,
                            AGENTNAME , SUM(Isnull(POINTS,0)) MTDPOINTS , SUM(Isnull(AchievedContacts,0)) AchievedContacts, SUM(ROW_NUM) TOT_CNT 
                            FROM CTE GROUP BY AgentFullName , AGENTNAME ),
                            CTETARGETMISSED AS (SELECT  AGENTNAME,COUNT(*) TM_CNT
                            FROM CTE WHERE   LEN(ISNULL(SELF_TARGET,0)) > 0 AND  ISNULL(SELF_TARGET, 0) > ISNULL(CTE.AchievedContacts, 0) GROUP BY AGENTNAME),
                            CTETARGETAchieved AS (SELECT  AGENTNAME,COUNT(*) TA_CNT
                            FROM CTE WHERE   LEN(ISNULL(SELF_TARGET,0)) > 0 AND  ISNULL(CTE.AchievedContacts, 0) >= ISNULL(SELF_TARGET, 0)   GROUP BY AGENTNAME)
                            SELECT  C1.AgentFullName ,C1.AchievedContacts ,C1.MTDPOINTS ,ISNULL(TM.TM_CNT,0) TM_CNT,ISNULL(TA.TA_CNT,0) TA_CNT
                            FROM CTECONSOLIDATE C1 
                            left Join CTETARGETAchieved TA on TA.AGENTNAME = C1.AGENTNAME
                            left JOIN CTETARGETMISSED TM ON TM.AGENTNAME = C1.AGENTNAME";
                        return sSQLText;
        }

    }
}
