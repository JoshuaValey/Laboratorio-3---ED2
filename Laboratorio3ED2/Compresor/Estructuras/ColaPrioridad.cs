using Compresor.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compresor.Estructuras
{
    public class ColaPrioridad<T> : QueueInterface<T> where T : IComparable
    {
        public NodoCola<T>[] insert(T value)
        {
            throw new NotImplementedException();
        }
    }
}
