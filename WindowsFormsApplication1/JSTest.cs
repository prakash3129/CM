using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Windows.Forms;
//using Newtonsoft.Json;
using DevComponents.DotNetBar;

namespace GCC
{
    public partial class JSTest : Form
    {
        public JSTest()
        {
            InitializeComponent();
        }
        DataTable table = new DataTable();
        Button b = new Button();
        private void Form2_Load(object sender, EventArgs e)
        {
            //b.Location = new Point(200, 200);
            //b.Size = new Size(50, 50);
            //b.Text = "Click";
            //this.Controls.Add(b);
            //b.Click += new EventHandler(DynamicbuttonClick);
            //b.MouseLeave += new EventHandler(DynamicbuttonMouse);
            cmbOfficeType.Text = "RQ";
                       
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            //
            // Here we add five DataRows.
            //
            table.Rows.Add(25, "Indocin", "David", GM.GetDateTime());
            table.Rows.Add(50, "Enebrel", "Sam", GM.GetDateTime());
            table.Rows.Add(10, "Hydralazine", "Christoff", GM.GetDateTime());
            table.Rows.Add(21, "Combivent", "Janet", GM.GetDateTime());
            table.Rows.Add(100, "Dilantin", "Melanie", GM.GetDateTime());
        }
        private void DynamicbuttonClick(object sender, EventArgs e)
        {
            MessageBoxEx.Show("Button Clicked");
        }
        private void DynamicbuttonMouse(object sender, EventArgs e)
        {
           // MessageBoxEx.Show("Mouse Leave");
        }
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            string sErrormsg = e.Message;
            MessageBoxEx.Show(sErrormsg);
            //sErrormsg = sErrormsg.Remove(sErrormsg.IndexOf("element is invalid")).Replace("The","").Trim();
            //MessageBoxEx.Show("The "+sErrormsg+" Field is invalid");
        }

        private void validate()
        {
            //WebBrowser brwsValidate = new WebBrowser();
            //brwsValidate.Navigate(txtJS.Text);

            XDocument xmlDoc = XDocument.Load(txtJS.Text);
            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(xmlDoc.Root.ToString() ?? ""));
            //DataTable dtXML = new DataTable();
            //dtXML.TableName = "Item";
            //dtXML.Columns.Add("Postcode");
            //dtXML.Columns.Add("PostcodeFrom");
            //dtXML.Columns.Add("Key");
            //dtXML.Columns.Add("List");
            //dtXML.Columns.Add("CountryISO");



            DataTable dtXML = new DataTable();
            dtXML.TableName = "AddressListItem";
            dtXML.Columns.Add("Address");
            dtXML.Columns.Add("PostKey");
            
            dtXML.ReadXml(s);

            //dataGridView1.DataSource = dtXML;




        }

        public DataTable XElementToDataTable(XElement x)
        {
            DataTable dtable = new DataTable();

            XElement setup = (from p in x.Descendants() select p).First();
            // build your DataTable
            foreach (XElement xe in setup.Descendants())
                dtable.Columns.Add(new DataColumn(xe.Name.ToString(), typeof(string))); // add columns to your dt

            var all = from p in x.Descendants(setup.Name.ToString()) select p;
            foreach (XElement xe in all)
            {
                DataRow dr = dtable.NewRow();
                foreach (XElement xe2 in xe.Descendants())
                    dr[xe2.Name.ToString()] = xe2.Value; //add in the values
                dtable.Rows.Add(dr);
            }
            return dtable;
        }


        private void btnValidate_Click(object sender, EventArgs e)
        {


            validate();





            return;
         //   string json = JsonConvert.SerializeObject(table);
            System.IO.StreamWriter file = new System.IO.StreamWriter("D:\\SVN\\Script Test\\kolandai.txt");
            //file.WriteLine(json);
            file.Close();
            string sName = TxtName.Text, sPhone = txtPhone.Text, sEmail = txtEmail.Text, sOfficeType = cmbOfficeType.Text, sNoEmp = txtNoEmp.Text, sBranch = "";
            if (swtchBranch.Value)
                sBranch = "Yes";
            else
                sBranch = "No";

            txtJS.Text = String.Format("Name={0}&Phone={1}&Email={2}&OfficeType={3}&Branch={4}&NoEmp={5}", sName, sPhone, sEmail, sOfficeType, sBranch, sNoEmp);
            WebBrowser brwsValidate = new WebBrowser();
            brwsValidate.Navigate(@"D:\SVN\Script Test\Validate.HTML?" + txtJS.Text);
            while (brwsValidate.Document.Body == null || brwsValidate.Document.Body.InnerText == null)
            {
                Application.DoEvents();
            }

            //txtmsg.Text = "1.Inner Text: "+brwsValidate.Document.Body.InnerText;
            brwsValidate.Dispose();
            #region MyRegion
            //MessageBoxEx.Show(brwsValidate.Document.);

            //    DataTable dtEmail = new DataTable();
            //    dtEmail.TableName = "Validation";
            //    dtEmail.Columns.Add("Email", typeof(string));
            //    dtEmail.Columns.Add("Phone", typeof(string));
            //    dtEmail.Columns.Add("Name", typeof(string));
            //    dtEmail.Columns.Add("OfficeType", typeof(string));
            //    dtEmail.Columns.Add("Branch", typeof(string));
            //    dtEmail.Columns.Add("NoEmp", typeof(int));

            //    string branch = "";

            //    if (swtchRQ.Value)
            //        branch = "Yes";
            //    else
            //        branch = "No";

            //    dtEmail.Rows.Add(txtEmail.Text, txtPhone.Text, TxtName.Text, cmbOfficeType.Text, branch, txtNoEmp.Value);

            //    //dtEmail.Rows.Add("Email", txtEmail.Text);
            //    //dtEmail.Rows.Add("Phone", txtPhone.Text);
            //    //dtEmail.Rows.Add("Name", TxtName.Text);
            //    dtEmail.WriteXml(@"D:\SVN\XML Test\FirstXML.xml");
            //    //dtEmail.WriteXmlSchema(@"D:\SVN\XML Test\FirstXMLSChema2.XSD");

            //    //Stream sStream = new Stream();
            //    //dtEmail.WriteXml(sStream);

            //    //XmlDocument XDC = new XmlDocument();
            //    //XDC.Load(@"D:\SVN\XML Test\FirstXML.xml");
            //    //XDC.Schemas.Add(null, @"D:\SVN\XML Test\FirstXMLSChema1.XSD");
            //    //XDC.Validate(ValidationCallBack);


            //    //SqlConnection con = new SqlConnection("Data Source=172.27.137.181;Initial Catalog=Merit_Validation_Tool;Persist Security Info=True;User ID=sa;Password='MeritGroup123'");
            //    //con.Open();
            //    //SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Personal_email_validation",con);
            //    //SqlDataAdapter da= new SqlDataAdapter(cmd);
            //    //da.Fill(dtTags);
            //    //dtTags.WriteXmlSchema(@"D:\SVN\XML Test\FirstSchema.xsd"); 
            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // lblTime.Text = GM.GetDateTime().ToString(); ;
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {

        }
        
    }
}
