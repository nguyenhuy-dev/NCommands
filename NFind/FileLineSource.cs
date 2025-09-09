namespace NFind
{
    internal class FileLineSource : ILineSource
    {
        private readonly string _path;
        private StreamReader? _reader;
        private int _lineNumber;
        private readonly string _fileName;

        public FileLineSource(string path)
        {
            _path = path;
            _fileName = Path.GetFileName(path);
        }

        public string FileName => _fileName;

        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }
        }

        public void Open()
        {
            if (_reader != null)
                throw new InvalidOperationException();

            _lineNumber = 0;
            _reader = new StreamReader(new FileStream(_path, FileMode.Open, FileAccess.Read));
        }

        public Line? ReadLine()
        {
            if (_reader == null)
                throw new InvalidOperationException();

            var s = _reader.ReadLine();

            if (s == null)
                return null;
            else
            {
                return new Line() { LineNumber = ++_lineNumber, Text = s };
            }
        }
    }
}