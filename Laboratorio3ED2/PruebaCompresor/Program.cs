using System;
using Compresor.Estructuras;
using System.Collections.Generic;
using Compresor.Estructuras;
using System.IO;




namespace PruebaCompresor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            /* char binario = 'H';
             long dato = Convert.ToInt64(binario);
             //byte num = Convert.ToByte(binario);
             string cadenaBinaria = Convert.ToString(dato, 2);


             Console.WriteLine(cadenaBinaria);*/

            Compresor.Huffman.Huffman<byte> compressor = new Compresor.Huffman.Huffman<byte>();
            try
            {
                FileStream file = new FileStream(@".\prueba2", FileMode.Create, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(file);
                streamWriter.WriteLine("ddabdccedchafbadgdcgabgccddbcdgg");
                streamWriter.Close();
                file.Close();
                FileStream file2 = new FileStream(@".\prueba2", FileMode.Open, FileAccess.Read);
                string comprimido = compressor.Comprimir(file2);
                file2.Close();


                Console.WriteLine(comprimido);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }



            Console.WriteLine("FinPrueba");

            int flag1 = 0;
            int close = 9;
        }
    }
}