using System;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace ImageProcessing
{
    public class ThumbnailControllerEventArgs : EventArgs
    {
        public ThumbnailControllerEventArgs(string imageFilename)
        {
            this.ImageFilename = imageFilename;
        }

        public string ImageFilename;
    }

    public delegate void ThumbnailControllerEventHandler(object sender, ThumbnailControllerEventArgs e);

    public class ThumbnailController
    {
        public int pagenationcnt = 10;
        private bool m_CancelScanning;
        static readonly object cancelScanningLock = new object();

        public bool CancelScanning
        {
            get
            {
                lock (cancelScanningLock)
                {
                    return m_CancelScanning;
                }
            }
            set
            {
                lock (cancelScanningLock)
                {
                    m_CancelScanning = value;
                }
            }
        }

        public event ThumbnailControllerEventHandler OnStart;
        public event ThumbnailControllerEventHandler OnAdd;
        public event ThumbnailControllerEventHandler OnEnd;

        public ThumbnailController()
        {
            
        }

        public void AddFolder(string folderPath,int pagenation=10)
        {
            pagenationcnt = pagenation;
            CancelScanning = false;

            Thread thread = new Thread(new ParameterizedThreadStart(AddFolder));
            thread.IsBackground = true;
            thread.Start(folderPath);
        }

        private void AddFolder(object folderPath)
        {
            string path = (string)folderPath;

            if (this.OnStart != null)
            {
                this.OnStart(this, new ThumbnailControllerEventArgs(null));
            }

            this.AddFolderIntern(path);

            if (this.OnEnd != null)
            {
                this.OnEnd(this, new ThumbnailControllerEventArgs(null));
            }

            CancelScanning = false;
        }

        private void AddFolderIntern(string folderPath)
        {
            if (CancelScanning) return;

            // not using AllDirectories
            string[] files = Directory.GetFiles(folderPath);
            string temp;
            for (int i = 0; i <= files.Length - 1; i++)

                // traverse i+1 to array length 
                for (int j = i + 1; j <= files.Length - 1; j++)
                {
                    string f = files[i].Replace(".png", "");
                    string g = files[j].Replace(".png", "");
                    string[] xarr = f.Split(' ');
                    string x = (xarr[xarr.Length - 1].Replace("_1", "")).Replace("_2", "");
                    string[] yarr = g.Split(' ');
                    string y = (yarr[yarr.Length - 1].Replace("_1", "")).Replace("_2", "");

                    if (x == y)
                    {
                        if (xarr[xarr.Length - 1].Contains("_") || yarr[yarr.Length - 1].Contains("_"))
                        {
                            string[] x1arr = xarr[xarr.Length - 1].Split('_');

                            int x1 = 0;
                            if (x1arr.Length > 1)
                            {
                                x1 = Int32.Parse(x1arr[1]);
                            }
                            string[] y1arr = yarr[yarr.Length - 1].Split('_');
                            int y1 = 0;
                            if (y1arr.Length > 1)
                            {
                                y1 = Int32.Parse(y1arr[1]);
                            }

                            string random;
                            if (x1 > y1)
                            {

                                random = files[i];
                                files[i] = files[j];
                                files[j] = random;
                            }
                        }
                    }
                    else
                    {

                        if (Int32.Parse(x) > Int32.Parse(y))
                        {

                            temp = files[i];
                            files[i] = files[j];
                            files[j] = temp;
                        }
                    }
                }
            foreach (string file in files.Take(pagenationcnt).Select(i => i.ToString()).ToArray())
            {
                if (CancelScanning) break;

                Image img = null;

                try
                {
                    img = Image.FromFile(file);
                }
                catch
                {
                    // do nothing
                }

                if (img != null)
                {
                    this.OnAdd(this, new ThumbnailControllerEventArgs(file));

                    img.Dispose();
                }
            }

            // not using AllDirectories
            string[] directories = Directory.GetDirectories(folderPath); 
            foreach(string dir in directories)
            {
                if (CancelScanning) break;

                AddFolderIntern(dir);
            }
        }
    }
}
