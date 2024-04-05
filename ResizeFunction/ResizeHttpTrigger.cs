using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
// using Newtonsoft.Json;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Processing;

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

            string name = req.Query['name'];
            // int w = req.Query["w"]; //récupérer le paramètre w
            // int h = req.Query["h"]; //récupérer le paramètre h

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Request body is :{requestBody}");
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            // byte[]  targetImageBytes;
            // using(var  msInput = new MemoryStream())
            // {
            //     // Récupère le corps du message en mémoire
            //     await req.Body.CopyToAsync(msInput);
            //     msInput.Position = 0;

            //     // Charge l'image       
            //     using (var image = Image.Load(msInput)) 
            //     {
            //         // Effectue la transformation
            //         image.Mutate(x => x.Resize(w, h));

            //         // Sauvegarde en mémoire               
            //         using (var msOutput = new MemoryStream())
            //         {
            //             image.SaveAsJpeg(msOutput);
            //             targetImageBytes = msOutput.ToArray();
            //         }
            //     }
            // }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
