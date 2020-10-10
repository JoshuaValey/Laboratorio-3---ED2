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
                FileStream fileStream = new FileStream($"./{name}.huff", FileMode.Create, FileAccess.ReadWrite);
                await file.CopyToAsync(fileStream);
                Huffman<string> compress = new Huffman<string>();
                string textoCom = compress.Comprimir(fileStream);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
            
         }

        [HttpPost("decompress")]
        public async Task<File> Decompress([FromFile] IFormFile file)
         {
            FileStream fileStream = new FileStream($"./newText.txt", FileMode.Create, FileAccess.ReadWrite);
            await file.CopyToAsync(fileStream);

            Huffman<string> decompress = new Huffman<string>();
            return decompress.DesComprimir(fileStream);
            
            //return StatusCode(200);
         }
        

        

    }
}

namespace Laboratorio3ED2
{
    class FromFileAttribute : Attribute
    {
    }
}