using DevExpress.XtraBars.Docking2010;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PS4ME;

namespace PS4Extreme
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        PS4ME.PS4ME PS4 = new PS4ME.PS4ME();
        PS4Extreme.IniFile ini = new PS4Extreme.IniFile(Application.StartupPath + @"\config.ini");
        private readonly CheckForUpdate checkForUpdate = null;

        public Form1()
        {
            string version = Application.ProductVersion;
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            this.checkForUpdate = new CheckForUpdate(this);

            jumpheight.Minimum = 0;
            jumpheight.Maximum = 20;

        }

        private void connectps4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            {
                try
                {
                    if (ini.IniReadValue("ps4", "Host") == "")
                    {
                        MessageBox.Show("Enter a valid PS4 IP in the settings!");
                        ps4ipform frm = new ps4ipform();
                        frm.Show();
                    }
                    else
                    {
                        PS4.Connect(ini.IniReadValue("ps4", "Host"));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = System.Environment.MachineName;
        }

        private void ps4ip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ps4ipform frm = new ps4ipform();
            frm.Show();
        }

        private void attachps4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (ini.IniReadValue("ps4", "Host") == "")
                {
                    MessageBox.Show("Enter a valid PS4 IP in the settings!");
                    ps4ipform frm = new ps4ipform();
                    frm.Show();
                }
                else
                {
                    PS4.AttachProcess("default_mp.elf");
                    MessageBox.Show("Attached Process");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void disconnectps4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (ini.IniReadValue("ps4", "Host") == "")
                {
                    MessageBox.Show("Enter a valid PS4 IP in the settings!");
                    ps4ipform frm = new ps4ipform();
                    frm.Show();
                }
                else
                {
                    PS4.Disconnect();
                    MessageBox.Show("PS4 Successfully Disconnected");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void payloadps4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PS4.SendPayload(ini.IniReadValue("ps4", "Host"), "PS4ME.bin");
        }

        private void Awclientsget_Click(object sender, EventArgs e)
        {
            string[] processList = PS4.getProcesses();

            dataGridView1.Rows.Clear();
            for (UInt32 i = 0; i < processList.Length; i++)
            {
                if (processList[i] == null)
                    continue;

                dataGridView1.Rows.Add(processList[i], i);
            }

        }

        private void ghostsnamechangerbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ini.IniReadValue("ghostsoffsets", "inmenunamechanger") == "")
                {

                    MessageBox.Show("Enter a valid ghosts namechanger offset in the changer!");
                }
                else
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(ghostsinmenutext.Text);
                    Array.Resize<byte>(ref bytes, bytes.Length + 1);
                    PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "inmenunamechanger"), "byte");
                }
            }
            catch
            {
                int num = (int)MessageBox.Show("Failed to set In-Menu name");
            }
        }
        private void ghostsuavcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostsuavcheck.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "uav"), "0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "uav"), "0x00");
            }
        }

        private void unlimitedammoghosts_CheckedChanged(object sender, EventArgs e)
        {
            if (unlimitedammoghosts.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "infinitieammoprimary"), "0xFF, 0xFF, 0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "infinitieammoprimary"), "0x00, 0x00, 0x10");
            }
        }

        private void aimbotghosts_CheckedChanged(object sender, EventArgs e)
        {
            if (aimbotghosts.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "aimbot"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "aimbot"), "0x00");
            }
        }

        private void fpsghosts_CheckedChanged(object sender, EventArgs e)
        {
            if (aimbotghosts.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "fpsghosts"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "fpsghosts"), "0x00");
            }
        }

        private void offsetschanger_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process.Start("notepad.exe", "config.ini");
        }

        private void NetworkSniffer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon!");
        }

        private void ps4cusnotify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Coming Soon!");
        }

        private void ghostsgetclients_Click(object sender, EventArgs e)
        {
            string[] processList = PS4.getProcesses();

            dataGridView2.Rows.Clear();
            for (UInt32 i = 0; i < processList.Length; i++)
            {
                if (processList[i] == null)
                    continue;

                dataGridView2.Rows.Add(processList[i], i);
            }
        }

        private void awfps_CheckedChanged(object sender, EventArgs e)
        {
            if (awfps.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "FPS"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "FPS"), "0x00");
            }
        }

        private void awgodmode_CheckedChanged(object sender, EventArgs e)
        {
            if (awgodmode.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "godmode"), "0xFF, 0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "godmode"), "0x64, 0x00");
            }
        }

        private void awblood_CheckedChanged(object sender, EventArgs e)
        {
            if (awblood.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "blood"), "0xFF, 0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "blood"), "0x64, 0x00");
            }
        }

        private void awlaser_CheckedChanged(object sender, EventArgs e)
        {
            if (awlaser.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "laser"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "laser"), "0x00");
            }
        }

        private void awradar_CheckedChanged(object sender, EventArgs e)
        {
            if (awradar.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "radar"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "radar"), "0x00");
            }
        }

        private void awinvisible_CheckedChanged(object sender, EventArgs e)
        {
            if (awinvisible.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "invisible"), "0x11");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "invisible"), "0x00");
            }
        }

        private void awredboxes_CheckedChanged(object sender, EventArgs e)
        {
            if (awredboxes.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "redbox"), "0x10");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "redbox"), "0x00");
            }
        }

        private void awthirdperson_CheckedChanged(object sender, EventArgs e)
        {
            if (awthirdperson.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "thirdperson"), "0x02");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "thirdperson"), "0x00");
            }
        }

        private void awnoclip_CheckedChanged(object sender, EventArgs e)
        {
            if (awnoclip.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "noclip"), "0x02");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "noclip"), "0x00");
            }
        }

        private void awspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (awspeed.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "speed"), "0xC3, 0xF5, 0x88, 0x40");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "speed"), "0xC3, 0xF5, 0x88, 0x3F");
            }
        }

        private void awfreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (awfreeze.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "freeze"), "0xC3, 0xF5, 0x88, 0x40");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "freeze"), "0xC3, 0xF5, 0x88, 0x3F");
            }

        }

        private void awfreezecontrol_CheckedChanged(object sender, EventArgs e)
        {
            if (awfreezecontrol.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "freezecontrol"), "0xC3, 0xF5, 0x88, 0x40");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "freezecontrol"), "0xC3, 0xF5, 0x88, 0x3F");
            }
        }

        private void awteleport_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "teleport"), "0x4f");
        }

        private void awnofriction_CheckedChanged(object sender, EventArgs e)
        {
            if (awnofriction.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "nofriction"), "0x01");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "nofriction"), "0x00");
            }
        }

        private void awvision_CheckedChanged(object sender, EventArgs e)
        {
            if (awvision.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "vision"), "0x28");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "vision"), "0x00");
            }
        }

        private void awjumpheightx2_CheckedChanged(object sender, EventArgs e)
        {
            if (awjumpheightx2.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x43");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x42");
            }
        }

        private void awjumpheightx4_CheckedChanged(object sender, EventArgs e)
        {
            if (awjumpheightx4.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x44");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x42");
            }
        }

        private void awjumpheightx8_CheckedChanged(object sender, EventArgs e)
        {
            if (awjumpheightx8.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x45");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpheight"), "0x00, 0x00, 0x9C, 0x42");
            }
        }

        private void awunlimitedjump_CheckedChanged(object sender, EventArgs e)
        {
            if (awunlimitedjump.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpunlimited"), "0xFF, 0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("awoffsets", "jumpunlimited"), "0x00, 0x00");
            }
        }

        private void awexothrower_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "exothrower"), "0xFF, 0xFF");
        }

        private void awprimaryammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "infinitieammoprimary"), "0xFF, 0xFF");
        }

        private void awsecondaryammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "infinitieammosecondary"), "0xFF, 0xFF");
        }

        private void awexoammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "infinitieammoexo"), "0xFF, 0xFF");
        }

        private void awsimpleButton1_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe1"), awTextBoxEx1.Text);
        }

        private void awsimpleButton2_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe2"), awTextBoxEx2.Text);
        }

        private void awsimpleButton3_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe3"), awTextBoxEx3.Text);
        }

        private void awsimpleButton4_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe4"), awTextBoxEx4.Text);
        }

        private void awsimpleButton5_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe5"), awTextBoxEx5.Text);
        }

        private void awsimpleButton6_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe6"), awTextBoxEx6.Text);
        }

        private void awsimpleButton7_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe7"), awTextBoxEx7.Text);
        }

        private void awsimpleButton8_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe8"), awTextBoxEx8.Text);
        }

        private void awsimpleButton9_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe9"), awTextBoxEx9.Text);
        }

        private void awsimpleButton10_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe10"), awTextBoxEx10.Text);
        }

        private void awpreset1_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("awoffsets", "Classe1"), awpreset1.Text);
        }

        private void ghostsgodmode_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostsgodmode.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "godmod"), "0xFF, 0xFF");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "godmod"), "0x64, 0x00");
            }
        }

        private void ghostsingamechanger_Click(object sender, EventArgs e)
        {
            try
            {
                if (ini.IniReadValue("ghostsoffsets", "ingamenamechanger") == "")
                {

                    MessageBox.Show("Enter a valid ghosts namechanger offset in the changer!");
                }
                else
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(ghostsinmenutext.Text);
                    Array.Resize<byte>(ref bytes, bytes.Length + 1);
                    PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "ingamenamechanger"), "byte");
                }
            }
            catch
            {
                int num = (int)MessageBox.Show("Failed to set In-Game name");
            }
        }

        private void ghoststhirdperson_CheckedChanged(object sender, EventArgs e)
        {
            if (ghoststhirdperson.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "thirdperson"), "0x02");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "thirdperson"), "0x00");
            }
        }

        private void ghostsdefaulltspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostsdefaulltspeed.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x66, 0x66, 0x66, 0x3F");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x66, 0x66, 0x66, 0x3F");
            }
        }

        private void ghostslowspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostslowspeed.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x00, 0x00, 0x40, 0x40");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x66, 0x66, 0x66, 0x3F");
            }
        }

        private void ghostsmediumspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostsmediumspeed.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x00, 0x00, 0x00, 0x40");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x66, 0x66, 0x66, 0x3F");
            }
        }

        private void ghostshighspeed_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostshighspeed.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x00, 0x0A, 0x41");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "speed"), "0x66, 0x66, 0x66, 0x3F");
            }
        }

        private void ghoststeamfederation_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "changeteam"), "0x01");
        }

        private void ghoststeamghosts_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "changeteam"), "0x02");
        }

        private void ghoststeamspectator_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "changeteam"), "0x03");
        }

        private void ghostsredboxes_CheckedChanged(object sender, EventArgs e)
        {
            if (ghostsredboxes.Checked)
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "redbox"), "0x10");
            }
            else
            {
                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "redbox"), "0x00");
            }
        }

        private void ghostssecondaryammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "infinitieammosecondary"), "0xFF, 0xFF, 0xFF");
        }

        private void ghostsgrenadeammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "infinitieammogrenadesspecial"), "0xFF, 0xFF, 0xFF");
        }

        private void ghostsmortarammo_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "infinitieammomortargrenades"), "0xFF, 0xFF, 0xFF");
        }

        private void jumpheight_Scroll(object sender, EventArgs e)
        {
            if (jumpheight.Value == 0) // Default Jump
            {

                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "jumpheight"), "0x00, 0x00, 0x1C, 0x42");
                label2.Text = "Default Jump";
            }
            else
            {
                if (jumpheight.Value == 5) // Low Jump
                {
                    PS4.writeMemory(ini.IniReadValue("ghostsoffset", "jumpheight"), "0x00, 0x00, 0x1C, 0x43");
                    label2.Text = "Low Jump";
                }
                else
                {
                    if (jumpheight.Value == 10) //Medium Jump
                    {
                        PS4.writeMemory(ini.IniReadValue("ghostsoffset", "jumpheight"), "0x00, 0x00, 0x1C, 0x44");
                        label2.Text = "Medium Jump";
                    }
                    else
                    {
                        if (jumpheight.Value == 15) //High Jump
                        {
                            PS4.writeMemory(ini.IniReadValue("ghostsoffset", "jumpheight"), "0x00, 0x00, 0x1C, 0x45");
                            label2.Text = "High Jump";
                        }
                        else
                        {
                            if (jumpheight.Value == 20) //No Jump
                            {
                                PS4.writeMemory(ini.IniReadValue("ghostsoffset", "jumpheight"), "0x00, 0x00, 0x80, 0x3F");
                                label2.Text = "No Jump";
                            }
                        }
                    }
                }
            }
        }

        private void checkforupdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // start the check for update process
            this.checkForUpdate.OnCheckForUpdate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // when the app is closing, this will stop the thread that checks for the
            // new version or downloads it
            this.checkForUpdate.StopThread();
        }
        // this method is called when the checkForUpdate finishes checking
        // for the new version. If this method returns true, our checkForUpdate
        // object will download the installer
        public bool OnCheckForUpdateFinished(DownloadedVersionInfo versionInfo)
        {
            if ((versionInfo.error) || (versionInfo.installerUrl.Length == 0) || (versionInfo.latestVersion == null))
            {
                MessageBox.Show(this, "Error while looking for the newest version", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // compare the current version with the downloaded version number
            Version curVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (curVer.CompareTo(versionInfo.latestVersion) >= 0)
            {
                // no new version
                MessageBox.Show(this, "No new version detected", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }

            // new version found, ask the user if he wants to download the installer
            string str = String.Format("New version found!\nYour version: {0}.\nNewest version: {1}.", curVer, versionInfo.latestVersion);
            return DialogResult.Yes == MessageBox.Show(this, str, "Check for updates", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        // called after the checkForUpdate object downloaded the installer
        public void OnDownloadInstallerinished(DownloadInstallerInfo info)
        {
            if (info.error)
            {
                MessageBox.Show(this, "Error while downloading the installer", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // ask the user if he want to start the installer
            if (DialogResult.Yes != MessageBox.Show(this, "Do you know to install the newest version?", "Check for updates", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                // it not - remove the downloaded file
                try
                {
                    File.Delete(info.path);
                }
                catch { }
                return;
            }
            // run the installer and exit the app
            try
            {
                Process.Start(info.path);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Error while running the installer.", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    File.Delete(info.path);
                }
                catch { }
                return;
            }
            return;
        }

        private void ghsimpleButton10_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass1"), ghrichTextBoxEx10.Text);
        }

        private void ghsimpleButton9_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass2"), ghrichTextBoxEx9.Text);
        }

        private void ghsimpleButton8_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass3"), ghrichTextBoxEx8.Text);
        }

        private void ghsimpleButton7_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass4"), ghrichTextBoxEx7.Text);
        }

        private void ghsimpleButton6_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass5"), ghrichTextBoxEx6.Text);
        }

        private void ghsimpleButton1_Click(object sender, EventArgs e)
        {
            PS4.writeMemory(ini.IniReadValue("ghostsoffsets", "WeaponClass6"), ghrichTextBoxEx1.Text);
        }
    }
}

