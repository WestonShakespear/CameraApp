using shakespear.cameraapp.camera;

namespace shakespear.cameraapp.gui
{
    public static class UserLogic
    {
        public static float fontSize = 0.6f;

        public static bool showDemoWindow = false;
        public static bool showStatistics = false;
        public static bool showMaterialEditor = false;
        public static bool showObjectProperties = true;
        public static bool showLightProperties = true;
        public static bool showOutliner = true;

        public static float spacing = 2f;

        public static List<string[]> LogData = new List<string[]>();



        public static string[] camNames = {"video0", "video1", "video2"};
        public static string[] resolutionItems = {"640x480", "1280x720", "1920x1080"};

        public static string[] CurrentCam = {"video0", "video0", "video0"};
        public static string[] CurrentResolution = {"640x480", "640x480", "640x480"};
        public static string[] CurrentFPS = {"30", "30", "30"};


        public static bool[] CaptureLive = {false, false, false};
        public static bool[] TrigConfigure = {false, false, false};
        public static bool[] TrigReport = {false, false, false};
        public static bool[] TrigSettings = {false, false, false};

        public static CameraInput? CameraOne;
        public static CameraInput? CameraTwo;

        public static byte[]? CameraOneImage;
        public static byte[]? CameraTwoImage;

        public static int CameraOneWidth = 0;
        public static int CameraOneHeight = 0;
        public static int CameraTwoWidth = 0;
        public static int CameraTwoHeight = 0;





        public static void Log(string level, string message)
        {
            LogData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }

        public static void Capture()
        {
            if (CameraOne != null && CaptureLive[0])
            {
                CameraOne.captureImage();

                CameraOne.getImage(ref CameraOneImage, ref CameraOneWidth, ref CameraOneHeight);
            } 

            if (CameraTwo != null && CaptureLive[1])
            {
                CameraTwo.captureImage();

                CameraTwo.getImage(ref CameraTwoImage, ref CameraTwoWidth, ref CameraTwoHeight);
            }
        }

        public static void Update()
        {
            for (int camera = 0; camera < 2; camera++)
            {
                if (TrigConfigure[camera])
            {
                TrigConfigure[camera] = false;

                string newCamera = CurrentCam[camera];

                string[] resolution = CurrentResolution[camera].Split("x");
                
                if (CameraOne != null)
                {
                    CameraOne.initCamera(
                        Int32.Parse(CurrentFPS[camera]),
                        Int32.Parse(resolution[0]),
                        Int32.Parse(resolution[1]));  
                }
                              
            } 
            
            else if (TrigReport[camera])
            {
                TrigReport[camera] = false;

                if (CameraOne != null)
                {
                    string[] values = CameraOne.reportCamera();
                    foreach (string value in values)
                    {
                        Log("USER", value);
                    }
                }
            }

            else if (TrigSettings[camera])
            {
                TrigSettings[camera] = false;

                if (CameraOne != null)
                {
                    CameraOne.toggleSettings();
                }
            }
            }


            Capture();


            
        }
    }
}