using System.Collections.Generic;
using Compresor.Huffman;

namespace Compresor.Huffman
{
    public class Huffman
    {
        public NodoHuff<byte> Raiz { get; set; }
        public Dictionary<byte, string> codigosPrefijo = new Dictionary<byte, string>();

        private void GenerarPrefijos()
        {
            //Recorrido preorder para generar los prefijos 
            //Hacer el reccorrido y cuando se encuentre unnodo sin hijos
            //agregar el value de ese nodo como llave del diccionario y el
            //string prefijo como valor. 
            SubArbolPrefijos(Raiz.Inzquierdo, "0");
            SubArbolPrefijos(Raiz.Derecho, "1");
        }
        private void SubArbolPrefijos(NodoHuff<byte> nodoActual, string codigo)
        {
            if (nodoActual.Inzquierdo == null && nodoActual.Derecho == null)
            {
                codigosPrefijo.Add(nodoActual.Value, codigo);
            }
            else
            {
                SubArbolPrefijos(nodoActual.Inzquierdo, $"{codigo}0");
                SubArbolPrefijos(nodoActual.Derecho, $"{codigo}1");
            }
        }
        private void CrearArbol(Queue<NodoHuff<byte>> cola)
        {
            NodoHuff<byte> auxIzqu = new NodoHuff<byte>();
            NodoHuff<byte> auxDer = new NodoHuff<byte>();
            NodoHuff<byte> auxPadre = new NodoHuff<byte>();
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
            catch (System.NullReferenceException ex)
            {
                Raiz = auxDer;
                Raiz.FrecPrio = 1;
                GenerarPrefijos();

            }

        }

        /// <summary>
        /// Este método recive un arreglo de bytes y por medio del algorítmo de Huffman
        /// comprime estos bytes a su correlativo en un prefijo de ceros y unos más peuqueño 
        /// que el valor original. 
        /// </summary>
        /// <param name="cadena">Arreglo de bytes, con el contenido del archivo, a ser comprimido</param> 
        /// <returns> Retorna una cadena con el mensaje comprimido en ceros y unos </returns>
        public string BynaryEncode(byte[] cadena, Queue<NodoHuff<byte>> cola)
        {
            CrearArbol(cola);
            string resultado = "";
            foreach (var item in cadena)
            {
                resultado += codigosPrefijo[item];
            }

            int byteFaltante = resultado.Length % 8;
            if (!(byteFaltante == 0))
            {
                int caracteres = 8 - byteFaltante;
                for (int i = caracteres; i > 0; i--) resultado += "0";
            }

            return resultado;
        }

    }
}