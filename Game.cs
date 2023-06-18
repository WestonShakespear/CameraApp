using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using ImGuiNET;

using shakespear.cameraapp.gui;
using shakespear.cameraapp.shadermanager;
using shakespear.cameraapp.utilities;

namespace shakespear.cameraapp {
    public class Window : GameWindow {

        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        public ImGuiController UIController;


        public ShaderManager[] shaderManagers;

        public Utilities.CameraDetails[]? details;
        


        

        public Window(int width, int height, string title, string fontPath, float fontSize)
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
            
            UserLogic.TotalCameras = 3;
            UserLogic.CameraParams = new UserLogic.CameraParameters[UserLogic.TotalCameras];
            this.details = Utilities.GetCameras();

            if (this.details is not null)
            {
                UserLogic.CreateCameraParameters(this.details);
            }

            // Environment.Exit(1);


            // Center the window
            this.CenterWindow();
            WindowHeight = Size.Y;
            WindowWidth = Size.X;
            CameraHeight = Size.Y;
            CameraWidth = Size.X;

            UIController = new ImGuiController((int)WindowWidth, (int)WindowHeight, fontPath, fontSize);

            // UserLogic.CameraOne = new camera.CameraInput(1);
            // UserLogic.CameraTwo = new camera.CameraInput(2);

            string[] paths = {@"noSignal.jpg", @"noSignal2.jpg", @"noSignal3.jpg"};

            this.shaderManagers = new ShaderManager[UserLogic.TotalCameras];

            for (int i = 0; i < UserLogic.TotalCameras; i++)
            {
                this.shaderManagers[i] = new ShaderManager(TextureUnit.Texture0, CameraWidth, CameraHeight, paths[i]);
            }       
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs();



            UserLogic.Update();

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                if (UserLogic.CameraParams[camera].CameraImage != null)
                {
                    this.shaderManagers[camera].UpdateTextureMemory(UserLogic.CameraParams[camera].CameraImage,
                                                                    UserLogic.CameraParams[camera].CameraWidth,
                                                                    UserLogic.CameraParams[camera].CameraHeight);
                }

                this.shaderManagers[camera].RenderFrame();
                int FBOutput = this.shaderManagers[camera].FramebufferTexture;

                GUI.CameraOneWindow("Camera " + (camera+1), ref CameraWidth, ref CameraHeight, ref FBOutput);
            }

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
                
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            foreach (ShaderManager sm in this.shaderManagers)
            {
                sm.GenFBO();
            }

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            foreach (ShaderManager sm in this.shaderManagers)
            {
                sm.GenFBO();
            }
            
            base.OnLoad();
        }

        protected override void OnUnload()
        {

            foreach (ShaderManager sm in this.shaderManagers)
            {
                sm.Dispose();
            }

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                UserLogic.CameraParams[camera].Camera?.destroy();
            }

            base.OnUnload();
        } 
    }
}

