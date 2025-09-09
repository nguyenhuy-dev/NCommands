namespace NFind
{
    internal class ConsoleLineSource : ILineSource
    {
        private int _number = 0;

        public string FileName => string.Empty;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public Line? ReadLine()
        {
            var s = Console.ReadLine();

            if (s == null)
                return null;
            else 
                return new Line() { LineNumber = ++_number, Text = s };
        }
    }
}