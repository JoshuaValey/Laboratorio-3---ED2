# Laboratorio #3

* Marcela Estrada 1010419
* Joshua Váley 1014416

### Objetivos:
-  Aplicar los conceptos de la codificación de Huffman
-  Aplicar los conceptos de razones de compresión 


## API debe cumplir con los siguientes requerimientos:
####  Ruta: /api/compress/{name}
■ POST
-  Recibe un archivo de texto que se deberá comprimir
-  Retorna un archivo <name>.huff con el contenido del archivo
comprimido

####  Ruta: /api/decompress
■ POST
 - Recibe un archivo .huff que se deberá descomprimir
 - Retorna el archivo de texto con el nombre original
 - Devuelve OK si no hubo error
 - Devuelve InternalServerError si hubo


####  Ruta: /api/compressions
■ GET 
 - Devuelve un JSON con el listado de todas las compresiones
con los siguientes valores:
*  Nombre del archivo original
*  Nombre y ruta del archivo comprimido
*  Razón de compresión
*  Factor de compresión
*  Porcentaje de reducción
