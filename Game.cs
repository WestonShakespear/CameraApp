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


        public ShaderManager[] shaderCameraManagers;
        public ShaderManager[] shaderOutputManagers;

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
            
            UserLogic.TotalCameras = 2;
            UserLogic.TotalOutputs = 3;

            UserLogic.CameraParams = new UserLogic.CameraParameters[UserLogic.TotalCameras];
            this.details = Utilities.GetCameras();

            if (this.details is not null)
            {
                UserLogic.CreateParameters(this.details);
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

                   
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs();

            UserLogic.Update();

            
            

            this.RenderShaderManagers("Camera", ref shaderCameraManagers, UserLogic.CameraParams);
            this.RenderShaderManagers("Output", ref shaderOutputManagers, UserLogic.OutputParams);

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

            this.ReloadShaderManagers(ref this.shaderCameraManagers);
            this.ReloadShaderManagers(ref this.shaderOutputManagers);

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            GUI.LoadTheme();
            
            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            this.LoadShaderManagers(UserLogic.TotalCameras, ref this.shaderCameraManagers);
            this.LoadShaderManagers(UserLogic.TotalOutputs, ref this.shaderOutputManagers);
            
            base.OnLoad();
        }

        protected override void OnUnload()
        {
            this.UnloadShaderManagers(ref this.shaderCameraManagers);
            this.UnloadShaderManagers(ref this.shaderOutputManagers);

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                UserLogic.CameraParams[camera].Camera?.destroy();
            }

            base.OnUnload();
        }

        private void LoadShaderManagers(int length, ref ShaderManager[] managers)
        {


            managers = new ShaderManager[length];

            for (int i = 0; i < length; i++)
            {
                managers[i] = new ShaderManager(TextureUnit.Texture0, CameraWidth, CameraHeight, @"noSignal.jpg");
                managers[i].GenFBO();
            }
        }

        private void ReloadShaderManagers(ref ShaderManager[] managers)
        {
            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].GenFBO();
            }
        }

        private void RenderShaderManagers(string prefix, ref ShaderManager[] managers, UserLogic.CameraParameters[] parameters)
        {
            for (int manager = 0; manager < managers.Length; manager++)
            {
                byte[]? image = parameters[manager].CameraImage;

                if (image is not null)
                {
                    managers[manager].UpdateTextureMemory(image,
                                                          parameters[manager].CameraWidth,
                                                          parameters[manager].CameraHeight);
                }

                managers[manager].RenderFrame();
                int FBOutput = managers[manager].FramebufferTexture;

                GUI.CameraOneWindow(prefix + " " + (manager+1), ref CameraWidth, ref CameraHeight, ref FBOutput);
            }
        }

        private void UnloadShaderManagers(ref ShaderManager[] managers)
        {
            foreach (ShaderManager sm in managers)
            {
                sm.Dispose();
            }
        }
    }
}

