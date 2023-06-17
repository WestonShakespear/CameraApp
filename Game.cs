using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using ImGuiNET;

using shakespear.cameraapp.gui;
using shakespear.cameraapp.shadermanager;

namespace shakespear.cameraapp {
    public class Window : GameWindow {

        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        public ImGuiController UIController;
        public ShaderManager shaderManager;
        public ShaderManager shaderManager2;
        

        

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
            // Center the window
            this.CenterWindow();
            WindowHeight = Size.Y;
            WindowWidth = Size.X;
            CameraHeight = Size.Y;
            CameraWidth = Size.X;

            UIController = new ImGuiController((int)WindowWidth, (int)WindowHeight, fontPath, fontSize);

            UserLogic.CameraOne = new camera.CameraInput(1);
            UserLogic.CameraTwo = new camera.CameraInput(2);

            this.shaderManager = new ShaderManager(TextureUnit.Texture0, CameraWidth, CameraHeight, @"noSignal.jpg");
            this.shaderManager2 = new ShaderManager(TextureUnit.Texture0, CameraWidth, CameraHeight, @"noSignal2.jpg");
            
            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            UserLogic.Update();

            if (UserLogic.CameraOneImage != null)
            {
                this.shaderManager.UpdateTextureMemory(UserLogic.CameraOneImage, UserLogic.CameraOneWidth, UserLogic.CameraOneHeight);
            }
            this.shaderManager.RenderFrame();


            if (UserLogic.CameraTwoImage != null)
            {
                this.shaderManager2.UpdateTextureMemory(UserLogic.CameraTwoImage, UserLogic.CameraTwoWidth, UserLogic.CameraTwoHeight);
            }
            this.shaderManager2.RenderFrame();
            

            

            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs();

            int FBOutputOne = this.shaderManager.FramebufferTexture;
            int FBOutputTwo = this.shaderManager2.FramebufferTexture;

            GUI.CameraOneWindow(ref CameraWidth, ref CameraHeight, ref FBOutputOne);
            GUI.CameraTwoWindow(ref CameraWidth, ref CameraHeight, ref FBOutputTwo);
            //UI.CameraThreeWindow(ref CameraWidth, ref CameraHeight, ref FBOutputOne);

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
            
            this.shaderManager.GenFBO();
            this.shaderManager2.GenFBO();

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            this.shaderManager.GenFBO();
            this.shaderManager2.GenFBO();
            
            

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            this.shaderManager.Dispose();
            this.shaderManager2.Dispose();

            UserLogic.CameraOne?.destroy();
            UserLogic.CameraOne?.destroy();

            base.OnUnload();
        } 
    }
}
