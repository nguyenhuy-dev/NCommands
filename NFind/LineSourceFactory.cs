using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    internal class LineSourceFactory
    {
        public static ILineSource[] CreateInstance(string path, bool skipOfflineFiles)
        {
            if (string.IsNullOrEmpty(path))
                return [new ConsoleLineSource()];
            else
            {
                string pattern;
                int idx = path.LastIndexOf(Path.PathSeparator);
                if (idx < 0)
                {
                    pattern = path;
                    path = ".";
                }
                else
                {
                    pattern = path.Substring(idx + 1);
                    path = path.Substring(0, idx);
                }

                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    var files = dir.GetFiles(pattern);
                    if (skipOfflineFiles)
                        files = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Offline)).ToArray();

                    return files.Select(f => new FileLineSource(f.FullName)).ToArray();
                }
            }

            return [];
        }
    }
}
