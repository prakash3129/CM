using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;

namespace GCC
{
    public partial class frmSearch : DevComponents.DotNetBar.Office2007Form
    {
        public frmSearch()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(Font.FontFamily, 22);
            #region Initilize Custom Components
            lstCompanyTxt.Add(txtCompany_SearchBox_1);
            lstCompanyTxt.Add(txtCompany_SearchBox_2);
            lstCompanyTxt.Add(txtCompany_SearchBox_3);
            lstCompanyTxt.Add(txtCompany_SearchBox_4);
            lstCompanyTxt.Add(txtCompany_SearchBox_5);

            lstCompanylbl.Add(lblCompany_SearchLabel_1);
            lstCompanylbl.Add(lblCompany_SearchLabel_2);
            lstCompanylbl.Add(lblCompany_SearchLabel_3);
            lstCompanylbl.Add(lblCompany_SearchLabel_4);
            lstCompanylbl.Add(lblCompany_SearchLabel_5);

            

            lstCompanyChkBox.Add(chkBoxCompany_1);
            lstCompanyChkBox.Add(chkBoxCompany_2);
            lstCompanyChkBox.Add(chkBoxCompany_3);
            lstCompanyChkBox.Add(chkBoxCompany_4);
            lstCompanyChkBox.Add(chkBoxCompany_5);


            lstContactTxt.Add(txtContact_SearchBox_1);
            lstContactTxt.Add(txtContact_SearchBox_2);
            lstContactTxt.Add(txtContact_SearchBox_3);
            lstContactTxt.Add(txtContact_SearchBox_4);
            lstContactTxt.Add(txtContact_SearchBox_5);

            lstContactlbl.Add(lblContact_SearchLabel_1);
            lstContactlbl.Add(lblContact_SearchLabel_2);
            lstContactlbl.Add(lblContact_SearchLabel_3);
            lstContactlbl.Add(lblContact_SearchLabel_4);
            lstContactlbl.Add(lblContact_SearchLabel_5);

         

            lstContactChkBox.Add(chkBoxContact_1);
            lstContactChkBox.Add(chkBoxContact_2);
            lstContactChkBox.Add(chkBoxContact_3);
            lstContactChkBox.Add(chkBoxContact_4);
            lstContactChkBox.Add(chkBoxContact_5); 
            #endregion
        }

        //BAL_GlobalMySQL objBALGlobalMySQL = new BAL_GlobalMySQL();        

        DataRow _drContact;
        public DataRow drContact
        {
            get { return _drContact; }
            set { _drContact = value; }
        }

        DataRow _drCompany;
        public DataRow drCompany
        {
            get { return _drCompany; }
            set { _drCompany = value; }
        }

        string _TableToReturn = string.Empty;
        public string TableToReturn
        {
            get { return _TableToReturn; }
            set { _TableToReturn = value; }
        }
        

        string _sMaster_ID = string.Empty;
        public string sMaster_ID
        {
            get { return _sMaster_ID; }
            set { _sMaster_ID = value; }
        }

        string _sSearchTriggeredFrom = string.Empty;
        public string SearchTriggeredFrom
        {
            get { return _sSearchTriggeredFrom; }
            set { _sSearchTriggeredFrom = value; }
        }

        int _iContactRowIndex = -1;
        public int iContactRowIndex
        {
            get { return _iContactRowIndex; }
            set { _iContactRowIndex = value; }
        }

        int _CompanyRowIndex =  -1;
        public int iCompanyRowIndex
        {
            get { return _CompanyRowIndex; }
            set { _CompanyRowIndex = value; }
        }
        
        DataTable _dtFieldMasterCompany = null;
        public DataTable dtFieldMasterCompany
        {
            get { return _dtFieldMasterCompany; }
            set { _dtFieldMasterCompany = value; }
        }

        DataTable _dtFieldMasterContact = null;
        public DataTable dtFieldMasterContact
        {
            get { return _dtFieldMasterContact; }
            set { _dtFieldMasterContact = value; }
        }

        DataRow _drReturnRow = null;
        public DataRow drReturnRow
        {
            get { return _drReturnRow; }
            set { _drReturnRow = value; }
        }

        bool IsExternalSearch = false;
                
        //DataSet dsSearchData = new DataSet();
        bool IsPAFSearchOpted = false;

        List<string> lstAND = new List<string>();
        List<string> lstOR = new List<string>();

        List<DevComponents.DotNetBar.Controls.TextBoxX> lstCompanyTxt = new List<DevComponents.DotNetBar.Controls.TextBoxX>();
        List<LabelX> lstCompanylbl = new List<LabelX>();
        List<DevComponents.DotNetBar.Controls.CheckBoxX> lstCompanyChkBox = new List<DevComponents.DotNetBar.Controls.CheckBoxX>();

        List<DevComponents.DotNetBar.Controls.TextBoxX> lstContactTxt = new List<DevComponents.DotNetBar.Controls.TextBoxX>();
        List<LabelX> lstContactlbl = new List<LabelX>();
        List<DevComponents.DotNetBar.Controls.CheckBoxX> lstContactChkBox = new List<DevComponents.DotNetBar.Controls.CheckBoxX>();

        public List<string> lstCompanyColumnsToPopulate = new List<string>();
        public List<string> lstContactColumnsToPopulate = new List<string>();
        public List<string> lstPAFColumns = new List<string>();
       // CancellableQuery Query = null;

        DataSet dsCompanySearch = new DataSet();
        DataSet dsContactSearch = new DataSet();
        string TableName = string.Empty;
        string sCompanyTableName = string.Empty;
        string sContactTableName = string.Empty;
        string sSearchOutsideColumns = string.Empty;

        private bool Cancel_Query = false;

        private void frmSearch_Load(object sender, EventArgs e)
        {


            

            TableName = TableToReturn;
            btnCloseCompany.Parent = this;
            btnSearchCompany.Parent = this;
            chkCompanyInclude.Parent = this;
            chkCompanyInclude.BackColor = GV.pnlGlobalColor.Style.BackColor1.Color;
            sdgvSearch.PrimaryGrid.ReadOnly = true;
            chkCompanyInclude.Visible = true;
            chkBoxCompletsOnly.Checked = GV.SearchOnlyCompletedContacts;

            sCompanyTableName = (GV.sCompany_View.Length > 0 ? GV.sCompany_View : GV.sCompanyTable);
            sContactTableName = (GV.sContact_View.Length > 0 ? GV.sContact_View : GV.sContactTable);
             
            
            //sdgvSearch.PrimaryGrid.SortLevel = SortLevel.Root;

            IsExternalSearch = SearchTriggeredFrom == "Exteral";

            chkCompanyInclude.Refresh();
            lblMessage.Visible = false;
            StartPosition = FormStartPosition.CenterScreen;            

            //if (GV.Company_AllowPopulateFromSearch)
            //    lstCompanyColumnsToPopulate = (from x in dtFieldMasterCompany.AsEnumerable() where x.Field<string>("ALLOW_POPULATE_FROM_SEARCH") == "Y" select x.Field<string>("FIELD_NAME_TABLE")).ToList();

            //if (GV.Contact_AllowPopulateFromSearch)
            //    lstContactColumnsToPopulate = (from x in dtFieldMasterContact.AsEnumerable() where x.Field<string>("ALLOW_POPULATE_FROM_SEARCH") == "Y" select x.Field<string>("FIELD_NAME_TABLE")).ToList();

            if (GV.Company_AllowPopulateFromSearch)
                lstCompanyColumnsToPopulate = (from x in dtFieldMasterCompany.AsEnumerable() where (x.Field<string>("ALLOW_POPULATE_FROM_SEARCH")??string.Empty).Length > 1 select x.Field<string>("ALLOW_POPULATE_FROM_SEARCH")).ToList();

            if (GV.Contact_AllowPopulateFromSearch)
                lstContactColumnsToPopulate = (from x in dtFieldMasterContact.AsEnumerable() where (x.Field<string>("ALLOW_POPULATE_FROM_SEARCH")??string.Empty).Length > 1 select x.Field<string>("ALLOW_POPULATE_FROM_SEARCH")).ToList();

            if (SearchTriggeredFrom == "Company")
            {
                foreach (DataRow drPAFCols in dtFieldMasterCompany.Rows)
                {
                    if (drPAFCols["PAF_COLUMN"].ToString().Trim().Length > 0)
                        lstPAFColumns.Add(drPAFCols["FIELD_NAME_TABLE"].ToString());
                }
                this.Text = "Company Search";
            }
            else if (SearchTriggeredFrom == "Contact")
            {
                foreach (DataRow drPAFCols in dtFieldMasterContact.Rows)
                {
                    if (drPAFCols["PAF_COLUMN"].ToString().Trim().Length > 0)
                        lstPAFColumns.Add(drPAFCols["FIELD_NAME_TABLE"].ToString());
                }
                this.Text = "Contact Search";
            }

            if (TableName == "Company")
                superTabSearch.SelectedTab = superTabCompany;
            else
                superTabSearch.SelectedTab = superTabContact;

            if (GV.AllowPopulateFromPAFSearch || GV.sEmployeeName == "THANGAPRAKASH")
            {
                chkBoxQuickPaf.Visible = true;
                superTabPAF.Visible = true;                
            }
            else
            {
                chkBoxQuickPaf.Visible = false;
                superTabPAF.Visible = false;
                chkBoxQuickPaf.Checked = false;
            }

            DataRow[] drrSearchableCompanyColumns = dtFieldMasterCompany.Select("SEARCHABLE_COLUMN = 'Y'", "SEQUENCE_NO ASC");
            DataRow[] drrSearchableContactColumns = dtFieldMasterContact.Select("SEARCHABLE_COLUMN = 'Y'", "SEQUENCE_NO ASC");


            for (int i = 0; i < 5; i++)
            {
                //Company
                if (drrSearchableCompanyColumns.Length > i)
                {
                    lstCompanyTxt[i].Tag = drrSearchableCompanyColumns[i]["FIELD_NAME_TABLE"].ToString();
                    lstCompanylbl[i].Text = (i + 1) +". " + drrSearchableCompanyColumns[i]["FIELD_NAME_CAPTION"].ToString();
                    if (drCompany == null)//Search from outside                    
                        chkCompanyInclude.Visible = false;
                    else
                        lstCompanyTxt[i].Text = drCompany[drrSearchableCompanyColumns[i]["FIELD_NAME_TABLE"].ToString()].ToString();                                           
                }
                else
                {
                    lstCompanyTxt[i].Visible = false;
                    lstCompanylbl[i].Visible = false;
                    lstCompanyChkBox[i].Visible = false;
                }

                //Contact
                if (drrSearchableContactColumns.Length > i)
                {
                    lstContactTxt[i].Tag = drrSearchableContactColumns[i]["FIELD_NAME_TABLE"].ToString();
                    lstContactlbl[i].Text = (i + 1) + ". " + drrSearchableContactColumns[i]["FIELD_NAME_CAPTION"].ToString();
                    if (drContact == null)
                        chkCompanyInclude.Visible = false;                        
                    else
                        lstContactTxt[i].Text = drContact[drrSearchableContactColumns[i]["FIELD_NAME_TABLE"].ToString()].ToString();
                }
                else
                {
                    lstContactTxt[i].Visible = false;
                    lstContactlbl[i].Visible = false;
                    lstContactChkBox[i].Visible = false;
                }
            }

            if (GV.sCompany_View.Length > 0 || GV.sContact_View.Length > 0)
                sSearchOutsideColumns = ",PROJECT_ID,PROJECT_NAME ";

            string sSchema = "SELECT '0' AS TableID, MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS,EMAIL_VERIFIED " + sSearchOutsideColumns + " FROM " + sContactTableName + " CONTACT WHERE 1= 0;";
            sSchema += "SELECT '1' AS TableID, MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE " + sSearchOutsideColumns + " FROM " + sCompanyTableName + " COMPANY WHERE 1=0;";
            sSchema += "SELECT '2' AS TableID, MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS,EMAIL_VERIFIED " + sSearchOutsideColumns + " FROM " + sContactTableName + " CONTACT WHERE 1=0;";

            dsContactSearch = GV.MYSQL.BAL_ExecuteQueryMySQLSet(sSchema);
            dsCompanySearch.Tables.Add(dsContactSearch.Tables[1].Copy());
            dsCompanySearch.Tables.Add(dsContactSearch.Tables[2].Copy());


            dsContactSearch.Tables[0].TableName = "Contact_Contact";
            dsContactSearch.Tables[1].TableName = "Contact_Company";
            dsContactSearch.Tables[2].TableName = "Contact_AllContacts";
            dsContactSearch.Namespace = "Contact_Contact";

            dsCompanySearch.Tables[0].TableName = "Company_Company";
            dsCompanySearch.Tables[1].TableName = "Company_Contact";
            dsCompanySearch.Namespace = "Company_Company";

            if (sSearchOutsideColumns.Length > 0) // For searching multiple projects projectIDs has to be linked
            {

                DataRelation newRelation1 = new DataRelation("1", new DataColumn[] { dsContactSearch.Tables[0].Columns["MASTER_ID"], dsContactSearch.Tables[0].Columns["PROJECT_ID"] },
                                                                  new DataColumn[] { dsContactSearch.Tables[1].Columns["MASTER_ID"], dsContactSearch.Tables[1].Columns["PROJECT_ID"] }, false);

                DataRelation newRelation2 = new DataRelation("2", new DataColumn[] { dsContactSearch.Tables[1].Columns["MASTER_ID"], dsContactSearch.Tables[1].Columns["PROJECT_ID"] },
                                                                  new DataColumn[] { dsContactSearch.Tables[2].Columns["MASTER_ID"], dsContactSearch.Tables[2].Columns["PROJECT_ID"] }, false);
                
                //dsContactSearch.Relations.Add("1", dsContactSearch.Tables[0].Columns["MASTER_ID"], dsContactSearch.Tables[1].Columns["MASTER_ID"], false);
                //dsContactSearch.Relations.Add("1", dsContactSearch.Tables[0].Columns["PROJECT_ID"], dsContactSearch.Tables[1].Columns["PROJECT_ID"], false);

                //dsContactSearch.Relations.Add("2", dsContactSearch.Tables[1].Columns["MASTER_ID"], dsContactSearch.Tables[2].Columns["MASTER_ID"], false);
                //dsContactSearch.Relations.Add("2", dsContactSearch.Tables[1].Columns["PROJECT_ID"], dsContactSearch.Tables[2].Columns["PROJECT_ID"], false);

                dsContactSearch.Relations.Add(newRelation1);
                dsContactSearch.Relations.Add(newRelation2);


                dsCompanySearch.Relations.Add("1", dsCompanySearch.Tables[0].Columns["MASTER_ID"], dsCompanySearch.Tables[1].Columns["MASTER_ID"], false);
                dsCompanySearch.Relations.Add("2", dsCompanySearch.Tables[0].Columns["PROJECT_ID"], dsCompanySearch.Tables[1].Columns["PROJECT_ID"], false);
            }
            else
            {

                dsContactSearch.Relations.Add("1", dsContactSearch.Tables[0].Columns["MASTER_ID"], dsContactSearch.Tables[1].Columns["MASTER_ID"], false);
                dsContactSearch.Relations.Add("2", dsContactSearch.Tables[1].Columns["MASTER_ID"], dsContactSearch.Tables[2].Columns["MASTER_ID"], false);

                dsCompanySearch.Relations.Add("1", dsCompanySearch.Tables[0].Columns["MASTER_ID"], dsCompanySearch.Tables[1].Columns["MASTER_ID"], false);
            }

            sdgvSearch.PrimaryGrid.DataSource = dsContactSearch;


            foreach (DevComponents.DotNetBar.Controls.CheckBoxX chk in lstCompanyChkBox)
            {
                if (chk.Visible)
                {
                    chk.Checked = (bool)Properties.Settings.Default[chk.Name];
                    chk.CheckedChanged += chkBox_CheckedChanged;
                }
            }

            foreach (DevComponents.DotNetBar.Controls.CheckBoxX chk in lstContactChkBox)
            {
                if (chk.Visible)
                {
                    chk.Checked = (bool)Properties.Settings.Default[chk.Name];
                    chk.CheckedChanged += chkBox_CheckedChanged;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        void ResetTables()
        {
            foreach (DataTable dt in dsContactSearch.Tables)            
                dt.Rows.Clear();
            foreach (DataTable dt in dsCompanySearch.Tables)
                dt.Rows.Clear();
            GC.Collect();
        }
        


        void Build_Query_List(string sTableName, string sSearch_Column, string sSearchText, bool IsChecked)
        {
            sSearchText = GM.RemoveQuote(GM.RemoveEndBackSlash(sSearchText));
            if (sTableName == "Company")
            {
                switch (sSearch_Column.ToUpper())
                {
                    case "COMPANY_NAME":
                        if (IsPAFSearchOpted)
                        {
                            if (sSearchText.Length > 0 && IsChecked)
                                lstAND.Add(sSearchText);

                            //lstAND.Add("Organisation=" + sSearchText);
                        }
                        else
                        {
                            if (IsChecked)
                            {
                                if (sSearchText.Length > 0)
                                    lstAND.Add("Company.COMPANY_NAME LIKE '%" + sSearchText + "%'");
                                else
                                    lstAND.Add("IFNULL(Company.COMPANY_NAME,'') = ''");
                            }
                            else
                            {
                                if (sSearchText.Length > 0)
                                    lstOR.Add("Company.COMPANY_NAME LIKE '%" + sSearchText + "%'");
                            }
                        }
                        break;

                    case "POST_CODE":
                        if (IsPAFSearchOpted)
                        {
                            if (sSearchText.Length > 0 && IsChecked)
                                lstAND.Add(sSearchText);

                            //lstAND.Add("Postcode=" + sSearchText);
                        }
                        else
                        {
                            if (IsChecked)
                            {
                                if (sSearchText.Length > 0)
                                    lstAND.Add("REPLACE(Company.POST_CODE,' ','') LIKE '%" + sSearchText + "%'");
                                else
                                    lstAND.Add("IFNULL(Company.POST_CODE,'') =''");
                            }
                            else
                            {
                                if (sSearchText.Length > 0)
                                    lstOR.Add("REPLACE(Company.POST_CODE,' ','') LIKE '%" + sSearchText + "%'");
                            }
                        }
                        break;

                    case "SWITCHBOARD":
                        if (!IsPAFSearchOpted)
                        {
                            if (sSearchText.Length > 0)
                            {
                                string sTelephone = sSearchText;
                                Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                                sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                                if (sTelephone.Length > 7)
                                {
                                    sTelephone = sTelephone.Substring(sTelephone.Length - 8);

                                    if (IsChecked)
                                        lstAND.Add("RIGHT(regex_replace('[^0-9]', '', Company.SWITCHBOARD),8) ='" + sTelephone + "'");
                                    else
                                        lstOR.Add("RIGHT(regex_replace('[^0-9]', '', Company.SWITCHBOARD),8) ='" + sTelephone + "'");
                                }
                                else
                                {
                                    if (IsChecked)
                                        lstAND.Add("REPLACE(Company.SWITCHBOARD,' ','') LIKE '%" + sTelephone + "%'");
                                    else
                                        lstOR.Add("REPLACE(Company.SWITCHBOARD,' ','') LIKE '%" + sTelephone + "%'");
                                }
                            }
                            else
                            {
                                if (IsChecked)
                                    lstAND.Add("IFNULL(Company.SWITCHBOARD,'') = ''");
                            }
                        }
                        break;

                    case "WEB":
                        if (!IsPAFSearchOpted)
                        {
                            string sWeb = string.Empty;
                            if (GM.Web_Check(sSearchText))
                            {
                                try
                                {
                                    UriBuilder url = new UriBuilder(sSearchText);
                                    sWeb = url.Uri.Host;
                                    if (sWeb.Contains("."))
                                    {
                                        List<string> lst = sWeb.Split('.').ToList();
                                        if (lst.Count > 2)
                                            sWeb = lst[1];
                                        else
                                            sWeb = lst[0];
                                    }
                                }
                                catch (Exception c)//Some URLs passing web check test but not valid URLs. Web check has to be refiend - "Invalid URI: The hostname could not be parsed."
                                {
                                    sWeb = sSearchText.Replace(" ", string.Empty);
                                }
                            }
                            else
                                sWeb = sSearchText;

                            if (IsChecked)
                            {
                                if (sWeb.Length > 0)
                                    lstAND.Add("Company.WEB LIKE '%" + sWeb.Replace("'", "''") + "%'");
                                else
                                    lstAND.Add("IFNULL(Company.WEB,'') = ''");
                            }
                            else
                            {
                                if (sWeb.Length > 0)
                                    lstOR.Add("Company.WEB LIKE '%" + sWeb.Replace("'", "''") + "%'");
                            }
                        }
                        break;

                    case "CITY":
                        if (IsPAFSearchOpted)
                        {
                            if (sSearchText.Length > 0 && IsChecked)
                                lstAND.Add(sSearchText);

                            //lstAND.Add("Town=" + sSearchText);
                        }
                        else
                        {
                            if (IsChecked)
                            {
                                if (sSearchText.Length > 0)
                                    lstAND.Add("Company." + sSearch_Column + " LIKE  '%" + sSearchText + "%'");
                                else
                                    lstAND.Add("IFNULL(Company." + sSearch_Column + ",'') =  ''");
                            }
                            else
                            {
                                if (sSearchText.Length > 0)
                                    lstOR.Add("Company." + sSearch_Column + " LIKE  '%" + sSearchText + "%'");
                            }
                        }
                        break;

                    default:
                        if (!IsPAFSearchOpted)
                        {
                            if (IsChecked)
                            {
                                if (sSearchText.Length > 0)
                                    lstAND.Add("Company." + sSearch_Column + " LIKE  '%" + sSearchText + "%'");
                                else
                                    lstAND.Add("IFNULL(Company." + sSearch_Column + ",'') =  ''");
                            }
                            else
                            {
                                if (sSearchText.Length > 0)
                                    lstOR.Add("Company." + sSearch_Column + " LIKE  '%" + sSearchText + "%'");
                            }
                        }
                        break;
                }
            }
            else if (sTableName == "Contact")
            {
                switch (sSearch_Column.ToUpper())
                {
                    case "FIRST_NAME":
                        if (IsChecked)
                        {
                            if (sSearchText.Length > 0)
                            {
                                if (chkBoxSoundX.Checked)
                                    lstAND.Add("Contact.First_Name_Soundx =  SOUNDEX('" + sSearchText + "')");
                                else
                                    lstAND.Add("Contact.FIRST_NAME LIKE '%" + sSearchText + "%'");
                            }
                            else
                                lstAND.Add("IFNULL(Contact.FIRST_NAME,'') = ''");
                        }
                        else
                        {
                            if (sSearchText.Length > 0)
                            {
                                if (chkBoxSoundX.Checked)
                                    lstOR.Add("Contact.First_Name_Soundx =  SOUNDEX('" + sSearchText + "')");
                                else
                                    lstOR.Add("Contact.FIRST_NAME LIKE '%" + sSearchText + "%'");
                            }
                        }

                        break;

                    case "LAST_NAME":
                        if (IsChecked)
                        {
                            if (sSearchText.Length > 0)
                            {
                                if (chkBoxSoundX.Checked)
                                    lstAND.Add("Contact.Last_Name_Soundx =  SOUNDEX('" + sSearchText + "')");
                                else
                                    lstAND.Add("Contact.LAST_NAME LIKE '%" + sSearchText + "%'");
                            }
                            else
                                lstAND.Add("IFNULL(Contact.LAST_NAME,'') = ''");
                        }
                        else
                        {
                            if (sSearchText.Length > 0)
                            {
                                if (chkBoxSoundX.Checked)
                                    lstOR.Add("Contact.Last_Name_Soundx =  SOUNDEX('" + sSearchText + "')");
                                else
                                    lstOR.Add("Contact.LAST_NAME LIKE '%" + sSearchText + "%'");
                            }
                        }
                        break;

                    case "CONTACT_EMAIL":
                        string sEmailName = string.Empty;
                        if (sSearchText.Length > 0 && GM.Email_Check(sSearchText))
                        {
                            sEmailName = sSearchText.Substring(0, sSearchText.IndexOf("@")).Trim();

                            if (IsChecked)
                                lstAND.Add("SUBSTRING_INDEX(Contact.CONTACT_EMAIL, '@', 1) =  '" + sEmailName.Replace("'", "''") + "'");
                            else
                                lstOR.Add("SUBSTRING_INDEX(Contact.CONTACT_EMAIL, '@', 1) =  '" + sEmailName.Replace("'", "''") + "'");
                        }
                        else
                        {
                            if (IsChecked)
                            {
                                if (sSearchText.Length > 0)
                                    lstAND.Add("Contact.CONTACT_EMAIL LIKE  '%" + sSearchText + "%'");
                                else
                                    lstAND.Add("IFNULL(Contact.CONTACT_EMAIL,'') =  ''");
                            }
                            else
                            {
                                if (sSearchText.Length > 0)
                                    lstOR.Add("Contact.CONTACT_EMAIL LIKE  '%" + sSearchText + "%'");
                            }
                        }
                        break;


                    case "JOB_TITLE":
                        if (IsChecked)
                        {
                            if (sSearchText.Length > 0)
                                lstAND.Add("(Contact.JOB_TITLE LIKE  '%" + sSearchText + "%' OR Contact.OTHERS_JOBTITLE LIKE '%" + sSearchText + "%')");
                            else
                                lstAND.Add("(IFNULL(Contact.JOB_TITLE,'') =  '' OR IFNULL(Contact.OTHERS_JOBTITLE,'') =  '')");
                        }
                        else
                        {
                            if (sSearchText.Length > 0)
                                lstOR.Add("Contact.JOB_TITLE LIKE  '%" + sSearchText + "%' OR Contact.OTHERS_JOBTITLE LIKE '%" + sSearchText + "%'");
                        }
                        break;

                    case "CONTACT_TELEPHONE":

                        if (sSearchText.Length > 0)
                        {
                            string sTelephone = sSearchText;
                            Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                            sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                            if (sTelephone.Length > 7)
                            {
                                sTelephone = sTelephone.Substring(sTelephone.Length - 8);

                                if (IsChecked)
                                    lstAND.Add("RIGHT(regex_replace('[^0-9]', '', Contact.CONTACT_TELEPHONE),8) ='" + sTelephone + "'");
                                else
                                    lstOR.Add("RIGHT(regex_replace('[^0-9]', '', Contact.CONTACT_TELEPHONE),8) ='" + sTelephone + "'");
                            }
                            else
                            {
                                if (IsChecked)
                                    lstAND.Add("REPLACE(Contact.CONTACT_TELEPHONE,' ','') LIKE '%" + sTelephone + "%'");
                                else
                                    lstOR.Add("REPLACE(Contact.CONTACT_TELEPHONE,' ','') LIKE '%" + sTelephone + "%'");
                            }
                        }
                        else
                        {
                            if (IsChecked)
                                lstAND.Add("IFNULL(Contact.CONTACT_TELEPHONE,'') = ''");
                        }
                        break;

                    default:
                        if (IsChecked)
                        {
                            if (sSearchText.Length > 0)
                                lstAND.Add("Contact."+sSearch_Column+" LIKE  '%" + sSearchText + "%'");
                            else
                                lstAND.Add("IFNULL(Contact." + sSearch_Column + ",'') =  ''");
                        }
                        else
                        {
                            if (sSearchText.Length > 0)
                                lstOR.Add("Contact."+sSearch_Column + " LIKE  '%" + sSearchText + "%'");
                        }
                        break;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            drReturnRow = null;
            lblMessage.Visible = false;            
            lblMessage.Refresh();
            lstAND.Clear();
            lstOR.Clear();
            IsPAFSearchOpted = false;//reset

            if (superTabSearch.SelectedTab == superTabContact)
                TableName = "Contact";
            else if (superTabSearch.SelectedTab == superTabCompany)
                TableName = "Company";
            else
                TableName = "PAF";

                                       
            string sContact_String_1 = GM.RemoveEndBackSlash(txtContact_SearchBox_1.Text.Trim().Replace("'", "''"));
            string sContact_String_2 = GM.RemoveEndBackSlash(txtContact_SearchBox_2.Text.Trim().Replace("'", "''"));
            string sContact_String_3 = GM.RemoveEndBackSlash(txtContact_SearchBox_3.Text.Trim().Replace("'", "''"));
            string sContact_String_4 = GM.RemoveEndBackSlash(txtContact_SearchBox_4.Text.Trim().Replace("'", "''"));
            string sContact_String_5 = GM.RemoveEndBackSlash(txtContact_SearchBox_5.Text.Trim().Replace("'", "''"));

            string sCompany_String_1 = GM.RemoveEndBackSlash(txtCompany_SearchBox_1.Text.Trim().Replace("'", "''"));
            string sCompany_String_2 = GM.RemoveEndBackSlash(txtCompany_SearchBox_2.Text.Trim().Replace("'", "''"));
            string sCompany_String_3 = GM.RemoveEndBackSlash(txtCompany_SearchBox_3.Text.Trim().Replace("'", "''"));
            string sCompany_String_4 = GM.RemoveEndBackSlash(txtCompany_SearchBox_4.Text.Trim().Replace("'", "''"));
            string sCompany_String_5 = GM.RemoveEndBackSlash(txtCompany_SearchBox_5.Text.Trim().Replace("'", "''"));
            
            IsPAFSearchOpted = chkBoxQuickPaf.Checked;
            string sSelect = string.Empty;            
            try
            {
                if (superTabSearch.SelectedTab == superTabContact)//Contact
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (lstContactTxt[i].Tag != null)
                            Build_Query_List("Contact", lstContactTxt[i].Tag.ToString(), lstContactTxt[i].Text, lstContactChkBox[i].Checked);
                    }

                    #region MyRegion

                    /////////////////////
                    ////FirstName///////
                    ////////////////////
                    //if (chkBoxContact_1.Checked)
                    //{
                    //    if (sContact_String_1.Length > 0)
                    //    {
                    //        if(chkBoxSoundX.Checked)
                    //            lstAND.Add("First_Name_Soundx =  SOUNDEX('" + sContact_String_1 + "')");
                    //        else
                    //            lstAND.Add("FIRST_NAME LIKE '%" + sContact_String_1 + "%'");
                    //    }
                    //    else
                    //        lstAND.Add("IFNULL(FIRST_NAME,'') = ''");
                    //}
                    //else
                    //{
                    //    if (sContact_String_1.Length > 0)
                    //    {
                    //        if (chkBoxSoundX.Checked)
                    //            lstOR.Add("First_Name_Soundx =  SOUNDEX('" + sContact_String_1 + "')");
                    //        else
                    //            lstOR.Add("FIRST_NAME LIKE '%" + sContact_String_1 + "%'");
                    //    }
                    //}


                    /////////////////////
                    ////LastName///////
                    ////////////////////
                    //if (chkBoxContact_2.Checked)
                    //{
                    //    if (sContact_String_2.Length > 0)
                    //    {
                    //        if (chkBoxSoundX.Checked)
                    //            lstAND.Add("Last_Name_Soundx =  SOUNDEX('" + sContact_String_2 + "')");
                    //        else
                    //            lstAND.Add("LAST_NAME LIKE '%" + sContact_String_2 + "%'");
                    //    }
                    //    else
                    //        lstAND.Add("IFNULL(LAST_NAME,'') = ''");
                    //}
                    //else
                    //{
                    //    if (sContact_String_2.Length > 0)
                    //    {
                    //        if (chkBoxSoundX.Checked)
                    //            lstOR.Add("Last_Name_Soundx =  SOUNDEX('" + sContact_String_2 + "')");
                    //        else
                    //            lstOR.Add("LAST_NAME LIKE '%" + sContact_String_2 + "%'");
                    //    }
                    //}





                    /////////////////////
                    ////Email///////
                    ////////////////////
                    //string sEmailName = string.Empty;
                    //if (sContact_String_3.Length > 0 && GM.Email_Check(sContact_String_3))
                    //{
                    //    sEmailName = sContact_String_3.Substring(0, sContact_String_3.IndexOf("@")).Trim();

                    //    if (chkBoxContact_3.Checked)
                    //        lstAND.Add("SUBSTRING_INDEX(CONTACT_EMAIL, '@', 1) =  '" + sEmailName.Replace("'", "''") + "'");
                    //    else
                    //        lstOR.Add("SUBSTRING_INDEX(CONTACT_EMAIL, '@', 1) =  '" + sEmailName.Replace("'", "''") + "'");
                    //}
                    //else
                    //{
                    //    if (chkBoxContact_3.Checked)
                    //    {
                    //        if (sContact_String_3.Length > 0)
                    //            lstAND.Add("CONTACT_EMAIL LIKE  '%" + sContact_String_3 + "%'");
                    //        else
                    //            lstAND.Add("IFNULL(CONTACT_EMAIL,'') =  ''");
                    //    }
                    //    else
                    //    {
                    //        if (sContact_String_3.Length > 0)
                    //            lstOR.Add("CONTACT_EMAIL LIKE  '%" + sContact_String_3 + "%'");
                    //    }
                    //}


                    /////////////////////
                    ////JobTitle///////
                    ////////////////////
                    //if (chkBoxContact_4.Checked)
                    //{
                    //    if (sContact_String_4.Length > 0)
                    //        lstAND.Add("(JOB_TITLE LIKE  '%" + sContact_String_4 + "%' OR OTHERS_JOBTITLE LIKE '%" + sContact_String_4 + "%')");
                    //    else
                    //        lstAND.Add("(IFNULL(JOB_TITLE,'') =  '' OR IFNULL(OTHERS_JOBTITLE,'') =  '')");
                    //}
                    //else
                    //{
                    //    if (sContact_String_4.Length > 0)
                    //        lstOR.Add("JOB_TITLE LIKE  '%" + sContact_String_4 + "%' OR OTHERS_JOBTITLE LIKE '%" + sContact_String_4 + "%'");
                    //}


                    /////////////////////
                    ////Telephone///////
                    ////////////////////
                    //if (sContact_String_5.Length > 0)
                    //{
                    //    string sTelephone = sContact_String_5;

                    //    Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    //    sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                    //    if (sTelephone.Length > 7)
                    //    {
                    //        sTelephone = sTelephone.Substring(sTelephone.Length - 8);

                    //        if (chkBoxContact_5.Checked)
                    //            lstAND.Add("RIGHT(regex_replace('[^0-9]', '', CONTACT_TELEPHONE),8) ='" + sTelephone + "'");
                    //        else
                    //            lstOR.Add("RIGHT(regex_replace('[^0-9]', '', CONTACT_TELEPHONE),8) ='" + sTelephone + "'");
                    //    }
                    //    else
                    //    {
                    //        if (chkBoxContact_5.Checked)
                    //            lstAND.Add("REPLACE(CONTACT_TELEPHONE,' ','') LIKE '%" + sTelephone + "%'");
                    //        else
                    //            lstOR.Add("REPLACE(CONTACT_TELEPHONE,' ','') LIKE '%" + sTelephone + "%'");
                    //    }
                    //}
                    //else
                    //{
                    //    if (chkBoxContact_5.Checked)
                    //        lstAND.Add("IFNULL(CONTACT_TELEPHONE,'') = ''");
                    //} 
                    #endregion
                }
                else if (superTabSearch.SelectedTab == superTabCompany)//Company or PAF
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (lstCompanyTxt[i].Tag != null)
                            Build_Query_List("Company", lstCompanyTxt[i].Tag.ToString(), lstCompanyTxt[i].Text, lstCompanyChkBox[i].Checked);                                                                                
                    }

                    #region MyRegion
                    /////////////////////
                    ////Company Name/////
                    ////////////////////
                    //if (chkBoxCompany_1.Checked)
                    //{
                    //    if (sCompany_String_1.Length > 0)
                    //        lstAND.Add("COMPANY_NAME LIKE '%" + sCompany_String_1 + "%'");
                    //    else
                    //        lstAND.Add("IFNULL(COMPANY_NAME,'') = ''");
                    //}
                    //else
                    //{
                    //    if (sCompany_String_1.Length > 0)
                    //        lstOR.Add("COMPANY_NAME LIKE '%" + sCompany_String_1 + "%'");
                    //}


                    ///////////////////
                    ////Address///////
                    //////////////////
                    //if (chkBoxCompany_2.Checked)
                    //{
                    //    if (sCompany_String_2.Length > 0)
                    //        lstAND.Add("(ADDRESS_1 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_2 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_3 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_4 LIKE '%" + sCompany_String_2 + "%' OR CITY LIKE '%" + sCompany_String_2 + "%' OR COUNTY LIKE '%" + sCompany_String_2 + "%' OR COUNTRY LIKE '%" + sCompany_String_2 + "%')");
                    //    else
                    //        lstAND.Add("(IFNULL(ADDRESS_1,'') =''  OR IFNULL(ADDRESS_2,'') = '' OR IFNULL(ADDRESS_3,'') ='' OR IFNULL(ADDRESS_4,'') ='' OR IFNULL(CITY,'') ='' OR IFNULL(COUNTY,'') ='' OR IFNULL(COUNTRY,'') ='')");
                    //}
                    //else
                    //{
                    //    if (sCompany_String_2.Length > 0)
                    //        lstOR.Add("(ADDRESS_1 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_2 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_3 LIKE '%" + sCompany_String_2 + "%' OR ADDRESS_4 LIKE '%" + sCompany_String_2 + "%' OR CITY LIKE '%" + sCompany_String_2 + "%' OR COUNTY LIKE '%" + sCompany_String_2 + "%' OR COUNTRY LIKE '%" + sCompany_String_2 + "%')");
                    //}


                    ///////////////////
                    ////postCode///////
                    //////////////////
                    //if (chkBoxCompany_3.Checked)
                    //{
                    //    if (sCompany_String_3.Length > 0)
                    //        lstAND.Add("REPLACE(POST_CODE,' ','') LIKE '%" + sCompany_String_3 + "%'");
                    //    else
                    //        lstAND.Add("IFNULL(POST_CODE,'') =''");
                    //}
                    //else
                    //{
                    //    if (sCompany_String_3.Length > 0)
                    //        lstOR.Add("REPLACE(POST_CODE,' ','') LIKE '%" + sCompany_String_3 + "%'");
                    //}


                    ////////////////////////////
                    ////Company Telephone///////
                    ///////////////////////////
                    //if (sCompany_String_4.Length > 0)
                    //{
                    //    string sTelephone = sCompany_String_4;

                    //    Regex rNumeric = new Regex(@"[^\d]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    //    sTelephone = rNumeric.Replace(sTelephone, string.Empty);
                    //    if (sTelephone.Length > 7)
                    //    {
                    //        sTelephone = sTelephone.Substring(sTelephone.Length - 8);

                    //        if (chkBoxCompany_4.Checked)
                    //            lstAND.Add("RIGHT(regex_replace('[^0-9]', '', SWITCHBOARD),8) ='" + sTelephone + "'");
                    //        else
                    //            lstOR.Add("RIGHT(regex_replace('[^0-9]', '', SWITCHBOARD),8) ='" + sTelephone + "'");
                    //    }
                    //    else
                    //    {
                    //        if (chkBoxCompany_4.Checked)
                    //            lstAND.Add("REPLACE(SWITCHBOARD,' ','') LIKE '%" + sTelephone + "%'");
                    //        else
                    //            lstOR.Add("REPLACE(SWITCHBOARD,' ','') LIKE '%" + sTelephone + "%'");
                    //    }
                    //}
                    //else
                    //{
                    //    if (chkBoxCompany_4.Checked)
                    //        lstAND.Add("IFNULL(SWITCHBOARD,'') = ''");
                    //}


                    ///////////////////
                    ////Website///////
                    //////////////////
                    //string sWeb = string.Empty;
                    //if (GM.Web_Check(sCompany_String_5))
                    //{
                    //    UriBuilder url = new UriBuilder(sCompany_String_5);
                    //    sWeb = url.Uri.Host;
                    //    if (sWeb.Contains("."))
                    //    {
                    //        List<string> lst = sWeb.Split('.').ToList();
                    //        if (lst.Count > 2)
                    //            sWeb = lst[1];
                    //        else
                    //            sWeb = lst[0];
                    //    }
                    //}
                    //else
                    //    sWeb = sCompany_String_5;

                    //if (chkBoxCompany_5.Checked)
                    //{
                    //    if (sWeb.Length > 0)
                    //        lstAND.Add("WEB LIKE '%" + sWeb.Replace("'", "''") + "%'");
                    //    else
                    //        lstAND.Add("IFNULL(WEB,'') = ''");
                    //}
                    //else
                    //{
                    //    if (sWeb.Length > 0)
                    //        lstOR.Add("WEB LIKE '%" + sWeb.Replace("'", "''") + "%'");
                    //} 
                    #endregion
                }
                else if (superTabSearch.SelectedTab == superTabPAF)
                    IsPAFSearchOpted = true;                
                
                if (IsPAFSearchOpted)
                {
                    DataTable dtPAFData = new DataTable("AddressListItem");
                    dtPAFData.Columns.Add("Address");
                    dtPAFData.Columns.Add("PostKey");
                    XDocument xmlDoc;
                    Stream sXMLStream;
                    string sSearchSource = string.Empty;
                    string sDelimiter = string.Empty;
                    if (superTabSearch.SelectedTab == superTabPAF)
                    {
                        sSearchSource = "addressfastfind.pce?fastfind=";
                        sDelimiter = ",";
                        if (txtPAFPostCode.Text.Trim().Length > 0)
                        {
                            lstAND.Add("Postcode=" + GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFPostCode.Text)));
                            sSearchSource = "addresslist.pce?";
                            sDelimiter = "&";

                            if (txtPAFCompany.Text.Trim().Length > 0)
                                lstAND.Add("Organisation=" + GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFCompany.Text)));

                            if (txtPAFStreetInfo.Text.Trim().Length > 0)
                                lstAND.Add("Street=" + GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFStreetInfo.Text)));

                            if (txtPAFProperty.Text.Trim().Length > 0)
                                lstAND.Add("Property=" + GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFProperty.Text)));

                            if (txtPAFTown.Text.Trim().Length > 0)
                                lstAND.Add("Town=" + GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFTown.Text)));
                        }
                        else
                        {
                            if (txtPAFCompany.Text.Trim().Length > 0)
                                lstAND.Add(GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFCompany.Text)));

                            if (txtPAFStreetInfo.Text.Trim().Length > 0)
                                lstAND.Add(GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFStreetInfo.Text)));

                            if (txtPAFProperty.Text.Trim().Length > 0)
                                lstAND.Add(GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFProperty.Text)));

                            if (txtPAFTown.Text.Trim().Length > 0)
                                lstAND.Add(GM.RemoveQuote(GM.RemoveEndBackSlash(txtPAFTown.Text)));
                        }
                    }
                    else
                    {
                        sSearchSource = "addressfastfind.pce?fastfind=";
                        sDelimiter = ",";
                    }

                    if (lstAND.Count > 0)
                    {
                        string sQueryString = string.Empty;                        
                        foreach (string sQuery in lstAND)
                        {
                            if (sQueryString.Length > 0)
                                sQueryString += sDelimiter + sQuery;
                            else
                                sQueryString = sQuery;
                        }

                        sQueryString = sSearchSource + sQueryString;
                        xmlDoc = XDocument.Load("http://172.27.137.182:81/" + sQueryString);
                        sXMLStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlDoc.Root.ToString() ?? ""));
                        dtPAFData.ReadXml(sXMLStream);

                        if (dtPAFData.Rows.Count > 0)
                        {
                            if (dtPAFData.Rows.Count == 1)
                            {
                                if (dtPAFData.Rows[0]["Address"].ToString().Contains("Error:"))
                                {
                                    ToastNotification.Show(this, "Your search did not match any data. Refine your search.");
                                    return;
                                }
                            }
                            if (GV.AllowPopulateFromPAFSearch && lstPAFColumns.Count > 0 && SearchTriggeredFrom != "External")
                            {
                                lblMessage.Text = "   Double-Clicking on results will get populated to " + SearchTriggeredFrom + " fields.";
                                lblMessage.Visible = true;                                
                                lblMessage.Refresh();
                                superTabSearch.Refresh();
                            }
                            TableName = "PAF";
                            TableToReturn = "PAF";
                            //dgvSearch.DataSource = dtPAFData;
                            sdgvSearch.PrimaryGrid.Rows.Clear();
                            sdgvSearch.PrimaryGrid.Columns.Clear();
                            sdgvSearch.PrimaryGrid.DataSource = null;
                            sdgvSearch.PrimaryGrid.DataSource = dtPAFData;
                        }
                    }
                }
                else
                {
                    //Query Formation
                    if (lstAND.Count > 0)//Only of a field checked
                    {
                        string sAnd = string.Empty;
                        string sOR = string.Empty;

                        foreach (string s in lstAND)
                        {
                            if (sAnd.Length > 0)
                                sAnd += " AND " + s;
                            else
                                sAnd = "(" + s;
                        }

                        if (sAnd.Length > 0)
                            sAnd += ")";

                        foreach (string s in lstOR)
                        {
                            if (sOR.Length > 0)
                                sOR += " OR " + s;
                            else
                                sOR = "(" + s;
                        }
                        if (sOR.Length > 0)
                            sOR += " OR 1=1)";

                        sAnd = sAnd.Trim();
                        sOR = sOR.Trim();

                        string sCompletedContacts = string.Empty;
                        if (TableName == "Contact" && chkBoxCompletsOnly.Visible && chkBoxCompletsOnly.Checked)
                            sCompletedContacts =  "(TR_CONTACT_STATUS IN (" + GV.sTRContactstatusTobeValidated + ") OR WR_CONTACT_STATUS IN (" + GV.sWRContactstatusTobeValidated + ")) AND ";

                        string sWhereClause = string.Empty;
                        if (sAnd.Length > 0 && sOR.Length > 0)
                            sWhereClause = sCompletedContacts + sAnd + " AND " + sOR;
                        else if (sAnd.Length > 0)
                            sWhereClause = sCompletedContacts + sAnd;
                        else if (sOR.Length > 0)
                            sWhereClause = sCompletedContacts + sOR;
                        else
                        {
                            ToastNotification.Show(this, "Invalid search criteria");
                            return;
                        }

                        DataTable dtQuery = new DataTable();
                        dtQuery.Columns.Add("Query");
                        //dtQuery.Columns.Add("TableName");
                        dtQuery.Columns.Add("TableID");
                        //dtQuery.Columns.Add("QueryColumn");                        
                        dtQuery.Columns.Add("Status");

                        #region Comment
                        //return;


                        //if (superTabSearch.SelectedTab == superTabContact)
                        //{
                        //    if (GV.sContact_View.Length > 0)
                        //    {
                        //        sSelect = "SELECT PROJECT_ID,PROJECT_NAME, MASTER_ID, COMPANY_NAME, FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS FROM " + GV.sContact_View + " Contact WHERE ";
                        //        sSelect = sSelect + " (" + sWhereClause + ")  LIMIT 100";
                        //    }
                        //    else
                        //    {
                        //        sSelect = "SELECT GROUP_CONCAT(MASTER_ID),GROUP_CONCAT(CONTACT_ID_P) FROM (SELECT MASTER_ID, CONTACT_ID_P FROM " + GV.sContactTable + " Contact WHERE (" + sWhereClause + ") LIMIT 100) AS X INTO @CompanyID, @ContactID;";
                        //        sSelect += "SELECT MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS FROM " + GV.sContactTable + " WHERE FIND_IN_SET(CONTACT_ID_P,@ContactID);";
                        //        sSelect += "SELECT MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE FROM " + GV.sCompanyTable + " WHERE FIND_IN_SET(MASTER_ID,@CompanyID);";
                        //        sSelect += "SELECT MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS FROM " + GV.sContactTable + " WHERE FIND_IN_SET(MASTER_ID,@CompanyID);";
                        //    }
                        //}
                        //else
                        //{
                        //    if (GV.sCompany_View.Length > 0)
                        //    {
                        //        sSelect = "SELECT PROJECT_ID, PROJECT_NAME, MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE FROM " + GV.sCompany_View + " Company WHERE ";
                        //        sSelect = sSelect + " (" + sWhereClause + ")  LIMIT 100";
                        //    }
                        //    else
                        //    {
                        //        sSelect = "SELECT GROUP_CONCAT(MASTER_ID) FROM (SELECT MASTER_ID FROM " + GV.sCompanyTable + " Company WHERE (" + sWhereClause + ") LIMIT 100) AS X INTO @CompanyID;";
                        //        sSelect += "SELECT MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE FROM " + GV.sCompanyTable + " WHERE FIND_IN_SET(MASTER_ID,@CompanyID);";
                        //        sSelect += "SELECT MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS FROM " + GV.sContactTable + " WHERE FIND_IN_SET(MASTER_ID,@CompanyID);";
                        //    }
                        //}  
                        #endregion

                        if (superTabSearch.SelectedTab == superTabContact)
                        {
                            dtQuery.Rows.Add("SELECT '0' AS TableID,MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS,EMAIL_VERIFIED " + sSearchOutsideColumns + " FROM " + sContactTableName + " Contact WHERE (" + sWhereClause + ") LIMIT 100;", "0", "0");
                            dtQuery.Rows.Add("SELECT '1' AS TableID,MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE " + sSearchOutsideColumns + " FROM " + sCompanyTableName + " Company", "1", "0");
                            dtQuery.Rows.Add("SELECT '2' AS TableID,MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS,EMAIL_VERIFIED " + sSearchOutsideColumns + " FROM " + sContactTableName + " Contact", "2", "0");
                        }
                        else
                        {
                            dtQuery.Rows.Add("SELECT '3' AS TableID,MASTER_ID,COMPANY_NAME,ADDRESS_1,ADDRESS_2,ADDRESS_3,ADDRESS_4,CITY,COUNTY,POST_CODE,COUNTRY,SWITCHBOARD,WEB," + GV.sAccessTo + "_PRIMARY_DISPOSAL," + GV.sAccessTo + "_SECONDARY_DISPOSAL,SOURCE " + sSearchOutsideColumns + " FROM " + sCompanyTableName + " Company WHERE (" + sWhereClause + ") LIMIT 100;", "3", "0");
                            dtQuery.Rows.Add("SELECT '4' AS TableID,MASTER_ID,CONTACT_ID_P,FIRST_NAME,LAST_NAME,JOB_TITLE,OTHERS_JOBTITLE,CONTACT_EMAIL,CONTACT_TELEPHONE,TR_CONTACT_STATUS,WR_CONTACT_STATUS,EMAIL_VERIFIED " + sSearchOutsideColumns + " FROM " + sContactTableName + " Contact", "4", "0");
                        }
                        
                        RunQuery(dtQuery);
                        
                        #region Comment
                        //dsSearchData = GV.MYSQL.BAL_ExecuteQueryMySQLSet(sSelect);
                        //if (dsSearchData.Tables[0].Rows.Count > 0)
                        //{
                        //    //dsSearchData.Tables[0].Columns["MASTER_ID"].ColumnName = "MASTER_ID";//Grid Column is Case sensitive
                        //    if (superTabSearch.SelectedTab == superTabContact)
                        //    {
                        //        TableName = "Contact";
                        //        if (GV.sContact_View.Length > 0)
                        //        {

                        //        }
                        //        else
                        //        {
                        //            dsSearchData.Tables[0].TableName = "Contact_Contact";
                        //            dsSearchData.Tables[1].TableName = "Contact_Company";
                        //            dsSearchData.Tables[2].TableName = "Contact_AllContacts";
                        //            dsSearchData.Namespace = "Contact_Contact";
                        //            dsSearchData.Relations.Add("1", dsSearchData.Tables[0].Columns["MASTER_ID"], dsSearchData.Tables[1].Columns["MASTER_ID"], false);
                        //            dsSearchData.Relations.Add("2", dsSearchData.Tables[1].Columns["MASTER_ID"], dsSearchData.Tables[2].Columns["MASTER_ID"], false);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        TableName = "Company";
                        //        if (GV.sCompany_View.Length > 0)
                        //        { }
                        //        else
                        //        {
                        //            dsSearchData.Tables[0].TableName = "Company_Company";
                        //            dsSearchData.Tables[1].TableName = "Company_Contact";
                        //            dsSearchData.Namespace = "Company_Company";
                        //            dsSearchData.Relations.Add("1", dsSearchData.Tables[0].Columns["MASTER_ID"], dsSearchData.Tables[1].Columns["MASTER_ID"], false);
                        //        }
                        //    }
                        //}

                        //if (dsSearchData.Tables[0].Rows.Count > 99)
                        //{
                        //    ResetGrid();
                        //    sdgvSearch.PrimaryGrid.DataSource = dsSearchData;
                        //    lblMessage.Text = "   Showing only top 100 results. Refine you search";
                        //    lblMessage.Visible = true;
                        //    superTabSearch.Refresh();
                        //    lblMessage.Refresh();
                        //    ToastNotification.Show(this, "Showing only top 100 results. Refine you search");
                        //}
                        //else
                        //{
                        //    ResetGrid();
                        //    sdgvSearch.PrimaryGrid.DataSource = dsSearchData;
                        //    lblMessage.Visible = false;
                        //    lblMessage.Refresh();
                        //    superTabSearch.Refresh();
                        //}

                        //IncludeCompany(chkCompanyInclude.Checked, sdgvSearch.PrimaryGrid);

                        //if (dsSearchData.Tables[0].Rows.Count == 0)
                        //    ToastNotification.Show(this, "Your search did not match any data. Refine your search.");  
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("Collection was modified") || ex.Message.Contains("DataTable internal index is corrupted"))) //Sometime thread not closing when user acts very fast.
                    GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
                //MessageBoxEx.Show(ex.Message, "Campaign Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void Query_Complete()
        {
            if (TableName == "Contact")
            {
                if (dsContactSearch.Tables[0].Rows.Count > 0)
                {
                    if (dsContactSearch.Tables[0].Rows.Count > 99)
                    {
                        ResetGrid();
                        sdgvSearch.PrimaryGrid.DataSource = dsContactSearch;
                        lblMessage.Text = "   Showing only top 100 results. Refine you search";
                        lblMessage.Visible = true;
                        superTabSearch.Refresh();
                        lblMessage.Refresh();
                        ToastNotification.Show(this, "Showing only top 100 results. Refine you search");
                    }
                    else
                    {
                        ResetGrid();
                        sdgvSearch.PrimaryGrid.DataSource = dsContactSearch;
                        lblMessage.Visible = false;
                        lblMessage.Refresh();
                        superTabSearch.Refresh();
                    }
                }
                else
                    ToastNotification.Show(this, "Your search did not match any data. Refine your search.");            
            }
            else if (TableName == "Company")
            {
                if (dsCompanySearch.Tables[0].Rows.Count > 0)
                {
                    if (dsCompanySearch.Tables[0].Rows.Count > 99)
                    {
                        ResetGrid();
                        sdgvSearch.PrimaryGrid.DataSource = dsCompanySearch;
                        lblMessage.Text = "   Showing only top 100 results. Refine you search";
                        lblMessage.Visible = true;
                        superTabSearch.Refresh();
                        lblMessage.Refresh();
                        ToastNotification.Show(this, "Showing only top 100 results. Refine you search");
                    }
                    else
                    {
                        ResetGrid();
                        sdgvSearch.PrimaryGrid.DataSource = dsCompanySearch;
                        lblMessage.Visible = false;
                        lblMessage.Refresh();
                        superTabSearch.Refresh();
                    }
                }
                else
                    ToastNotification.Show(this, "Your search did not match any data. Refine your search.");            
            }
                      
            IncludeCompany(chkCompanyInclude.Checked, sdgvSearch.PrimaryGrid);
            
        }
        
                
        public void RunQuery(DataTable dtQuery)
        {
            
            
            //sdgvSearch.PrimaryGrid.Rows.Clear();
            //sdgvSearch.PrimaryGrid.Columns.Clear();            
            cProgress.IsRunning = true;
            //if (Query != null)
            //{
            //    Query.CancelQuery();
            //    Query.Dispose();
            //    Query = null;
            //}

            ResetTables();            
            using (CancellableQuery Query = new CancellableQuery { dtQuery = dtQuery, ReturnDataInChunks = true, QueryChunkSize = 100, ClearObjectsOnChunkCompleted = false })
            {
                Query.QueryCompleted += (sender, args) =>
                {
                    if (Cancel_Query)
                    {
                        Query.CancelQuery();
                        return;
                    }

                    Append_Results(args.Results);
                    if (TableName == "Contact")
                    {
                        sdgvSearch.PrimaryGrid.DataSource = dsContactSearch;
                        lblCount.Text = " Total Rows :" + (dsContactSearch.Tables[0].Rows.Count + dsContactSearch.Tables[1].Rows.Count + dsContactSearch.Tables[2].Rows.Count);
                    }
                    else if (TableName == "Company")
                    {
                        sdgvSearch.PrimaryGrid.DataSource = dsCompanySearch;
                        lblCount.Text = " Total Rows :" + (dsCompanySearch.Tables[0].Rows.Count + dsCompanySearch.Tables[1].Rows.Count);
                    }                    
                    //lblCount2.Text = " Contact Search : " + dsContactSearch.Tables[0].Rows.Count + " Company : " + dsContactSearch.Tables[1].Rows.Count + " All Contact :" + dsContactSearch.Tables[2].Rows.Count;
                    cProgress.IsRunning = Query.IsRunning;
                    if (!Query.IsRunning)
                        Query_Complete();
                    //lblQueryStatus.Text = "Query Completed";
                    //lblRowsReturned.Text = string.Format("{0} rows returned", superGridControl1.PrimaryGrid.Rows.Count);
                    //btnStopQuery.Enabled = false;
                    //btnRunQuery.Enabled = true;
                };
                Query.QueryChunkCompleted += (sender, args) =>
                {
                    if (Cancel_Query)
                    {
                        Query.CancelQuery();
                        return;
                    }

                    Append_Results(args.Results);
                    //lblRowsReturned.Text = string.Format("{0} rows returned", superGridControl1.PrimaryGrid.Rows.Count);
                    if (TableName == "Contact")
                        lblCount.Text = " Total Rows :" + (dsContactSearch.Tables[0].Rows.Count + dsContactSearch.Tables[1].Rows.Count + dsContactSearch.Tables[2].Rows.Count);
                    else if (TableName == "Company")
                        lblCount.Text = " Total Rows :" + (dsCompanySearch.Tables[0].Rows.Count + dsCompanySearch.Tables[1].Rows.Count);
                    //lblCount2.Text = " Contact Search : " + dsContactSearch.Tables[0].Rows.Count + " Company : " + dsContactSearch.Tables[1].Rows.Count + " All Contact :" + dsContactSearch.Tables[2].Rows.Count;
                };
                Query.OnQueryError += (sender, args) =>
                {
                   // GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  args.ex, true, false);
                };
                Query.StartQueryExecution();
            }
        }

        private void sdgvSearch_BeforeExpand(object sender, GridBeforeExpandEventArgs e)
        {
            //if(Query.IsRunning)
            //ToastNotification.Show(this, e.GridPanel.Name);
        }

        void Append_Results(List<Object[]> val)
        {
            if (val.Count > 0)
            {
                switch (val[0].GetValue(0).ToString())
                {
                    case "0":                    
                        foreach (Object[] Values in val)
                            dsContactSearch.Tables[0].Rows.Add(Values);
                        break;
                    
                    case "1":                    
                        foreach (Object[] Values in val)
                            dsContactSearch.Tables[1].Rows.Add(Values);
                        break;
                    
                    case "2":                    
                        foreach (Object[] Values in val)
                            dsContactSearch.Tables[2].Rows.Add(Values);
                        break;

                    case "3":
                        foreach (Object[] Values in val)
                            dsCompanySearch.Tables[0].Rows.Add(Values);
                        break;

                    case "4":
                        foreach (Object[] Values in val)
                            dsCompanySearch.Tables[1].Rows.Add(Values);
                        break;                    
                }
            }
        }

        void ResetGrid()
        {
            sdgvSearch.PrimaryGrid.Columns.Clear();
            sdgvSearch.PrimaryGrid.Rows.Clear();
            sdgvSearch.PrimaryGrid.DataSource = null;
        }

        void IncludeCompany(bool Include, GridPanel Gpanel)
        {
            if (IsPAFSearchOpted)
                return;
            if (sMaster_ID.Length > 0)
            {
                Background b = null;
                if (Include)
                {
                    b = new Background(Color.Gainsboro);
                    if (sSearchOutsideColumns.Length > 0)
                    {
                        foreach (GridRow GR in Gpanel.Rows)
                        {
                            if (GR.Cells["MASTER_ID"].Value.ToString() == sMaster_ID && GR.Cells["PROJECT_ID"].Value.ToString().ToUpper() == GV.sProjectID)
                            {
                                GR.CellStyles.Default.Background = b;
                                GR.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (GridRow GR in Gpanel.Rows)
                        {
                            if (GR.Cells["MASTER_ID"].Value.ToString() == sMaster_ID)
                            {
                                GR.CellStyles.Default.Background = b;
                                GR.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    b = new Background(Color.White);
                    if (sSearchOutsideColumns.Length > 0)
                    {
                        foreach (GridRow GR in Gpanel.Rows)
                        {
                            if (GR.Cells["MASTER_ID"].Value.ToString() == sMaster_ID && GR.Cells["PROJECT_ID"].Value.ToString().ToUpper() == GV.sProjectID)
                            {
                                GR.CellStyles.Default.Background = b;
                                GR.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (GridRow GR in Gpanel.Rows)
                        {
                            if (GR.Cells["MASTER_ID"].Value.ToString() == sMaster_ID)
                            {
                                GR.CellStyles.Default.Background = b;
                                GR.Visible = false;
                            }
                        }
                    }
                }                
            }
            
        }

        void Proper_Case_ColHeader(GridPanel GPanel)
        {
            foreach (GridColumn GC in GPanel.Columns)
            {
                GC.FillWeight = 100;
                GC.AutoSizeMode = ColumnAutoSizeMode.Fill;

                //GC.FilterAutoScan = true;
                //GC.FilterPopupMaxItems = 200;
                                              
                //Gpanel.GroupHeaderClickBehavior = GroupHeaderClickBehavior.
                GC.Name = GC.Name.ToUpper();
                switch (GC.Name)
                {                        
                    case "MASTER_ID":
                        GC.HeaderText = "ID";
                        break;                    
                    case "TR_CONTACT_STATUS":
                        GC.HeaderText = "TR Status";
                        break;
                    case "WR_CONTACT_STATUS":
                        GC.HeaderText = "WR Status";
                        break;
                    case "COMPANY_NAME":
                        GC.HeaderText = "Company";
                        break;
                    case "ADDRESS_1":
                        GC.HeaderText = "Add1";
                        break;
                    case "ADDRESS_2":
                        GC.HeaderText = "Add2";
                        break;
                    case "ADDRESS_3":
                        GC.HeaderText = "Add3";
                        break;
                    case "ADDRESS_4":
                        GC.HeaderText = "Add4";
                        break;
                    case "CONTACT_TELEPHONE":
                        GC.HeaderText = "Phone";
                        break;
                    case "CONTACT_EMAIL":
                        GC.HeaderText = "Email";
                        break;
                    case "PROJECT_NAME":
                        GC.HeaderText = "Project";
                        break;
                    case "PROJECT_ID":
                        GC.Visible = false;
                        break;
                    case "CONTACT_ID_P":
                        GC.Visible = false;
                        break;
                    case "POSTKEY":
                        GC.Visible = false;
                        break;
                    case "TABLEID":
                        GC.Visible = false;
                        break;

                    case "EMAIL_VERIFIED":
                        GC.Visible = false;
                        break;


                    default:
                        GC.HeaderText = GM.ProperCase_ProjectSpecific(GC.Name.Replace("_", " "));
                        break;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /////Key press listener for overall window/////
            if (keyData == Keys.Enter)
                btnSearchCompany.PerformClick();

            //ToastNotification.Show(this, keyData.ToString());

            
            switch (keyData)
            { 
                case (Keys.Alt | Keys.D1):
                    if (superTabCompany.IsSelected && chkBoxCompany_1.Visible)
                    {
                        chkBoxCompany_1.Checked = !chkBoxCompany_1.Checked;
                        if(chkBoxCompany_1.Checked)
                            txtCompany_SearchBox_1.Focus();
                    }

                    if (superTabContact.IsSelected && chkBoxContact_1.Visible)
                    {
                        chkBoxContact_1.Checked = !chkBoxContact_1.Checked;
                        if(chkBoxContact_1.Checked)
                            txtContact_SearchBox_1.Focus();
                    }
                    break;
                case (Keys.Alt | Keys.D2):
                    if (superTabCompany.IsSelected && chkBoxCompany_2.Visible)
                    {
                        chkBoxCompany_2.Checked = !chkBoxCompany_2.Checked;
                        if (chkBoxCompany_2.Checked)
                            txtCompany_SearchBox_2.Focus();
                    }

                    if (superTabContact.IsSelected && chkBoxContact_2.Visible)
                    {
                        chkBoxContact_2.Checked = !chkBoxContact_2.Checked;
                        if (chkBoxContact_2.Checked)
                            txtContact_SearchBox_2.Focus();
                    }
                    break;
                case (Keys.Alt | Keys.D3):
                    if (superTabCompany.IsSelected && chkBoxCompany_3.Visible)
                    {
                        chkBoxCompany_3.Checked = !chkBoxCompany_3.Checked;
                        if (chkBoxCompany_3.Checked)
                            txtCompany_SearchBox_3.Focus();
                    }

                    if (superTabContact.IsSelected && chkBoxContact_3.Visible)
                    {
                        chkBoxContact_3.Checked = !chkBoxContact_3.Checked;
                        if (chkBoxContact_3.Checked)
                            txtContact_SearchBox_3.Focus();
                    }
                    break;
                case (Keys.Alt | Keys.D4):
                    if (superTabCompany.IsSelected && chkBoxCompany_4.Visible)
                    {
                        chkBoxCompany_4.Checked = !chkBoxCompany_4.Checked;
                        if (chkBoxCompany_4.Checked)
                            txtCompany_SearchBox_4.Focus();
                    }

                    if (superTabContact.IsSelected && chkBoxContact_4.Visible)
                    {
                        chkBoxContact_4.Checked = !chkBoxContact_4.Checked;
                        if (chkBoxContact_4.Checked)
                            txtContact_SearchBox_4.Focus();
                    }
                    break;
                case (Keys.Alt | Keys.D5):
                    if (superTabCompany.IsSelected && chkBoxCompany_5.Visible)
                    {
                        chkBoxCompany_5.Checked = !chkBoxCompany_5.Checked;
                        if (chkBoxCompany_5.Checked)
                            txtCompany_SearchBox_5.Focus();
                    }

                    if (superTabContact.IsSelected && chkBoxContact_5.Visible)
                    {
                        chkBoxContact_5.Checked = !chkBoxContact_5.Checked;
                        if (chkBoxContact_5.Checked)
                            txtContact_SearchBox_5.Focus();
                    }
                    break;

                case (Keys.Alt | Keys.Q):
                    if (superTabCompany.IsSelected && chkBoxQuickPaf.Visible)                    
                        chkBoxQuickPaf.Checked = !chkBoxQuickPaf.Checked;                    
                    break;

                case (Keys.Alt | Keys.S):
                    if (superTabContact.IsSelected && chkBoxSoundX.Visible)
                        chkBoxSoundX.Checked = !chkBoxSoundX.Checked;                    
                    break;

                case (Keys.Alt | Keys.I):
                    if ((superTabCompany.IsSelected || superTabContact.IsSelected) && chkCompanyInclude.Visible)
                        chkCompanyInclude.Checked = !chkCompanyInclude.Checked;
                    break;

                case (Keys.Alt | Keys.M):
                    if (superTabContact.IsSelected)
                        chkBoxCompletsOnly.Checked = !chkBoxCompletsOnly.Checked;
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void chkCompanyInclude_CheckedChanged(object sender, EventArgs e)
        {
            IncludeCompany(chkCompanyInclude.Checked,sdgvSearch.PrimaryGrid);
        }

        

        private void superTabSearch_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (e.NewValue == superTabPAF)
                chkCompanyInclude.Visible = false;
            else
            {
                chkCompanyInclude.Visible = true;
                if(drCompany == null && drContact == null)
                    chkCompanyInclude.Visible = false;
            }
        }

        private void frmSearch_Shown(object sender, EventArgs e)
        {
            chkCompanyInclude.Refresh();
            if (!superTabCompany.Visible && !superTabContact.Visible && !superTabPAF.Visible)
            {
                Close();
                ToastNotification.Show(Owner, "Search not available", eToastPosition.TopRight);
            }
        }

        private void sdgvSearch_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
        {
            GridPanel Gpanel = e.GridPanel;
            Gpanel.ReadOnly = true;
            Gpanel.ShowRowHeaders = false;
            Gpanel.SelectionGranularity = SelectionGranularity.Row;
            //Gpanel.SortLevel = SortLevel.Root;
            Gpanel.RowDoubleClickBehavior = RowDoubleClickBehavior.Activate;
            
            Proper_Case_ColHeader(Gpanel);
            if (IsPAFSearchOpted)            
                Gpanel.Caption.Visible = false;            
            else
            {                
                Gpanel.ShowTreeLines = false;
                Gpanel.ExpandButtonType = ExpandButtonType.Triangle;
                
                //Gpanel.SortLevel = SortLevel.Expanded;
                //Gpanel.EnableRowFiltering = true;
                //Gpanel.EnableColumnFiltering = true;
                //Gpanel.EnableFiltering = true;            

                if (Gpanel.Caption == null)
                    Gpanel.Caption = new GridCaption();
                Gpanel.DefaultVisualStyles.CaptionStyles.Default.Alignment = Alignment.MiddleLeft;
                if(TableName == "Contact")
                    Customize_Grid(Gpanel, dsContactSearch);
                else if (TableName == "Company")
                    Customize_Grid(Gpanel, dsCompanySearch);
                
                IncludeCompany(chkCompanyInclude.Checked, Gpanel);
            }
        }

        void Customize_Grid(GridPanel Gpanel, DataSet dsSearchData)
        {
            string sTable = string.Empty;
            if (Gpanel.DataMember == null)
                sTable = ((DataSet)Gpanel.DataSource).Namespace;
            else
                sTable = Gpanel.DataMember;

            if (sTable.StartsWith(TableName + "_"))
            {
                Gpanel.Caption.Visible = true;
                switch (sTable)
                {
                    case "Contact_Contact":                        
                        Gpanel.Caption.Text = String.Format("TR Completes : <font color='OrangeRed'>{0}</font> | WR Completes : <font color='OrangeRed'>{1}</font> | Email Passed : <font color='OrangeRed'>{2}</font> | Email Bounced : <font color='OrangeRed'>{3}</font>  | Total Completes : <font color='OrangeRed'>{4}</font> | Record Count : <font color='OrangeRed'>{5}</font>",
                            dsSearchData.Tables[0].Select("TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ")").Length,
                            dsSearchData.Tables[0].Select("WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length,                            
                            dsSearchData.Tables[0].Select("TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length - dsSearchData.Tables[0].Select("(TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                            dsSearchData.Tables[0].Select("(TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                            dsSearchData.Tables[0].Select("TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length,
                            Gpanel.Rows.Count
                            );
                        break;

                    case "Contact_Company":
                        Gpanel.Caption.Text = String.Format("Company of contact : <font color=\"OrangeRed\"><i>{0}</i></font> ", ((GridRow)Gpanel.Parent)["FIRST_NAME"].Value + "&nbsp;" + ((GridRow)Gpanel.Parent)["LAST_NAME"].Value);
                        break;

                    case "Contact_AllContacts":
                        if (Gpanel.Rows.Count > 0)
                        {
                            Gpanel.GroupByRow.Visible = true;
                            string sID = ((GridRow)Gpanel.Rows[0]).Cells["MASTER_ID"].Value.ToString();
                            Gpanel.Caption.Text = String.Format("Showing all contacts of :  <font color=\"OrangeRed\"><i>{0}</i></font>", ((GridRow)Gpanel.Parent)["COMPANY_NAME"].Value) + "   |   " + String.Format("TR Completes : <font color='OrangeRed'>{0}</font> | WR Completes : <font color='OrangeRed'>{1}</font> | Email Passed : <font color='OrangeRed'>{2}</font> | Email Bounced : <font color='OrangeRed'>{3}</font>  | Total Completes : <font color='OrangeRed'>{4}</font> | Record Count : <font color='OrangeRed'>{5}</font>",
                                dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ")").Length,
                                dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length,                                
                                dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))").Length - dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                                dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                                dsSearchData.Tables[2].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))").Length,
                                Gpanel.Rows.Count);
                        }

                        //if (Gpanel.Footer == null)
                        //    Gpanel.Footer = new GridFooter();
                        //Gpanel.Footer.Text = String.Format("Total Contacts : <font color=\"Green\"><i>{0}</i></font>", Gpanel.Rows.Count);
                        
                        break;

                    case "Company_Company":
                        Gpanel.ShowRowHeaders = false;
                        Gpanel.Caption.Text = String.Format("Total : {0} records(s)", Gpanel.Rows.Count);
                        break;

                    case "Company_Contact":
                        if (Gpanel.Rows.Count > 0)
                        {
                            Gpanel.GroupByRow.Visible = true;
                            string sID = ((GridRow)Gpanel.Rows[0]).Cells["MASTER_ID"].Value.ToString();
                            Gpanel.Caption.Text = String.Format("Showing all contacts of :  <font color=\"OrangeRed\"><i>{0}</i></font> ", ((GridRow)Gpanel.Parent)["COMPANY_NAME"].Value) + "  |   " + String.Format("TR Completes : <font color='OrangeRed'>{0}</font> | WR Completes : <font color='OrangeRed'>{1}</font> | Email Passed : <font color='OrangeRed'>{2}</font> | Email Bounced : <font color='OrangeRed'>{3}</font>  | Total Completes : <font color='OrangeRed'>{4}</font> | Record Count : <font color='OrangeRed'>{5}</font>",
                                    //dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ")").Length,
                                    //dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length,
                                    //dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))").Length, 
                                    //Gpanel.Rows.Count);
                                    dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ")").Length,
                                    dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")").Length,                                
                                    dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))").Length - dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                                    dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + ")) AND EMAIL_VERIFIED IN ('BOUNCED','NOT VERIFIED')").Length,
                                    dsSearchData.Tables[1].Select("MASTER_ID = " + sID + " AND (TR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstTRContactStatusToBeValidated, "String") + ") OR WR_CONTACT_STATUS IN (" + GM.ListToQueryString(GV.lstWRContactStatusToBeValidated, "String") + "))").Length,
                                    Gpanel.Rows.Count);
                            //if (Gpanel.Footer == null)
                            //    Gpanel.Footer = new GridFooter();
                            //Gpanel.Footer.Text = String.Format("Total Contacts : <font color=\"Green\"><i>{0}</i></font>", Gpanel.Rows.Count);
                        }
                        break;
                }
            }                            
        }

        private void sdgvSearch_ColumnGrouped(object sender, GridColumnGroupedEventArgs e)
        {            
            GridGroup GG = e.GridGroup;            
            GG.Text = GG.Text + " : (" + GG.Rows.Count + ")";
        }    

        private void sdgvSearch_RowDoubleClick(object sender, GridRowDoubleClickEventArgs e)
        {
            GridRow GRow = (GridRow)e.GridRow;
            GridPanel GPanel = e.GridPanel;

            if (sdgvSearch.ActiveGrid.DataMember != GPanel.DataMember)
                return;
 
            if (SearchTriggeredFrom == "External")
            {
                if (GPanel.Columns.Contains("MASTER_ID"))
                {
                    string sID = GRow["MASTER_ID"].Value.ToString();
                    if (sID.Length > 0)
                        GM.OpenContactUpdate(sID, false, true, this, null);
                }
            }
            else
            {
                
                drReturnRow = null;
                if (GPanel.Columns.Contains("MASTER_ID"))//To differetiate from PAF search. Database search must contain MasterID
                {
                    string sCondition = string.Empty;
                    string sProjectIDToSearch = string.Empty;
                    bool dbFetchNeeded = false;
                    bool AllowPopulate = false;
                    List<string> lstPopulateColumns = new List<string>();

                    if (GPanel.Columns.Contains("PROJECT_ID"))
                        sProjectIDToSearch = GRow["PROJECT_ID"].Value.ToString();
                    else
                        sProjectIDToSearch = GV.sProjectID;

                    if (GPanel.Columns.Contains("CONTACT_ID_P"))                    
                        sCondition = sProjectIDToSearch + "_Mastercontacts WHERE CONTACT_ID_P = " + GRow["CONTACT_ID_P"].Value;                    
                    else                    
                        sCondition = sProjectIDToSearch + "_MasterCompanies WHERE MASTER_ID = " + GRow["MASTER_ID"].Value;                    

                    if (TableToReturn == "Contact" && GV.Contact_AllowPopulateFromSearch && lstContactColumnsToPopulate.Count > 0)
                    {
                        AllowPopulate = true;
                        lstPopulateColumns = lstContactColumnsToPopulate;
                        foreach (string sColumns in lstContactColumnsToPopulate)
                        {
                            if (!GPanel.Columns.Contains(sColumns.ToUpper()))
                            {
                                dbFetchNeeded = true;
                                break;
                            }
                        }
                    }
                    else if (TableToReturn == "Company" && GV.Company_AllowPopulateFromSearch && lstCompanyColumnsToPopulate.Count > 0)
                    {
                        AllowPopulate = true;
                        lstPopulateColumns = lstCompanyColumnsToPopulate;
                        foreach (string sColumns in lstCompanyColumnsToPopulate)
                        {
                            if (!GPanel.Columns.Contains(sColumns.ToUpper()))
                            {
                                dbFetchNeeded = true;
                                break;
                            }
                        }
                    }

                    if (AllowPopulate)
                    {
                        if (dbFetchNeeded)
                        {
                            DataTable dtTableToReturn;
                            dtTableToReturn = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT * FROM " + sCondition);                            
                            foreach (string sCol in lstPopulateColumns)
                            {
                                bool ColumnsMatch = false;
                                foreach (DataColumn DC in dtTableToReturn.Columns)
                                {
                                    if (sCol.ToUpper() == DC.ColumnName.ToUpper())
                                        ColumnsMatch = true;
                                }

                                if (!ColumnsMatch)
                                {
                                    drReturnRow = null;
                                    return;
                                }
                            }
                            //dtTableToReturn = GV.MYSQL.BAL_ExecuteQueryMySQL("SELECT " + GM.ListToQueryString(lstPopulateColumns, "Int") + " FROM " + sCondition);
                            if (dtTableToReturn.Rows.Count > 0)
                                drReturnRow = dtTableToReturn.Rows[0];
                            else
                                drReturnRow = null;
                        }
                        else
                            drReturnRow = ((System.Data.DataRowView)(GRow.DataItem)).Row;
                    }
                }
                else if (GPanel.Columns.Contains("POSTKEY") && lstPAFColumns.Count > 0 && TableName == "PAF" && GV.AllowPopulateFromPAFSearch)
                {
                    DataTable dtPAFData = (DataTable)sdgvSearch.PrimaryGrid.DataSource;
                    if (dtPAFData.Columns.Contains("PostKey") && dtPAFData.Rows[e.GridRow.Index]["PostKey"].ToString().Length > 0)
                    {
                        DataRow[] drrPAFColumnsTobePopulated = null;

                        if (SearchTriggeredFrom == "Company")
                            drrPAFColumnsTobePopulated = dtFieldMasterCompany.Select("LEN(TRIM(PAF_COLUMN)) > 0");
                        else
                            drrPAFColumnsTobePopulated = dtFieldMasterContact.Select("LEN(TRIM(PAF_COLUMN)) > 0");

                        if (drrPAFColumnsTobePopulated.Length > 0)
                        {
                            DataTable dtPAFDetailedAddress = new DataTable("Address");
                            foreach (DataRow drCols in drrPAFColumnsTobePopulated)
                                dtPAFDetailedAddress.Columns.Add(drCols["PAF_COLUMN"].ToString());

                            if (!dtPAFDetailedAddress.Columns.Contains("Postcode"))// Postcode column required
                                dtPAFDetailedAddress.Columns.Add("Postcode");

                            XDocument xmlDoc = XDocument.Load("http://172.27.137.182:81/addresslookup.pce?postkey=" + dtPAFData.Rows[e.GridRow.Index]["PostKey"].ToString().Replace(" ", "%20"));
                            MemoryStream sXMLStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlDoc.Root.ToString() ?? ""));
                            dtPAFDetailedAddress.ReadXml(sXMLStream);

                            if (!dtPAFDetailedAddress.Rows[0]["Postcode"].ToString().Contains("Error:"))
                            {
                                foreach (DataRow drCols in drrPAFColumnsTobePopulated)
                                    dtPAFDetailedAddress.Columns[drCols["PAF_COLUMN"].ToString()].ColumnName = drCols["FIELD_NAME_TABLE"].ToString();
                                drReturnRow = dtPAFDetailedAddress.Rows[0];
                            }
                        }
                    }
                }

                if (drReturnRow != null)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
            }
        }

        private void chkBoxCompletsOnly_CheckedChanged(object sender, EventArgs e)
        {
            GV.SearchOnlyCompletedContacts = chkBoxCompletsOnly.Checked;
        }

        private void frmSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Query != null)
            //{
            //    Query.CancelQuery();
            //    Query.Dispose();
            //    Query = null;
            //}
            Cancel_Query = true;
            ResetTables();
            dsCompanySearch.Relations.Clear();
            dsContactSearch.Relations.Clear();
            dsCompanySearch.Tables.Clear();
            dsContactSearch.Tables.Clear();
            dsCompanySearch.Dispose();
            dsContactSearch.Dispose();
            dsCompanySearch = null;
            dsContactSearch = null;            
            GC.Collect();
        }

        private void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Controls.CheckBoxX chk = sender as DevComponents.DotNetBar.Controls.CheckBoxX;                           
            Properties.Settings.Default[chk.Name] = chk.Checked;
            Properties.Settings.Default.Save();            
        }
        


        //private void chkBoxCompany_1_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        
    }
}


