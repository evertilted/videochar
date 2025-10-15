using System.Runtime.Versioning;
using videochar.Core;

namespace videochar
{
    [SupportedOSPlatform("Windows")]
    public class Videochar
    {
        public static void Main(string[] args)
        {
            if (!args.Any())
            {
#if DEBUG
                // this video actually exists in my current system; replace with another for testing purposes
                args = new string[] { "C:\\Users\\David Tiltman\\Desktop\\【東方】Bad Apple! ＰＶ【影絵】.mp4" };
#else
                Console.WriteLine("Expected a path to a video as an argument; try drag&dropping a file on this executable.\nPress any key to exit");
                Console.ReadKey();
                return;
#endif
            }

            try
            {
                var videoData = VideoProcessor.ExtractVideoData(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured:\n{ex}\nPress any key to exit");
                Console.ReadKey();
                return;
            }
        }
    }
}