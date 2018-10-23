/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ImageEdgeDetection
{
    public partial class MainForm : Form
    {
        private Bitmap originalBitmap = null;
        private Bitmap previewBitmap = null;
        private Bitmap resultBitmap = null;

        private Color customColor = Color.Black;

        public MainForm()
        {
            InitializeComponent();

            cmbEdgeDetection.SelectedIndex = 0;
            cmbFilter.SelectedIndex = 0;
        }

        private void btnOpenOriginal_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file.";
            ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
            ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(ofd.FileName);
                originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                streamReader.Close();

                previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                picPreview.Image = previewBitmap;

                ApplyFilter(true);
            }
        }

        private void btnSaveNewImage_Click(object sender, EventArgs e)
        {
            ApplyFilter(false);

            if (resultBitmap != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Specify a file name and file path";
                sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                    ImageFormat imgFormat = ImageFormat.Png;

                    if (fileExtension == "BMP")
                    {
                        imgFormat = ImageFormat.Bmp;
                    }
                    else if (fileExtension == "JPG")
                    {
                        imgFormat = ImageFormat.Jpeg;
                    }

                    StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                    resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                    streamWriter.Flush();
                    streamWriter.Close();

                    resultBitmap = null;
                }
            }
        }

        private void ApplyFilter(bool preview)
        {
            if (previewBitmap == null || cmbEdgeDetection.SelectedIndex == -1)
            {
                return;
            }

            Bitmap selectedSource = null;
            Bitmap bitmapFiltered = null;
            Bitmap bitmapResult = null;

            if (preview == true)
            {
                selectedSource = previewBitmap;
            }
            else
            {
                selectedSource = originalBitmap;
            }

            if (selectedSource != null)
            {

                switch (cmbFilter.SelectedItem.ToString())
                {
                    case "None":
                        bitmapFiltered = selectedSource;
                        break;
                    case "Magic Mosaic":
                        try
                        {
                            bitmapFiltered = ImageFilters.DivideCrop(new Bitmap(selectedSource));
                        }
                        catch (System.ArgumentOutOfRangeException e)
                        {
                            bitmapFiltered = selectedSource;
                            MessageBox.Show("Image is too large");
                        }
                        break;
                    case "Zen Filter":
                        bitmapFiltered = ImageFilters.ApplyFilter(new Bitmap(selectedSource), 1, 10, 1, 1);
                        break;
                    case "Miami Filter":
                        bitmapFiltered = ImageFilters.ApplyFilter(new Bitmap(selectedSource), 1, 1, 10, 1);
                        break;
                    case "Night Filter":
                        bitmapFiltered = ImageFilters.ApplyFilter(new Bitmap(selectedSource), 1, 1, 1, 25);
                        break;
                    case "Hell Filter":
                        bitmapFiltered = ImageFilters.ApplyFilter(new Bitmap(selectedSource), 1, 1, 10, 15);
                        break;
                    case "Black and White":
                        bitmapFiltered = ImageFilters.BlackWhite(new Bitmap(selectedSource));
                        break;
                    case "Swap Filter":
                        bitmapFiltered = ImageFilters.ApplyFilterSwap(new Bitmap(selectedSource));
                        break;
                    case "Crazy Filter":
                        System.Drawing.Image te = ImageFilters.ApplyFilterSwapDivide(new Bitmap(selectedSource), 1, 1, 2, 1);
                        bitmapFiltered = ImageFilters.ApplyFilterSwap(new Bitmap(te));
                        break;
                    case "Mega Filter Green":
                        bitmapFiltered = ImageFilters.ApplyFilterMega(new Bitmap(selectedSource), 230, 110, Color.Green);
                        break;
                    case "Mega Filter Blue":
                        bitmapFiltered = ImageFilters.ApplyFilterMega(new Bitmap(selectedSource), 230, 110, Color.Blue);
                        break;
                    case "Mega Filter Orange":
                        bitmapFiltered = ImageFilters.ApplyFilterMega(new Bitmap(selectedSource), 230, 110, Color.Orange);
                        break;
                    case "Mega Filter Pink":
                        bitmapFiltered = ImageFilters.ApplyFilterMega(new Bitmap(selectedSource), 230, 110, Color.Pink);
                        break;
                    case "Mega Filter Custom":
                        bitmapFiltered = ImageFilters.ApplyFilterMega(new Bitmap(selectedSource), 230, 110, customColor);
                        break;
                    case "Rainbow Filter":
                        bitmapFiltered = ImageFilters.RainbowFilter(new Bitmap(selectedSource));
                        break;
                }

                switch (cmbEdgeDetection.SelectedItem.ToString())
                {
                    case "None":
                        bitmapResult = bitmapFiltered;
                        break;

                    case "Laplacian 3x3":
                        bitmapResult = bitmapFiltered.Laplacian3x3Filter(false);
                        break;

                    case "Laplacian 3x3 Grayscale":
                        bitmapResult = bitmapFiltered.Laplacian3x3Filter(true);
                        break;

                    case "Laplacian 5x5":
                        bitmapResult = bitmapFiltered.Laplacian5x5Filter(false);
                        break;

                    case "Laplacian 5x5 Grayscale":
                        bitmapResult = bitmapFiltered.Laplacian5x5Filter(true);
                        break;

                    case "Laplacian of Gaussian":
                        bitmapResult = bitmapFiltered.LaplacianOfGaussianFilter();
                        break;
                    case "Laplacian 3x3 of Gaussian 3x3":
                        bitmapResult = bitmapFiltered.Laplacian3x3OfGaussian3x3Filter();
                        break;

                    case "Laplacian 3x3 of Gaussian 5x5 - 1":
                        bitmapResult = bitmapFiltered.Laplacian3x3OfGaussian5x5Filter1();
                        break;

                    case "Laplacian 3x3 of Gaussian 5x5 - 2":
                        bitmapResult = bitmapFiltered.Laplacian3x3OfGaussian5x5Filter2();
                        break;

                    case "Laplacian 5x5 of Gaussian 3x3":
                        bitmapResult = bitmapFiltered.Laplacian5x5OfGaussian3x3Filter();
                        break;

                    case "Laplacian 5x5 of Gaussian 5x5 - 1":
                        bitmapResult = bitmapFiltered.Laplacian5x5OfGaussian5x5Filter1();
                        break;

                    case "Laplacian 5x5 of Gaussian 5x5 - 2":
                        bitmapResult = bitmapFiltered.Laplacian5x5OfGaussian5x5Filter2();
                        break;

                    case "Sobel 3x3":
                        bitmapResult = bitmapFiltered.Sobel3x3Filter(false);
                        break;

                    case "Sobel 3x3 Grayscale":
                        bitmapResult = bitmapFiltered.Sobel3x3Filter();
                        break;

                    case "Prewitt":
                        bitmapResult = bitmapFiltered.PrewittFilter(false);
                        break;

                    case "Prewitt Grayscale":
                        bitmapResult = bitmapFiltered.PrewittFilter();
                        break;

                    case "Kirsch":
                        bitmapResult = bitmapFiltered.KirschFilter(false);
                        break;

                    case "Kirsch Grayscale":
                        bitmapResult = bitmapFiltered.KirschFilter();
                        break;
                }                
            }

            if (bitmapResult != null)
            {
                if (preview == true)
                {
                    picPreview.Image = bitmapResult;
                }
                else
                {
                    resultBitmap = bitmapResult;
                }
            }
        }

        private void NeighbourCountValueChangedEventHandler(object sender, EventArgs e)
        {
            ApplyFilter(true);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.SelectedItem.ToString() == "Mega Filter Custom")
            {
                ColorDialog colorDialog = new ColorDialog();
                colorDialog.ShowDialog();
                customColor = colorDialog.Color;
            }
            ApplyFilter(true);
        }
    }
}
