using System;
using System.Collections.Generic;
using System.Text;

namespace Compresor.ColaLabED1
{
    public class Node<T> where T: IComparable
    {
        public T value { get; set; }
        public decimal priority { get; set; }
    }
}
