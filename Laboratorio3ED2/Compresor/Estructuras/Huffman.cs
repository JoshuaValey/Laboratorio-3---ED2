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
        public string textoComprimido = "";
        ColaED1<NodoHuff<byte>> colaPrioridad = new ColaED1<NodoHuff<byte>>();
        static ColaPrioridad<byte> cola = new ColaPrioridad<byte>();
        List<datosArchivo> listaDatos = new List<datosArchivo>();
        int cantidadCaracteres = cola.cantidadBytes;
        int cantidadValores = 0;
        StreamWriter documento = new StreamWriter(@".\datosCompresion.txt");
        string lineaArchivo;

        public string Comprimir(FileStream archivo)
        {
            colaPrioridad = cola.insert(archivo);
            string codigoBinario = BynaryEncode(cola.arregloBytes, colaPrioridad);
            devolverASCII(codigoBinario);
            lineaArchivo = escribirArchivo(datosParaArchivo());
            documento.Write(lineaArchivo);
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
                    Raiz.Frecuencia = 1;
                    GenerarPrefijos();
                }
                else
                {
                    auxPadre.Frecuencia = auxDer.Frecuencia + auxIzqu.Frecuencia;
                    auxPadre.Derecho = auxDer;
                    auxPadre.Inzquierdo = auxIzqu;

                    cola.Insert(auxPadre.Frecuencia, auxPadre);

                    CrearArbol(cola);
                }
            }
            catch (System.NullReferenceException ex)
            {
                Raiz = auxDer;
                Raiz.Frecuencia = 1;
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
                //textoComprimido += encoder.GetString(paraASCII);
                textoComprimido += Encoding.Convert(Encoding.Unicode, Encoding.ASCII, paraASCII);
            }
            return textoComprimido;
        }
        private List<string> codigosSplit(int splitSize, string codigoBinario)
        {
            int stringLength = codigoBinario.Length;
            List<string> codigos = new List<string>();
            for (int i = 0; i < stringLength; i += splitSize)
            {
                if ((i + splitSize) < stringLength)
                {
                    codigos.Add(codigoBinario.Substring(i, 8));
                }
            }
            return codigos;
        }
        private List<datosArchivo> datosParaArchivo()
        {
            cantidadValores = 1;
            System.Text.Encoding encoder = System.Text.ASCIIEncoding.ASCII;
            int valor1 = 0;
            int valor2 = 0;
            string valorCero;
            string valorBinario;
            byte[] paraArchivo = new byte[8];
            byte[] paraArchivo2 = new byte[8];
            datosArchivo datos = new datosArchivo();

            //verifica si hay algun caracter que se repita mas se 256 veces, si lo hace, se agrega un numero a la cantidad de valores despues de la letra
            for (int i = 0; i < cantidadCaracteres; i++)
            {
                if (cola.colaPrioridad[i].prioridad > 256)
                {
                    cantidadValores = 2;
                }
            }

            //metodo para crear lista con datos para archivo
            for (int i = 0; i < cantidadCaracteres; i++)
            {
                //caso de que ningun valor pase las 256 repeticiones
                if (cantidadValores == 1)
                {
                    valorBinario = Convert.ToString(cola.colaPrioridad[i].prioridad, 2);
                    paraArchivo = Encoding.ASCII.GetBytes(valorBinario);
                    datos.caracter = cola.colaPrioridad[i].valor.ToString();
                    datos.valorASCII = encoder.GetString(paraArchivo);
                    listaDatos.Add(datos);
                }
                else //caso de que si pasen las 256 repeticiones 
                {
                    if (cola.colaPrioridad[i].prioridad <= 256) //caso de que ese en especifico no pase las 256
                    {
                        valorBinario = Convert.ToString(cola.colaPrioridad[i].prioridad, 2);
                        valorCero = Convert.ToString(0, 2);
                        paraArchivo = Encoding.ASCII.GetBytes(valorBinario);
                        paraArchivo2 = Encoding.ASCII.GetBytes(valorCero);
                        datos.caracter = cola.colaPrioridad[i].valor.ToString();
                        datos.valorASCII = encoder.GetString(paraArchivo2) + encoder.GetString(paraArchivo);
                        listaDatos.Add(datos);
                    }
                    else //caso de que si pasa las 256
                    {
                        if ((cola.colaPrioridad[i].prioridad % 2) == 0) //frecuencia es par
                        {
                            valor2 = cola.colaPrioridad[i].prioridad / 2;
                            valorBinario = Convert.ToString(valor2, 2);
                            paraArchivo = Encoding.ASCII.GetBytes(valorBinario);
                            datos.caracter = cola.colaPrioridad[i].valor.ToString();
                            datos.valorASCII = encoder.GetString(paraArchivo) + encoder.GetString(paraArchivo);
                            listaDatos.Add(datos);
                        }
                        else //frecuencia es impar
                        {
                            valor1 = (cola.colaPrioridad[i].prioridad - 1) / 2;
                            valor2 = valor1 + 1;
                            valorBinario = Convert.ToString(valor1, 2);
                            valorCero = Convert.ToString(valor2, 2);
                            paraArchivo = Encoding.ASCII.GetBytes(valorBinario);
                            paraArchivo2 = Encoding.ASCII.GetBytes(valorCero);
                            datos.caracter = cola.colaPrioridad[i].valor.ToString();
                            datos.valorASCII = encoder.GetString(paraArchivo) + encoder.GetString(paraArchivo2);
                            listaDatos.Add(datos);
                        }
                    }
                }
            }

            //retornar listado con string caracter y su frecuencia en ascii
            return listaDatos;
        }


        private string escribirArchivo(List<datosArchivo> datos)
        {
            string linea;
            linea = cantidadCaracteres + cantidadValores.ToString();
            foreach (var item in datos)
            {
                linea += item.caracter + item.valorASCII;
            }
            linea += textoComprimido;
            return linea;
        }


        /// <summary>
        /// Método que resive la cadena de caracteres a descomprimir
        /// y la convierte a una cadena de ceros y unos.
        /// 
        /// </summary>
        /// <param name="cadenaCaracteres">Arreglo de caracteres a descomprimir</param> 
        /// <returns>Una cadena de ceros y unos</returns>
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

        /// <summary>
        /// Método que resive la cadena de ceros y unos
        /// y a través del diccionario con los códigos prefigo
        /// vuelve a armar el mensaje original en bytes
        /// </summary>
        /// <param name="binaryString">Cadena de ceros y unos</param> 
        /// <returns>Listado de bytes del el mensaje original</returns>
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