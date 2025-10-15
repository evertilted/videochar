using System.Drawing;

namespace videochar.Core
{
    /// <summary>
    /// Video info needed to recreate an ascii version
    /// </summary>
    internal class VideoData
    {
        private int _width;
        private int _height;
        private double _avgFramerate;
        private Bitmap[]? _frames;

        public int Width
        {
            get => _width;
            private set
            {
                _width = value;
            }
        }

        public int Height
        {
            get => _height;
            private set
            {
                _height = value;
            }
        }

        public double AvgFramerate
        {
            get => _avgFramerate;
            private set
            {
                _avgFramerate = value;
            }
        }

        public Bitmap[]? Frames
        {
            get => _frames;
            private set
            {
                _frames = value;
            }
        }

        // todo audiostream

        public VideoData(int width, int height, double avgFramerate, int frameCount)
        {
            Width = width;
            Height = height;
            _avgFramerate = avgFramerate;
            Frames = new Bitmap[frameCount];
        }
    }
}
