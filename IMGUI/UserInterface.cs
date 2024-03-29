﻿using OpenTK.Graphics.OpenGL4;

using ImGuiNET;

namespace shakespear.cameraapp.gui
{
    public static class GUI
    {
        

        public static void WindowOnOffs()
        {
            LoadMenuBar();
            CameraSettingsWindow();
            LogWindow();
            FilterWindow();
            ControlWindow();
        }

        public static void ControlWindow()
        {
            ImGui.Begin("Control");

            if(ImGui.Button("Trigger All Cameras"))
            {
                for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
                {
                    UserLogic.CameraParams[camera].TrigTrigger = true;
                }
            }
            if(ImGui.Button("Save All Cameras"))
            {
                for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
                {
                    UserLogic.CameraParams[camera].TrigSave = true;
                }
            }

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));
                if(ImGui.Button("Camera " + (camera+1) + " Trigger"))
                {
                    UserLogic.CameraParams[camera].TrigTrigger = true;
                }
                if(ImGui.Button("Camera " + (camera+1) + " Save"))
                {
                    UserLogic.CameraParams[camera].TrigSave = true;
                }
                // CreateFilter(camera);
            }
            ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 30));

            if(ImGui.Button("Trigger All Outputs"))
            {
                for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
                {
                    UserLogic.OutputParams[camera].TrigTrigger = true;
                }
            }
            if(ImGui.Button("Save All Outputs"))
            {
                for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
                {
                    UserLogic.OutputParams[camera].TrigSave = true;
                }
            }

            for (int output = 0; output < UserLogic.TotalOutputs; output++)
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));
                if(ImGui.Button("Output " + (output+1) + " Trigger"))
                {
                    UserLogic.OutputParams[output].TrigTrigger = true;
                }
                if(ImGui.Button("Output " + (output+1) + " Save"))
                {
                    UserLogic.OutputParams[output].TrigSave = true;
                }
                // CreateFilter(camera);
            }



            ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));
            
            ImGui.SliderInt("Disparities", ref UserLogic.Disparities, 0, 255);

            ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));

            ImGui.SliderInt("Block Size", ref UserLogic.BlockSize, 5, 255);

            ImGui.End();
        }

        public static void FilterWindow()
        {
            ImGui.Begin("Filters");

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 30));
                ImGui.Text("Camera " + (camera+1));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));
                // CreateFilter(camera);
            }

            ImGui.Separator();
            ImGui.End();
        }

        // private static void CreateFilter(int _camera)
        // {
        //     ImGui.Checkbox("Canny " + (_camera+1), ref UserLogic.FilterStatus[_camera,0]);
        //     ImGui.SliderInt("A " + (_camera+1), ref UserLogic.FilterSettings[_camera,0,0], 0, 255);
        //     ImGui.SliderInt("B " + (_camera+1), ref UserLogic.FilterSettings[_camera,0,1], 0, 255);
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));

        //     ImGui.Checkbox("Guassian " + (_camera+1), ref UserLogic.FilterStatus[_camera,1]);
        //     ImGui.SliderInt("Size " + (_camera+1), ref UserLogic.FilterSettings[_camera,1,0], 1, 255);
        //     ImGui.SliderInt("Sigma " + (_camera+1), ref UserLogic.FilterSettings[_camera,1,1], 0, 255);
        //     ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));

        //     ImGui.Checkbox("Threshold " + (_camera+1), ref UserLogic.FilterStatus[_camera,2]);
        //     ImGui.SliderInt("Min " + (_camera+1), ref UserLogic.FilterSettings[_camera,2,0], 0, 255);
        //     ImGui.SliderInt("Set " + (_camera+1), ref UserLogic.FilterSettings[_camera,2,1], 0, 255);

        //     ImGui.Separator();
        // }


        public static void CameraOneWindow(string name, ref float CameraWidth, ref float CameraHeight, ref int framebufferTexture)
        {
            ImGui.Begin(name);

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
                    ImGui.Checkbox("Statistics", ref UserLogic.showStatistics);
                    ImGui.Separator();
                    ImGui.Checkbox("ImGUI Demo ", ref UserLogic.showDemoWindow);
                    ImGui.EndMenu();
                }

                ImGui.Separator();

                if (ImGui.BeginMenu("Editor"))
                {
                    ImGui.Checkbox("Show Object Properties", ref UserLogic.showObjectProperties);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Light Properties", ref UserLogic.showLightProperties); 
                    ImGui.Separator();
                    ImGui.Checkbox("Show Outliner", ref UserLogic.showOutliner);
                    ImGui.Separator();
                    // ImGui.Checkbox("Show Settings", ref showSettings);
                    ImGui.Separator();
                    ImGui.Checkbox("Show Material Editor", ref UserLogic.showMaterialEditor);
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


        



        public static void LogWindow()
        {
            int col = 3;
            ImGui.Begin("Log");
            
            if (ImGui.Button("Clear"))
                UserLogic.LogData = new List<string[]>();

            ImGui.Separator();

            ImGui.BeginTable("Table", col);

            ImGui.TableSetupColumn("Level");

            ImGui.TableSetupColumn("Time");
            ImGui.TableSetupColumn("Message");
            ImGui.TableHeadersRow();

            List<string[]> rev = new List<string[]>(UserLogic.LogData);
            
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

        private static void CreateCameraSettings(int _camera)
        {
            ImGui.Checkbox("Capture " + (_camera+1), ref UserLogic.CameraParams[_camera].CaptureLive);

            ImGui.Separator();

            if (ImGui.Button("Report Config " + (_camera+1)))
            {
                UserLogic.CameraParams[_camera].TrigReport = true;
            }
            
            createBasicDropdown("Camera " + (_camera+1),
                                UserLogic.CameraParams[_camera].camNames,
                                ref UserLogic.CameraParams[_camera].CurrentCam);

            createBasicDropdown("Resolution " + (_camera+1),
                                UserLogic.CameraParams[_camera].resolutionItems,
                                ref UserLogic.CameraParams[_camera].CurrentResolution);

            Dictionary<string, string[]> fpsItems = new Dictionary<string, string[]>();

            string[] sFPS = {"30", "35", "40", "45", "50", "55", "56", "60"};
            string[] mFPS = {"5", "10", "15"};
            string[] lFPS = {"30", "35", "38", "40"};

            fpsItems.Add("640x480", sFPS);
            //fpsItems.Add("960x600", mFPS);
            fpsItems.Add("1280x720", lFPS);
            //fpsItems.Add("1920x1200", mFPS);
            fpsItems.Add("1920x1080", mFPS);
            
            createBasicDropdown("FPS " +  (_camera+1),
                                fpsItems[UserLogic.CameraParams[_camera].CurrentResolution],
                                ref UserLogic.CameraParams[_camera].CurrentFPS);
            

            if (ImGui.Button("Configure " + (_camera+1)))
            {
                UserLogic.CameraParams[_camera].TrigConfigure = true;
            }

            ImGui.Separator();

            if (ImGui.Button("Toggle Settings " + (_camera+1)))
            {
                UserLogic.CameraParams[_camera].TrigSettings = true;
            }
        }

        

        public static void CameraSettingsWindow()
        {
            ImGui.Begin("Camera Settings");

            for (int camera = 0; camera < UserLogic.TotalCameras; camera++)
            {
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 30));
                ImGui.Text("Camera " + (camera+1));
                ImGui.Separator();
                ImGui.Dummy(new System.Numerics.Vector2(0f, UserLogic.spacing * 10));
                CreateCameraSettings(camera);
            }

            ImGui.Separator();
            ImGui.End();
        }

        // public static void demoValueWindow()
        // {
        //     ImGui.Begin("Values");
        //     ImGui.Text(Game.angle.ToString());
        //     ImGui.Separator();
        //     ImGui.Text("R: " + Game.colorPicked.X.ToString());
        //     ImGui.Text("G: " + Game.colorPicked.Y.ToString());
        //     ImGui.Text("B: " + Game.colorPicked.Z.ToString());
        //     ImGui.Text("A: " + Game.colorPicked.W.ToString());
        //     ImGui.End();
        // }

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


        static System.Numerics.Vector4 base03 = new System.Numerics.Vector4(0f, 43f, 54f, 255f);
        static System.Numerics.Vector4 base02 = new System.Numerics.Vector4(7f, 54f, 66f, 255f);
        static System.Numerics.Vector4 base01 = new System.Numerics.Vector4(88f, 110f, 117f, 255f);
        static System.Numerics.Vector4 base00 = new System.Numerics.Vector4(101f, 123f, 131f, 255f);

        static System.Numerics.Vector4 base0 = new System.Numerics.Vector4(131f, 148f, 150f, 255f);
        static System.Numerics.Vector4 base1 = new System.Numerics.Vector4(147f, 161f, 161f, 255f);
        static System.Numerics.Vector4 base2 = new System.Numerics.Vector4(238f, 232f, 213f, 255f);
        static System.Numerics.Vector4 base3 = new System.Numerics.Vector4(253f, 246f, 227f, 255f);


        static System.Numerics.Vector4 cyan = new System.Numerics.Vector4(42f, 161f, 152f, 255f);
        static System.Numerics.Vector4 cyanH = new System.Numerics.Vector4(42f, 161f, 152f, 100f);
        static System.Numerics.Vector4 cyanA = new System.Numerics.Vector4(42f, 161f, 152f, 50f);

        static System.Numerics.Vector4 magenta = new System.Numerics.Vector4(211f, 54f, 130f, 255);
        static System.Numerics.Vector4 violet = new System.Numerics.Vector4(108f, 113f, 196f, 255f);


        static System.Numerics.Vector4 back0 = base02;
        static System.Numerics.Vector4 back1 = base03;

        static System.Numerics.Vector4 fore0 = base2;
        static System.Numerics.Vector4 fore1 = base3;
        
        public static void LoadTheme()
        {
            // Styling
            ImGui.GetStyle().FrameRounding = 0;
            ImGui.GetStyle().FrameBorderSize = 1;
            ImGui.GetStyle().TabRounding = 0;
            ImGui.GetStyle().FramePadding = new System.Numerics.Vector2(8);
            ImGui.GetStyle().ItemSpacing = new System.Numerics.Vector2(16, 2);
            ImGui.GetStyle().ItemInnerSpacing = new System.Numerics.Vector2(1, 4);
            //ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
            //ImGui.GetIO().FontGlobalScale = UserLogic.fontSize;

            ImGui.PushStyleColor(ImGuiCol.Text, fore0 / 255);

            // Background color
            ImGui.PushStyleColor(ImGuiCol.WindowBg, back1 / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBg, back1 / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, back1 / 255);
            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, back1 / 255);

            ImGui.PushStyleColor(ImGuiCol.MenuBarBg, back0 / 255);


            ImGui.PushStyleColor(ImGuiCol.Border, base01 / 255);

            // Popup BG
            //ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, base3 / 255);
            //ImGui.PushStyleColor(ImGuiCol.TextDisabled, new System.Numerics.Vector4(150f, 150f, 150f, 255f) / 255);

            // Titles
            ImGui.PushStyleColor(ImGuiCol.TitleBgActive, base03 / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBg, base03 / 255);
            ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, base03 / 255);
            

            // Tabs
            ImGui.PushStyleColor(ImGuiCol.Tab, violet / 255);
            ImGui.PushStyleColor(ImGuiCol.TabActive, violet / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocused, violet / 255);
            ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, violet / 255);
            ImGui.PushStyleColor(ImGuiCol.TabHovered, violet / 255);
            
            // Header
            ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(108f, 113f, 196f, 255f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(108f, 113f, 196f, 180f) / 255);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(108f, 113f, 196f, 255f) / 255);

            // Rezising bar
            ImGui.PushStyleColor(ImGuiCol.Separator, base01 / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorHovered,base01 / 255);
            ImGui.PushStyleColor(ImGuiCol.SeparatorActive, base01 / 255);

            // Buttons
            ImGui.PushStyleColor(ImGuiCol.Button, cyanH / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, cyan / 255);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, cyanA / 255);

            ImGui.PushStyleColor(ImGuiCol.CheckMark, cyanH / 255);


            
            // Docking and rezise
            ImGui.PushStyleColor(ImGuiCol.DockingPreview, magenta / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, magenta / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, magenta / 255);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, magenta / 255);

            // Sliders, buttons, etc
            ImGui.PushStyleColor(ImGuiCol.SliderGrab, cyanH / 255);
            ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, cyan / 255);
        }
        // public static void LoadTheme()
        // {
        //     // Styling
        //     ImGui.GetStyle().FrameRounding = 6;
        //     ImGui.GetStyle().FrameBorderSize = 1;
        //     ImGui.GetStyle().TabRounding = 2;
        //     ImGui.GetStyle().FramePadding = new System.Numerics.Vector2(4);
        //     ImGui.GetStyle().ItemSpacing = new System.Numerics.Vector2(8, 2);
        //     ImGui.GetStyle().ItemInnerSpacing = new System.Numerics.Vector2(1, 4);
        //     ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.None;
        //     ImGui.GetIO().FontGlobalScale = UserLogic.fontSize;

        //     ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(new System.Numerics.Vector3(0.9f), 1));

        //     // Background color
        //     ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(22f, 22f, 22f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.FrameBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.FrameBgActive, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);

        //     // Popup BG
        //     ImGui.PushStyleColor(ImGuiCol.ModalWindowDimBg, new System.Numerics.Vector4(30f, 30f, 30f, 150f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TextDisabled, new System.Numerics.Vector4(150f, 150f, 150f, 255f) / 255);

        //     // Titles
        //     ImGui.PushStyleColor(ImGuiCol.TitleBgActive, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TitleBg, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TitleBgCollapsed, new System.Numerics.Vector4(15f, 15f, 15f, 255f) / 255);

        //     // Tabs
        //     ImGui.PushStyleColor(ImGuiCol.Tab, new System.Numerics.Vector4(20f, 20f, 20f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TabActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TabUnfocused, new System.Numerics.Vector4(16f, 16f, 16f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TabUnfocusedActive, new System.Numerics.Vector4(35f, 35f, 35f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.TabHovered, new System.Numerics.Vector4(80f, 80f, 80f, 255f) / 255);
            
        //     // Header
        //     ImGui.PushStyleColor(ImGuiCol.Header, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(0f, 153f, 76f, 180f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(0f, 153f, 76f, 255f) / 255);

        //     // Rezising bar
        //     ImGui.PushStyleColor(ImGuiCol.Separator, new System.Numerics.Vector4(40f, 40f, 40f, 255) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, new System.Numerics.Vector4(60f, 60f, 60f, 255) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.SeparatorActive, new System.Numerics.Vector4(80f, 80f, 80f, 255) / 255);

        //     // Buttons
        //     ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4(255, 41, 55, 200) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new System.Numerics.Vector4(255, 41, 55, 150) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.ButtonActive, new System.Numerics.Vector4(255, 41, 55, 100) / 255);

        //     // Docking and rezise
        //     ImGui.PushStyleColor(ImGuiCol.DockingPreview, new System.Numerics.Vector4(0f, 153f, 76f, 220) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.ResizeGrip, new System.Numerics.Vector4(217, 35, 35, 255) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, new System.Numerics.Vector4(217, 35, 35, 200) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, new System.Numerics.Vector4(217, 35, 35, 150) / 255);

        //     // Sliders, buttons, etc
        //     ImGui.PushStyleColor(ImGuiCol.SliderGrab, new System.Numerics.Vector4(120f, 120f, 120f, 255f) / 255);
        //     ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, new System.Numerics.Vector4(180f, 180f, 180f, 255f) / 255);
        // }
    }
}
