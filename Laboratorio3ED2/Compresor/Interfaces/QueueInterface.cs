using Compresor.Estructuras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compresor.Interfaces
{
    interface QueueInterface<T> where T : IComparable
    {
        public List<NodoCola<T>> insert(FileStream archivo, T value);
    }
}
