namespace NFind
{
    public class Program
    {
        static void Main(string[] args)
        {
            var findOptions = BuildOptions(args);

            if (string.IsNullOrEmpty(findOptions.StringToFind))
            {
                Console.WriteLine("FIND: Parameter format not correct");
                return;
            }

            if (findOptions.HelpMode)
            {
                PrintHelp();
                return;
            }

            var sources = LineSourceFactory.CreateInstance(findOptions.Path, findOptions.SkipOfflineFiles);

            if (sources is FileLineSource[])
                foreach (var source in sources)
                    ProcessSource(source, findOptions);
            else
                ProcessConsoleSource(findOptions);
        }

        private static void ProcessConsoleSource(FindOptions findOptions)
        {
            string flagToExit = "exit";

            string? input = Console.ReadLine();

            while (input != null && !input.Equals(flagToExit, StringComparison.OrdinalIgnoreCase))
            {
                if (input.Contains(findOptions.StringToFind))
                    Console.WriteLine(input);

                input = Console.ReadLine();
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Searches for a text string in a file or files.\r\n\r\nFIND [/V] [/C] [/N] [/I] [/OFF[LINE]] \"string\" [[drive:][path]filename[ ...]]\r\n\r\n  /V         Displays all lines NOT containing the specified string.\r\n  /C         Displays only the count of lines containing the string.\r\n  /N         Displays line numbers with the displayed lines.\r\n  /I         Ignores the case of characters when searching for the string.\r\n  /OFF[LINE] Do not skip files with offline attribute set.\r\n  \"string\"   Specifies the text string to find.\r\n  [drive:][path]filename\r\n             Specifies a file or files to search.\r\n\r\nIf a path is not specified, FIND searches the text typed at the prompt\r\nor piped from another command.");
        }

        private static void ProcessSource(ILineSource source, FindOptions findOptions)
        {
            StringComparison stringComparison = findOptions.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            source = new FilteredLineSource(source,
                line => findOptions.FindDontConstain ? !line.Text.Contains(findOptions.StringToFind, stringComparison) : line.Text.Contains(findOptions.StringToFind, stringComparison)
                );

            Console.WriteLine($"\n---------- {source.FileName}".ToUpper());

            try
            {
                source.Open();
                var line = source.ReadLine();
                int count = 0;
                bool countMode = findOptions.CountMode;

                while (line != null)
                {
                    if (countMode)
                        count++;
                    else
                        Print(line, findOptions);

                    line = source.ReadLine();
                }
                if (countMode)
                    Console.Write(count);
            }
            finally
            {
                source.Close();
            }
        }

        private static void Print(Line line, FindOptions options)
        {
            if (options.ShowLineNumber)
                Console.WriteLine($"[{line.LineNumber}]{line.Text}");
            else
                Console.WriteLine(line.Text);
        }

        public static FindOptions BuildOptions(string[] args)
        {
            var options = new FindOptions();

            foreach (var arg in args)
            {
                if (arg == "/v")
                    options.FindDontConstain = true;
                else if (arg == "/c")
                    options.CountMode = true;
                else if (arg == "/n")
                    options.ShowLineNumber = true;
                else if (arg == "i")
                    options.IsCaseSensitive = false;
                else if (arg == "/off" || arg == "offline")
                    options.SkipOfflineFiles = false;
                else if (arg == "/?")
                    options.HelpMode = true;
                else
                {
                    if (string.IsNullOrEmpty(options.StringToFind))
                        options.StringToFind = arg;
                    else if (string.IsNullOrEmpty(options.Path))
                        options.Path = arg;
                }
            }

            return options;
        }
    }
}
