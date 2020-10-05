using Compresor.ColaLabED1;
using Compresor.Estructuras;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compresor.Interfaces
{
    interface QueueInterface<T> where T : IComparable
    {
        public ColaED1<byte> insert(FileStream archivo);
    }
}
