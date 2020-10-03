using Compresor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compresor.Estructuras
{
    public class ColaPrioridad<T> : QueueInterface<T> where T : IComparable
    {
        int contador = 0;
        List<NodoCola<T>> colaPrioridad = new List<NodoCola<T>>();
        public List<NodoCola<T>> insert(FileStream archivo, T value)
        {
            using var reader = new BinaryReader(archivo);
            var buffer = new byte[2000000];
            while (archivo.Position < archivo.Length)
            {
                buffer = reader.ReadBytes(2000000);
                evaluarCadena(buffer);
            }
            reader.Close();
            archivo.Close();
            return colaPrioridad;
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
                            colaPrioridad[colaPrioridad.Count + 1].valor = item.ToString();
                            colaPrioridad[colaPrioridad.Count + 1].prioridad++;
                        }
                    }
                }
                else //aun no existe nodo para la cola 
                {
                    colaPrioridad[0].valor = cadena[0].ToString();
                    colaPrioridad[0].prioridad++;
                }
            }
        }
    }
}