using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Company.Function
{
    public static class ResizeHttpTrigger
    {
        [FunctionName("ResizeHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int w = 0;
            int h = 0;
            
            try
            {
                w = Int32.Parse(req.Query["w"]); //récupérer le paramètre w
                h = Int32.Parse(req.Query["h"]); //récupérer le paramètre h
                // if (w < 0)
                // {
                //     throw FormatException("No negative number for width");
                // }
                // if (h < 0)
                // {
                //     throw FormatException("No negative number for heigth");
                // }
            }
            catch (FormatException e)
            {
                log.LogInformation($"Exception caught : {e}");
            }

            byte[]  targetImageBytes;
            using(var  msInput = new MemoryStream())
            {
                // Récupère le corps du message en mémoire
                await req.Body.CopyToAsync(msInput);
                msInput.Position = 0;

                // Charge l'image       
                using (var image = Image.Load(msInput)) 
                {
                    // Effectue la transformation
                    image.Mutate(x => x.Resize(w, h));

                    // Sauvegarde en mémoire               
                    using (var msOutput = new MemoryStream())
                    {
                        image.SaveAsJpeg(msOutput);
                        targetImageBytes = msOutput.ToArray();
                    }
                }
            }

            log.LogInformation("End of the Azure function !");

            return new FileContentResult(targetImageBytes, "image/jpeg");
        }
    }
}
