using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

using System.Drawing;
using System.Diagnostics;
using StbImageSharp;

namespace testOne
{
    public class CameraInput
    {
        private event EventHandler capture;

        private byte[]? currentRawImage;
        private byte[]? currentConImage;

        private Mat? currentMat;

        private VideoCapture? camera;

        private bool ready;

        private int a = 50;
        private int b = 100;


        Stopwatch timer;

        

        public CameraInput(int _cameraIndex)
        {
            this.camera = new VideoCapture(_cameraIndex);

            
            this.camera.initCamera(15, 1920, 1200);
            this.camera.displaySettings();
            
            

            this.ready = true;

            capture += async (o, e) =>
            {
                if (this.camera != null)
                {
                    Mat newImage = await Task.Run(() => this.camera.QueryFrame());
                    Mat conImage = await Task.Run(() => this.convert(newImage));
                   

                    byte[] image0 = await Task.Run(() => this.convertImage(newImage));
                    byte[] image1 = await Task.Run(() => this.convertImage(conImage));

                    this.captureComplete(newImage, image0, image1);
                }
            };

            this.timer = new Stopwatch();
        }

        public void startTimer()
        {
            this.timer.Reset();
            this.timer.Start();
        }

        public void stopTimer()
        {
            this.timer.Stop();
            TimeSpan ts = this.timer.Elapsed;

            string elapsedTime = String.Format("{0:00}",
            ts.Milliseconds);

            Console.WriteLine(elapsedTime);
        }

        private byte[] convertImage(Mat image)
        {
            var buffer = new VectorOfByte();
            CvInvoke.Imencode(".jpg", image, buffer);

            ImageResult output = ImageResult.FromMemory(buffer.ToArray(), ColorComponents.RedGreenBlueAlpha);

            return output.Data;
        }

        public void captureComplete(Mat imageMat, byte[] image0, byte[] image1)
        {
            this.currentRawImage = image0;
            this.currentConImage = image1;
            this.currentMat = imageMat;
            this.ready = true;
        }

        private Mat convert(Mat image)
        {
            Mat imgGrayscale = new Mat(image.Size, DepthType.Cv8U, 1);
            Mat imgBlurred = new Mat(image.Size, DepthType.Cv8U, 1);
            Mat imgCanny = new Mat(image.Size, DepthType.Cv8U, 1);

            CvInvoke.CvtColor(image, imgGrayscale, ColorConversion.Bgr2Gray);

            CvInvoke.GaussianBlur(imgGrayscale, imgBlurred, new Size(5, 5), 1.5);

            CvInvoke.Canny(imgBlurred, imgCanny, this.a, this.b);

            return imgCanny;
        }

        public void captureImage(int _a, int _b)
        {
            this.a = _a;
            this.b = _b;
            

            if (this.ready == true)
            {
                this.ready = false;
                capture?.Invoke(this, EventArgs.Empty);
            } 
        }

        public void getImage(out byte[]? rawImage, out byte[]? conImage, out int width, out int height)
        {
            rawImage = null;
            conImage = null;
            width = 0;
            height = 0;
            
            if (this.currentMat != null && this.currentRawImage != null && this.currentConImage != null)
            {
                width = this.currentMat.Width;
                height = this.currentMat.Height;

                rawImage = this.currentRawImage;
                conImage = this.currentConImage;
            }
        }
    }
}