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

        private int camFPS = 15;
        private int camWidth = 1920;
        private int camHeight = 1080;

        private int camIndex = 0;

        private string[] reportList;

        
        private VideoCapture.API api = VideoCapture.API.DShow;
        

        public CameraInput(int _cameraIndex)
        {
            this.camIndex = _cameraIndex;
            this.camera = new VideoCapture(this.camIndex, api);
            this.camera.initCamera(camFPS, camWidth, camHeight);
            this.ready = true;

            this.SetupEventHandlers();
            
            this.reportList = new string[3];

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
                        this.camera.initCamera(camFPS, camWidth, camHeight);
                        this.ready = true;
                    });
                    
                }
            };

         


        }

        public void destroy()
        {
            if (this.camera != null)
            {
                this.camera.Dispose();
            }
        }

        public void toggleSettings()
        {
            if (this.camera != null)
            {
                this.camera.toggleSettings();
            }
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

            this.camFPS = fps;
            this.camWidth = width;
            this.camHeight = height;
            init?.Invoke(this, EventArgs.Empty);
        }

        public string[] reportCamera()
        {
            if (camera != null)
            {
                this.ready = false;
                this.reportList = this.camera.getSettings().Split("\n\r");
                this.ready = true;
            }
            return reportList;
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