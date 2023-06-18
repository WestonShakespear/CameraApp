using shakespear.cameraapp.camera;
using Emgu.CV;

using Emgu.CV.Util;
using Emgu.CV.CvEnum;

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


        public static bool[] CaptureLive = {true, true, true};
        public static bool[] TrigConfigure = {false, false, false};
        public static bool[] TrigReport = {false, false, false};
        public static bool[] TrigSettings = {false, false, false};

        public static bool[,] FilterStatus = {{false, false, false}, {false, false, false}, {false, false, false}};
        public static int[,,] FilterSettings = { { {0, 0}, {5, 0}, {0, 0} },
                                                 { {0, 0}, {5, 0}, {0, 0} },
                                                 { {0, 0}, {5, 0}, {0, 0} } };

        public static CameraInput? CameraOne;
        public static CameraInput? CameraTwo;

        public static byte[]? CameraOneImage;
        public static byte[]? CameraTwoImage;
        public static byte[]? OutputImage;

        public static int CameraOneWidth = 0;
        public static int CameraOneHeight = 0;
        public static int CameraTwoWidth = 0;
        public static int CameraTwoHeight = 0;


        private static Emgu.CV.Mat? CameraOneMat;
        private static Emgu.CV.Mat? CameraTwoMat;


        public static int Disparities = 0;
        public static int BlockSize = 5;





        public static void Log(string level, string message)
        {
            LogData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }

        public static void Capture()
        {
            if (CameraOne != null && CaptureLive[0])
            {
                CameraOne.UpdateFilterStatus(FilterStatus[0,0], FilterStatus[0,1], FilterStatus[0,2]);

                CameraOne.UpdateFilterSettings(FilterSettings[0,0,0],
                                               FilterSettings[0,0,1],
                                               FilterSettings[0,1,0],
                                               FilterSettings[0,1,1],
                                               FilterSettings[0,2,0],
                                               FilterSettings[0,2,1]);
                CameraOne.captureImage();

                CameraOne.getImage(ref CameraOneMat, ref CameraOneImage, ref CameraOneWidth, ref CameraOneHeight);
            } 

            if (CameraTwo != null && CaptureLive[1])
            {
                CameraTwo.UpdateFilterStatus(FilterStatus[1,0], FilterStatus[1,1], FilterStatus[1,2]);

                CameraTwo.UpdateFilterSettings(FilterSettings[1,0,0],
                                               FilterSettings[1,0,1],
                                               FilterSettings[1,1,0],
                                               FilterSettings[1,1,1],
                                               FilterSettings[1,2,0],
                                               FilterSettings[1,2,1]);
                CameraTwo.captureImage();

                CameraTwo.getImage(ref CameraTwoMat, ref CameraTwoImage, ref CameraTwoWidth, ref CameraTwoHeight);
            }
        }

        public static async void Update()
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
            if (CameraOneMat != null && CameraTwoMat != null)
            {
                Mat OutputMat = new Mat();

                int disp = (Disparities % 32) * 32;
                int block = BlockSize;
                if (block % 2 == 0)
                {
                    block -= 1;
                }

                StereoBM stereo = new StereoBM(disp, block);
                
                stereo.Compute(CameraOneMat, CameraTwoMat, OutputMat);

                Mat show = new Mat();

                OutputMat.ConvertTo(show, DepthType.Cv8U);

                if (CameraOne != null)
                {
                    OutputImage = await CameraOne.TaskConvert(show);
                }
                
            }
            

            
        }
    }
}