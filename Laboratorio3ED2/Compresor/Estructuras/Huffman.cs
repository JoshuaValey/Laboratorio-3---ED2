using System.Collections.Generic;
using Compresor.Huffman;

namespace Compresor.Huffman
{
    public class Huffman<T> where T : System.IComparable
    {
        public NodoHuff<T> Raiz { get; set; }

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
                if (cola.Dequeue() == null)
                {
                    Raiz = auxDer;
                    Raiz.FrecPrio = 1;

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
            }

        }
        public string BynaryEncode()
        {
            return "";
        }

    }
}