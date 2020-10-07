using System;
using Compresor.Estructuras;
using System.Collections.Generic;



namespace PruebaCompresor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            char binario = 'H';
            long dato = Convert.ToInt64(binario);
            //byte num = Convert.ToByte(binario);
            string cadenaBinaria = Convert.ToString(dato, 2);


            Console.WriteLine(cadenaBinaria);

            int flag1 = 0;
            int close = 9;
        }
    }
}