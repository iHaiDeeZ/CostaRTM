using ps4Extreme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4Extreme
{
    public partial class ps4ipform : DevExpress.XtraEditors.XtraForm
    {
        IniFile ini = new IniFile(Application.StartupPath + @"\config.ini");
        public ps4ipform()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
        }

        private void ps4ipform_Load(object sender, EventArgs e)
        {
            ps4iptextBox.Text = ini.IniReadValue("ps4", "Host");
        }

        private void setip_Click(object sender, EventArgs e)
        {
            try
            {
                if (ps4iptextBox.Text == "")
                {
                    MessageBox.Show("Please Enter a Vaild IP!");
                }
                else
                {
                    ini.IniWriteValue("ps4", "Host", ps4iptextBox.Text);
                    MessageBox.Show("IP Changed To: " + ps4iptextBox.Text);
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Failed to Change IP");
            }
        }
    }
}
