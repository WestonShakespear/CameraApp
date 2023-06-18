using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

using System.Drawing;
using System.Diagnostics;
using StbImageSharp;
using Emgu.CV.Structure;

namespace shakespear.cameraapp.camera
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

        Stopwatch timer;

        private int camFPS = 5;
        private int camWidth = 640;
        private int camHeight = 480;

        private int camIndex = 0;

        private string[] reportList;

        
        private VideoCapture.API api = VideoCapture.API.DShow;


        private bool canny = false;
        private bool guassian = false;
        private bool threshold = false;


        int cannyA = 0;
        int cannyB = 0;
        int guassSize = 0;
        int guassSigma = 0;
        int thresMin = 0;
        int thresMax = 0;
        

        public CameraInput(int _cameraIndex)
        {
            this.camIndex = _cameraIndex;
            this.camera = new VideoCapture(this.camIndex, api);
            //this.camera.initCamera(camFPS, camWidth, camHeight);
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
                return await Task.Run(() => convert(this.camera.QueryFrame()));
            }

            return null;
        }

        public async Task<byte[]> TaskConvert(Mat image)
        {
            var buffer = new VectorOfByte();

            await Task.Run(() => {
                CvInvoke.Imencode(".jpg", image, buffer);
            });

            return await Task.Run(() => ImageResult.FromMemory(buffer.ToArray(), ColorComponents.RedGreenBlueAlpha).Data);
        }

        public Task<byte[]> TaskPoint(Mat image, VectorOfKeyPoint keys)
        {
            foreach (MKeyPoint data in keys.ToArray())
            {
                int x = (int)data.Point.X;
                int y = (int)data.Point.Y;

                CvInvoke.Circle(image, new Point(x, y), 5, new MCvScalar(0.5, 0.0, 0.0), 10);
            }

            return this.TaskConvert(image);
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
            Mat imgOutput = new Mat(image.Size, DepthType.Cv8U, 1);
            Mat imgTemp = new Mat(image.Size, DepthType.Cv8U, 1);

            

            CvInvoke.CvtColor(image, imgOutput, ColorConversion.Bgr2Gray);

            if (this.threshold)
            {
                imgTemp = imgOutput.Clone();
                CvInvoke.GaussianBlur(imgTemp, imgOutput, new Size(5, 5), 1.5);
            }

            if (this.guassian)
            {
                imgTemp = imgOutput.Clone();
                CvInvoke.GaussianBlur(imgTemp, imgOutput, new Size(guassSize, guassSize), guassSigma);
            }

            if (this.canny)
            {
                imgTemp = imgOutput.Clone();
                CvInvoke.Canny(imgTemp, imgOutput, cannyA, cannyB);
            }

            

            // CvInvoke.Canny(imgBlurred, imgCanny, 10, 5);

            return imgOutput;
        }

        public void UpdateFilterStatus(bool _canny, bool _gaussian, bool _threshold)
        {
            this.canny = _canny;
            this.guassian = _gaussian;
            this.threshold = _threshold;
        }

        public void UpdateFilterSettings(int _cannyA,
                                         int _cannyB,
                                         int _guassSize,
                                         int _guassSigma,
                                         int _thresMin,
                                         int _thresMax)
        {
            this.cannyA = _cannyA;
            this.cannyB = _cannyB;
            this.guassSize = _guassSize;
            if (this.guassSize % 2 == 0)
            {
                this.guassSize -= 1;
            }
            
            this.guassSigma = _guassSigma;
            this.thresMin = _thresMin;
            this.thresMax = _thresMax;
        }

        public void captureImage()
        {
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

        public void getImage(ref Mat? image, ref byte[]? rawImage, ref int width, ref int height)
        {
            image = null;
            rawImage = null;
            width = 0;
            height = 0;
            
            if (this.currentMat != null && this.currentRawImage != null && this.currentConImage != null)
            {
                width = this.currentMat.Width;
                height = this.currentMat.Height;

                rawImage = this.currentRawImage;
                image = this.currentMat;
            }
        }
    }
}