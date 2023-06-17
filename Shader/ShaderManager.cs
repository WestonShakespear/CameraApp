using OpenTK.Graphics.OpenGL4;

namespace testOne
{
    public class ShaderManager
    {
        private readonly float[] vertices =
        {
            //Position         //Texture coordinates
            1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
            1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private readonly String imagePath = @"noSignal.jpg";

        private Shader shader;
        private Texture texture0;

        private int FBO = -1;
        private int framebufferTexture = -1;
        private int vertexArrayObject = -1;
        private int vertexBufferObject = -1;
        private int elementBufferObject = -1;

        public int FramebufferTexture { get { return framebufferTexture; } }

        private float cameraWidth;
        private float cameraHeight;

        public ShaderManager(float _cameraWidth, float _cameraHeight)
        {
            this.cameraWidth = _cameraWidth;
            this.cameraHeight = _cameraHeight;

            this.shader = new Shader("Shader/shader.vert", "Shader/shader.frag");
            this.texture0 = new Texture();
        }


        public void RenderFrame()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.FBO);
            GL.BindVertexArray(this.vertexArrayObject);

            this.texture0.Use(TextureUnit.Texture0);
            this.shader.Use();
            
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);          
        }

        public void UpdateTextureMemory(byte[] _imageRaw, int _width, int _height)
        {
            texture0.LoadFromMemory(TextureUnit.Texture0, _imageRaw, _width, _height);
        }

        public void DisposeBuffer()
        {
            GL.DeleteFramebuffer(this.FBO);
        }

        public void Dispose()
        {
            this.DisposeBuffer();
            this.shader?.Dispose();
        }

        public void GenFBO()
        {
            if (this.FBO != -1)
            {
                this.DisposeBuffer();
            }

            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Color Texture
            framebufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, framebufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, (int)cameraWidth, (int)cameraHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);



            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

                vertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

                elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            

            this.texture0.LoadFromFile(TextureUnit.Texture0, this.imagePath);
            this.texture0.Use(TextureUnit.Texture0);
            
            // Attach color to FBO
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, framebufferTexture, 0);

            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer error: " + fboStatus);
            }

            shader.SetInt("texture0", 0);
        }
    }
}