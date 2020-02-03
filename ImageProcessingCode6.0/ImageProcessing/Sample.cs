using System;
using System.IO;

namespace ImageProcessing
{
    public class Sample
    {
        public void savejpg(string pdffile,string jpegdir)
        {
            // Convert PDF to JPG with high Quality
            SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();

            // This property is necessary only for registered version
            f.Serial = "70037913529";
            string pdfFile = pdffile;
            string jpegDir = jpegdir;

            f.OpenPdf(pdfFile);

            if (f.PageCount > 0)
            {
                // Set image properties: Jpeg, 200 dpi
                f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;
                f.ImageOptions.Dpi = 300;

                // Set 95 as JPEG quality
                f.ImageOptions.JpegQuality = 95;

                //Save all PDF pages to image folder, each file will have name Page 1.png, Page 2.png, Page N.png
                for (int page = 1; page <= f.PageCount; page++)
                {
                    string jpegFile = Path.Combine(jpegDir, String.Format("Page {0}.png", page));

                    // 0 - converted successfully                
                    // 2 - can't create output file, check the output path
                    // 3 - conversion failed
                    int result = f.ToImage(jpegFile, page);

                    // Show only 1st page
                    if (result != 0)
                    {
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine(page);
                    }
                }
            }
        }
    }
}
