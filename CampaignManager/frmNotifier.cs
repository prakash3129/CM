using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmNotifier : Office2007Form
    {
        public frmNotifier()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;            
            circularProgressValidating.IsRunning = true;
        }        

        private void msgRefresh_Tick(object sender, EventArgs e)
        {
            lblValidationMessage.Text = "Current Operation :" + GV.sValidationMessage;

            for (int i = 0; i < 7; i++)
            {
                switch (i)
                { 
                        // 1 - Completed Sucessfully
                        // 2 - Error and not completed yet
                        // 3 - Completed with Error

                    case 0://0 Company Validation
                        if (GV.iNotifier[0] == 2)
                            pBoxPoint0.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[0] == 1 || GV.iNotifier[0] == 3)
                            pBoxPoint0.Image = Properties.Resources.Exclamation_mark_icon;

                        if ((GV.iNotifier[0] == 0 || GV.iNotifier[0] == 1) && GV.iNotifier[1] == 0)
                            circularProgressValidating.Location = new Point(210, 16);
                        break;

                    case 1://1 SwitchboardDuplicate
                        if (GV.iNotifier[1] == 2)
                            pBoxPoint1.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[1] == 1 || GV.iNotifier[1] == 3)
                            pBoxPoint1.Image = Properties.Resources.Exclamation_mark_icon;

                        if (GV.iNotifier[2] == 0 && GV.iNotifier[0] > 1)
                            circularProgressValidating.Location = new Point(216, 43);
                        break;

                    case 2://2 Contact Validation
                        if (GV.iNotifier[2] == 2)
                            pBoxPoint2.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[2] == 1 || GV.iNotifier[2] == 3)
                            pBoxPoint2.Image = Properties.Resources.Exclamation_mark_icon;
                        
                        if (GV.iNotifier[3] == 0 && GV.iNotifier[1] > 1)
                            circularProgressValidating.Location = new Point(201, 70);
                        break;

                    case 3://3 Email Duplicate
                        if (GV.iNotifier[3] == 2)
                        {
                            pBoxPoint3.Image = Properties.Resources.ok_icon_Small;
                            btnCancel.Visible = false;
                        }
                        else if (GV.iNotifier[3] == 1 || GV.iNotifier[3] == 3)
                        {
                            pBoxPoint3.Image = Properties.Resources.Exclamation_mark_icon;
                            btnCancel.Visible = false;
                        }

                        if (GV.iNotifier[4] == 0 && GV.iNotifier[2] > 1)
                            circularProgressValidating.Location = new Point(181, 97);
                        break;

                    case 4://4 Tag Date
                        if (GV.iNotifier[4] == 2)
                            pBoxPoint4.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[4] == 1 || GV.iNotifier[4] == 3)
                            pBoxPoint4.Image = Properties.Resources.Exclamation_mark_icon;

                        if (GV.iNotifier[5] == 0 && GV.iNotifier[3] > 1)
                            circularProgressValidating.Location = new Point(174, 124);
                        break;

                    case 5://5 Logging
                        if (GV.iNotifier[5] == 2)
                            pBoxPoint5.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[5] == 1 || GV.iNotifier[5] == 3)
                            pBoxPoint5.Image = Properties.Resources.Exclamation_mark_icon;

                        if (GV.iNotifier[6] == 0 && GV.iNotifier[4] > 1)
                            circularProgressValidating.Location = new Point(230, 150);
                        break;

                    case 6://6 Save to DB
                        if (GV.iNotifier[6] == 2)
                            pBoxPoint6.Image = Properties.Resources.ok_icon_Small;
                        else if (GV.iNotifier[6] == 1 || GV.iNotifier[6] == 3)
                            pBoxPoint6.Image = Properties.Resources.Exclamation_mark_icon;

                        if (GV.iNotifier[5] > 1)
                            circularProgressValidating.Location = new Point(109, 178);
                        break;
                }
            }            
        }

        public void ReloadScreen()
        {
            msgRefresh.Start();
            lblValidationMessage.Text = string.Empty;
            pBoxPoint0.Image = pBoxPoint1.Image = pBoxPoint2.Image = pBoxPoint3.Image = pBoxPoint4.Image = pBoxPoint5.Image = pBoxPoint6.Image = Properties.Resources.loading_blue2;
            GV.iNotifier[0] = GV.iNotifier[1] = GV.iNotifier[2] = GV.iNotifier[3] = GV.iNotifier[4] = GV.iNotifier[5] = GV.iNotifier[6] = 0;
            circularProgressValidating.Location = new Point(210, 16);
            btnCancel.Visible = true;
        }

        //private void frmNotifier_Deactivate(object sender, EventArgs e)
        //{
        //    this.Owner.Activate();
        //    this.Owner.Enabled = true;
        //}

        //private void frmNotifier_Activated(object sender, EventArgs e)
        //{
        //    this.Owner.Enabled = false;
        //}
    }
}
