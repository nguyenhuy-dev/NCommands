using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    //DECORATOR PATTERN: THÊM CHỨC NĂNG CHO 1 LỚP
    class FilteredLineSource : ILineSource
    {
        private readonly ILineSource _parent;
        private readonly Func<Line, bool> _f;
        private int _count = 0;

        public string NameFile => _parent.NameFile;

        public int NumberOfLines => _count;

        public FilteredLineSource(ILineSource parent, Func<Line, bool> f)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _f = f;
        }

        public Line? ReadLine()
        {
            var line = _parent.ReadLine();
            if (line == null)
                return null;
            else
            {
                while (line != null && _f(line) == false)
                    line = _parent.ReadLine();

                if (line != null)
                    _count++;

                return line;
            }
        }

        public void Open()
        {
            _parent.Open();
        }

        public void Close()
        {
            _parent.Close();
        }
    }
}
