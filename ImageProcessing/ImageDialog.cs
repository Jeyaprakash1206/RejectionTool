using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace ImageProcessing
{
    public partial class ImageDialog : Form
    {
        public ImageDialog()
        {
            InitializeComponent();
        }
        private Image Img;
        private Size OriginalImageSize;
        private Size ModifiedImageSize;

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

        public void SetImage(string filename)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(SetImageIntern));
            thread.IsBackground = true;
            thread.Start(filename);
        }

        private void SetImageIntern(object filename)
        {
            this.imageViewerFull.Image = Image.FromFile((string)filename);
            this.imageViewerFull.Invalidate();
        }

        private void ImageDialog_Resize(object sender, EventArgs e)
        {
            this.imageViewerFull.Invalidate();
        }

        private void imageViewerFull_Load(object sender, EventArgs e)
        {

        }
        public int dialogloc(string axis)
        {
            int loc = 0;
            if (axis == "X")
            {
                loc = Cursor.Position.X+ 20;
            }
            else if (axis == "Y")
            {
                //loc = Cursor.Position.Y;
                loc = 1;
            }
            return loc;
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
                this.imageViewerFull.Refresh();
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
                        if (this.imageViewerFull.Image == null)
                            return;


                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        {
                            this.imageViewerFull.Refresh();
                            cropWidth = e.X - cropX;
                            cropHeight = e.Y - cropY;
                            this.imageViewerFull.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
                        }



                    }
                    catch (Exception ex)
                    {
                        //if (ex.Number == 5)
                        //    return;
                    }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.Default;

            try
            {
                if (cropWidth < 1)
                {
                    return;
                }
                Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                //First we define a rectangle with the help of already calculated points
                Bitmap OriginalImage = new Bitmap(this.imageViewerFull.Image, this.imageViewerFull.Width, this.imageViewerFull.Height);
                //Original image
                Bitmap _img = new Bitmap(cropWidth, cropHeight);
                // for cropinf image
                Graphics g = Graphics.FromImage(_img);
                // create graphics
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //set image attributes
                g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);

                this.imageViewerFull.Image = _img;
                this.imageViewerFull.Width = _img.Width;
                this.imageViewerFull.Height = _img.Height;
               // PictureBoxLocation();
            }
            catch (Exception ex)
            {
            }
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
            this.imageViewerFull.Location = new Point(_x, _y);
        }
    }
}