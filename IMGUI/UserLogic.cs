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

        public static string CurrentCam = "video0";
        public static string CurrentResolution = "640x480";
        public static string CurrentFPS = "30";


        public static bool captureLive = false;
        public static bool trigConfigure = false;
        public static bool trigReport = false;
        public static bool trigSettings = false;

        public static CameraInput? CameraOne;
        public static byte[]? CameraOneImage;
        public static int CameraOneWidth = 0;
        public static int CameraOneHeight = 0;





        public static void Log(string level, string message)
        {
            LogData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }

        public static void Capture()
        {
            if (CameraOne != null)
            {
                CameraOne.captureImage();

                CameraOne.getImage(ref CameraOneImage, ref CameraOneWidth, ref CameraOneHeight);
            } 
        }

        public static void Update()
        {
            if (trigConfigure)
            {
                trigConfigure = false;

                string newCamera = CurrentCam;

                string[] resolution = CurrentResolution.Split("x");
                
                if (CameraOne != null)
                {
                    CameraOne.initCamera(
                        Int32.Parse(CurrentFPS),
                        Int32.Parse(resolution[0]),
                        Int32.Parse(resolution[1]));  
                }
                              
            } 
            
            else if (trigReport)
            {
                trigReport = false;

                if (CameraOne != null)
                {
                    string[] values = CameraOne.reportCamera();
                    foreach (string value in values)
                    {
                        Log("USER", value);
                    }
                }
            }

            else if (trigSettings)
            {
                trigSettings = false;

                if (CameraOne != null)
                {
                    CameraOne.toggleSettings();
                }
            }
            
            else {

                if (captureLive)
                {
                    Capture();
                }
            }
        }
    }
}