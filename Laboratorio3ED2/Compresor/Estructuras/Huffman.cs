using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Compresor.ColaLabED1;
using Compresor.Estructuras;
using Compresor.Huffman;
using Compresor.Interfaces;

namespace Compresor.Huffman
{
    public class Huffman<T> : HuffmanInterface<T> where T : IComparable
    {
        public NodoHuff<byte> Raiz { get; set; }
        public Dictionary<byte, string> codigosBytePrefijo = new Dictionary<byte, string>();
        public Dictionary<string, byte> cogdigosPrefijoByte = new Dictionary<string, byte>();
        public string textoComprimido { get; set; }
        ColaED1<NodoHuff<byte>> colaPrioridad = new ColaED1<NodoHuff<byte>>();
        ColaPrioridad<byte> cola = new ColaPrioridad<byte>();

        public string Comprimir(FileStream archivo)
        {
            colaPrioridad = cola.insert(archivo);
            string codigoBinario = BynaryEncode(cola.arregloBytes, colaPrioridad);
            devolverASCII(codigoBinario);
            return textoComprimido;
        }
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
                codigosBytePrefijo.Add(nodoActual.Value, codigo);
                cogdigosPrefijoByte.Add(codigo, nodoActual.Value);
            }
            else
            {
                SubArbolPrefijos(nodoActual.Inzquierdo, $"{codigo}0");
                SubArbolPrefijos(nodoActual.Derecho, $"{codigo}1");
            }
        }
        private void CrearArbol(ColaED1<NodoHuff<byte>> cola)
        {
            NodoHuff<byte> auxIzqu = new NodoHuff<byte>();
            NodoHuff<byte> auxDer = new NodoHuff<byte>();
            NodoHuff<byte> auxPadre = new NodoHuff<byte>();
            try
            {
                auxDer = cola.Delete();
                auxIzqu = cola.Delete();

                //Esta validación o validar si el elemento de la cola auxDer es auxDer.FrePrio = 1
                if (auxIzqu == null)
                {
                    Raiz = auxDer;
                    Raiz.ProbPrio = 1;
                    GenerarPrefijos();
                }
                else
                {
                    auxPadre.ProbPrio = auxDer.ProbPrio + auxIzqu.ProbPrio;
                    auxPadre.Derecho = auxDer;
                    auxPadre.Inzquierdo = auxIzqu;

                    cola.Insert(auxPadre.ProbPrio, auxPadre);

                    CrearArbol(cola);
                }
            }
            catch (System.NullReferenceException ex)
            {
                Raiz = auxDer;
                Raiz.ProbPrio = 1;
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
        public string BynaryEncode(List<byte> cadena, ColaED1<NodoHuff<byte>> cola)
        {
            CrearArbol(cola);
            string resultado = "";
            foreach (var item in cadena)
            {
                resultado += codigosBytePrefijo[item];
            }

            int byteFaltante = resultado.Length % 8;
            if (!(byteFaltante == 0))
            {
                int caracteres = 8 - byteFaltante;
                for (int i = caracteres; i > 0; i--) resultado += "0";
            }

            return resultado;
        }

        public string devolverASCII(string codigoBinario)
        {
            System.Text.Encoding encoder = System.Text.ASCIIEncoding.ASCII;
            List<string> codigosOcho = new List<string>();
            codigosOcho = codigosSplit(8, codigoBinario);
            byte[] paraASCII = new byte[8];

            foreach (var item in codigosOcho)
            {
                paraASCII = Encoding.ASCII.GetBytes(item);
                textoComprimido += encoder.GetString(paraASCII);
            }
            return textoComprimido;
        }
        private List<string> codigosSplit(int splitSize, string codigoBinario)
        {
            int stringLength = codigoBinario.Length;
            List<string> codigos = new List<string>();
            for (int i = 0; i < stringLength; i += splitSize)
            {
                if ((i + splitSize) > stringLength)
                {
                    splitSize = stringLength - 1;
                    codigos.Add(codigoBinario.Substring(i, splitSize));
                }
            }
            return codigos;
        }


        public string StringCompressedToBinaryString(string cadenaCaracteres)
        {
            string binaryString = "";

            for (int i = 0; i < cadenaCaracteres.Length; i++)
            {
                char caracter = Convert.ToChar(cadenaCaracteres[i]);
                long dato = Convert.ToInt32(caracter);
                string cadenaBinaria = Convert.ToString(dato, 2);

                int cerosFaltantes;
                if (!((cerosFaltantes = cadenaBinaria.Length % 8) == 0))
                {
                    string nuevaCadena = "";
                    string ceros = "";

                    for (int j = 0; j < cerosFaltantes; j++) ceros += "0";
                    nuevaCadena = ceros + cadenaBinaria;
                    cadenaBinaria = nuevaCadena;


                }
                binaryString += cadenaBinaria;

            }

            return binaryString;
        }

        public List<byte> StringBinarioAMensaje(string binaryString)
        {
            List<byte> mensaje = new List<byte>();
            //El arbol debe estar creado en este momento para que el diccionario de prefijos exista. 
            string prefijo = "";
            for (int i = 0; i < binaryString.Length; i++)
            {
                prefijo += binaryString[i];
                if (cogdigosPrefijoByte.ContainsKey(prefijo))
                {
                    mensaje.Add(cogdigosPrefijoByte[prefijo]);
                    prefijo = "";
                }

            }
            return mensaje;
        }

    }

}