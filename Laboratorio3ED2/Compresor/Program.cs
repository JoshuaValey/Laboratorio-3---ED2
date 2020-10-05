using Compresor.Estructuras;
using System;
using System.IO;

namespace Compresor
{
    class Program
    {
        static void Main(string[] args)
        {
            ColaPrioridad<string> cola = new ColaPrioridad<string>();
            Console.WriteLine("Hello World!");
            FileStream file = File.Create(@"C:\Escritorio\test.txt");
            cola.insert(file);
        }

    }
}
