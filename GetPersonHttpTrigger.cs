using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using lab1;
using System.Net.Http;

namespace lab1
{
    public static class GetPersonHttpTrigger
    {
        [FunctionName("GetPersonHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("PeopleDb");
                log.LogInformation(connectionString);
                ObjectResult result;
                string responseMessage ="";
                var personId = req.Query["id"];

                if (string.IsNullOrEmpty(personId)){
                    responseMessage = "Provide an id";
                    result = new BadRequestObjectResult(responseMessage);
                }
                else{                   
                    var db = new DatabaseContext(connectionString);
                    var person = db.GetPerson(Int32.Parse(personId));         
                    return new JsonResult(person);
                }
                return result;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }
    }
}