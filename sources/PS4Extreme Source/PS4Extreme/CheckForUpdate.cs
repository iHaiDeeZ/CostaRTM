using System;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;

namespace PS4Extreme
{

    // this struct will contain the info from the xml file
    public struct DownloadedVersionInfo
    {
        public bool error;
        public Version latestVersion;
        public string installerUrl;
        public string homeUrl;
    }

    // this will contain info about the downloaded installer
    public struct DownloadInstallerInfo
    {
        public bool error;
        public string path;
    }

    // delegates (will forward the request to our Form1)
    // this of course could be done in a better (more flexible) way
    public delegate bool DelegateCheckForUpdateFinished(DownloadedVersionInfo versionInfo);
    public delegate void DelegateDownloadInstallerFinished(DownloadInstallerInfo info);


    class CheckForUpdate
    {
        private static string xmlFileUrl = "https://raw.githubusercontent.com/Vultra/vultra.github.io/master/xml/update.xml";

        private readonly Form1 mainApp;

        Thread m_WorkerThread;
        // events used to stop worker thread
        readonly ManualResetEvent m_EventStopThread;
        readonly ManualResetEvent m_EventThreadStopped;

        public CheckForUpdate(Form1 mainApp)
        {
            this.mainApp = mainApp;
            m_EventStopThread = new ManualResetEvent(false);
            m_EventThreadStopped = new ManualResetEvent(false);
        }

        // start the check for update process (if it is not already running)
        public void OnCheckForUpdate()
        {
            if ((this.m_WorkerThread != null) && (this.m_WorkerThread.IsAlive)) return;
            m_WorkerThread = new Thread(this.CheckForUpdateFunction);
            m_EventStopThread.Reset();
            m_EventThreadStopped.Reset();
            m_WorkerThread.Start();
        }

        // when the worker thread is running - let it know it should stop
        public void StopThread()
        {
            if ((this.m_WorkerThread != null) && this.m_WorkerThread.IsAlive)
            {
                m_EventStopThread.Set();
                while (m_WorkerThread.IsAlive)
                {
                    if (WaitHandle.WaitAll(
                                            (new ManualResetEvent[] { m_EventThreadStopped }),
                                            100,
                                            true))
                    {
                        break;
                    }
                    Application.DoEvents();
                }
            }
        }

        // internal method - return true when the thread is supposed to stop
        private bool StopWorkerThread()
        {
            if (m_EventStopThread.WaitOne(0, true))
            {
                m_EventThreadStopped.Set();
                return true;
            }
            return false;
        }

        // this is run in a thread. do the whole updating process:
        // - check for the new version (downloading the xml file)
        // - download the installer
        // the communication with the Form is done with the delegates

        private void CheckForUpdateFunction()
        {
            DownloadedVersionInfo i = new DownloadedVersionInfo();
            i.error = true;
            i.installerUrl = "";
            i.homeUrl = "";

            try
            {
                XmlTextReader reader = new XmlTextReader(xmlFileUrl);

                reader.MoveToContent();
                string elementName = "";
                Version newVer = null;
                string url = "";
                string msiUrl = "";
                if (StopWorkerThread()) return;

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "appinfo"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element) elementName = reader.Name;
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                            {

                                switch (elementName)
                                {
                                    case "version":
                                        newVer = new Version(reader.Value);
                                        break;
                                    case "url":
                                        url = reader.Value;
                                        break;
                                    case "installer":
                                        msiUrl = reader.Value;
                                        break;
                                    case "date":
                                        //elementName = elementName;
                                        break;
                                }
                            }
                        }
                    }
                }
                reader.Close();

                i.error = false;
                i.latestVersion = newVer;
                i.homeUrl = url;
                i.installerUrl = msiUrl;
            }
            catch (Exception)
            {
            }
            if (StopWorkerThread()) return;
            bool download = (bool)this.mainApp.Invoke(new DelegateCheckForUpdateFinished(mainApp.OnCheckForUpdateFinished), new Object[] { i });
            if (!download) return;

            // download and let the main thread know
            DownloadInstallerInfo i2 = new DownloadInstallerInfo();
            i2.error = true;
            string filepath = "";
            try
            {
                WebRequest request = WebRequest.Create(i.installerUrl);
                WebResponse response = request.GetResponse();
                string filename = "";
                int contentLength = 0;
                for (int a = 0; a < response.Headers.Count; a++)
                {
                    try
                    {
                        string val = response.Headers.Get(a);

                        switch (response.Headers.GetKey(a).ToLower())
                        {
                            case "content-length":
                                contentLength = Convert.ToInt32(val);
                                break;
                            case "content-disposition":
                                string[] v2 = val.Split(';');
                                foreach (string s2 in v2)
                                {
                                    string[] v3 = s2.Split('=');
                                    if (v3.Length == 2)
                                    {
                                        if (v3[0].Trim().ToLower() == "filename")
                                        {
                                            char[] sss = { ' ', '"' };
                                            filename = v3[1].Trim(sss);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception) { };
                }
                if (StopWorkerThread()) return;
                if (filename.Length == 0) filename = "PS4Extreme.rar";
                filepath = Path.Combine(Path.GetTempPath(), filename);

                if (File.Exists(filepath))
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                    if (File.Exists(filepath))
                    {
                        string rname = Path.GetRandomFileName();
                        rname.Replace('.', '_');
                        rname += ".rar";
                        filepath = Path.Combine(Path.GetTempPath(), rname);
                    }
                }
                Stream stream = response.GetResponseStream();
                int pos = 0;
                byte[] buf2 = new byte[8192];
                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                while ((0 == contentLength) || (pos < contentLength))
                {
                    int maxBytes = 8192;
                    if ((0 != contentLength) && ((pos + maxBytes) > contentLength)) maxBytes = contentLength - pos;
                    int bytesRead = stream.Read(buf2, 0, maxBytes);
                    if (bytesRead <= 0) break;
                    fs.Write(buf2, 0, bytesRead);
                    if (StopWorkerThread()) return;
                    pos += bytesRead;
                }
                fs.Close();
                stream.Close();
                i2.error = false;
                i2.path = filepath;
            }
            catch
            {
                // when something goes wrong - at least do the cleanup :)
                if (filepath.Length > 0)
                {
                    try
                    {
                        File.Delete(filepath);
                    }
                    catch
                    {
                    }
                }
            }
            if (StopWorkerThread()) return;
            this.mainApp.BeginInvoke(new DelegateDownloadInstallerFinished(mainApp.OnDownloadInstallerinished), new Object[] { i2 });
        }
    }
}

