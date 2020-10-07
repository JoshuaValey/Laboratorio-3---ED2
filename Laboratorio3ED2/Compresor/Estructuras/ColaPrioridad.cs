using Compresor.ColaLabED1;
using Compresor.Huffman;
using Compresor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Compresor.Estructuras
{
    public class ColaPrioridad<T> : QueueInterface<T> where T : IComparable
    {
        int contador = 0;
        public List<NodoCola<T>> colaPrioridad = new List<NodoCola<T>>();
        NodoHuff<byte> nodoHuffman = new NodoHuff<byte>();
        ColaED1<NodoHuff<byte>> priorityQueue = new ColaED1<NodoHuff<byte>>();
        public List<byte> arregloBytes = new List<byte>();
        public int cantidadBytes = 0;
        public ColaED1<NodoHuff<byte>> insert(FileStream archivo)
        {
            using var reader = new BinaryReader(archivo);
            var buffer = new byte[2000000];
            while (archivo.Position < archivo.Length)
            {
                buffer = reader.ReadBytes(2000000);
                evaluarCadena(buffer);
                devolverBytes(buffer);
            }
            reader.Close();
            archivo.Close();

            for(int i = 0; i < colaPrioridad.Count; i++)
            {
                nodoHuffman.Value = colaPrioridad[i].valor;
                nodoHuffman.Frecuencia = colaPrioridad[i].prioridad;
                nodoHuffman.ProbPrio = colaPrioridad[i].prioridad / contador;
                priorityQueue.Insert(nodoHuffman.ProbPrio, nodoHuffman);
            }
            return priorityQueue;
        }
        public void evaluarCadena(byte[] cadena)
        {
            int aux = contador;
            NodoCola<T> nodo = new NodoCola<T>();
            for(int i = 0; i<cadena.Length; i++)
            {
                //ya existe la cadena de nodos para la cola  
                if(contador != 0)
                {
                    foreach(var item in cadena)
                    {
                        for(int j = 0; j < colaPrioridad.Count; j++)
                        {
                            if (item.Equals(colaPrioridad[j]))
                            {
                                colaPrioridad[j].prioridad++;
                                contador++;
                                break;
                            }
                        }
                        if(contador == aux)
                        {
                            colaPrioridad[colaPrioridad.Count + 1].valor = item;
                            colaPrioridad[colaPrioridad.Count + 1].prioridad++;
                            cantidadBytes++;
                            contador++;
                        }
                    }
                }
                else //aun no existe nodo para la cola 
                {
                    colaPrioridad[0].valor = cadena[0];
                    colaPrioridad[0].prioridad++;
                    contador++;
                }
            }
        }
        public void devolverBytes(byte[] cadena)
        {
            foreach(var item in cadena)
            {
                arregloBytes.Add(item);
            }
        }
    }
}