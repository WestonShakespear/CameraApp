using Emgu.CV;
using Emgu.CV.CvEnum;

namespace testOne
{
    public static class CamExtension
    {
         public static void initCamera(this VideoCapture capture, int frame, int width, int height)
        {
            Console.WriteLine(capture.setTest(CapProp.Fps, frame));
            Console.WriteLine(capture.setTest(CapProp.FrameWidth, width));
            Console.WriteLine(capture.setTest(CapProp.FrameHeight, height));
        }

        public static bool setTest(this VideoCapture capture, CapProp prop, double value)
        {
            capture.Set(prop, value);

            double nValue = capture.Get(prop);

            Console.WriteLine("Set: {0} Get: {1}", value, nValue);

            return (nValue == value);
        }

        public static void displaySettings(this VideoCapture capture)
        {
            Console.WriteLine("Width: {0}", capture.Get(CapProp.FrameWidth));
            Console.WriteLine("Height: {0}", capture.Get(CapProp.FrameHeight));
            Console.WriteLine("Frame: {0}", capture.Get(CapProp.Fps));
        }
    }
}