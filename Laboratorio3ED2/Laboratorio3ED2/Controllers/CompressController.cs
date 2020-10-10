using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compresor.Huffman;
using Laboratorio3ED2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Laboratorio3ED2.Controllers
{

    [ApiController]
    [Route("api")]
    public class CompressController : ControllerBase
    {
         [HttpPost("compress/{name}")]
         public async Task<ActionResult> Compress([FromFile] IFormFile file, string name)
         {
            try
            {
                FileStream fileStream = new FileStream($"./{name}.huff", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                await file.CopyToAsync(fileStream);
                Huffman<string> compress = new Huffman<string>();
                string textoCom = compress.Comprimir(fileStream);
                FileStreamResult fileResultado = new FileStreamResult(fileStream, "text/txt");
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
            
         }

        [HttpPost("decompress")]
        public async Task<ActionResult> Decompress([FromFile] IFormFile file)
         {
             try
             {
<<<<<<< HEAD
                 FileStream fileStream = new FileStream($"./Descomprimido.txt", FileMode.Create, FileAccess.Read);
=======
                 FileStream fileStream = new FileStream($"./Descomprimido.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
>>>>>>> 7417fcb66faea2f65ecb48ff82822a429cd5c8ff
                 await file.CopyToAsync(fileStream);
                 fileStream.Close();
            
           
                string data = System.IO.File.ReadAllText($"./Descomprimido.txt");
    
                 Huffman<string> decompress = new Huffman<string>();
                 decompress.Descomprimir(data);
            
                 return StatusCode(200);
             }
             catch (System.Exception)
             {
                 return StatusCode(500);
                 throw;
             }
         }
        

        

    }
}

namespace Laboratorio3ED2
{
    class FromFileAttribute : Attribute
    {
    }
}