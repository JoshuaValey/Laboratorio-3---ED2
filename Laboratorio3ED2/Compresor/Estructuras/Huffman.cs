using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Compresor.ColaLabED1;
using Compresor.Estructuras;
using Compresor.Huffman;
using Compresor.Interfaces;
using Microsoft.VisualBasic.CompilerServices;

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
        StreamWriter documento = new StreamWriter(@"C:\Users\marce\Desktop\2020\Semestre II 2020\Estructura de datos II\Laboratorio\Laboratorio-3---ED2\Laboratorio3ED2\PruebaCompresor\datosCompresion.txt");
        string lineaArchivo;
        List<byte> cadenaBytes = new List<byte>();
        string cadenaB;
        int contador;

        public string Comprimir(FileStream archivo)
        {
            colaPrioridad = cola.insert(archivo);
            string codigoBinario = BynaryEncode(cola.arregloBytes, colaPrioridad);
            devolverASCII(codigoBinario);
            lineaArchivo = escribirArchivo(datosParaArchivo());
            documento.WriteLine(lineaArchivo);
            documento.Close();
            codigosBytePrefijo = new Dictionary<byte, string>();
            cogdigosPrefijoByte = new Dictionary<string, byte>();
            return textoComprimido;
        }

        public string Descomprimir(string lineaArch)
        {
            List<byte> listaADescomprimir = new List<byte>();
            List<string> listaBin = new List<string>();
            string docDescomprimido = "";
            CrearArbol(leerArchivo(lineaArch));
            leerArchivo(lineaArch);
            string cadenaAscii = "";

            foreach(var item in cadenaBytes)
            {
                cadenaAscii += Convert.ToChar(item);
            }

            string bynariString = StringCompressedToBinaryString(cadenaAscii);
            listaADescomprimir = StringBinarioAMensaje(bynariString);

            foreach(var item in listaADescomprimir)
            {
                docDescomprimido += Convert.ToChar(item);
            }

            return docDescomprimido;

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
            //byte[] paraASCII = new byte[8];
            Queue<byte> paraASCII = new Queue<byte>();
            
            foreach (var item in codigosOcho)
            {
                //paraASCII = Encoding.ASCII.GetBytes(item);
                paraASCII.Enqueue(Convert.ToByte(CadenaBinAInt(item)));
                //textoComprimido += encoder.GetString(paraASCII);
                //textoComprimido += Encoding.Convert(Encoding.Unicode, Encoding.ASCII, paraASCII);
            }
            foreach(var item in paraASCII)
            {
                textoComprimido += Convert.ToChar(item);
            }
            //textoComprimido += Encoding.Convert(Encoding.Unicode, Encoding.ASCII, paraASCII.ToArray());
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
            cantidadValores = 2;
            System.Text.Encoding encoder = System.Text.ASCIIEncoding.ASCII;
            int valor1 = 0;
            int valor2 = 0;
            string valorCero;
            string valorBinario;
            Queue<byte> paraArchivo = new Queue<byte>();
            Queue<byte> paraArchivo2 = new Queue<byte>();

            //metodo para crear lista con datos para archivo
            for (int i = 0; i < cola.cantidadBytes; i++)
            {
                datosArchivo datos = new datosArchivo();
                if (cola.colaPrioridad[i].prioridad <= 256) //caso de que ese en especifico no pase las 256
                {
                    valorBinario = Convert.ToString(cola.colaPrioridad[i].prioridad, 2);
                    valorCero = Convert.ToString(0, 2);
                    paraArchivo.Enqueue(Convert.ToByte(CadenaBinAInt(RellenarCeros2Bytes(valorBinario))));
                    paraArchivo2.Enqueue(Convert.ToByte(CadenaBinAInt(RellenarCeros2Bytes(valorCero))));
                    datos.caracter = Convert.ToChar(cola.colaPrioridad[i].valor);
                    datos.valorASCII = Convert.ToChar(paraArchivo2.Dequeue());
                    datos.valorASCII += Convert.ToChar(paraArchivo.Dequeue());
                    listaDatos.Add(datos);
                }
                else //caso de que si pasa las 256
                {
                    if ((cola.colaPrioridad[i].prioridad % 2) == 0) //frecuencia es par
                    {
                        valor2 = cola.colaPrioridad[i].prioridad / 2;
                        valorBinario = Convert.ToString(valor2, 2);
                        paraArchivo.Enqueue(Convert.ToByte(CadenaBinAInt(RellenarCeros2Bytes(valorBinario))));
                        paraArchivo.Enqueue(Convert.ToByte(CadenaBinAInt(RellenarCeros2Bytes(valorBinario))));
                        datos.caracter = Convert.ToChar(cola.colaPrioridad[i].valor);
                        datos.valorASCII = Convert.ToChar(paraArchivo.Dequeue());
                        datos.valorASCII += Convert.ToChar(paraArchivo.Dequeue());
                        listaDatos.Add(datos);
                    }
                    else //frecuencia es impar
                    {
                        valor1 = (cola.colaPrioridad[i].prioridad - 1) / 2;
                        valor2 = valor1 + 1;
                        valorBinario = Convert.ToString(valor1, 2);
                        valorCero = Convert.ToString(valor2, 2);
                        paraArchivo.Enqueue(Convert.ToByte(CadenaBinAInt(RellenarCeros2Bytes(valorBinario))));
                        paraArchivo2.Enqueue(Convert.ToByte(CadenaBinAInt(valorCero)));
                        datos.caracter = Convert.ToChar(cola.colaPrioridad[i].valor);
                        datos.valorASCII = Convert.ToChar(paraArchivo.Dequeue());
                        datos.valorASCII += Convert.ToChar(paraArchivo2.Dequeue());
                        listaDatos.Add(datos);
                    }
                }
            }

            //retornar listado con string caracter y su frecuencia en ascii
            return listaDatos;
        }

        private string escribirArchivo(List<datosArchivo> datos)
        {
            string linea = "";
            linea += Convert.ToChar(cola.cantidadBytes);
            linea += Convert.ToChar(cantidadValores);
            foreach (var item in datos)
            {
                linea += item.caracter;
                linea += item.valorASCII;
            }
            linea += textoComprimido;
            return linea;
        }

        /// <summary>
        /// Método que recibe la cadena de caracteres a descomprimir
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
        /// Este metodo rellena con ceros a la izquierda una cadena de ceros y unos
        /// para convertirla posteriormente a decimal y a ascci luego 
        /// de separarla en dos bytes 
        /// </summary>
        /// <param name="cadenaBinaria">Arreglo de ceros y unos</param> 
        /// <returns>Una cadena de ceros y unos de 16 pos</returns>
        private string RellenarCeros2Bytes(string cadenaBinaria){
            string bynariString = "";

            if (cadenaBinaria != "0")
            {
                int cerosFaltantes;
                if (!((cerosFaltantes = 8-cadenaBinaria.Length) == 0))
                {
                    string nuevaCadena = "";
                    string ceros = "";

                    for (int i = 0; i < cerosFaltantes; i++) ceros += "0";
                    nuevaCadena = ceros + cadenaBinaria;
                    cadenaBinaria = nuevaCadena;
                }
                bynariString += cadenaBinaria;
            }
            else
            {
                bynariString = "00000000";
            }
            

            return bynariString;
        }

        /// <summary>
        /// Separar un arreglo de ceros y uno de 16 pos
        /// en dos de 8 para manejar dos bytes
        /// </summary>
        /// <param name="_16Bits">Arreglo de ceros y unos</param> 
        /// <returns>Un arreglo de dos posiciones de cadenas de ceros y unos de de 8 pos</returns>
        private string[] SepararEn2Bytes(string _16Bits)
        {
           string[] resultado = {_16Bits.Substring(0, 8),
                _16Bits.Substring(8,8)};

                return resultado;
        }

        /// <summary>
        /// Método que recibe la cadena de ceros y unos
        /// y a través del diccionario con los códigos prefigo
        /// vuelve a armar el mensaje original en bytes
        /// </summary>
        /// <param name="binaryString">Cadena de ceros y unos</param> 
        /// <returns>Listado de bytes del el mensaje original</returns>
        public List<byte> StringBinarioAMensaje(string binaryString)
        {
            //TODO: Recordar evaluar los ultimos bits que pueden escribirse para rellenar....
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

        /// <summary>
        /// Este método resive una cadena de ceros y unos, ejemplo:
        /// "10110011" y la convierte en un número entero para posteriormente
        /// convertir ese entero en un caracter ASCII.
        /// </summary>
        /// <param name="cadenaBinaria">Cadena de ceros y unos</param> 
        /// <returns>Entero decimal que será convertido a ASCII</returns>
        private int CadenaBinAInt(string cadenaBinaria)
        {
            int resultado = 0;

            int[] baseDecimal = {128,64,32,16,8,4,2,1};
            
            for (int i = 0; i < 8; i++)
                if (cadenaBinaria[i] == '1') resultado += baseDecimal[i];
            
            return resultado;
        }

        public ColaED1<NodoHuff<byte>> leerArchivo(string fileArchivo)
        {
            System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
            byte[] bytesLinea = codificador.GetBytes(fileArchivo);

            int noValores = bytesLinea[0];
            int noBytes = bytesLinea[1];

            for (int i = 2; i <= (noValores * noBytes) + 1; i++)
            {
                contador = contador + bytesLinea[i + 1];
                i++;
            }

            for (int i = 2; i <= (noValores * noBytes)+1; i++)
            {
                NodoHuff<byte> nodoH = new NodoHuff<byte>();
                nodoH.Value = bytesLinea[i];
                nodoH.ProbPrio = bytesLinea[i+1];
                nodoH.Frecuencia = Decimal.Divide(nodoH.ProbPrio, contador);
                colaPrioridad.Insert(nodoH.Frecuencia, nodoH);
                i++;
            }

            for(int i = (noValores * noBytes) + 2; i < bytesLinea.Length; i++)
            {
                cadenaBytes.Add(bytesLinea[i]);
                cadenaB += Convert.ToChar(bytesLinea[i]);
            }

            colaPrioridad.ordenar();
            return colaPrioridad;
        }












    }

}