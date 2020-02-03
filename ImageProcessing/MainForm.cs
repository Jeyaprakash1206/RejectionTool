using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessing
{
    class WriteLogFile
    {
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", @"C:/RejectionTool", strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage+" "+ DateTime.Now);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    public partial class MainForm : Form
    {
        public event ThumbnailImageEventHandler OnImageSizeChanged;

        private ThumbnailController m_Controller;

        private ImageDialog m_ImageDialog;

        private ImageViewer m_ActiveImageViewer;
        public string fs = "";
        private ArrayList PanelContainer = new ArrayList();
        private ArrayList PanelContainerurl = new ArrayList();
        private ArrayList AllPanelContainer = new ArrayList();
        private ArrayList AllPanelContainerurl = new ArrayList();
        static readonly string rootFolder = @"D:\SRO\ThumbnailDotnet_demo\OutDoc\";
       // public bool Makeselection = false;
        public string srocode = "";
        public string outputjpgpath = "";
        public string outputtiffpath = "";
        //public string dpi = "";
        //public string colordepth = "";
        public string currentimg = "";
        public string deleteurl = "";
        int imageviewerHeight = 0;
        int imageviewerWidth = 0;
        int cropX;
        int cropY;
        int cropWidth;

        int cropHeight;
        int oCropX;
        int oCropY;
        public Pen cropPen;

        public DashStyle cropDashStyle = DashStyle.DashDot;
        public bool Makeselection = false;

        public bool CreateText = false;
        public string pdfpath = "";
        public DateTime lastopentime;
        public DateTime TimeDiff;
        public string selectmode = "";
        public string pdfname = "";
        public string sequence = "";
        public int subfile = 0;

        Image[] scannedImages;
        private int ImageSize
        {
            get
            {
                return (64 * (this.trackBarSize.Value + 1));
            }
        }
        public void LoadConfig()
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "LoadConfig"));
                string textFile = @"C:/RejectionTool/config.txt";
                using (StreamReader file = new StreamReader(textFile))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        Console.WriteLine(ln);
                        counter++;
                        if (ln.Contains("srocode="))
                        {
                            srocode = ln.Replace("srocode=", "");
                        }
                        if (ln.Contains("outputjpgpath="))
                        {
                            outputjpgpath = ln.Replace("outputjpgpath=", "");
                        }
                        if (ln.Contains("outputtiffpath="))
                        {
                            outputtiffpath = ln.Replace("outputtiffpath=", "");
                        }
                        //if (ln.Contains("dpi="))
                        //{
                        //    dpi = ln.Replace("dpi=", "");
                        //}
                        //if (ln.Contains("ColorDepth="))
                        //{
                        //    colordepth = ln.Replace("ColorDepth=", "");
                        //}
                    }
                    file.Close();
                    Console.WriteLine($"File has {counter} lines.");
                }
            }catch(Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Load Config", ex.Message));
            }
        }

        public class Item
        {
            public int millis;
            public string stamp;
            public DateTime datetime;
            public string light;
            public float temp;
            public float vcc;
        }

        public MainForm()
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Log is Created at", DateTime.Now));
                LoadConfig();

                InitializeComponent();
                //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                // FormBorderStyle = FormBorderStyle.None;
                // this.WindowState = FormWindowState.Maximized;
                //TopMost = true;
               // SROCODETXT.Text = srocode;
                this.buttonCancel.Enabled = false;

                m_ImageDialog = new ImageDialog();

                m_AddImageDelegate = new DelegateAddImage(this.AddImage);

                m_Controller = new ThumbnailController();
                m_Controller.OnStart += new ThumbnailControllerEventHandler(m_Controller_OnStart);
                m_Controller.OnAdd += new ThumbnailControllerEventHandler(m_Controller_OnAdd);
                m_Controller.OnEnd += new ThumbnailControllerEventHandler(m_Controller_OnEnd);
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                this.FormClosing += Form1_FormClosing;
                imageviewerHeight = imageViewer1.Height;
                imageviewerWidth = imageViewer1.Width;
                loadfirstdoc();
            }
            catch(Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "MainForm Error", ex.Message));
            }

        }
        private void loadfirstdoc()
        {
            int i = 0;
            foreach (string file in Directory.EnumerateFiles("C:\\RejectionTool\\PDF", "*.pdf"))
            {
                if (i == 0)
                {
                    pdfname = file.Replace("C:\\RejectionTool\\PDF\\", "");
                    SROCODETXT.Text = pdfname.Replace(".pdf", "");
                    AddFolder(file);
                }
                i++;
            }
            subfile = 1;
        }
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "Form1_FormClosing"));
                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                foreach (Image p in PanelContainer)
                {

                    p.Dispose();
                }
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
                //if (selectmode != "Folder")
                //{
                //    System.IO.DirectoryInfo di = new DirectoryInfo(outputjpgpath);

                //    foreach (FileInfo file in di.GetFiles())
                //    {
                //        file.Delete();
                //    }
                //    foreach (DirectoryInfo dir in di.GetDirectories())
                //    {
                //        dir.Delete(true);
                //    }
                //}
            }catch(Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Form1_FormClosing Error", ex.Message));
            }
        }
        private void Closing()
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "Closing"));
                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                foreach (Image p in PanelContainer)
                {

                    p.Dispose();
                }
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
                System.IO.DirectoryInfo di = new DirectoryInfo(outputjpgpath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }catch(Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Closing Error", ex.Message));
            }
        }
        private void deleteFile(string filename)
        {
            string authorsFile = filename;

            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "deleteFile"));
                // Check if file exists with its full path    
                if (File.Exists(filename))
                {
                    // If file found, delete it
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(filename);
                    Console.WriteLine("File deleted.");
                    DeleteImage(filename);
                }
                else Console.WriteLine("File not found");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "deleteFile", ioExp.Message));
            }

        }
        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            selectmode = "File";
          //  this.AddFolder();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.m_Controller.CancelScanning = true;
        }

        private async Task convertPdfToJpg(string pdfpath,string jpgdir)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "convertPdfToJpg"));
                await Task.Run(() =>
                 {
                     SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();

                // This property is necessary only for registered version
                f.Serial = "70037913529";
                     string pdfFile = pdfpath;
                     string jpegDir = jpgdir;

                     f.OpenPdf(pdfFile);

                     if (f.PageCount > 0)
                     {
                    // Set image properties: Jpeg, 200 dpi
                    f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                    //if (dpi != "")
                    //{
                    //    f.ImageOptions.Dpi = System.Convert.ToInt32(dpi);
                    //}
                    //else
                    //{
                    f.ImageOptions.Dpi = 120;
                    //}
                    //if (colordepth != "")
                    //{
                    //    if (colordepth == "BlackWhite1bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                    //    }
                    //    else if (colordepth == "Grayscale8bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale8bpp;
                    //    }
                    //    else if (colordepth == "Grayscale24bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale24bpp;
                    //    }
                    //    else if (colordepth == "Rgb24bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Rgb24bpp;
                    //    }
                    //    else if (colordepth == "Grayscale32bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale32bpp;
                    //    }
                    //    else if (colordepth == "Rgb32bpp")
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Rgb32bpp;
                    //    }
                    //    else
                    //    {
                    //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                    //    }
                    //}
                    //else
                    //{
                    f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                    // f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale8bpp;
                    // }

                    // Set 95 as JPEG quality
                    //f.ImageOptions.JpegQuality = 95;

                    //Save all PDF pages to image folder, each file will have name Page 1.png, Page 2.png, Page N.png
                    this.progressBar1.Minimum = 0;
                    //this.progressBar1.Maximum = f.PageCount;
                    MethodInvoker m = new MethodInvoker(() => this.progressBar1.Maximum = f.PageCount);
                         this.progressBar1.Invoke(m);
                         int successcount = 0;
                         // count.Text = "loading images " + successcount + " out of " + f.PageCount;
                         sequence = "";
                    for (int page = 1; page <= f.PageCount; page++)
                         {
                             if (page != 1)
                             {
                                 sequence = sequence + "," + page;
                             }
                             else
                             {
                                 sequence = sequence + page;
                             }
                             string jpegFile = Path.Combine(jpegDir, String.Format("Page {0}.png", page));

                        // 0 - converted successfully                
                        // 2 - can't create output file, check the output path
                        // 3 - conversion failed
                        int result = f.ToImage(jpegFile, page);
                        //this.progressBar1.Value = page;
                        MethodInvoker m1 = new MethodInvoker(() => this.progressBar1.Value = page);
                             this.progressBar1.Invoke(m1);
                             if (result == 0)
                             {
                                 successcount = successcount + 1;
                             }
                             else
                             {
                                 Console.WriteLine(page);
                             }
                        // count.Text = "loading images " + successcount + " out of " + f.PageCount;
                        label1.Invoke((MethodInvoker)(() => label1.Text = "loading images " + successcount + " out of " + f.PageCount));
                        //Thread.Sleep(31);
                        // Show only 1st page
                        //if (page == 1 && result == 0)
                        //    System.Diagnostics.Process.Start(jpegFile);
                    }
                        // StartDocNumTxt.Text = sequence;
                         StartDocNumTxt.Invoke((MethodInvoker)(() => StartDocNumTxt.Text = sequence));
                         EndTimeTxt.Invoke((MethodInvoker)(() => EndTimeTxt.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
                         if (this.flowLayoutPanelMain.Controls.Count <= 0)
                         {
                             string targetPath = outputjpgpath;
                             m_Controller.AddFolder(targetPath);
                         }
                         f.ClosePdf();
                     }
                 });
            }catch(Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "convertPdfToJpg Error", ex.Message));
            }
            
        }
        [STAThread]
        private void AddFolder(string filename)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "AddFolder"));

              //  OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Description = @"Choose folder path";
            string fileName = "";
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
                delete();
                Closing();
                this.flowLayoutPanelMain.Controls.Clear();
                fileName = filename;
                pdfpath = filename;
                this.buttonCancel.Enabled = true;
                this.buttonBrowseFolder.Enabled = false;
                //WindowState = FormWindowState.Maximized;
               // BookNoTxt.Text = "";
                StartDocNumTxt.Text = "";
                EndDocNumTxt.Text = "";
                TotNumPagesTxt.Text = "";
                RunningDocNum.Text = "";
                YearTxt.Text = "";
                label1.Text = "Not Started";
                StartTimeTxt.Text = "";
                EndTimeTxt.Text = "";
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCApproach();
                GC.WaitForFullGCComplete();
                GC.Collect();
                if (fileName != "")
                {
                    StartTimeTxt.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    convertPdfToJpg(fileName, outputjpgpath);
                    //m_Controller.AddFolder(outputjpgpath);
                }
           // }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "AddFolder Error", ex.Message));
            }
        }
        private void imageViewer_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "imageViewer_MouseHover"));
                //if (m_ActiveImageViewer != null)
                //{
                //    m_ActiveImageViewer.IsActive = false;
                //}

                m_ActiveImageViewer = (ImageViewer)sender;
          //  m_ActiveImageViewer.IsActive = true;

            if (m_ImageDialog.IsDisposed) m_ImageDialog = new ImageDialog();
            m_ImageDialog.SetImage(m_ActiveImageViewer.ImageLocation);
            if (!m_ImageDialog.Visible) m_ImageDialog.Show();

            var t = new System.Windows.Forms.Timer();
            lastopentime= DateTime.Now;
            t.Interval = 10000; // it will Tick in 3 seconds
            t.Tick += (s, ex) =>
            {
                DateTime nw = DateTime.Now;
                if ( (nw - lastopentime).TotalSeconds>= 9.5)
                {

                    m_ImageDialog.Dispose();
                    m_ImageDialog = new ImageDialog();
                    m_ImageDialog.Close();
                }
                t.Stop();
            };
            t.Start();
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "imageViewer_MouseHover Error", ex.Message));
            }
        }
        private void doMultipageTiffSave(string loc)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "doMultipageTiffSave"));
                
                for (int i = 0; i <= PanelContainerurl.Count - 1; i++)

                // traverse i+1 to array length 
                for (int j = i + 1; j <= PanelContainerurl.Count - 1; j++)
                {
                    string f = PanelContainerurl[i].ToString().Replace(".png", "");
                    string g = PanelContainerurl[j].ToString().Replace(".png", "");
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
                            Object container;
                            if (x1 > y1)
                            {

                                random = PanelContainerurl[i].ToString();
                                PanelContainerurl[i] = PanelContainerurl[j];
                                PanelContainerurl[j] = random;


                                container = PanelContainer[i].ToString();
                                PanelContainer[i] = PanelContainer;
                                PanelContainer[j] = container;
                            }
                        }
                    }
                    else
                    {
                        object temp;
                        object tempcontainer;
                        if (Int32.Parse(x) > Int32.Parse(y))
                        {

                            temp = PanelContainerurl[i].ToString(); ;
                            PanelContainerurl[i] = PanelContainerurl[j];
                            PanelContainerurl[j] = temp;

                            tempcontainer = PanelContainer[i];
                            PanelContainer[i] = PanelContainer[j];
                            PanelContainer[j] = tempcontainer;
                        }
                    }
                }
            if (PanelContainer.Count > 0)
            {
                scannedImages = new Image[PanelContainer.Count];
                bool isSave = false;
                int j = 0;

                try
                {
                    foreach (Image p in PanelContainer)
                    {
                        scannedImages[j] = p;
                        j++;
                        isSave = true;
                    }


                    bool res = false;
                    if (isSave)
                    {
                        res = saveMultipage(scannedImages, loc, "TIFF");
                    }

                    //if (res)
                    //{
                    //    MessageBox.Show("All Images saved successfully");
                    //}
                }
                catch (System.Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "doMultipageTiffSave Error", ex.Message));
            }

        }
        public bool saveMultipage(Image[] bmp, string location, string type)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "saveMultipage"));
                if (bmp != null)
            {
                try
                {
                    ImageCodecInfo codecInfo = getCodecForstring(type);

                    for (int i = 0; i < bmp.Length; i++)
                    {
                        if (bmp[i] == null)
                            break;
                        bmp[i] = (Image)ConvertToBitonal((Bitmap)bmp[i]);
                    }

                    if (bmp.Length == 1)
                    {

                        EncoderParameters iparams = new EncoderParameters(1);
                        Encoder iparam = Encoder.Compression;
                        EncoderParameter iparamPara = new EncoderParameter(iparam, (long)(EncoderValue.CompressionCCITT4));
                        iparams.Param[0] = iparamPara;
                        bmp[0].Save(location, codecInfo, iparams);


                    }
                    else if (bmp.Length > 1)
                    {

                        Encoder saveEncoder;
                        Encoder compressionEncoder;
                        EncoderParameter SaveEncodeParam;
                        EncoderParameter CompressionEncodeParam;
                        EncoderParameters EncoderParams = new EncoderParameters(2);

                        saveEncoder = Encoder.SaveFlag;
                        compressionEncoder = Encoder.Compression;

                        // Save the first page (frame).
                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.MultiFrame);
                        CompressionEncodeParam = new EncoderParameter(compressionEncoder, (long)EncoderValue.CompressionCCITT4);
                        EncoderParams.Param[0] = CompressionEncodeParam;
                        EncoderParams.Param[1] = SaveEncodeParam;

                        File.Delete(location);
                        bmp[0].Save(location, codecInfo, EncoderParams);


                        for (int i = 1; i < bmp.Length; i++)
                        {
                            if (bmp[i] == null)
                                break;

                            SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.FrameDimensionPage);
                            CompressionEncodeParam = new EncoderParameter(compressionEncoder, (long)EncoderValue.CompressionCCITT4);
                            EncoderParams.Param[0] = CompressionEncodeParam;
                            EncoderParams.Param[1] = SaveEncodeParam;
                            bmp[0].SaveAdd(bmp[i], EncoderParams);

                        }

                        SaveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.Flush);
                        EncoderParams.Param[0] = SaveEncodeParam;
                        bmp[0].SaveAdd(EncoderParams);
                    }
                    return true;


                }
                catch (System.Exception ee)
                {
                    throw new Exception(ee.Message + "  Error in saving as multipage ");
                }
            }
            else
                return false;
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "saveMultipage Error", ex.Message));
                return false;
            }

        }
        public Bitmap ConvertToBitonal(Bitmap original)
        {
                Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                using (Graphics g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(original, 0, 0);
                }
            }
            else
            {
                source = original;
            }

            // Lock source bitmap in memory
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            int imageSize = sourceData.Stride * sourceData.Height;

            byte[] sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits(sourceData);

            // Create destination bitmap
            Bitmap destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "ConvertToBitonal"));
                // Lock destination bitmap in memory
                BitmapData destinationData = destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            byte[] destinationBuffer = new byte[imageSize];

            int sourceIndex = 0;
            int destinationIndex = 0;
            int pixelTotal = 0;
            byte destinationValue = 0;
            int pixelValue = 128;
            int height = source.Height;
            int width = source.Width;
            int threshold = 500;

            // Iterate lines
            for (int y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;

                // Iterate pixels
                for (int x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits(destinationData);

                // Return
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "ConvertToBitonal Error", ex.Message));
                return destination;
            }
            return destination;
            
        }
        private ImageCodecInfo getCodecForstring(string type)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "getCodecForstring"));
                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < info.Length; i++)
            {
                string EnumName = type.ToString();
                if (info[i].FormatDescription.Equals(EnumName))
                {
                    return info[i];
                }
            }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "getCodecForstring Error", ex.Message));
            }
            return null;

        }
        
        private void m_Controller_OnStart(object sender, ThumbnailControllerEventArgs e)
        {

        }

        private void m_Controller_OnAdd(object sender, ThumbnailControllerEventArgs e)
        {
            this.AddImage(e.ImageFilename);
            this.Invalidate();
        }

        private void m_Controller_OnEnd(object sender, ThumbnailControllerEventArgs e)
        {
            // thread safe
            if (this.InvokeRequired)
            {
                this.Invoke(new ThumbnailControllerEventHandler(m_Controller_OnEnd), sender , e);
            }
            else
            {
                this.buttonCancel.Enabled = false;
                this.buttonBrowseFolder.Enabled = true;
            }
        }

        delegate void DelegateAddImage(string imageFilename);
        private DelegateAddImage m_AddImageDelegate;

        private void DeleteImage(string imageFilename)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "DeleteImage"));
                foreach (ImageViewer c in this.flowLayoutPanelMain.Controls)
            {
                if (c.ImageLocation == imageFilename)
                {
                    this.flowLayoutPanelMain.Controls.Remove(c);
                }
            }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "DeleteImage Error", ex.Message));
            }
        }
        private void AddCropImage(string imageFilename,string oldfilename,int pos)
        {
            // thread safe
            //if (imageFilename != deleteurl)
            //{
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "AddCropImage"));
                if (this.InvokeRequired)
            {
                this.Invoke(m_AddImageDelegate, imageFilename);
            }
            else
            {
                int size = ImageSize;

                ImageViewer imageViewer = new ImageViewer();
                imageViewer.Dock = DockStyle.Bottom;
                imageViewer.LoadImage(imageFilename, 256, 256);
                imageViewer.Width = size;
                imageViewer.Height = size;
                imageViewer.IsThumbnail = true;
                imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick);
                imageViewer.MouseEnter += new System.EventHandler(imageViewer_MouseDoubleClick2);
                imageViewer.MouseLeave += new System.EventHandler(imageViewer_MouseOut);

                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                this.flowLayoutPanelMain.Controls.Add(imageViewer);
                int index = 0;
                int oldindex = 0;
                foreach (ImageViewer c in this.flowLayoutPanelMain.Controls)
                {
                    if (c.ImageLocation == oldfilename)
                    {
                        oldindex = index;
                        break;
                    }
                    index++;
                }
                this.flowLayoutPanelMain.Controls.SetChildIndex(imageViewer, oldindex + pos);
            }
                //}
                //else
                //{
                //    deleteFile(deleteurl);
                //    deleteurl = "";
                //}
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "AddCropImage Error", ex.Message));
            }
        }
        private void Form1_KeyDown(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && cropX>0)
            {
                Cropbtn.PerformClick();
            }
            if (e.KeyChar == 'o' && cropX > 0)
            {
                OutsideCropbtn.PerformClick();
            }
        }
        private void AddImage(string imageFilename)
        {
            // thread safe
            //if (imageFilename != deleteurl)
            //{
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(m_AddImageDelegate, imageFilename);
                }
                else
                {
                    int size = ImageSize;

                    ImageViewer imageViewer = new ImageViewer();
                    imageViewer.Dock = DockStyle.Bottom;
                    imageViewer.LoadImage(imageFilename, 256, 256);
                    imageViewer.Width = size;
                    imageViewer.Height = size;
                    imageViewer.IsThumbnail = true;
                    imageViewer.MouseClick += new MouseEventHandler(imageViewer_MouseClick);
                   imageViewer.MouseHover += new System.EventHandler(imageViewer_MouseHover);
                    imageViewer.MouseLeave += new System.EventHandler(imageViewer_MouseOut);

                this.OnImageSizeChanged += new ThumbnailImageEventHandler(imageViewer.ImageSizeChanged);

                    this.flowLayoutPanelMain.Controls.Add(imageViewer);
                }
                //}
                //else
                //{
                //    deleteFile(deleteurl);
                //    deleteurl = "";
                //}
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "AddImage Error", ex.Message));
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {

            try
            {
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
            }
            catch { }
        }
        //private void imageViewer_MouseHover(object sender, MouseEventArgs e)
        //{
        //    m_ActiveImageViewer = (ImageViewer)sender;
        //    this.imageViewer1.Image = Image.FromFile((string)m_ActiveImageViewer.ImageLocation);
        //    this.imageViewer1.Invalidate();
        //    // this.imageViewer1.Image.Dispose();

        //   // currentimg = m_ActiveImageViewer.ImageLocation;
        //}
        private void imageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "imageViewer_MouseClick"));
                //this.imageViewer1.Width = imageviewerWidth;
                //this.imageViewer1.Height = imageviewerHeight;

                //if (m_ActiveImageViewer != null)
                //{
                //    m_ActiveImageViewer.IsActive = false;
                //}
                m_ImageDialog.Dispose();
            m_ImageDialog = new ImageDialog();
            m_ImageDialog.Close();
            m_ActiveImageViewer = (ImageViewer)sender;
            if (m_ActiveImageViewer.IsActive == true)
            {
                m_ActiveImageViewer.IsActive = false;
                if (fs != "")
                {
                    string[] filenamearr = fs.Split(',');
                    var list = new List<string>(filenamearr);
                    list.Remove(m_ActiveImageViewer.ImageLocation);
                    filenamearr = list.ToArray();
                    fs= string.Join(",", filenamearr);
                    Image loadImage = Image.FromFile(m_ActiveImageViewer.ImageLocation);
                        loadImage.Dispose();
                    int index = PanelContainerurl.IndexOf(m_ActiveImageViewer.ImageLocation);
                    if (index > -1)
                    {
                            int x = 0;
                            foreach (Image p in PanelContainer)
                            {
                                if (index == x)
                                {
                                    p.Dispose();
                                }
                                x = x + 1;
                            }
                            PanelContainer.RemoveAt(index);
                        PanelContainerurl.RemoveAt(index);
                    }
                }

            }
            else
            {
                    m_ActiveImageViewer.IsActive = true;
                    Image loadImage = Image.FromFile(m_ActiveImageViewer.ImageLocation);
                    PanelContainer.Add(loadImage);
                    PanelContainerurl.Add(m_ActiveImageViewer.ImageLocation);
                    if (fs != "")
                    {
                        fs = fs + "," + m_ActiveImageViewer.ImageLocation;
                    }
                    else
                    {
                        fs = m_ActiveImageViewer.ImageLocation;
                    }
                    // filecsv.push(sender.filename);
                }
                if (this.imageViewer1.Image != null)
                {
                    this.imageViewer1.Image.Dispose();
                }

                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                this.imageViewer1.Image = Image.FromFile((string)m_ActiveImageViewer.ImageLocation);
            this.imageViewer1.Invalidate();
            // this.imageViewer1.Image.Dispose();
            
            currentimg = m_ActiveImageViewer.ImageLocation;
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "imageViewer_MouseClick Error", ex.Message));
            }
        }
        private void imageViewer_MouseDoubleClick2(object sender, System.EventArgs e)
        {
            //Makeselection = true;
            //if (m_ActiveImageViewer != null)
            //{
            //    m_ActiveImageViewer.IsActive = false;
            //}

            m_ActiveImageViewer = (ImageViewer)sender;
            //m_ActiveImageViewer.IsActive = true;
            this.imageViewer1.Image = Image.FromFile((string)m_ActiveImageViewer.ImageLocation);
            this.imageViewer1.Invalidate();
            //if (m_ImageDialog.IsDisposed) m_ImageDialog = new ImageDialog();
            //if (!m_ImageDialog.Visible) m_ImageDialog.Show();

            //m_ImageDialog.SetImage(m_ActiveImageViewer.ImageLocation);
        }
        private void hideDialog()
        {
            Thread.Sleep(3000);
            m_ImageDialog.Close();
        }
        private void imageViewer_MouseOut(object sender, System.EventArgs e)
        {
            //this.imageViewer1.Image = null;
            //this.imageViewer1.Invalidate();
            m_ImageDialog.Dispose();
            m_ImageDialog = new ImageDialog();
            m_ImageDialog.Close();
        }
        //private void imageViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    Makeselection = true;
        //    if (m_ActiveImageViewer != null)
        //    {
        //        m_ActiveImageViewer.IsActive = false;
        //    }

        //    m_ActiveImageViewer = (ImageViewer)sender;
        //    m_ActiveImageViewer.IsActive = true;

        //    if (m_ImageDialog.IsDisposed) m_ImageDialog = new ImageDialog();
        //    if (!m_ImageDialog.Visible) m_ImageDialog.Show();

        //    m_ImageDialog.SetImage(m_ActiveImageViewer.ImageLocation);
        //}

        private void trackBarSize_ValueChanged(object sender, EventArgs e)
        {
            this.OnImageSizeChanged(this, new ThumbnailImageEventArgs(ImageSize));
        }

        private void flowLayoutPanelMain_Paint(object sender, PaintEventArgs e)
        {

        }
        public void pdftotif(ArrayList pn,string filename)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "pdftotif"));
                // Convert PDF file to BlackAndWhite Multipage-TIFF.
                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();

            // This property is necessary only for registered version.
            f.Serial = "70037913529";

            string pdfPath = pdfpath;
            string tiffPath = filename;

            f.OpenPdf(pdfPath);

            if (f.PageCount > 0)
            {
                //f.ImageOptions.Dpi = 150;
                //f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale32bpp;
                //if (dpi != "")
                //{
                //    f.ImageOptions.Dpi = System.Convert.ToInt32(dpi);
                //}
                //else
                //{
                    f.ImageOptions.Dpi = 120;
                //}
                //if (colordepth != "")
                //{
                //    if (colordepth == "BlackWhite1bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                //    }
                //    else if (colordepth == "Grayscale8bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale8bpp;
                //    }
                //    else if (colordepth == "Grayscale24bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale24bpp;
                //    }
                //    else if (colordepth == "Rgb24bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Rgb24bpp;
                //    }
                //    else if (colordepth == "Grayscale32bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Grayscale32bpp;
                //    }
                //    if (colordepth == "Rgb32bpp")
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.Rgb32bpp;
                //    }
                //    else
                //    {
                //        f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                //    }
                //}
                //else
                //{
                    f.ImageOptions.ColorDepth = SautinSoft.PdfFocus.CImageOptions.eColorDepth.BlackWhite1bpp;
                //}

                int[] pnarray = pn.OfType<int>().ToArray();
                f.RenderPages = new int[][] { pnarray };
                // EncoderValue.CompressionCCITT4 - also makes image black&white 1 bit
                if (f.ToMultipageTiff(tiffPath, EncoderValue.CompressionCCITT4) == 0)
                {
                   // System.Diagnostics.Process.Start(tiffPath);
                }
            }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "pdftotif Error", ex.Message));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "button1_Click"));
                this.imageViewer1.Image = null;
            this.imageViewer1.Invalidate();
            string DocType = DocumentTypeTxt.Text;
            string FileName = SROCODETXT.Text;
           // string BookNumber = BookNoTxt.Text;
            string DocNo = RunningDocNum.Text;
            string Year = YearTxt.Text;
                //if (DocType == "" || SROCODE == "" || BookNumber == "" || DocNo == "" || Year == "")
                //{
                //    MessageBox.Show("Please Fill All Fields");
                //}
                //else if (PanelContainer.Count <= 0)
                //{
                //    MessageBox.Show("Please Select Any File");
                //}
                //else
                //{
                fs = "";
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
                foreach (string file in Directory.EnumerateFiles("C:\\RejectionTool\\OutputDoc", "*.png"))
                {
                    Image loadImage = Image.FromFile(file);
                    PanelContainer.Add(loadImage);
                    PanelContainerurl.Add(file);
                    if (fs != "")
                    {
                        fs = fs + "," + file;
                    }
                    else
                    {
                        fs = file;
                    }
                }
                string filename = FileName + ".tif";

                string outputfolder = outputtiffpath + filename;
                string[] filearr = fs.Split(',');
                ArrayList pn = new ArrayList();
                bool isCutImage = false;
                foreach (string file in filearr)
                {
                    try
                    {
                        string filewithoutext = file.Replace(".png", "");
                        string[] pagearr = filewithoutext.Split(' ');
                        string pagenotxt = pagearr[pagearr.Length - 1];
                        if (pagenotxt.Contains('_'))
                        {
                            isCutImage = true;
                        }
                        int pageno = System.Convert.ToInt32(pagearr[pagearr.Length - 1]);
                        pn.Add(pageno);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //doMultipageTiffSave(outputfolder);
              //  RunningDocNum.Text = (Int32.Parse(RunningDocNum.Text) + 1).ToString();
                string targetPath = outputjpgpath;
                //if ((dpi=="120" && colordepth== "BlackWhite1bpp") || isCutImage)
                //{
                //    doMultipageTiffSave(outputfolder);
                //}
                //else
                //{
                //    ArrayList renderarray = new ArrayList();
                //    pn.Sort();
                //    renderarray.Add(pn[0]);
                //    renderarray.Add(pn[pn.Count - 1]);
                //    pdftotif(renderarray, outputfolder);
                //}
                doMultipageTiffSave(outputfolder);
                this.flowLayoutPanelMain.Controls.Clear();
                this.flowLayoutPanelMain.Invalidate();
                //PanelContainer = new ArrayList();
                //PanelContainerurl = new ArrayList();
                m_ImageDialog.Dispose();
                m_ImageDialog = new ImageDialog();
                string[] filenamearr = fs.Split(',');
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                foreach (string file in filenamearr)
                {
                    deleteFile(file);
                }
                fs = "";
                EndDocNumTxt.Text = (this.flowLayoutPanelMain.Controls.Count).ToString();
                m_Controller.AddFolder(targetPath);
                string sourceFile = @"C:\RejectionTool\PDF\"+pdfname;
                string destinationFile = @"C:\RejectionTool\ProccessPDF\" + pdfname;

                // To move a file or folder to a new location:
                System.IO.Directory.CreateDirectory(@"C:\RejectionTool\ProccessPDF\");
                if (System.IO.File.Exists(destinationFile))
                {
                    System.IO.File.Delete(destinationFile);
                }
                System.IO.File.Move(sourceFile, destinationFile);
                loadfirstdoc();
           // }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "button1_Click Error", ex.Message));
            }
        }
        private void SelectedTif_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "button1_Click"));
                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                string DocType = DocumentTypeTxt.Text;
                string FileName = SROCODETXT.Text+ "_"+subfile.ToString();
                // string BookNumber = BookNoTxt.Text;
                string DocNo = RunningDocNum.Text;
                string Year = YearTxt.Text;
                //if (DocType == "" || SROCODE == "" || BookNumber == "" || DocNo == "" || Year == "")
                //{
                //    MessageBox.Show("Please Fill All Fields");
                //}
                //else if (PanelContainer.Count <= 0)
                //{
                //    MessageBox.Show("Please Select Any File");
                //}
                //else
                //{
                //fs = "";
                //PanelContainerurl = new ArrayList();
                //PanelContainer = new ArrayList();
                //foreach (string file in Directory.EnumerateFiles("C:\\RejectionTool\\OutputDoc", "*.png"))
                //{
                //    Image loadImage = Image.FromFile(file);
                //    PanelContainer.Add(loadImage);
                //    PanelContainerurl.Add(file);
                //    if (fs != "")
                //    {
                //        fs = fs + "," + file;
                //    }
                //    else
                //    {
                //        fs = file;
                //    }
                //}
                string filename = FileName + ".tif";

                string outputfolder = outputtiffpath + filename;
                string[] filearr = fs.Split(',');
                ArrayList pn = new ArrayList();
                bool isCutImage = false;
                foreach (string file in filearr)
                {
                    try
                    {
                        string filewithoutext = file.Replace(".png", "");
                        string[] pagearr = filewithoutext.Split(' ');
                        string pagenotxt = pagearr[pagearr.Length - 1];
                        if (pagenotxt.Contains('_'))
                        {
                            isCutImage = true;
                        }
                        int pageno = System.Convert.ToInt32(pagearr[pagearr.Length - 1]);
                        pn.Add(pageno);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //doMultipageTiffSave(outputfolder);
                //  RunningDocNum.Text = (Int32.Parse(RunningDocNum.Text) + 1).ToString();
                string targetPath = outputjpgpath;
                //if ((dpi=="120" && colordepth== "BlackWhite1bpp") || isCutImage)
                //{
                //    doMultipageTiffSave(outputfolder);
                //}
                //else
                //{
                //    ArrayList renderarray = new ArrayList();
                //    pn.Sort();
                //    renderarray.Add(pn[0]);
                //    renderarray.Add(pn[pn.Count - 1]);
                //    pdftotif(renderarray, outputfolder);
                //}
                doMultipageTiffSave(outputfolder);
                subfile = subfile + 1;
                this.flowLayoutPanelMain.Controls.Clear();
                this.flowLayoutPanelMain.Invalidate();
                //PanelContainer = new ArrayList();
                //PanelContainerurl = new ArrayList();
                m_ImageDialog.Dispose();
                m_ImageDialog = new ImageDialog();
                string[] filenamearr = fs.Split(',');
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                foreach (string file in filenamearr)
                {
                    deleteFile(file);
                }
                fs = "";
                EndDocNumTxt.Text = (this.flowLayoutPanelMain.Controls.Count).ToString();
                m_Controller.AddFolder(targetPath);
                string[] files = Directory.GetFiles("C:\\RejectionTool\\OutputDoc");
                if(files.Length < 1)
                {
                    string sourceFile = @"C:\RejectionTool\PDF\" + pdfname;
                    string destinationFile = @"C:\RejectionTool\ProccessPDF\" + pdfname;

                    // To move a file or folder to a new location:
                    System.IO.Directory.CreateDirectory(@"C:\RejectionTool\ProccessPDF\");
                    if (System.IO.File.Exists(destinationFile))
                    {
                        System.IO.File.Delete(destinationFile);
                    }
                    System.IO.File.Move(sourceFile, destinationFile);
                    loadfirstdoc();
                }
                else
                {
                    sequence = "";
                    int i = 1;
                    foreach (string seq in files)
                    {
                        if (i != 1)
                        {
                            sequence = sequence + "," + i.ToString();
                        }
                        else
                        {
                            sequence = sequence + i.ToString();
                        }
                        i++;
                    }
                    StartDocNumTxt.Text = sequence;
                }
                // }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "button1_Click Error", ex.Message));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "button2_Click"));
                this.imageViewer1.Image = null;
            this.imageViewer1.Invalidate();
            m_ImageDialog.Dispose();
            // this.flowLayoutPanelMain.Controls.Clear();
            string[] filenamearr = fs.Split(',');
            foreach (Image p in PanelContainer)
            {
                p.Dispose();
            }
            PanelContainer = new ArrayList();
            PanelContainerurl = new ArrayList();
            foreach (string file in filenamearr)
            {
                deleteFile(file);
            }
            fs = "";
                //string targetPath = outputjpgpath;
                //m_Controller.AddFolder(targetPath);
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "button2_Click Error", ex.Message));
            }

        }
        private void delete()
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "delete"));
                this.imageViewer1.Image = null;
            this.imageViewer1.Invalidate();
            m_ImageDialog.Dispose();
            this.flowLayoutPanelMain.Controls.Clear();
            string[] filenamearr = fs.Split(',');
            foreach (Image p in PanelContainer)
            {
                p.Dispose();
            }
            PanelContainer = new ArrayList();
            PanelContainerurl = new ArrayList();
            foreach (string file in filenamearr)
            {
                deleteFile(file);
            }
            fs = "";
                //string targetPath = outputjpgpath;
                //m_Controller.AddFolder(targetPath);
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "delete Error", ex.Message));
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Makeselection = true;
        }

        private void imageViewer1_Load(object sender, EventArgs e)
        {

        }

        private void DocumentTypeTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void StartDocNumTxt_TextChanged(object sender, EventArgs e)
        {
            RunningDocNum.Text = StartDocNumTxt.Text;
        }

        private void EndDocNumTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void StartNum_Click(object sender, EventArgs e)
        {

        }

        private void TotNumPagesTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void RunningDocNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void EndDocNumLbl_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Cursor = Cursors.Cross;
                    cropX = e.X;
                    cropY = e.Y;

                    cropPen = new Pen(Color.Black, 1);
                    cropPen.DashStyle = DashStyle.DashDotDot;


                }
                this.imageViewer1.Refresh();
            }
            catch (Exception ex)
            {
            }


        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Makeselection)
            {
                Cursor = Cursors.Default;
            }

        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            try
            {
                if (this.imageViewer1.Image == null)
                    return;


                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.imageViewer1.Refresh();
                    cropWidth = e.X - cropX;
                    cropHeight = e.Y - cropY;
                    //this.imageViewer1.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
                    // this.imageViewer1.CreateGraphics().DrawLine(cropPen, cropX, cropY, e.X, e.Y);
                   this.imageViewer1.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
                   // System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                   // this.imageViewer1.CreateGraphics().FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));
                }



            }
            catch (Exception ex)
            {
                //if (ex.Number == 5)
                //    return;
            }
        }
        public Bitmap Resize(Bitmap image, int newWidth, int newHeight, string message)
        {
            try
            {
                Bitmap newImage = new Bitmap(newWidth, Calculations(image.Width, image.Height, newWidth));

                using (Graphics gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height));

                    var myBrush = new SolidBrush(Color.FromArgb(70, 205, 205, 205));

                    double diagonal = Math.Sqrt(newImage.Width * newImage.Width + newImage.Height * newImage.Height);

                    Rectangle containerBox = new Rectangle();

                    containerBox.X = (int)(diagonal / 10);
                    float messageLength = (float)(diagonal / message.Length * 1);
                    containerBox.Y = -(int)(messageLength / 1.6);

                    Font stringFont = new Font("verdana", messageLength);

                    StringFormat sf = new StringFormat();

                    float slope = (float)(Math.Atan2(newImage.Height, newImage.Width) * 180 / Math.PI);

                    gr.RotateTransform(slope);
                   // gr.DrawString(message, stringFont, myBrush, containerBox, sf);
                    return newImage;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int Calculations(decimal w1, decimal h1, int newWidth)
        {
            decimal height = 0;
            decimal ratio = 0;


            if (newWidth < w1)
            {
                ratio = w1 / newWidth;
                height = h1 / ratio;

                return Convert.ToInt32(height);
            }

            if (w1 < newWidth)
            {
                ratio = newWidth / w1;
                height = h1 * ratio;
                return Convert.ToInt32(height);
            }

            return Convert.ToInt32(height);
        }
        private void Erasebtn_Click(object sender, EventArgs e)
        {
            try
            {

                // Create image.
                Bitmap imageFile  = (Bitmap)Image.FromFile("C:/Users/jeyap/OneDrive/Desktop/Nancy.JPG");

                // Create graphics object for alteration.
                Graphics newGraphics = Graphics.FromImage(imageFile);

                // Alter image.
                newGraphics.FillRectangle(new SolidBrush(Color.Red), 100, 50, 100, 100);

                // Draw image to screen.
                // e.Graphics.DrawImage(imageFile, new PointF(0.0F, 0.0F));

                // Dispose of graphics object.
                newGraphics.Dispose();
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void Reorderbtn_Click(object sender, EventArgs e)
        {
            try
            {
                 string[] seqarr = StartDocNumTxt.Text.Split(',');
                string[] files = Directory.GetFiles("C:\\RejectionTool\\OutputDoc");
                string subPath = "C:\\RejectionTool\\TempOutputDoc"; // your code goes here

                bool exists = System.IO.Directory.Exists((subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory((subPath));

                System.IO.DirectoryInfo di = new DirectoryInfo(subPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                int i = 1;
                foreach (Image p in PanelContainer)
                {

                    p.Dispose();
                }
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
                foreach (Image p in PanelContainer)
                {

                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                if (m_ActiveImageViewer != null)
                {
                    m_ActiveImageViewer.Dispose();
                }
                //Refresh Page
                this.flowLayoutPanelMain.Controls.Clear();
                //this.flowLayoutPanelMain.Dispose();
                m_ImageDialog.Dispose();
                if (this.imageViewer1.Image != null)
                {
                    this.imageViewer1.Image.Dispose();
                }

                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                string targetPath = outputjpgpath;
                int pagenationcnt = Int32.Parse(pagenationtxt.Text);
               
                fs = "";
                // System.Windows.Forms.Application.ExitThread();
                sequence = "";
                    foreach (string seq in seqarr)
                {
                    System.IO.File.Move(files[Int32.Parse(seq) - 1], @"C:\RejectionTool\TempOutputDoc\Page " + i.ToString() + ".png");
                    if (i != 1)
                    {
                        sequence = sequence + "," + i.ToString();
                    }
                    else
                    {
                        sequence = sequence + i.ToString();
                    }
                    i++;
                }
                StartDocNumTxt.Text = sequence;
                System.IO.DirectoryInfo sdi = new DirectoryInfo("C:\\RejectionTool\\OutputDoc");

                sdi.Delete();
                System.IO.Directory.Move(@"C:\RejectionTool\TempOutputDoc\", @"C:\RejectionTool\OutputDoc");
                //MainForm mf = new MainForm();
                //mf.Show();

                 m_Controller.AddFolder(targetPath, pagenationcnt);
            }
            catch(Exception exc)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "ReOrder Error1", exc.Message));
            }



        }
        private void Rescanbtn_Click(object sender, EventArgs e)
        {
            try
            {
                //I declare first my variables


                this.flowLayoutPanelMain.Controls.Clear();
                this.flowLayoutPanelMain.Invalidate();
                //PanelContainer = new ArrayList();
                //PanelContainerurl = new ArrayList();
                m_ImageDialog.Dispose();
                m_ImageDialog = new ImageDialog();
                string[] filenamearr = fs.Split(',');
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                foreach (string file in filenamearr)
                {
                    deleteFile(file);
                }
                fs = "";
                EndDocNumTxt.Text = (this.flowLayoutPanelMain.Controls.Count).ToString();
               // m_Controller.AddFolder(targetPath);
                string sourceFile = @"C:\RejectionTool\PDF\" + pdfname;
                string destinationFile = @"C:\RejectionTool\Rescan\" + pdfname;

                // To move a file or folder to a new location:
                System.IO.Directory.CreateDirectory(@"C:\RejectionTool\Rescan\");
                if (System.IO.File.Exists(destinationFile))
                {
                    System.IO.File.Delete(destinationFile);
                }
                System.IO.File.Move(sourceFile, destinationFile);
                loadfirstdoc();
                button1.Select();
            }
            catch (Exception exc)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "ReOrder Error1", exc.Message));
            }



        }
        private void Copybtn_Click(object sender, EventArgs e)
        {
            try
            {
                //I declare first my variables
                string sourcePath = @"C:\RejectionTool\PDF";
                string targetPath = @"C:\RejectionTool\Copy";

                string destFile = Path.Combine(targetPath, pdfname);
                string sourceFile = Path.Combine(sourcePath, pdfname);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                // To copy a file to another location and 
                // overwrite the destination file if it already exists.
                File.Copy(sourceFile, destFile, true);
                //System.Windows.Forms.MessageBox.Show("File Copied: " + destFile, "Message");

                this.imageViewer1.Image = null;
                this.imageViewer1.Invalidate();
                foreach (Image p in PanelContainer)
                {

                    p.Dispose();
                }
                PanelContainer = new ArrayList();
                PanelContainerurl = new ArrayList();
                //deleteFile(currentimg);
                if (m_ActiveImageViewer != null)
                {
                    m_ActiveImageViewer.Dispose();
                }

                //Refresh Page
                this.flowLayoutPanelMain.Controls.Clear();
                //this.flowLayoutPanelMain.Dispose();
                m_ImageDialog.Dispose();
                if (this.imageViewer1.Image != null)
                {
                    this.imageViewer1.Image.Dispose();
                }
                try
                {
                    foreach (Image p in PanelContainer)
                    {
                        p.Dispose();
                    }
                    PanelContainerurl = new ArrayList();
                    PanelContainer = new ArrayList();
                }
                catch { }
                fs = "";
                m_Controller.AddFolder(outputjpgpath);
                button1.Select();
            }
            catch (Exception exc)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "ReOrder Error1", exc.Message));
            }



        }
        private void leftbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "OutsideCropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid) / 4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);



                    //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping


                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        // graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                       // graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, x.Width, Decimal.ToInt32(y));
                        graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y), Decimal.ToInt32(x1), Decimal.ToInt32(chig));
                       // graphics.FillRectangle(new SolidBrush(Color.White), (Decimal.ToInt32(x1) + Decimal.ToInt32(cwid)), Decimal.ToInt32(y), x.Width, Decimal.ToInt32(chig));
                       // graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y) + Decimal.ToInt32(chig), x.Width, x.Height - (Decimal.ToInt32(y) + Decimal.ToInt32(chig)));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                    //  x3.Dispose();
                    //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void topbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "OutsideCropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid) / 4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);



                    //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping


                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        // graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                        graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, x.Width, Decimal.ToInt32(y));
                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y), Decimal.ToInt32(x1), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), (Decimal.ToInt32(x1) + Decimal.ToInt32(cwid)), Decimal.ToInt32(y), x.Width, Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y) + Decimal.ToInt32(chig), x.Width, x.Height - (Decimal.ToInt32(y) + Decimal.ToInt32(chig)));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                    //  x3.Dispose();
                    //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void rightbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "OutsideCropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid) / 4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);



                    //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping


                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        // graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, x.Width, Decimal.ToInt32(y));
                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y), Decimal.ToInt32(x1), Decimal.ToInt32(chig));
                        graphics.FillRectangle(new SolidBrush(Color.White), (Decimal.ToInt32(x1) + Decimal.ToInt32(cwid)), Decimal.ToInt32(y), x.Width, Decimal.ToInt32(chig));
                       // graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y) + Decimal.ToInt32(chig), x.Width, x.Height - (Decimal.ToInt32(y) + Decimal.ToInt32(chig)));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                    //  x3.Dispose();
                    //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void bottombtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "OutsideCropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid) / 4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);



                    //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping


                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        // graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, x.Width, Decimal.ToInt32(y));
                        //graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y), Decimal.ToInt32(x1), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), (Decimal.ToInt32(x1) + Decimal.ToInt32(cwid)), Decimal.ToInt32(y), x.Width, Decimal.ToInt32(chig));
                        graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y) + Decimal.ToInt32(chig), x.Width, x.Height - (Decimal.ToInt32(y) + Decimal.ToInt32(chig)));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                    //  x3.Dispose();
                    //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void OutsideCropbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "OutsideCropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid) / 4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);



                    //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping


                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        // graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                        graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, x.Width, Decimal.ToInt32(y));
                        graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y), Decimal.ToInt32(x1), Decimal.ToInt32(chig));
                        graphics.FillRectangle(new SolidBrush(Color.White), (Decimal.ToInt32(x1) + Decimal.ToInt32(cwid)), Decimal.ToInt32(y), x.Width, Decimal.ToInt32(chig));
                        graphics.FillRectangle(new SolidBrush(Color.White), 0, Decimal.ToInt32(y)+ Decimal.ToInt32(chig), x.Width, x.Height - (Decimal.ToInt32(y) + Decimal.ToInt32(chig)));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                    //  x3.Dispose();
                    //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void Cropbtn_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "Cropbtn_Click"));
                Cursor = Cursors.Default;
                string[] patharr = currentimg.Split('/');
                if (patharr.Length <= 1)
                {
                    patharr = currentimg.Split('\\');
                }
                string filename = patharr[patharr.Length - 1];
                string[] filewithoutext = filename.Split('.');
                string fname = filewithoutext[0];

                try
                {
                    if (cropWidth < 1)
                    {
                        return;
                    }
                    // Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

                    //First we define a rectangle with the help of already calculated points
                    Bitmap OriginalImage = new Bitmap(this.imageViewer1.Image, this.imageViewer1.Width, this.imageViewer1.Height);
                    Rectangle rect = new Rectangle(0, 0, OriginalImage.Width, (OriginalImage.Height));
                    //Original image
                    Bitmap _img = new Bitmap(OriginalImage.Width, (OriginalImage.Height));
                    // for cropinf image
                    Graphics g = Graphics.FromImage(_img);
                    // create graphics
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    //set image attributes
                    g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();

                    this.imageViewer1.Image = _img;
                    this.imageViewer1.Width = _img.Width;
                    this.imageViewer1.Height = _img.Height;
                    this.imageViewer1.Invalidate();


                    Bitmap x = (Bitmap)Image.FromFile(currentimg);
                    //real height
                    decimal rhigper = (Convert.ToDecimal(x.Width) / x.Height) * 100;
                    decimal rwid = (rhigper / 100) * OriginalImage.Height;

                    decimal widthdif = (OriginalImage.Width - rwid)/4;
                    decimal rcropX = Convert.ToDecimal(cropX);
                    decimal x1percentage = (rcropX / OriginalImage.Width) * 100;
                    decimal x1 = (x1percentage / 100) * x.Width;
                    decimal ypercentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    decimal y = (ypercentage / 100) * x.Height;
                    Rectangle ret = new Rectangle(0, 0, x.Width, Decimal.ToInt32(y));
                    Rectangle retimg = new Rectangle(0, 0, x.Width, x.Height);
                    decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    Image cimg = x.Clone(retimg, x.PixelFormat);

                   

                  //  decimal cwidper = (Convert.ToDecimal(cropWidth) / rwid) * 100;
                    decimal cwidper = (Convert.ToDecimal(cropWidth) / OriginalImage.Width) * 100;
                    decimal cwid = (cwidper / 100) * x.Width;

                    decimal chigper = (Convert.ToDecimal(cropHeight) / OriginalImage.Height) * 100;
                    decimal chig = (chigper / 100) * x.Height;
                    //Test Cropping

                    
                    // Image img = Image.FromFile(outputjpgpath + "/" + fname + "_1.png");
                    Image img = new Bitmap(cimg);

                    using (img)
                    using (var graphics = Graphics.FromImage(img))
                    using (var font = new Font("Arial", 20, FontStyle.Regular))
                    {
                        // Do what you want using the Graphics object here.
                        Pen p = new Pen(Brushes.White);
                        // graphics.FillPath(p, ret);
                        // FillRoundedRectangle(graphics, 0, Decimal.ToInt32(y), x.Width, x.Height);
                        // FillRoundedRectangle(graphics, Decimal.ToInt32(x1), Decimal.ToInt32(y), cropWidth, cropHeight);
                        graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x1), Decimal.ToInt32(y), Decimal.ToInt32(cwid), Decimal.ToInt32(chig));
                        //graphics.FillRectangle(new SolidBrush(Color.White), Decimal.ToInt32(x), Decimal.ToInt32(y), 100, 100);
                        //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        //graphics.FillRectangle(myBrush, new Rectangle(cropX, cropY, cropWidth, cropHeight));

                        // Important part!
                        string path = outputjpgpath + "/" + fname + "_1.png";
                        using (var newBitmap = new Bitmap(img))
                        {
                            if (System.IO.File.Exists(path))
                            {
                                Console.WriteLine("Exist");
                            }
                            newBitmap.Save(outputjpgpath + "/" + fname + "_1.png", ImageFormat.Png);
                        }
                        graphics.Dispose();
                    }
                    img.Dispose();
                    cimg.Dispose();

                    // x2.Save(outputjpgpath + "/" + fname + "_1.png");
                    AddCropImage(outputjpgpath + "/" + fname + "_1.png", currentimg, 0);





                    //decimal y2percentage = (Convert.ToDecimal(cropY) / OriginalImage.Height) * 100;
                    //Rectangle ret2 = new Rectangle(0, Decimal.ToInt32(y), x.Width, (x.Height - Decimal.ToInt32(y)));
                    //Image cx3 = x.Clone(retimg, x.PixelFormat);
                    //Image x3 = new Bitmap(cx3);

                    //using (x3)
                    //using (var graphics = Graphics.FromImage(x3))
                    //using (var font = new Font("Arial", 20, FontStyle.Regular))
                    //{
                    //    // graphics.FillPath(p, ret);
                    //    FillRoundedRectangle(graphics, 0, 0, x.Width, Decimal.ToInt32(y));

                    //    // Important part!
                    //    using (var newBitmap = new Bitmap(x3))
                    //    {
                    //        newBitmap.Save(outputjpgpath + "/" + fname + "_2.png", ImageFormat.Png);
                    //    }
                    //    graphics.Dispose();
                    //}

                    ////x3.Save(outputjpgpath + "/" + fname + "_2.png");
                    //AddCropImage(outputjpgpath + "/" + fname + "_2.png", currentimg, 0);
                    x.Dispose();
                  //  x3.Dispose();
                  //  cx3.Dispose();
                    _img.Dispose();
                    g.Dispose();
                    img.Dispose();
                    OriginalImage.Dispose();
                    this.imageViewer1.Image = null;
                    this.imageViewer1.Invalidate();
                    foreach (Image p in PanelContainer)
                    {

                        p.Dispose();
                    }
                    PanelContainer = new ArrayList();
                    PanelContainerurl = new ArrayList();
                    deleteFile(currentimg);
                    m_ActiveImageViewer.Dispose();

                    //Refresh Page
                    this.flowLayoutPanelMain.Controls.Clear();
                    //this.flowLayoutPanelMain.Dispose();
                    m_ImageDialog.Dispose();
                    if (this.imageViewer1.Image != null)
                    {
                        this.imageViewer1.Image.Dispose();
                    }
                    string targetPath = outputjpgpath;
                    int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                    try
                    {
                        foreach (Image p in PanelContainer)
                        {
                            p.Dispose();
                        }
                        PanelContainerurl = new ArrayList();
                        PanelContainer = new ArrayList();
                    }
                    catch { }
                    fs = "";
                    m_Controller.AddFolder(targetPath, pagenationcnt);
                }
                catch (Exception ex)
                {
                    WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error1", ex.Message));
                }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "Cropbtn_Click Error", ex.Message));
            }
        }
        private void FillRoundedRectangle(Graphics G, int X1, int Y1, int X2, int Y2)
        {
            int width = X2 - X1, height = Y2 - Y1;

           // G.FillRectangle(Brushes.White, X1, Y1, X2, Y2);
            G.FillRectangle(Brushes.White, X1, Y1 + 1, width, height - 2);
        }
        public static void SaveJpeg(string path, Bitmap image)
        {
            SaveJpeg(path, image, 95L);
        }
        public static void SaveJpeg(string path, Bitmap image, long quality)
        {
            using (EncoderParameters encoderParameters = new EncoderParameters(1))
            using (EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, quality))
            {
                ImageCodecInfo codecInfo = ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                encoderParameters.Param[0] = encoderParameter;
                image.Save(path, codecInfo, encoderParameters);
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void PictureBoxLocation()
        {
            int _x = 0;
            int _y = 0;
            //if (SplitContainer1.Panel1.Width > this.imageViewerFull.Width)
            //{
            //    _x = (SplitContainer1.Panel1.Width - this.imageViewerFull.Width) / 2;
            //}
            //if (SplitContainer1.Panel1.Height > this.imageViewerFull.Height)
            //{
            //    _y = (SplitContainer1.Panel1.Height - this.imageViewerFull.Height) / 2;
            //}
            this.imageViewer1.Location = new Point(_x, _y);
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            this.imageViewer1.Width = imageviewerWidth;
            this.imageViewer1.Height = imageviewerHeight;
            this.imageViewer1.Image = Image.FromFile((string)currentimg);
            this.imageViewer1.Invalidate();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void ResetAll_Click(object sender, EventArgs e)
        {
           this.flowLayoutPanelMain.Controls.Clear();
            m_ImageDialog.Dispose();
            if (this.imageViewer1.Image != null)
            {
                this.imageViewer1.Image.Dispose();
            }
            string targetPath = outputjpgpath;
            int pagenationcnt = Int32.Parse(pagenationtxt.Text);
            try
            {
                foreach (Image p in PanelContainer)
                {
                    p.Dispose();
                }
                PanelContainerurl = new ArrayList();
                PanelContainer = new ArrayList();
            }
            catch { }
            fs = "";
            m_Controller.AddFolder(targetPath, pagenationcnt);
             

        }

        private void trackBarSize_Scroll(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void folderbrowse_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLogFile.WriteLog("access", String.Format("{0} @ {1}", "Function", "folderbrowse_Click"));
                selectmode = "Folder";
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    outputjpgpath = fbd.SelectedPath;
                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                    if(files.Length == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                    }
                    else
                    {
                        this.flowLayoutPanelMain.Controls.Clear();
                        m_ImageDialog.Dispose();
                        if (this.imageViewer1.Image != null)
                        {
                            this.imageViewer1.Image.Dispose();
                        }
                        string targetPath = outputjpgpath;
                        int pagenationcnt = Int32.Parse(pagenationtxt.Text);
                        try
                        {
                            foreach (Image p in PanelContainer)
                            {
                                p.Dispose();
                            }
                            PanelContainerurl = new ArrayList();
                            PanelContainer = new ArrayList();
                        }
                        catch { }
                        fs = "";
                        m_Controller.AddFolder(targetPath, pagenationcnt);

                    }
                }
            }
            }
            catch (Exception ex)
            {
                WriteLogFile.WriteLog("error", String.Format("{0} @ {1}", "folderbrowse_Click Error", ex.Message));
            }

        }
    }

    public class ThumbnailImageEventArgs : EventArgs
    {
        public ThumbnailImageEventArgs(int size)
        {
            this.Size = size;
        }

        public int Size;
    }

    public delegate void ThumbnailImageEventHandler(object sender, ThumbnailImageEventArgs e);


}