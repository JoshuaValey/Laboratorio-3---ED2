using System;

namespace Compresor.Huffman
{
    public class NodoHuff<T> : IComparable where T : IComparable
    {
        public T Value { get; set; }
        public int Frecuencia { get; set; }
        public decimal ProbPrio { get; set; }
        public NodoHuff<T> Derecho { get; set; }
        public NodoHuff<T> Inzquierdo { get; set; }

        public int CompareTo(object obj)
        {
            return this.Value.CompareTo(obj);
        }
    }
    
}
