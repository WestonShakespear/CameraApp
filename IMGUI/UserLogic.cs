using shakespear.cameraapp.camera;
using shakespear.cameraapp.utilities;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using System.Drawing;

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

        public struct CameraParameters
        {
            public string[] camNames;
            public string[] resolutionItems;

            public string CurrentCam = "";
            public string CurrentResolution = "640x480";
            public string CurrentFPS = "30";

            public bool CaptureLive = false;
            public bool TrigConfigure = false;
            public bool TrigReport = false;
            public bool TrigSettings = false;
            public bool TrigTrigger = false;

            public CameraInput? Camera;
            public Emgu.CV.Mat? CameraMat;
            public byte[]? CameraImage;
            public int CameraWidth = 0;
            public int CameraHeight = 0;
            

            public CameraParameters()
            {
                camNames = new string[1];

                resolutionItems = new string[1];
            }
        }

        public static int TotalCameras = 0;

        public static CameraParameters[] CameraParams = new CameraParameters[TotalCameras];

        public static Utilities.CameraDetails[]? Details;

        public static void CreateCameraParameters(Utilities.CameraDetails[] _details)
        {
            Details = new Utilities.CameraDetails[_details.Length];

            string[] camNames = new string[_details.Length];

            for (int i = 0; i < _details.Length; i++)
            {
                camNames[i] = _details[i].CatName();
                Details[i] = _details[i];
            }

            for (int i = 0; i < TotalCameras; i++)
            {
                CameraParameters p = new CameraParameters();
                p.camNames = camNames;
                p.resolutionItems = resolutionItems;

                CameraParams[i] = p;
            }
            Console.WriteLine(CameraParams[0].CaptureLive);
        }


        public static string[] resolutionItems = {"640x480", "1280x720", "1920x1080"};

        public static int Disparities = 0;
        public static int BlockSize = 5;





        public static void Log(string level, string message)
        {
            LogData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }


            //     // CameraOne.UpdateFilterStatus(FilterStatus[0,0], FilterStatus[0,1], FilterStatus[0,2]);

            //     // CameraOne.UpdateFilterSettings(FilterSettings[0,0,0],
            //     //                                FilterSettings[0,0,1],
            //     //                                FilterSettings[0,1,0],
            //     //                                FilterSettings[0,1,1],
            //     //                                FilterSettings[0,2,0],
            //     //                                FilterSettings[0,2,1]);

        public static void Capture(int _camera)
        {
            if (CameraParams[_camera].CaptureLive || CameraParams[_camera].TrigTrigger)
            {
                CameraParams[_camera].TrigTrigger = false;
                CameraParams[_camera].Camera?.captureImage();
            }

            CameraParams[_camera].Camera?.getImage(ref CameraParams[_camera].CameraMat,
                                                   ref CameraParams[_camera].CameraImage,
                                                   ref CameraParams[_camera].CameraWidth,
                                                   ref CameraParams[_camera].CameraHeight);
        }

        public static void UpdateCamera(int _camera)
        {
            if (CameraParams[_camera].TrigConfigure)
            {
                CameraParams[_camera].TrigConfigure = false;

                if (Details is not null)
                {
                    string[] resolution = CameraParams[_camera].CurrentResolution.Split("x");
                    int resX = Int32.Parse(resolution[0]);
                    int resY = Int32.Parse(resolution[1]);
                    int fps = Int32.Parse(CameraParams[_camera].CurrentFPS);

                    int index = Utilities.IndexFromCatAndList(Details, CameraParams[_camera].CurrentCam);

                    if (CameraParams[_camera].Camera is null)
                    {
                        CameraParams[_camera].Camera = new CameraInput(index);
                    }

                    CameraParams[_camera].Camera?.initCamera(
                        fps,
                        resX,
                        resY);
                    
                    
                }
                         
            }

            else if (CameraParams[_camera].TrigReport)
            {
                CameraParams[_camera].TrigReport = false;

                string[]? values = CameraParams[_camera].Camera?.reportCamera();
                if (values is not null)
                {
                    foreach (string value in values)
                    {
                        Log("USER", value);
                    }
                }
            }

            else if (CameraParams[_camera].TrigSettings)
            {
                CameraParams[_camera].TrigSettings = false;

                CameraParams[_camera].Camera?.toggleSettings();
            }

            Capture(_camera);
        }

        public static void Update()
        {
            for (int i = 0; i < TotalCameras; i++)
            {
                UpdateCamera(i);
            }
        }



            
            
        //     if (CameraOneMat != null && CameraTwoMat != null && CaptureLive[2])
        //     {
        //         Mat OutputMat = new Mat();

        //         // int disp = (Disparities % 32) * 32;
        //         // int block = BlockSize;
        //         // if (block % 2 == 0)
        //         // {
        //         //     block -= 1;
        //         // }

        //         // StereoBM stereo = new StereoBM(disp, block);
                
        //         // stereo.Compute(CameraOneMat, CameraTwoMat, OutputMat);

        //         // Mat show = new Mat();

        //         // SIFT sift = new SIFT();

        //         // VectorOfKeyPoint points1 = new VectorOfKeyPoint();
        //         // VectorOfKeyPoint points2 = new VectorOfKeyPoint();

        //         // Mat desc1 = new Mat();
        //         // Mat desc2 = new Mat();
                

        //         // sift.DetectAndCompute(CameraOneMat, null, points1, desc1, false);
        //         // sift.DetectAndCompute(CameraTwoMat, null, points2, desc2, false);
                
        //         // DistanceType dt = new DistanceType();

        //         // BFMatcher bf = new BFMatcher(dt);

        //         // VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();

        //         // Features2D.DescriptorMatcher matcher = DescriptorMatcher();

                
                
        //         // CameraOneMat.ConvertTo(show, DepthType.Cv8U);

        //         // if (CameraOne != null)
        //         // {
        //         //     CameraOneImage = await CameraOne.TaskPoint(show, points1);
        //         // }

        //         // CameraTwoMat.ConvertTo(show, DepthType.Cv8U);

        //         // if (CameraTwo != null)
        //         // {
        //         //     CameraTwoImage = await CameraTwo.TaskPoint(show, points2);
        //         // }

        //         // // CameraTwoMat.ConvertTo(show, DepthType.Cv8U);

        //         // // Features2DToolbox.DrawMatches(CameraOneMat, points1, CameraTwoMat, points2, matches, show, new MCvScalar(0.5, 0, 0), new MCvScalar(0.5, 0, 0), null, Features2DToolbox.KeypointDrawType.Default);

        //         // if (CameraTwo != null)
        //         // {
        //         //     OutputImage = await CameraTwo.TaskPoint(show, points2);
        //         // }





        //         // Mat OutputMat = new Mat();

        //         int disp = (Disparities % 32) * 32;
        //         int block = BlockSize;
        //         if (block % 2 == 0)
        //         {
        //             block -= 1;
        //         }

        //         StereoBM stereo = new StereoBM(disp, block);
                
        //         stereo.Compute(CameraOneMat, CameraTwoMat, OutputMat);

        //         Mat show = new Mat();

        //         OutputMat.ConvertTo(show, DepthType.Cv8U);

        //         if (CameraOne != null)
        //         {
        //             OutputImage = await CameraOne.TaskConvert(show);
        //         }
                
        //     } 
        // }



    }
}