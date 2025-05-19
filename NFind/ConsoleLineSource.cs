namespace NFind
{
    //Decorator Pattern
    internal class ConsoleLineSource : ILineSource
    {
        private int _number = 0;

        public ConsoleLineSource()
        {
        }

        public string NameFile => string.Empty;

        public int NumberOfLines => 0;

        public void Close()
        {
        }

        public void Open()
        {
            _number = 0;
        }

        public Line? ReadLine()
        {
            var s = Console.ReadLine();
            if (s == null) 
                return null;
            else
                return new Line { LineNumber = ++_number, Text = s };
        }
    }
}