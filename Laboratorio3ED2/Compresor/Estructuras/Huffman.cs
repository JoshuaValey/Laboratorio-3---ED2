using System.Collections.Generic;
using Compresor.Huffman;

namespace Compresor.Huffman
{
    public class Huffman<T> where T : System.IComparable
    {
        public NodoHuff<T> Raiz { get; set; }
        public Dictionary<byte, string> codigosPrefijo = new Dictionary<byte, string>();

        private void GenerarPrefijos()
        {
            //Recorrido preorder para generar los prefijos 
            //Hacer el reccorrido y cuando se encuentre unnodo sin hijos
            //agregar el value de ese nodo como llave del diccionario y el
            //string prefijo como valor. 
        }

        private void CrearArbol(Queue<NodoHuff<T>> cola)
        {

            NodoHuff<T> auxIzqu = new NodoHuff<T>();
            NodoHuff<T> auxDer = new NodoHuff<T>();
            NodoHuff<T> auxPadre = new NodoHuff<T>();


            try
            {
                auxDer = cola.Dequeue();
                auxIzqu = cola.Dequeue();

                //Esta validación o validar si el elemento de la cola auxDer es auxDer.FrePrio = 1
                if (auxIzqu == null)
                {
                    Raiz = auxDer;
                    Raiz.FrecPrio = 1;
                    GenerarPrefijos();

                }
                else
                {
                    auxPadre.FrecPrio = auxDer.FrecPrio + auxIzqu.FrecPrio;
                    auxPadre.Derecho = auxDer;
                    auxPadre.Inzquierdo = auxIzqu;

                    cola.Enqueue(auxPadre);

                    CrearArbol(cola);
                }
            }
            catch (System.NullReferenceException)
            {
                Raiz = auxDer;
                Raiz.FrecPrio = 1;
                GenerarPrefijos();
            }

        }
        public string BynaryEncode()
        {
            return "";
        }

    }
}