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
        const int imageSizeX = 2498;
        const int imageSizeY = 675;
        const float imageSizeZ = 2.500f;//in inches
        const float sphereSizeZ = .3f; //radius in images


        float numImages = 0; //float becuase lost inches want to have every single layer and an acuracy of at most +-layerSize

        const int sphereOneOffset = 499;
        const int sphereTwoOffset = 1249;
        const int sphereThreeOffset = 1999;

        const int sphereOneColor = 93;
        const int sphereTwoColor = 58;
        const int sphereThreeColor = 208;

        const int backgroundColor = 41;
        const int maxColor = 255;

        const int sphereSize = 180;

        const int bluePixel = 0;
        const int greenPixel = 1;
        const int redPixel = 2;

        public MainWindow()
        {
            InitializeComponent();

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
            int sphereOffsetY = imageSizeY / 2;
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


                WriteableBitmap bitmapImages = new WriteableBitmap(imageSizeX, imageSizeY, (double)600, (double)600, PixelFormats.Gray8, palette);
                WriteableBitmap bitmapImagesW = new WriteableBitmap(imageSizeX, imageSizeY, (double)600, (double)600, PixelFormats.Gray8, palette);


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
                    //--------------------------------------
                    //start for loop for number of images here

                    //4.626in (2498px) X
                    //1.126in (675px) Y
                    //1.126in (161px) Z

                    //background 292929 (backgroundColor)
                    //circle 1 (left) 5d5d5d (sphereOneColor) 
                    //circle 2 (middle) 3a3a3a (sphereTwoColor)
                    //circle 3 (right) d0d0d0 (sphereThreeColor)



                    for (int i = 0; i < numImages; i++)
                    {
                        for (int locationIndex = 0, colorIndex = 0; colorIndex < pixels.Length; locationIndex++, colorIndex += 1)
                        {


                            int centerImage = (int)numImages / 2;
                            int columnLocation = locationIndex % imageSizeX;
                            int rowLocation = (int)(locationIndex / imageSizeX);
                            int sphereOffsetY = imageSizeY / 2;

                            pixels[colorIndex] = backgroundColor;
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

                            int centerImage = (int)numImages / 2;
                            int sphereOffsetY = imageSizeY / 2;
                            int columnLocation = locationIndex % imageSizeX;
                            int rowLocation = (int)(locationIndex / imageSizeX);

                            pixels[colorIndex] = backgroundColor;
                            pixelsW[colorIndex] = maxColor;

                            int sphereOffset = sphereOneOffset;
                            if (currentImage > centerImage - (sphereSizeZ / layerSize) && currentImage < centerImage + (sphereSizeZ / layerSize) && columnLocation == sphereOffset && rowLocation == sphereOffsetY)
                            {
                                int x, y, r2;

                                /*r2 = sphereSize * sphereSize;
                                for (x = -sphereSize; x <= sphereSize; x++)
                                {
                                    y = (int)(Math.Sqrt(r2 - x * x) + .5);
                                    pixels[(int)((sphereOffset + x) + ((sphereOffsetY + y) * imageSizeX))] = sphereOneColor;
                                    pixels[(int)((sphereOffset + x) + ((sphereOffsetY - y) * imageSizeX))] = sphereOneColor;
                                }*/
                                //pixels[colorIndex] = sphereOneColor;
                                //pixelsW[colorIndex] = maxColor;

                                r2 = sphereSize * sphereSize;
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


                                for (int testX = 0; testX < imageSizeX; testX++)
                                {
                                    int hitColor = backgroundColor;
                                    int sphereStart = 0;
                                    int sphereEnd = 0;
                                    int sphereMiddle = 0;
                                    for (int testY = 0; testY < imageSizeY; testY++)
                                    {
                                        if (pixels[testY] != (byte)backgroundColor)
                                        {
                                            if (sphereStart == 0)
                                            {
                                                hitColor = pixels[testY];
                                                sphereStart = testY;
                                            }
                                            else if (sphereMiddle != 0)
                                            {
                                                sphereEnd = testY;
                                            }
                                        }
                                        else if (pixels[testY] == (byte)backgroundColor)
                                        {
                                            if (sphereMiddle == 0)
                                            {
                                                sphereMiddle = testY;
                                            }
                                        }

                                    }
                                    for (int colorY = sphereStart; colorY < sphereEnd; colorY++)
                                    {
                                        pixels[colorY + (testX * imageSizeX)] = (byte)hitColor;
                                    }
                                }
                            }
                        }
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


                        System.IO.File.WriteAllLines(imagesDirectory + "\\images\\forPrint\\images.z2c", z2cFile);
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
        }
    }
}
