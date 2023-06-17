
namespace shakespear.application {
    public class Program {
        public static void Main(string[] args)
        {
            int initWidth = 3840;
            int initHeight = 2066;

            string initWindowText = "Camera App";
            string fontPath = @"C:\Windows\Fonts\calibri.ttf";

            float fontSize = 25f;

            using (shakespear.cameraapp.Window game = new shakespear.cameraapp.Window(initWidth, initHeight, initWindowText, fontPath, fontSize))
            {
                game.Run();
            }
        }
    }
}