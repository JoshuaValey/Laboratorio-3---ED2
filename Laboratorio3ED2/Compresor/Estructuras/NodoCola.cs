using System;
using System.Collections.Generic;
using System.Text;

namespace Compresor.Estructuras
{
    public class NodoCola<T> where T : IComparable
    {
        public byte valor { get; set; }
        public int prioridad { get; set; }
    }
}
