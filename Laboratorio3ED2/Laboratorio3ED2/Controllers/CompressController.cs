using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laboratorio3ED2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Laboratorio3ED2.Controllers
{

    [ApiController]
    [Route("api")]
    public class CompressController : ControllerBase
    {
        /* [HttpPost("compress/{name}")]
         public async Task<File> Compress([FromFile] IFormFile file, string name)
         {
            FileStream fileStream = new FileStream($"./{name}.huff", FileMode.Create, FileAccess.ReadWrite);
            await file.CopyToAsync(fileStream);

            //Huffman<T> compress = new Huffman<T>();
            //return compress.Comprimir(fileStream);
            
            // return StatusCode(200);
         }*/

       /*  [HttpPost("decompress")]

        public async Task<File> Decompress([FromFile] IFormFile file)
         {
            FileStream fileStream = new FileStream($"./newText.txt", FileMode.Create, FileAccess.ReadWrite);
            await file.CopyToAsync(fileStream);

            //Huffman<T> decompress = new Huffman<T>();
            //return decompress.DesComprimir(fileStream);
            
            // return StatusCode(200);
         }*/
        

        

    }
}