using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CustomUIControls;

namespace GCC
{
    public partial class frmProjectUpdates : DevComponents.DotNetBar.Office2007Form
    {
        public frmProjectUpdates()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);/////Gets the icon for current window////
        }

        DataTable _dtProjectUpdates = new DataTable();
        public DataTable dtProjectUpdates
        {
            get { return _dtProjectUpdates; }
            set { _dtProjectUpdates = value; }
        }

        List<DevComponents.AdvTree.Node> lstNodes = new List<DevComponents.AdvTree.Node>();
        TaskbarNotifier tNotifier = new TaskbarNotifier();

        private void frmProjectUpdates_Load(object sender, EventArgs e)
        {
           // wBrowser.DocumentText = dtProjectUpdates.Rows[0]["DESCRIPTION_HTML"].ToString();
            foreach(DataRow drProjectUpdates in dtProjectUpdates.Rows)
            {
                DevComponents.AdvTree.Node nNode = new DevComponents.AdvTree.Node();
                nNode.Name = drProjectUpdates["ID"].ToString();
                nNode.NodeClick += new EventHandler(Node_NodeClick);
                DevComponents.AdvTree.Cell cellSubject = new DevComponents.AdvTree.Cell();
                DevComponents.AdvTree.Cell cellDate = new DevComponents.AdvTree.Cell();                
                cellSubject.Text = drProjectUpdates["SUBJECT"].ToString();
                cellDate.Text = GM.TimeAgo(Convert.ToDateTime(drProjectUpdates["CREATED_DATE"]));
                nNode.Text = GM.ProperCase(drProjectUpdates["CREATED_BY"].ToString());
                nNode.Cells.Add(cellSubject);
                nNode.Cells.Add(cellDate);
                if (drProjectUpdates["ACTIVE"].ToString().ToUpper() == "N")
                { }
                else if (drProjectUpdates["ACTIVE"].ToString().ToUpper() == "Y")
                {
                    if (drProjectUpdates["REQUIRE_ACKNOWLEDGEMENT"].ToString() == "Y" && drProjectUpdates["USER_ACKNOWLEDGEMENT"].ToString().ToUpper().Contains("|" + GV.sEmployeeNo.ToUpper() + "~"))
                    {

                    }
                    else
                    {
                        if (drProjectUpdates["REQUIRE_ACKNOWLEDGEMENT"].ToString() != "Y" && drProjectUpdates["USER_READ"].ToString().ToUpper().Contains("|" + GV.sEmployeeNo.ToUpper() + "~"))
                        { }
                        else
                            nNode.Style = BoldStyle;//Unread
                    }                    
                }

                lstNodes.Add(nNode);

                switch (drProjectUpdates["INSTRUCTION_TYPE"].ToString().ToUpper())
                {
                    case "PR_INFO":
                        nodeProjectInfo.Nodes.Add(nNode);
                        break;
                    case "PR_UPDATE":
                        nodeProjectUpdate.Nodes.Add(nNode);
                        break;
                    case "PR_NOTIFICATION":
                        nodeProjectNotification.Nodes.Add(nNode);
                        break;
                    case "CM_RELEASE":
                        nodeCMRelease.Nodes.Add(nNode);
                        break;
                    case "CM_BUGFIX":
                        nodeCMBugFixes.Nodes.Add(nNode);
                        break;
                    case "CM_NOTIFICATION":
                        nodeCMNotification.Nodes.Add(nNode);
                        break;
                }
            }
        }

        private void Node_NodeClick(object sender, EventArgs e)
        {
            DevComponents.AdvTree.Node nNode = sender as DevComponents.AdvTree.Node;
            string sID = nNode.Name;
            DataRow[] drrProjectUpdates = dtProjectUpdates.Select("ID = '" + sID + "'");
            if (drrProjectUpdates.Length > 0)
            {
                wBrowser.DocumentText = drrProjectUpdates[0]["DESCRIPTION_HTML"].ToString();
                lblFrom.Text = "<b>" + drrProjectUpdates[0]["CREATED_BY"] + "</b> : " + drrProjectUpdates[0]["SUBJECT"];                
                if (drrProjectUpdates[0]["REQUIRE_ACKNOWLEDGEMENT"].ToString().ToUpper() == "Y" && !drrProjectUpdates[0]["USER_ACKNOWLEDGEMENT"].ToString().ToUpper().Contains("|" + GV.sEmployeeNo + "~"))
                {
                    btnAknowledge.Visible = true;
                    btnAknowledge.Tag = sID;
                }
                else
                {
                    btnAknowledge.Visible = false;
                    btnAknowledge.Tag = string.Empty;
                    nNode.Style = elementStyle1;
                }


                tNotifier.CloseClickable = true;
                tNotifier.TitleClickable = true;
                tNotifier.ContentClickable = false;
                tNotifier.EnableSelectionRectangle = false;
                tNotifier.KeepVisibleOnMousOver = false;    // Added Rev 002
                tNotifier.ReShowOnMouseOver = false;         // Added Rev 002
                tNotifier.Show(drrProjectUpdates[0]["CREATED_BY"].ToString(), drrProjectUpdates[0]["Subject"].ToString(), 500, 3000, 500);

                if (!drrProjectUpdates[0]["USER_READ"].ToString().ToUpper().Contains("|" + GV.sEmployeeNo.ToUpper() + "~"))
                    GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE c_project_instructions set USER_READ = '" + drrProjectUpdates[0]["USER_READ"].ToString().Replace("'", "''").ToUpper() + "|" + GV.sEmployeeNo.ToUpper() + "~" + GM.GetDateTime().ToString("dd/MM/yyyy hh:mm:ss tt") + "|' WHERE ID = '" + sID + "'");
            }
        }

        private void btnAknowledge_Click(object sender, EventArgs e)
        {
            DataRow[] drrProjectUpdates = dtProjectUpdates.Select("ID = '" + btnAknowledge.Tag.ToString() + "'");
            string sID = btnAknowledge.Tag.ToString();
            if (sID.Length > 0 && drrProjectUpdates.Length > 0)
            {                
                GV.MSSQL1.BAL_ExecuteNonReturnQuery("UPDATE c_project_instructions set USER_ACKNOWLEDGEMENT = '"+ drrProjectUpdates[0]["USER_ACKNOWLEDGEMENT"] + "|" + GV.sEmployeeNo.ToUpper() + "~" + GM.GetDateTime().ToString("dd/MM/yyyy hh:mm:ss tt") + "|' WHERE ID = '" + sID + "'");
                drrProjectUpdates[0]["USER_ACKNOWLEDGEMENT"] += "|" + GV.sEmployeeNo.ToUpper() + "~" + GM.GetDateTime().ToString("dd/MM/yyyy hh:mm:ss tt");
                btnAknowledge.Visible = false;
                foreach (DevComponents.AdvTree.Node nNode in lstNodes)
                {
                    if (nNode.Name == sID)
                    {
                        nNode.Style = elementStyle1;
                        break;
                    }
                }
            }
        }
    }
}
