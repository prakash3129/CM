using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using System.Collections.Generic;

using System.Linq;

namespace GCC
{
    public partial class frmCallLog : Office2007Form
    {
        public frmCallLog()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }

        DataTable _dtCallLog = null;
        public DataTable dtCallLog
        {
            get { return _dtCallLog; }
            set { _dtCallLog = value; }
        }

        string _sCompany_Name = string.Empty;
        public string sCompany_Name
        {
            get { return _sCompany_Name; }
            set { _sCompany_Name = value; }
        }

        string _sSwitchboard = string.Empty;
        public string sSwitchboard
        {
            get { return _sSwitchboard; }
            set { _sSwitchboard = value; }
        }

        private void frmCallLog_Load(object sender, EventArgs e)
        {

            if (GV.sDialerType == "Vortex")
            {

                this.Text = "Audio Configuration | Call Log";
                splitContainerAudioCallLog.Panel1Collapsed = false;

                if (GV.VorteX.IsConnected)
                {
                    splitContainerAudioCallLog.Panel1.Enabled = true;
                    string sAudioConfig = GV.VorteX.GetAudioConfig();
                    if (sAudioConfig.Length > 0)
                    {
                        List<string> lstConfigs = sAudioConfig.Split('|').ToList();
                        if (lstConfigs.Count == 5)
                        {
                            chkNoiseCancellation.Checked = lstConfigs[0].EndsWith("1");
                            chkEchoCancelation.Checked = lstConfigs[1].EndsWith("1");
                            chkAutoGainControl.Checked = lstConfigs[2].EndsWith("1");
                            knobMicVolume.Value = Convert.ToDecimal(lstConfigs[3].Split(':')[1]);
                            knobPhoneVolume.Value = Convert.ToDecimal(lstConfigs[4].Split(':')[1]);
                        }
                    }
                }
                else
                {
                    splitContainerAudioCallLog.Panel1.Enabled = false;
                }
            }
            else
            {
                this.Text = "Call Log";
                splitContainerAudioCallLog.Panel1Collapsed = true;
            }

            lblInformation.Text = "  Call Log  |  Company : " + sCompany_Name + "  |  Total Dial(s) : " + dtCallLog.Select("RecordingID = '0'").Length;
            if (dtCallLog.Select("RecordingID = '1'").Length > 0)
            {
                sdgvCallLog.PrimaryGrid.DataSource = dtCallLog.Select("RecordingID = '1'").CopyToDataTable();
            }
        }

        private void sdgvCallLog_DataBindingComplete(object sender, DevComponents.DotNetBar.SuperGrid.GridDataBindingCompleteEventArgs e)
{
            try
            {
                GridPanel GP = e.GridPanel;
                foreach (GridColumn GC in GP.Columns)
                {

                    GC.RenderType = typeof(GridLabelXEditControl);
                    switch (GC.Name.ToUpper())
                    {
                        case "LOGINID":
                            GC.HeaderText = "Agent";
                            break;
                        case "STATIONID":
                            GC.HeaderText = "Ext";
                            break;
                        case "DURATION":
                            GC.HeaderText = "Duration";
                            break;
                        case "DATETIMESTAMP":
                            GC.HeaderText = "Date";
                            break;
                        case "TELEPHONENUMBER":
                            GC.HeaderText = "Number";
                            break;
                        default:
                            GC.Visible = false;
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                GM.Error_Log(System.Reflection.MethodBase.GetCurrentMethod(),  ex, true, true);
            }
        }
       

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sdgvCallLog_RowDoubleClick(object sender, GridRowDoubleClickEventArgs e)
        {
            Clipboard.SetText(((GridRow)e.GridRow).Cells[4].Value.ToString());
            ToastNotification.Show(this, "Number :" + ((GridRow)e.GridRow).Cells[4].Value + " is copied");
        }

        private void knobPhoneVolume_ValueChanged(object sender, DevComponents.Instrumentation.ValueChangedEventArgs e)
        {
            if (GV.VorteX.IsConnected)
            {
                if (GV.VorteX != null)
                {
                    GV.VorteX.SetSpeakerVolume(Convert.ToInt32(e.NewValue));
                }
            }
        }

        private void knobMicVolume_ValueChanged(object sender, DevComponents.Instrumentation.ValueChangedEventArgs e)
        {
            if (GV.VorteX.IsConnected)
            {
                if (GV.VorteX != null)
                {
                    GV.VorteX.SetMicVolume(Convert.ToInt32(e.NewValue));
                }
            }
        }
    }
}
