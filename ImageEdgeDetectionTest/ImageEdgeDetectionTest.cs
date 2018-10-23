using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Security.Cryptography;
using System.IO;
using ImageEdgeDetection;

namespace ImageEdgeDetectionTest
{
    /// <summary>
    /// Description résumée pour ImageEdgeDetectionTest
    /// </summary>
    [TestClass]
    public class ImageEdgeDetectionTest
    {



        [TestMethod]
        public void BlackAndWhiteFilterTest()
        {

            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ;

            StreamReader streamReader = new StreamReader(Path.Combine(projectFolder, @"data\red.bmp"));
            Bitmap bmp1 = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            streamReader = new StreamReader(Path.Combine(projectFolder, @"data\grey.bmp"));
            Bitmap bmp2 = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();

            bmp1 = ImageFilters.BlackWhite(bmp1);
            Assert.IsTrue(CompareBitmaps(bmp1, bmp2));
        }     
        

        public bool CompareBitmaps(Bitmap bmp1, Bitmap bmp2)
        {

            if (bmp1.Size.Height != bmp2.Size.Height || bmp1.Size.Width != bmp2.Size.Width) { return false; }

            for (int i = 0; i < bmp1.Size.Height; i++)
            {
                for (int j = 0; j < bmp1.Size.Width; j++)
                {
                    if (!bmp1.GetPixel(j, i).Equals(bmp2.GetPixel(j, i)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
