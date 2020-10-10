using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compresor.Interfaces
{
    interface HuffmanInterface<T> where T : IComparable
    {
        public string Comprimir(FileStream archivo, string nombre);
    }
}
