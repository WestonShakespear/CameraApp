using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using ImGuiNET;

namespace testOne {
    public class Game : GameWindow {

        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        public ImGuiController UIController;
        public CameraInput test;
        public ShaderManager shaderManager;
        

        List<string[]> logData = new List<string[]>();

             
        public static bool captureLive = false;

        

        public float cameraWidth = 800f;
        public float cameraHeight = 600f;


        public static bool trigConfigure = false;
        public static bool trigReport = false;
        public static bool trigSettings = false;

        public Game(int width, int height, string title, string fontPath, float fontSize)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Resizable,
                StartVisible = false,
                StartFocused = true,
                WindowState = WindowState.Normal,
                API = ContextAPI.OpenGL,
                Profile = ContextProfile.Core,
                APIVersion = new Version(3, 3)
            })

        {
            // Center the window
            this.CenterWindow();
            WindowHeight = Size.Y;
            WindowWidth = Size.X;
            CameraHeight = Size.Y;
            CameraWidth = Size.X;

            UIController = new ImGuiController((int)WindowWidth, (int)WindowHeight, fontPath, fontSize);

            this.test = new CameraInput(1);

            this.shaderManager = new ShaderManager(CameraWidth, CameraHeight);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            this.updateGLogic();

            this.shaderManager.RenderFrame();

            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs(this.logData);

            int FBOutput = this.shaderManager.FramebufferTexture;
            GUI.LoadOCCTWindow(ref CameraWidth, ref CameraHeight, ref FBOutput);
            UIController.Render();
            ImGuiController.CheckGLError("End of frame");

            Context.SwapBuffers();  

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                this.log("DEBUG", "Application close");
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            this.shaderManager.GenFBO();

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            this.shaderManager.GenFBO();

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            this.shaderManager.Dispose();

            if (test != null)
            {
                test.destroy();
            }

            base.OnUnload();
        }

        

        






























        public void capture()
        {
            
            test.captureImage();

            test.getImage(out byte[]? imageRaw,
                          out byte[]? imageCon,
                          out int width,
                          out int height);
            

            if (imageRaw != null)
            {
                this.shaderManager.UpdateTextureMemory(imageRaw, width, height);
            } 
            
        }

        public void log(string level, string message)
        {
            this.logData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
        }

        private void updateGLogic()
        {
            if (trigConfigure)
            {
                trigConfigure = false;

                string newCamera = GUI.currentCam;

                string[] resolution = GUI.currentResolution.Split("x");
                string newWidth = resolution[0];
                string newHeight = resolution[1];
       
                string newFPS = GUI.currentFPS;


                Console.WriteLine("Triggered: c:{0} w:{1} h:{2} f:{3}", newCamera, newWidth, newHeight, newFPS);

                this.test.initCamera(Int32.Parse(newFPS), Int32.Parse(newWidth), Int32.Parse(newHeight));                
            } 
            
            else if (trigReport)
            {
                trigReport = false;

                string[] values = this.test.reportCamera();
                foreach (string value in values)
                {
                    this.log("USER", value);
                }
            }

            else if (trigSettings)
            {
                trigSettings = false;

                if (this.test != null)
                {
                    this.test.toggleSettings();
                }
            }
            
            else {
                if (captureLive)
                {
                    this.capture();
                }
            }
        }
    }
}
