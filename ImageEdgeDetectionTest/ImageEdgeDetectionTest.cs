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
            ImageConverter converter = new ImageConverter();
            byte[] byte1 = converter.ConvertTo(bmp1, typeof(byte[])) as byte[];
            byte[] byte2 = converter.ConvertTo(bmp2, typeof(byte[])) as byte[];

            //MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            
            if (byte1.Equals(byte2))
            {
                return true;
            }

            return false;
        }
    }
}
