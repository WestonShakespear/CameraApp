using Emgu.CV;

namespace testOne
{

    public delegate void ImageCallbackDelegate(Mat image);

    public class CameraHelper
    {

        private VideoCapture camera;
        private int resolutionX;
        private int resolutionY;

        private ImageCallbackDelegate imageCallbackDelegate;

        public CameraHelper(VideoCapture _camera, int _resolutionX, int _resolutionY, ImageCallbackDelegate _imageCallbackDelegate)
        {
            this.camera = _camera;
            this.resolutionX = _resolutionX;
            this.resolutionY = _resolutionY;

            this.imageCallbackDelegate = _imageCallbackDelegate;
        }

        public void Capture()
        {
            var image = this.camera.QueryFrame();

            if (this.imageCallbackDelegate != null) 
            {
                this.imageCallbackDelegate(image);
            }
        }
    }
}