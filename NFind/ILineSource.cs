using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    interface ILineSource
    {
        int NumberOfLines { get; }
        string NameFile { get; }
        Line? ReadLine();

        void Open();
        void Close();
    }
}
