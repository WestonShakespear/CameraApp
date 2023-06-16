using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

using ImGuiNET;

using static testOne.Game;

namespace testOne
{
    class GUI
    {
        public static float fontSize = 0.6f;

        public static bool showDemoWindow = false;
        public static bool showStatistics = false;
        public static bool showMaterialEditor = false;
        public static bool showObjectProperties = true;
        public static bool showLightProperties = true;
        public static bool showOutliner = true;

        public static float spacing = 2f;

        public static void WindowOnOffs(List<string[]> log)
        {
            LoadMenuBar();
            demoWindow();
            LogWindow(log);
            //demoValueWindow();
            //ImGui.ShowDemoWindow();
        }

        public static void LoadOCCTWindow(ref float CameraWidth, ref float CameraHeight, ref int framebufferTexture)
        {
            ImGui.Begin("OCCT");

            CameraWidth = ImGui.GetWindowWidth();
            CameraHeight = ImGui.GetWindowHeight() - ImGui.GetIO().FontGlobalScale * 71;

            //isMainHovered = ImGui.IsWindowHovered();

            ImGui.Image((IntPtr)framebufferTexture,
                new System.Numerics.Vector2(CameraWidth, CameraHeight),
                new System.Numerics.Vector2(0, 0.95f),
                new System.Numerics.Vector2(1, 0),
                new System.Numerics.Vector4(1.0f),
                new System.Numerics.Vector4(1, 1, 1, 0.2f));
            ImGui.End();
        }
       

        public static void LoadMenuBar()
        {
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Save", "Ctrl + S")) 
                {
                    Console.WriteLine("Saved");
                }
                ImGui.Separator();
                if (ImGui.MenuItem("Load"))
                {
                    Console.WriteLine("Load");
                }

                ImGui.Separator();
                if (ImGui.MenuItem("Quit", "Alt+F4")) Console.WriteLine("Close Window");

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Window"))
            {
                if (ImGui.BeginMenu("Debug"))
                {
                    ImGui.Checkbox("Statistics", ref showStatistics);
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref showObjectProperties);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref showLightProperties); 
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref showOutliner);
                    ImGui.Separator();
                    // ImGui.Checkbox("Show Settings", ref showSettings);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Material Editor", ref showMaterialEditor);
                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }

            ImGui.Dummy(new System.Numerics.Vector2(ImGui.GetWindowWidth() -1000, 0));
            // ImGui.TextDisabled("Objects: " + (Objects.Count).ToString());
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled(GL.GetString(StringName.Renderer));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("FPS: " + ImGui.GetIO().Framerate.ToString("0"));
            ImGui.TextDisabled(" | ");
            ImGui.TextDisabled("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));

            ImGui.EndMainMenuBar();
        }


        



        public static void LogWindow(List<string[]> logData)
        {
            int col = 3;
            ImGui.Begin("Log");
            ImGui.BeginTable("Table", col);

            ImGui.TableSetupColumn("Level");

            ImGui.TableSetupColumn("Time");
            ImGui.TableSetupColumn("Message");
            ImGui.TableHeadersRow();

            List<string[]> rev = new List<string[]>(logData);
            
            rev.Reverse();

            foreach (string[] data in rev)
            {
                for (int i = 0;i < col; i++)
                {
                    ImGui.TableNextColumn();
                    ImGui.Text(data[i]);
                }
                ImGui.TableNextRow();
                
            }
            ImGui.EndTable();
            ImGui.End();
        }

        // min s=384x4 fps=5 
        // max s=1920x1200 fps=15

        // min s=1920x1080 fps=5 
        // max s=1920x1080 fps=15

        // min s=1280x720 fps=5 
        // max s=1280x720 fps=40

        // min s=640x480 fps=5 
        // max s=640x480 fps=60.0002

        public static string[] camNames = {"video0", "video1", "video2"};
        public static string[] resolutionItems = {"640x480", "1280x720", "1920x1080"};

        public static string currentCam = "video0";
        public static string currentResolution = "640x480";
        public static string currentFPS = "30";

        


        public static void createBasicDropdown(string name, string[] items, ref string value)
        {
            if (ImGui.BeginCombo(name, value))
            {
                foreach (string item in items)
                {
                    bool selected = false;
                    
                    if (ImGui.Selectable(item, selected))
                    {
                        value = item;
                    }

                    if (selected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }

        

        public static void demoWindow()
        {
            ImGui.Begin("Demo");

            ImGui.Checkbox("Capture", ref Game.captureLive);

            ImGui.Separator();

            // ImGui.InputText("FPS: ", Game.fps, 100);

            
            createBasicDropdown("Camera:", camNames, ref currentCam);
            createBasicDropdown("Resolution:", resolutionItems, ref currentResolution);

            Dictionary<string, string[]> fpsItems = new Dictionary<string, string[]>();

            string[] sFPS = {"30", "35", "40", "45", "50", "55", "56", "60"};
            string[] mFPS = {"5", "10", "15"};
            string[] lFPS = {"30", "35", "38", "40"};

            fpsItems.Add("640x480", sFPS);
            //fpsItems.Add("960x600", mFPS);
            fpsItems.Add("1280x720", lFPS);
            //fpsItems.Add("1920x1200", mFPS);
            fpsItems.Add("1920x1080", mFPS);
            
            createBasicDropdown("FPS:", fpsItems[currentResolution], ref currentFPS);
            

            if (ImGui.Button("Configure"))
            {
                Game.trigConfigure = true;
            }

            if (ImGui.Button("Report Config"))
            {
                Game.trigReport = true;
            }

            ImGui.Separator();

            // ImGui.Checkbox("Canny", ref Game.canny);
            // ImGui.SliderInt("A:", ref Game.a, 0, 255);
            // ImGui.SliderInt("B:", ref Game.b, 0, 255);


            // ImGui.SliderFloat("Vert1Z", ref Game.vertices[2], -1.0f, 1.0f);

            // ImGui.SliderFloat("Vert2X", ref Game.vertices[3], -1.0f, 1.0f);
            // ImGui.SliderFloat("Vert2Y", ref Game.vertices[4], -1.0f, 1.0f);
            // ImGui.SliderFloat("Vert2Z", ref Game.vertices[5], -1.0f, 1.0f);

            // ImGui.SliderFloat("Vert3X", ref Game.vertices[6], -1.0f, 1.0f);
            // ImGui.SliderFloat("Vert3Y", ref Game.vertices[7], -1.0f, 1.0f);
            // ImGui.SliderFloat("Vert3Z", ref Game.vertices[8], -1.0f, 1.0f);


            // ImGui.ColorPicker4("color", ref Game.colorPicked);
            ImGui.End();
        }

        public static void demoValueWindow()
        {
            ImGui.Begin("Values");
            ImGui.Text(Game.angle.ToString());
            ImGui.Separator();
            ImGui.Text("R: " + Game.colorPicked.X.ToString());
            ImGui.Text("G: " + Game.colorPicked.Y.ToString());
            ImGui.Text("B: " + Game.colorPicked.Z.ToString());
            ImGui.Text("A: " + Game.colorPicked.W.ToString());
            ImGui.End();
        }

        // public static void LoadStatistics(float spacing)
        // {
        //     Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        //     // Stats
        //     ImGui.Begin("Statistics");
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

        //     if (ImGui.TreeNode("Rendering"))
        //     {
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Renderer: " + GL.GetString(StringName.Renderer));
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("FPS: " + ImGui.GetIO().Framerate.ToString("0.00"));
        //         ImGui.Text("MS: " + (1000 / ImGui.GetIO().Framerate).ToString("0.00"));
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         // ImGui.Text("Objects: " + (Objects.Count).ToString());
        //         // ImGui.Text("Point Lights: " + (numPL).ToString());
        //         // ImGui.Text("Selected Object: " + selectedObject.ToString());
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Viewport Width: "); ImGui.SameLine(); ImGui.Text(" | "); ImGui.SameLine(); ImGui.Text("Viewport Height: ");
        //         ImGui.TreePop();
        //     }

        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));

        //     if (ImGui.TreeNode("Camera"))
        //     {
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Text("Yaw: ");
        //         ImGui.Text("Pitch: ");
        //         ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //         ImGui.Separator();
        //         ImGui.Text("X: " + position.X);
        //         ImGui.Text("Y: " + position.Y);
        //         ImGui.Text("Z: " + position.Z);
        //         ImGui.TreePop();
        //     }

        //     ImGui.Dummy(new System.Numerics.Vector2(0f, spacing));
        //     ImGui.Separator();
        //     ImGui.End();
        // }




        public static void LoadTheme()
        {
            // Styling
            ImGui.GetStyle().FrameRounding = 6;
            ImGui.GetStyle().FrameBorderSize = 1;
            ImGui.GetStyle().TabRounding = 2;
            ImGui.GetStyle().FramePadding = new System.Numerics.Vector2(4);
            ImGui.GetStyle().ItemSpacing = new System.Numerics.Vector2(8, 2);
            ImGui.GetStyle().ItemInnerSpacing = new System.Numerics.Vector2(1, 4);
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
            ImGui.GetIO().FontGlobalScale = fontSize;

            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(new System.Numerics.Vector3(0.9f), 1));

            // Background color
            ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(22f, 22f, 22f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);

            // Popup BG
            ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, new System.Numerics.Vector4(30f, 30f, 30f, 150f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TextDisabled, new System.Numerics.Vector4(150f, 150f, 150f, 255f) / 255);

            // Titles
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);

            // Tabs
            ImGui.PushStyleColor(ImGuiCol.Tab, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, new System.Numerics.Vector4(16f, 16f, 16f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);
            
            // Header
            ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(0f, 153f, 76f, 180f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);

            // Rezising bar
            ImGui.PushStyleColor(ImGuiCol.Separator, new System.Numerics.Vector4(40f, 40f, 40f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorActive, new System.Numerics.Vector4(80f, 80f, 80f, 255) / 255);

            // Buttons
            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(255, 41, 55, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(255, 41, 55, 150) / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(255, 41, 55, 100) / 255);

            // Docking and rezise
            ImGui.PushStyleColor(ImGuiCol.DockingPreview, new System.Numerics.Vector4(0f, 153f, 76f, 220) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, new System.Numerics.Vector4(217, 35, 35, 255) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, new System.Numerics.Vector4(217, 35, 35, 200) / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, new System.Numerics.Vector4(217, 35, 35, 150) / 255);

            // Sliders, buttons, etc
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, new System.Numerics.Vector4(120f, 120f, 120f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new System.Numerics.Vector4(180f, 180f, 180f, 255f) / 255);
        }
    }
}
