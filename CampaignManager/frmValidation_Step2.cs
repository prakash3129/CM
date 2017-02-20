using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class frmValidation_Step2 : Office2007Form
    {
        public frmValidation_Step2()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);//Gets the icon for current window            
            ToastNotification.DefaultToastGlowColor = eToastGlowColor.None;
            ToastNotification.DefaultTimeoutInterval = 2000;
            ToastNotification.ToastFont = new Font(this.Font.FontFamily, 22);
        }
    }
}
