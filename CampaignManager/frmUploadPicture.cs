using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Drawing.Drawing2D;

namespace GCC
{
    public partial class frmUploadPicture : Office2007Form
    {
        public frmUploadPicture()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //Gets the icon for current window
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
            MessageBoxEx.EnableGlass = false;
        }

        private Image Img;
        private Image ImgBackup;
        private Size OriginalImageSize;
        private Size ModifiedImageSize;

        int iCropX;
        int iCropY;
        int iCropWidth;
        int iCropHeight;

        int oCropX;
        int oCropY;
        public Pen cropPen;
        bool IsImageLoading = true;

        //BAL_Global objBAL_Global = new BAL_Global();
        private void frmUploadPicture_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            OpenFileDialog objOpenFileDialog = new OpenFileDialog();
            if (objOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Img = Image.FromFile(objOpenFileDialog.FileName);
                ImgBackup = Img;
                OriginalImageSize = new Size(Img.Width, Img.Height);
                pictureDP.Image = Img;
                pictureDP.SizeMode = PictureBoxSizeMode.Zoom;
                IsImageLoading = true;    
               // btnReset.PerformClick();
            }
            else
                this.Close();

            //dtp.KeyDown += new KeyEventHandler(dtp_KeyDown);
            pictureDP.MouseUp += new MouseEventHandler(pictureDP_MouseUp);
            pictureDP.MouseDown += new MouseEventHandler(pictureDP_MouseDown);
            pictureDP.MouseEnter += new EventHandler(pictureDP_MouseEnter);
            pictureDP.MouseLeave += new EventHandler(pictureDP_MouseLeave);
            pictureDP.MouseMove += new MouseEventHandler(pictureDP_MouseMove);
            
        }

        private void pictureDP_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    IsImageLoading = false;
                    iCropX = e.X;
                    iCropY = e.Y;
                    cropPen = new Pen(Color.Black, 1);
                    cropPen.DashStyle = DashStyle.DashDot;
                }
                pictureDP.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void pictureDP_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void pictureDP_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void pictureDP_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (pictureDP.Image == null || IsImageLoading)
                    return;

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureDP.Refresh();
                    iCropWidth = e.X - iCropX;
                    iCropHeight = e.Y - iCropY;
                    pictureDP.CreateGraphics().DrawRectangle(cropPen, iCropX, iCropY, iCropWidth, iCropHeight);
                }
            }
            catch (Exception ex)
            {
                //if (ex.Number == 5)
                //    return;
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            pictureDP.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureDP.Refresh();
        }

        private void pictureDP_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (iCropWidth < 1 || IsImageLoading)
                {
                    return;
                }
                Rectangle rect = new Rectangle(iCropX, iCropY, iCropWidth, iCropHeight);
                //First we define a rectangle with the help of already calculated points
                Bitmap OriginalImage = new Bitmap(pictureDP.Image, pictureDP.Width, pictureDP.Height);
                //Original image
                Bitmap _img = new Bitmap(iCropWidth, iCropHeight);
                // for cropinf image
                Graphics g = Graphics.FromImage(_img);
                // create graphics
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                //set image attributes
                g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);

                //pictureDP.Image = _img;
                //pictureDP.Width = _img.Width;
                //pictureDP.Height = _img.Height;

                pictureBoxPreview.Image = _img;

                iCropWidth = 0;
                iCropHeight = 0;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Img = ImgBackup;
            OriginalImageSize = new Size(Img.Width, Img.Height);
            pictureDP.Image = Img;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            if (pictureBoxPreview.Image == null)
                pictureDP.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            else
                pictureBoxPreview.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bStream = ms.GetBuffer();

            if (GV.sEmployeeNo.Length > 0)
            {
                DataTable dtImage = GV.MSSQL1.BAL_FetchTable("RM..EmployeeImage", "EmployeeID = '" + GV.sEmployeeNo + "'");
                string sSQLText  =string.Empty;
                if (dtImage.Rows.Count > 0)
                    sSQLText = "UPDATE RM..EmployeeImage SET EmployeeImage = @Binary WHERE EmployeeID='" + GV.sEmployeeNo + "'";
                else
                    sSQLText = "INSERT INTO RM..EmployeeImage( EmployeeID,EmployeeImage ) VALUES( '" + GV.sEmployeeNo + "',@Binary )";

                if (DialogResult.Yes == MessageBoxEx.Show("Are you sure to update this Image ?", "Campaign Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    SqlConnection connection = new SqlConnection(GV.sMSSQL1);
                    SqlCommand command = new SqlCommand(sSQLText, connection);
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    command.Parameters.AddWithValue("@Binary", bStream);
                    command.ExecuteNonQuery();
                    connection.Close();
                    GV.imgEmployeeImage = Image.FromStream(new System.IO.MemoryStream(bStream));
                    ToastNotification.Show(this, "Image updated sucessfully.", eToastPosition.TopRight);
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
