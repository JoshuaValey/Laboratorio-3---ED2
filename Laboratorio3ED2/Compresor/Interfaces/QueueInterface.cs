using Compresor.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compresor.Interfaces
{
    interface QueueInterface<T> where T : IComparable
    {
        public NodoCola<T>[] insert(T value);
    }
}
