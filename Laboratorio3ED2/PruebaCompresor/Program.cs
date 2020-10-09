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
            Compresor.Huffman.Huffman<byte> compresor = new Compresor.Huffman.Huffman<byte>();
            FileStream file = new FileStream(@"./Prueba.txt", FileMode.Open);
            string comprimido = compresor.Comprimir(file);

            
            //Console.WriteLine(CadenaBinAInt("11111111").ToString());
        }

         static int CadenaBinAInt(string cadenaBinaria)
        {
            int resultado = 0;

            int[] baseDecimal = {128,64,32,16,8,4,2,1};
            
            for (int i = 0; i < 8; i++)
                if (cadenaBinaria[i] == '1') resultado += baseDecimal[i];
            
            return resultado;
        }
    }
}