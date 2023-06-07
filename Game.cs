using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using ImGuiNET;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Util;

namespace testOne {
    public class Game : GameWindow {

        public static float WindowWidth;
        public static float WindowHeight;
        public static float CameraWidth;
        public static float CameraHeight;

        public CameraInput test;

        ImGuiController UIController;

        List<string[]> logData = new List<string[]>();


        Stopwatch timer;
        Shader? shader;

        public static float[] vertices =
        {
            //Position          Texture coordinates
            1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
            1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };

        // public static float[] vertices =
        // {
        //     //Position          Texture coordinates
        //     0.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
        //     0.0f, 0.0f, 0.0f, 1.0f, 0.0f, // bottom right
        //     -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, // bottom left
        //     -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        // };

        public static uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        int VertexArrayObject;
        int elementBufferObject;
        int vertexBufferObject;

        public Texture texture0;

        public Texture texture1;

        private byte[]? currentImage;

        public static int a = 0;
        public static int b = 0;

        public static bool canny = false;

        public static bool captureLive = false;

        public int FBO; //RBO;
        public int framebufferTexture;

        public float camWidth = 800f;
        public float camHeight = 600f;

        public static float angle = 0.0f;
        public static System.Numerics.Vector4 colorPicked = new System.Numerics.Vector4(0.0f);


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

            log("DEBUG", "message1");
            log("DEBUG", "message2");
            log("DEBUG", "message3");
            log("DEBUG", "message4");

            this.test = new CameraInput(1);
            test.captureImage(a, b);

            
            //Mat noSignal = CvInvoke.Imread(imagePath, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            

            this.timer = new Stopwatch();
            this.timer.Start();
            
        }

        public void log(string level, string message)
        {
            this.logData.Add(new string[] {level, DateTime.Now.ToString("HH:mm:ss"), message});
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

        protected override void OnLoad()
        {
            //GUI.LoadTheme();

            this.VSync = VSyncMode.On;
            this.IsVisible = true;

            this.GenFBO(WindowWidth, WindowHeight);

            base.OnLoad();

            
        }

        protected override void OnUnload()
        {
            if (shader != null)
            {
                shader.Dispose();
            }

            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            WindowWidth = e.Width;
            WindowHeight = e.Height;

            GL.DeleteFramebuffer(FBO);
            GenFBO(WindowWidth, WindowHeight);

            UIController.WindowResized((int)WindowWidth, (int)WindowHeight);

            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        

        public void capture()
        {
            
            test.captureImage(a, b);

            test.getImage(out byte[]? imageRaw,
                          out byte[]? imageCon,
                          out int width,
                          out int height);
            

            if (imageRaw != null)
            {
                texture0 = texture0.LoadFromMemory(TextureUnit.Texture0, imageRaw, width, height);
            }

            if (imageCon != null)
            {
                texture1 = texture1.LoadFromMemory(TextureUnit.Texture1, imageCon, width, height);
            }

            
            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            if (shader != null)
            {
                
                if (captureLive)
                {
                    this.capture();
                }
                
                
                
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

                GL.BindVertexArray(VertexArrayObject);

                texture0.Use(TextureUnit.Texture0);
                texture1.Use(TextureUnit.Texture1);
                shader.Use();

                //GL.BindVertexArray(VertexArrayObject);
                
                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            }

            UIController.Update(this, (float)args.Time);
            ImGui.DockSpaceOverViewport();
            GUI.WindowOnOffs();
            GUI.LoadOCCTWindow(ref camWidth, ref camHeight, ref framebufferTexture);
            UIController.Render();
            ImGuiController.CheckGLError("End of frame");

            Context.SwapBuffers();  

            base.OnRenderFrame(args);      
            
            
        }


        public void startTimer()
        {
            this.timer.Reset();
            this.timer.Start();
        }

        public void stopTimer()
        {
            this.timer.Stop();
            TimeSpan ts = this.timer.Elapsed;

            string elapsedTime = String.Format("{0:00}",
            ts.Milliseconds);

            Console.WriteLine(elapsedTime);
        }


        

        public void GenFBO(float CamWidth, float CamHeight)
        {
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Color Texture
            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, (int)CamWidth, (int)CamHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);



            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

                vertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

                elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader("shader.vert", "shader.frag");

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            String imagePath = @"noSignal.jpg";

            texture0 = Texture.LoadFromFile(TextureUnit.Texture0, imagePath);
            texture0.Use(TextureUnit.Texture0);

            texture1 = Texture.LoadFromFile(TextureUnit.Texture1, imagePath);
            texture1.Use(TextureUnit.Texture1);
            
            
            // Attach color to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }

            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);
        }
    }
}
