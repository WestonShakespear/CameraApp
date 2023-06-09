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
        private event EventHandler? capture;
        private event EventHandler? init;
        //private event EventHandler? changeSettings;

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

            this.ready = true;

            this.SetupEventHandlers();
            


            this.timer = new Stopwatch();
        }


        private void SetupEventHandlers()
        {
            capture += async (o, e) =>
            {
                Mat? image = await TaskCapture();

                if (image != null)
                {
                    byte[] conImage = await TaskConvert(image);

                    this.captureComplete(image, conImage);
                }
                
            };

            init += async (o, e) =>
            {
                if (camera != null)
                    {
                    await Task.Run(() => {
                        
                        this.camera.initCamera(15, 1920, 1200);
                    });
                    this.camera.displaySettings();
                }
            };


        }




        private async Task<Mat?> TaskCapture()
        {
            if (this.camera != null)
            {      
                return await Task.Run(() => this.camera.QueryFrame());
            }

            return null;
        }

        private async Task<byte[]> TaskConvert(Mat image)
        {
            var buffer = new VectorOfByte();

            await Task.Run(() => CvInvoke.Imencode(".jpg", image, buffer));

            return await Task.Run(() => ImageResult.FromMemory(buffer.ToArray(), ColorComponents.RedGreenBlueAlpha).Data);
        }

        public void captureComplete(Mat imageMat, byte[] image0)
        {
            this.currentRawImage = image0;
            this.currentConImage = image0;
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

        public void initCamera(int fps, int width, int height)
        {
            this.ready = false;
            init?.Invoke(this, EventArgs.Empty);
            this.ready = true;
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