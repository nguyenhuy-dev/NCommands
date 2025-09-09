using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    internal class FilteredLineSource : ILineSource
    {
        private readonly ILineSource _parent;
        private readonly Func<Line, bool>? _f;

        public FilteredLineSource(ILineSource parent, Func<Line, bool>? f)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _f = f;
        }

        public string FileName => _parent.FileName;

        public void Close()
        {
            _parent.Close();
        }

        public void Open()
        {
            _parent.Open();
        }

        public Line? ReadLine()
        {
            if (_f == null) 
                return _parent.ReadLine();

            var line = _parent.ReadLine();
            if (line == null)
                return null;
            
            while (line != null && _f(line) == false)
                line = _parent.ReadLine();

            return line;
        }
    }
}
