using Emgu.CV;
using Emgu.CV.CvEnum;

namespace testOne
{
    public static class CamExtension
    {
         public static void initCamera(this VideoCapture capture, int frame, int width, int height)
        {
            capture.setTest(CapProp.Fps, frame);

            //capture.setTest(CapProp.FourCC, VideoWriter.Fourcc('m', 'j', 'p', 'g'));
            
            capture.setTest(CapProp.FrameWidth, width);
            capture.setTest(CapProp.FrameHeight, height);

            //capture.setTest(CapProp.ConvertRgb, 1);
        }

        public static bool setTest(this VideoCapture capture, CapProp prop, double value)
        {
            capture.Set(prop, value);

            double nValue = capture.Get(prop);

            //Console.WriteLine("Set: {0} Get: {1}", value, nValue);

            return (nValue == value);
        }

        public static string getSettings(this VideoCapture capture)
        {
            string output = "";

            Dictionary<string, CapProp> props = new Dictionary<string, CapProp>()
            {
                {"Fps", CapProp.Fps},

                {"Width", CapProp.FrameWidth},
                {"Height", CapProp.FrameHeight},

                {"Format", CapProp.Format},
                {"Mode", CapProp.Mode},
                {"FOURCC", CapProp.FourCC},

                {"Settings", CapProp.Settings},
                {"Autofocus", CapProp.Autofocus},
                {"Autoexposure", CapProp.AutoExposure},
                {"Backend", CapProp.Backend},
                {"GUID", CapProp.Guid},
                {"Channel", CapProp.Channel},
                {"Pixel Format", CapProp.CodecPixelFormat},

                {"Pan", CapProp.Pan},
                {"Tilt", CapProp.Tilt},
                {"Roll", CapProp.Roll},
                

                {"Zoom", CapProp.Zoom},
            };

            foreach (KeyValuePair<string, CapProp> prop in props)
            {
                output += prop.Key + ": " + capture.Get(prop.Value);
                output += "\n\r";
            }
            


            
            output += "\n\r";

            // Console.WriteLine(output);

            return output;
        }
    }
}