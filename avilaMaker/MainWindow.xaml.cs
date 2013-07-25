using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace avilaMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        string imageDeleteLocation = Directory.GetCurrentDirectory() + "\\images\\";
        string imageCopyLocation = Directory.GetCurrentDirectory() + "\\toPrint\\";

        float layerSize = 0.000f;
        const int imageSizeX = 2832;
        const int baseImageSizeY = 708;
        const int gapSize = 600;
        const int imageSizeY = (baseImageSizeY * 3) + (gapSize * 2);
        const float imageSizeZ = 1.18f;//in inches
        const float sphereSizeZ = .29f; //radius in imnches


        float numImages = 0; //float becuase lost inches want to have every single layer and an acuracy of at most +-layerSize

        const int sphereOneOffset = 588;
        const int sphereTwoOffset = 1416;
        const int sphereThreeOffset = 2244;

        const int sphereOneColorGlobal = 93;
        const int sphereTwoColorGlobal = 58;
        const int sphereThreeColorGlobal = 208;

        const byte colorOffsetLower1 = 15;
        const byte colorOffsetLower2 = 15;
        const byte colorOffsetLower3 = 10;

        const byte colorOffsetUpper1 = 10;
        const byte colorOffsetUpper2 = 10;
        const byte colorOffsetUpper3 = 20;

        const int backgroundColor = 41;
        const int maxColor = 255;

        const int sphereSizeGlobal = 177; //radius Diameter is 354

        const int bluePixel = 0;
        const int greenPixel = 1;
        const int redPixel = 2;

        public MainWindow()
        {
            InitializeComponent();

        }

        public int getRadius(int i, byte[] cicle)
        {
            int chord = i;
            int imageSizeY = (int) numImages;

            int radiusStart = 0;
            int radiusEnd = 0;
            //for (int testY = 0; testY < imageSizeY; testY++)
            //{
            int testY = i;
                for (int testX = testY * imageSizeY; testX < (testY * imageSizeY) + imageSizeY; testX++)
                {
                    if (cicle[testX] == (byte)sphereOneColorGlobal)
                    {
                        if (radiusStart == 0)
                        {
                            radiusStart = testX;
                        }
                        else
                        {
                            radiusEnd = testX;
                        }
                    }
                }
                return (int) ((radiusEnd - radiusStart) * 1.25);
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public bool isSphere(int sphereOffset, int pixelLocation, int currentImage)
        {
            //+-1.25in from center 


            //currentImage++;
            const int sphereSize = 180; //radius
            const float sphereSizeZ = .3f; //radius in images

            int centerImage = (int)numImages / 2;
            int sphereOffsetY = baseImageSizeY / 2;
            int columnLocation = pixelLocation % imageSizeX;
            int rowLocation = (int)(pixelLocation / imageSizeX);

            //if within +- .3in in Z                                                                                               //and if within +- .3in in X                                                               //and if within +- .3in in y
            if (currentImage > centerImage - (sphereSizeZ / layerSize) && currentImage < centerImage + (sphereSizeZ / layerSize) && columnLocation > sphereOffset - sphereSize && columnLocation < sphereOffset + sphereSize && rowLocation > sphereOffsetY - sphereSize && rowLocation < sphereOffsetY + sphereSize)
            {
                return true;
            }
            return false;
        }

        public void avilaImages()
        {
            try
            {
                
                Stream imageStreamSource = new FileStream("base.bmp", FileMode.Open, FileAccess.Read, FileShare.Read);


                BmpBitmapDecoder baseImage = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource baseImageSource = baseImage.Frames[0];


                //List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
                //colors.Add(System.Windows.Media.Colors.Red);
                //colors.Add(System.Windows.Media.Colors.Blue);
                //colors.Add(System.Windows.Media.Colors.Green);
                BitmapPalette palette = new BitmapPalette(baseImageSource, 8);


                WriteableBitmap bitmapImages = new WriteableBitmap(imageSizeX, imageSizeY, (double)600, (double)560, PixelFormats.Gray8, palette);
                WriteableBitmap bitmapImagesW = new WriteableBitmap(imageSizeX, imageSizeY, (double)600, (double)560, PixelFormats.Gray8, palette);


                string photoSaveLocation = Directory.GetCurrentDirectory();
                photoSaveLocation = photoSaveLocation + "\\images\\";

                imageDeleteLocation = photoSaveLocation;
                if (Directory.Exists(photoSaveLocation))
                {
                    Directory.Delete(photoSaveLocation, true);
                }
                Directory.CreateDirectory(photoSaveLocation);
                if (Directory.Exists(imageCopyLocation))
                {
                    Directory.Delete(imageCopyLocation, true);
                }
                Directory.CreateDirectory(imageCopyLocation);


                const int stride = imageSizeX;
                byte[] pixels = new byte[imageSizeX * imageSizeY];
                byte[] pixelsW = new byte[imageSizeX * imageSizeY];
                int currentImage = 0;
                if (layerSize < .001f)
                {
                    textBlock3.Text = "Layer size is too small";
                }
                else if (layerSize > .009f)
                {
                    textBlock3.Text = "Layer size is too large";
                }
                else
                {
                    numImages = imageSizeZ / layerSize;

                    int centerImage = (int)numImages / 2;
                    int sphereOffsetY = baseImageSizeY / 2;


                    int x, y, r2;


                    int pixelsX = (int)numImages;

                    byte[] circlePixels = new byte[pixelsX * pixelsX];

                    int offset = (int)(numImages) / 2;
                    int radius = (int)(sphereSizeZ / layerSize);
                    //int offsetY = radius;

                    byte sphereOneColor = sphereOneColorGlobal;

                    r2 = (int)radius * (int)radius;

                    circlePixels[(int)(offset + (offset + radius) * pixelsX)] = sphereOneColor;
                    circlePixels[(int)(offset + (offset - radius) * pixelsX)] = sphereOneColor;
                    circlePixels[(int)(offset + radius + (offset) * pixelsX)] = sphereOneColor;
                    circlePixels[(int)(offset - radius + (offset) * pixelsX)] = sphereOneColor;

                    y = radius;
                    x = 1;
                    y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                    while (x < y)
                    {
                        circlePixels[(int)(offset + x + (offset + y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset + x + (offset - y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - x + (offset + y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - x + (offset - y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset + y + (offset + x) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset + y + (offset - x) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - y + (offset + x) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - y + (offset - x) * pixelsX)] = sphereOneColor;
                        x += 1;
                        y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                    }
                    if (x == y)
                    {
                        circlePixels[(int)(offset + x + (offset + y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset + x + (offset - y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - x + (offset + y) * pixelsX)] = sphereOneColor;
                        circlePixels[(int)(offset - x + (offset - y) * pixelsX)] = sphereOneColor;
                    }


                    WriteableBitmap view3D = new WriteableBitmap((int)pixelsX, (int)pixelsX, (double)600, (double)600, PixelFormats.Gray8, palette);
                    view3D.WritePixels(new Int32Rect(0, 0, pixelsX, pixelsX), circlePixels, pixelsX, 0);

                    BitmapEncoder view3DEnc = new BmpBitmapEncoder();
                    view3DEnc.Frames.Add(BitmapFrame.Create(view3D));

                    using (FileStream fs = new FileStream(photoSaveLocation + "view3D.bmp", FileMode.Create, FileAccess.Write))
                    {
                        view3DEnc.Save(fs);
                    }

                    for (int i = 0; i < numImages; i++)
                    {
                        for (int locationIndex = 0, colorIndex = 0; colorIndex < pixels.Length; locationIndex++, colorIndex += 1)
                        {
                            int columnLocation = locationIndex % imageSizeX;
                            int rowLocation = (int)(locationIndex / imageSizeX);

                            if (locationIndex >= (baseImageSizeY) * imageSizeX && locationIndex <= (baseImageSizeY + gapSize) * imageSizeX || locationIndex >= (baseImageSizeY * 2 + gapSize) * imageSizeX && locationIndex <= (baseImageSizeY * 2 + gapSize + gapSize) * imageSizeX)
                            {
                                pixels[colorIndex] = 0;
                            }
                            else
                            {
                                if (locationIndex < baseImageSizeY * imageSizeX)
                                {
                                    pixels[colorIndex] = backgroundColor;
                                }
                                else if (locationIndex < (baseImageSizeY * 2 + gapSize) * imageSizeX )
                                {
                                    pixels[colorIndex] = backgroundColor - colorOffsetLower1;
                                }
                                else if (locationIndex < (baseImageSizeY * 3 + gapSize + gapSize) * imageSizeX)
                                {
                                    pixels[colorIndex] = backgroundColor + colorOffsetUpper1;
                                }
                            }
                            pixelsW[colorIndex] = maxColor;
                        }

                        //bitmapImages.WritePixels(new Int32Rect(0, 0, imageSizeX, imageSizeY), pixels, stride, 0);
                        //Bitmap bmp;

                        //using (MemoryStream outStream = new MemoryStream())
                        //{
                        //    BitmapEncoder enc = new BmpBitmapEncoder();
                        //    enc.Frames.Add(BitmapFrame.Create((bitmapImages)));
                        //    enc.Save(outStream);
                        //    bmp = new System.Drawing.Bitmap(outStream);
                        //}



                        //Graphics graphicsProccesor = Graphics.FromImage(bmp);

                        //System.Drawing.Pen backgroudPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(backgroundColor, backgroundColor, backgroundColor));

                        //System.Drawing.Pen sphere1Pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(sphereOneColor, sphereOneColor, sphereOneColor));
                        //System.Drawing.Pen sphere2Pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(sphereTwoColor, sphereTwoColor, sphereTwoColor));
                        //System.Drawing.Pen sphere3Pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(sphereThreeColor, sphereThreeColor, sphereThreeColor));

                        //graphicsProccesor.DrawEllipse(sphere1Pen, sphereOneOffset, (imageSizeY / 2), sphereSize, sphereSize);

                        //for (int x = 0; x < imageSizeX; x++)
                        //    for (int y = 0; y < imageSizeY; y++)
                        //    {
                        //        bmp.GetPixel(x, y).ToString();
                        //    }


                        for (int locationIndex = 0, colorIndex = 0; colorIndex < pixels.Length; locationIndex++, colorIndex += 1)
                        {

                            int columnLocation = locationIndex % imageSizeX;
                            int rowLocation = (int)(locationIndex / imageSizeX);

                            

                            //pixels[colorIndex] = backgroundColor;
                            //pixelsW[colorIndex] = maxColor;

                            int sphereSize = 0;

                            int sphereOffset = sphereOneOffset;

                            //if (sphereSize > 0)
                            if (currentImage > centerImage - (sphereSizeZ / layerSize) && currentImage < centerImage + (sphereSizeZ / layerSize) && columnLocation == sphereOffset && rowLocation == sphereOffsetY)
                            {
                                sphereSize = (getRadius(currentImage, circlePixels));
                                
                                r2 = sphereSize * sphereSize;
                                sphereOneColor = sphereOneColorGlobal;
                                byte sphereTwoColor = sphereTwoColorGlobal;
                                byte sphereThreeColor = sphereThreeColorGlobal;


                                pixels[(int)(sphereOffset + (sphereOffsetY + sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + (sphereOffsetY - sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + sphereSize + (sphereOffsetY) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset - sphereSize + (sphereOffsetY) * imageSizeX)] = sphereOneColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                }
                            


                                sphereOffset = sphereTwoOffset;
                                r2 = sphereSize * sphereSize;

                                pixels[(int)(sphereOffset + (sphereOffsetY + sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + (sphereOffsetY - sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + sphereSize + (sphereOffsetY) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset - sphereSize + (sphereOffsetY) * imageSizeX)] = sphereTwoColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                }
                            


                                sphereOffset = sphereThreeOffset;
                                r2 = sphereSize * sphereSize;

                                pixels[(int)(sphereOffset + (sphereOffsetY + sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + (sphereOffsetY - sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + sphereSize + (sphereOffsetY) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset - sphereSize + (sphereOffsetY) * imageSizeX)] = sphereThreeColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (sphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (sphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (sphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (sphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                }


                                //new circle sets 2 and 3 

                                int localSphereOffsetY = (baseImageSizeY / 2) + baseImageSizeY + gapSize;
                                sphereOffset = sphereOneOffset;
                                sphereOneColor = sphereOneColorGlobal - colorOffsetLower1;
                                sphereTwoColor = sphereTwoColorGlobal - colorOffsetLower2;
                                sphereThreeColor = sphereThreeColorGlobal - colorOffsetLower3;

                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereOneColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                }



                                sphereOffset = sphereTwoOffset;
                                r2 = sphereSize * sphereSize;

                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereTwoColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                }



                                sphereOffset = sphereThreeOffset;
                                r2 = sphereSize * sphereSize;
                                
                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereThreeColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                }


                                //circle set 3

                                localSphereOffsetY = (baseImageSizeY / 2) + baseImageSizeY + gapSize + baseImageSizeY + gapSize;
                                sphereOffset = sphereOneOffset;
                                sphereOneColor = sphereOneColorGlobal + colorOffsetUpper1;
                                sphereTwoColor = sphereTwoColorGlobal + colorOffsetUpper2;
                                sphereThreeColor = sphereThreeColorGlobal + colorOffsetUpper3;

                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereOneColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereOneColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereOneColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereOneColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereOneColor;
                                }



                                sphereOffset = sphereTwoOffset;
                                r2 = sphereSize * sphereSize;


                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereTwoColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereTwoColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereTwoColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereTwoColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereTwoColor;
                                }



                                sphereOffset = sphereThreeOffset;
                                r2 = sphereSize * sphereSize;

                                pixels[(int)(sphereOffset + (localSphereOffsetY + sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + (localSphereOffsetY - sphereSize) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset + sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereThreeColor;
                                pixels[(int)(sphereOffset - sphereSize + (localSphereOffsetY) * imageSizeX)] = sphereThreeColor;

                                y = sphereSize;
                                x = 1;
                                y = (int)(Math.Sqrt(r2 - 1) + 0.5);
                                while (x < y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + y + (localSphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY + x) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - y + (localSphereOffsetY - x) * imageSizeX)] = sphereThreeColor;
                                    x += 1;
                                    y = (int)(Math.Sqrt(r2 - x * x) + 0.5);
                                }
                                if (x == y)
                                {
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset + x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY + y) * imageSizeX)] = sphereThreeColor;
                                    pixels[(int)(sphereOffset - x + (localSphereOffsetY - y) * imageSizeX)] = sphereThreeColor;
                                }
                            }
                        }
                            //if (currentImage > centerImage - (sphereSizeZ / layerSize) && currentImage < centerImage + (sphereSizeZ / layerSize))
                            //{
                        for (int testY = 0; testY < imageSizeY; testY++)
                                {
                                    int hitColor = backgroundColor;
                                    int hitEndColor = backgroundColor;
                                    int sphereStart = 0;
                                    int sphereEnd = 0;
                                    int sphereMiddle = 0;
                                    for (int testX = testY * imageSizeX; testX < (testY * imageSizeX) + imageSizeX; testX++)
                                    {
                                        if (pixels[testX] != backgroundColor && pixels[testX] != backgroundColor - colorOffsetLower1 && pixels[testX] != backgroundColor + colorOffsetUpper1)
                                        {
                                            if (sphereStart == 0)
                                            {
                                                hitColor = pixels[testX];
                                                sphereStart = testX;
                                            }
                                            else if (sphereMiddle != 0)
                                            {
                                                hitEndColor = pixels[testX];
                                                sphereEnd = testX;
                                            }
                                        }
                                        if (pixels[testX] == backgroundColor || pixels[testX] == backgroundColor - colorOffsetLower1 || pixels[testX] == backgroundColor + colorOffsetUpper1)
                                        {
                                            if (sphereStart != 0)
                                            {
                                                if (sphereMiddle == 0)
                                                {
                                                    sphereMiddle = testX;
                                                }
                                                else if (sphereMiddle != 0)
                                                {
                                                    if (sphereEnd != 0)
                                                    {
                                                        if (hitEndColor == hitColor)
                                                        {
                                                            for (int colorY = sphereStart; colorY < sphereEnd; colorY++)
                                                            {
                                                                pixels[colorY] = (byte)hitColor;
                                                            }
                                                        }
                                                        hitColor = backgroundColor;
                                                        sphereStart = 0;
                                                        sphereEnd = 0;
                                                        sphereMiddle = 0;
                                                        
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        //    }
                        //}
                        //    sphereOffset = sphereTwoOffset;
                        //    if (currentImage == centerImage - (sphereSizeZ / layerSize) && columnLocation == sphereOffset - sphereSize && rowLocation == sphereOffsetY - sphereSize)
                        //    {
                        //        pixels[colorIndex] = sphereTwoColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }
                        //    sphereOffset = sphereThreeOffset;
                        //    if (currentImage == centerImage - (sphereSizeZ / layerSize) && columnLocation == sphereOffset - sphereSize && rowLocation == sphereOffsetY - sphereSize)
                        //    {
                        //        pixels[colorIndex] = sphereThreeColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }

                        //    if within +- .3in in Z    
                        //    if (isSphere(sphereOneOffset, locationIndex, i)) //left circle
                        //    {
                        //        pixels[colorIndex] = sphereOneColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }
                        //    else if (isSphere(sphereTwoOffset, locationIndex, i)) //middle circle
                        //    {
                        //        pixels[colorIndex] = sphereTwoColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }
                        //    else if (isSphere(sphereThreeOffset, locationIndex, i)) //right circle
                        //    {
                        //        pixels[colorIndex] = sphereThreeColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }
                        //    else
                        //    {
                        //        pixels[colorIndex] = backgroundColor;
                        //        pixelsW[colorIndex] = maxColor;
                        //    }
                        //}

                        ////Test loop
                        //for (int j = 0; j < pixels.Length; j++)
                        //{
                        //    pixels[j] = 0;
                        //    pixelsW[j] = maxColor;
                        //}

                        for (int pixelsCount = 0; pixelsCount < pixels.Length; pixelsCount++)
                        {
                            if (pixels[pixelsCount] != 0)
                            {
                                pixelsW[pixelsCount] = (byte)((byte)(255) - pixels[pixelsCount]);
                            }
                            else
                            {
                                pixelsW[pixelsCount] = 0;
                            }
                        }

                        bitmapImages.WritePixels(new Int32Rect(0, 0, imageSizeX, imageSizeY), pixels, stride, 0);

                        image1.Source = bitmapImages;



                        BitmapEncoder avilaImages = new BmpBitmapEncoder();
                        avilaImages.Frames.Add(BitmapFrame.Create(bitmapImages));


                        bitmapImagesW.WritePixels(new Int32Rect(0, 0, imageSizeX, imageSizeY), pixelsW, stride, 0);

                        BitmapEncoder avilaImagesW = new BmpBitmapEncoder();
                        avilaImagesW.Frames.Add(BitmapFrame.Create(bitmapImagesW));


                        try
                        {
                            int intNumImages = (int)numImages;
                            string currentImageName = currentImage.ToString().PadLeft(intNumImages.ToString().Length, '0') + ".bmp";
                            using (FileStream fs = new FileStream(photoSaveLocation + currentImageName, FileMode.Create, FileAccess.Write))
                            {
                                avilaImages.Save(fs);
                                textBlock4.Text = "Image base number " + currentImage + " created";
                            }
                            string currentImageNameW = currentImage.ToString().PadLeft(intNumImages.ToString().Length, '0') + "_w.bmp";
                            using (FileStream fs = new FileStream(photoSaveLocation + currentImageNameW, FileMode.Create, FileAccess.Write))
                            {
                                avilaImagesW.Save(fs);
                                textBlock4.Text = "Image base inverted number " + currentImage + " created";
                            }
                            currentImage++;
                        }
                        catch (IOException)
                        {
                            textBlock3.Text = "IO Exception caught, is //images// write locked?";
                        }
                    }
                }
                //end loop here
                //--------------------------------------
                imageStreamSource.Close();
                textBlock4.Text = currentImage + " images were created successfully";

            }
            catch (IOException)
            {
                textBlock3.Text = "IO Exception caught, does image base.bmp exist in runtime directory?";
            }

            //---------------------------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------------------------
            //----------------------------------------This Code is from bmpCopyPasta-----------------------------------------------
            //-------The following code is from a script I made earlier to copy a series of images to be print ready---------------
            //-------it requires a series of images that end with _w and for every one of these there is an image without _w-------
            //---------------------------------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------------------------------

            int numImagesCopy = 0;

            string[] oldPathAndNameRGB = new string[50];
            //string[] newPathAndNameRGB = new string[50];

            string[] oldPathAndNameW;
            //string[] newPathAndNameW = new string[50];

            string[] errorLocations = new string[50];

            string[] z2cFile = new string[3];

            string imagesDirectory = imageCopyLocation;

            string blankImage = imagesDirectory + "\\blank.bmp"; //Black Image/RGB
            string baseImageCopy = imagesDirectory + "\\base.bmp"; //White Image/W

            try
            {
                /*numImagesRGB = Directory.GetFiles(imagesDirectory, "*.bmp", SearchOption.TopDirectoryOnly).Length; //count the number of .jpg files in the directory found above  
                oldPathAndNameRGB = Directory.GetFiles(imagesDirectory, "*.bmp", SearchOption.TopDirectoryOnly);*/

                numImagesCopy = Directory.GetFiles(imageDeleteLocation, "*_w.bmp", SearchOption.TopDirectoryOnly).Length; //count the number of .jpg files in the directory found above  
                oldPathAndNameW = Directory.GetFiles(imageDeleteLocation, "*_w.bmp", SearchOption.TopDirectoryOnly);

                if (numImagesCopy != 0)
                {
                    oldPathAndNameRGB = new string[numImagesCopy];
                    errorLocations = new string[numImagesCopy];
                }
                for (int i = 0; i < numImagesCopy; i++)
                {
                    oldPathAndNameRGB[i] = oldPathAndNameW[i].Substring(0, oldPathAndNameW[i].Length - 6) + ".bmp";
                }


                //if (Directory.Exists(imagesDirectory))
                //{
                //    Directory.Delete(imagesDirectory, true);
                //}

                //System.IO.Directory.CreateDirectory(imagesDirectory);

                for (int i = 0; i < numImagesCopy; i++)
                {
                    if (File.Exists(oldPathAndNameRGB[i]) && File.Exists(oldPathAndNameW[i]))
                    {

                    }
                    else
                    {
                        for (int j = 0; j < errorLocations.Length; j++)
                        {
                            errorLocations[j] = "There was an issue with image number " + i;
                        }
                    }
                }

                if (errorLocations[0] == null)
                {

                    for (int i = 0; i < numImagesCopy; i++)
                    {
                        string imageName = "Layer" + i.ToString().PadLeft(/*numImagesRGB.ToString().Length*/ 4, '0'); //May need to just be 4 later

                        System.IO.File.Copy(oldPathAndNameRGB[i], imagesDirectory + imageName + "_c.bmp");
                        System.IO.File.Copy(oldPathAndNameRGB[i], imagesDirectory + imageName + "_m.bmp");
                        System.IO.File.Copy(oldPathAndNameRGB[i], imagesDirectory + imageName + "_y.bmp");

                        //System.IO.File.Copy(oldPathAndNameW[i], imagesDirectory + "\\images\\" + imageName + "_c.bmp");//transition code
                        //System.IO.File.Copy(oldPathAndNameW[i], imagesDirectory + "\\images\\" + imageName + "_m.bmp");//transition code
                        //System.IO.File.Copy(oldPathAndNameW[i], imagesDirectory + "\\images\\" + imageName + "_y.bmp");//transition code

                        //System.IO.File.Copy(blankImage, imagesDirectory + "\\images\\" + imageName + "_c.bmp");//davelike test code
                        //System.IO.File.Copy(blankImage, imagesDirectory + "\\images\\" + imageName + "_m.bmp");//davelike test code
                        //System.IO.File.Copy(blankImage, imagesDirectory + "\\images\\" + imageName + "_y.bmp");//davelike test code

                        textBlock4.Text = "CMY of layer: " + i + " is done";

                        System.IO.File.Copy(oldPathAndNameW[i], imagesDirectory + imageName + "_w.bmp");

                        //System.IO.File.Copy(oldPathAndNameRGB[i], imagesDirectory + "\\images\\" + imageName + "_w.bmp"); //davelike test code

                        //System.IO.File.Copy(baseImage, imagesDirectory + "\\images\\" + imageName + "_w.bmp"); //davelike test code

                        textBlock4.Text = "W of layer: " + i + " is done";

                        //System.IO.File.Copy(oldPathAndNameW[i], imagesDirectory + "\\images\\" + imageName + "_k.bmp"); //davelike test code

                        //System.Console.WriteLine("K of layer: " + i + " is done"); //davelike test code


                    }

                    System.Console.WriteLine("All Done Copying Images");

                    if (numImagesCopy != 0)
                    {
                        z2cFile[0] = "inches";
                        z2cFile[1] = ".007";
                        z2cFile[2] = numImagesCopy.ToString();


                        System.IO.File.WriteAllLines(imagesDirectory + "\\images.z2c", z2cFile);
                        textBlock4.Text = "All Done Creating .z2c file";
                    }
                }

                else
                {
                    for (int i = 0; i < errorLocations.Length; i++)
                    {
                        if (errorLocations[i] != null)
                        {
                            System.Console.WriteLine(errorLocations[i]);
                        }
                    }
                }
            }
            catch
            {
                textBlock4.Text = "Exception caught, the folder \\forPrint\\ may be write locked.";
            }



            System.Console.WriteLine("The number of images found was " + numImagesCopy + ". Press enter to finish");
            Console.ReadLine();

        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            layerSize = .004f;
            avilaImages();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            layerSize = .007f;
            avilaImages();
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                layerSize = Convert.ToSingle(textBox1.Text.ToString());
                textBlock3.Visibility = System.Windows.Visibility.Hidden;
                avilaImages();
            }
            catch (FormatException)
            {
                textBlock3.Visibility = System.Windows.Visibility.Visible;
                textBlock3.Text = "Unable to convert " + textBox1.Text + " to a Single.";
            }
            catch (OverflowException)
            {
                textBlock3.Visibility = System.Windows.Visibility.Visible;
                textBlock3.Text = textBox1.Text + " is outside the range of a Single.";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Directory.Exists(imageDeleteLocation))
                {
                    Directory.Delete(imageDeleteLocation, true);
                }
                if (Directory.Exists(imageCopyLocation))
                {
                    Directory.Delete(imageCopyLocation, true);
                }
                radioButton1.IsChecked = false;
                radioButton2.IsChecked = false;
                radioButton3.IsChecked = false;
                textBlock4.Text = "";
            }
            catch (IOException)
            {
                textBlock4.Text = "Delete failed";
            }
        }
    }
}
