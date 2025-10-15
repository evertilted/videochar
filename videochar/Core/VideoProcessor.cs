using FFMpegCore;
using FFMpegCore.Pipes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace videochar.Core
{
    [SupportedOSPlatform("Windows")]
    internal sealed class VideoProcessor
    {
        static VideoProcessor()
        {
            GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = Path.Combine(Environment.CurrentDirectory, "ffmpeg_bin");
            });
        }

        public static VideoData ExtractVideoData(string source)
        {
            var videoInfo = FFProbe.Analyse(source);
            var videoStream = videoInfo?.PrimaryVideoStream;

            if (videoStream == null)
            {
                throw new FileNotFoundException($"Failed to find a video at specified path ({source})");
            }

            int width = videoStream.Width;
            int height = videoStream.Height;
            var result = new VideoData(
                width,
                height,
                videoStream.AvgFrameRate,
                (int)(videoStream.AvgFrameRate * videoStream.Duration.TotalSeconds));            

            using (var fileStream = new MemoryStream())
            {
                FFMpegArguments
                .FromFileInput(source)
                .OutputToPipe(new StreamPipeSink(fileStream), options => options
                    .WithVideoCodec("rawvideo")
                    .ForceFormat("rawvideo"))
                .ProcessSynchronously();

                fileStream.Position = 0;
                int frameSize = width * height * 3;
                var buffer = new byte[frameSize];
                int frameIndex = 0;

                while (fileStream.Position < fileStream.Length)
                {
                    int bytesRead = fileStream.Read(buffer, 0, frameSize);
                    if (bytesRead < frameSize)
                    {
                        break;
                    }

                    var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    var bitmapData = bitmap.LockBits(
                        new Rectangle(0, 0, width, height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format24bppRgb
                    );

                    Marshal.Copy(buffer, 0, bitmapData.Scan0, frameSize);
                    bitmap.UnlockBits(bitmapData);

                    result.Frames[frameIndex] = bitmap;
                    frameIndex++;
                }
            }
            return result;
        }
    }
}
