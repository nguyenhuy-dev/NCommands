namespace NFind
{
    //Decorator Pattern
    internal class FileLineSource : ILineSource
    {
        private readonly string path;
        private readonly string nameFile;
        private StreamReader? _reader;
        private int _number = 0;

        public string NameFile => nameFile;

        public int NumberOfLines => 0;

        public FileLineSource(string path, string nameFile)
        {
            this.path = path;
            this.nameFile = nameFile;
        }

        public string GetPath() { return path; }

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

            _reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public Line? ReadLine()
        {
            if (_reader == null)
                throw new InvalidOperationException();
            var s = _reader.ReadLine();

            if (s == null)
                return null;
            else
                return new Line() { LineNumber = ++_number, Text = s };
        }
    }
}