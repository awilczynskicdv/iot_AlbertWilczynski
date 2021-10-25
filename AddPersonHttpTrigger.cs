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
    public static class AddPersonHttpTrigger
    {
        [FunctionName("AddPersonHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("PeopleDb");
                log.LogInformation(connectionString);
                ObjectResult result;
                string responseMessage ="";
                var person = await req.Content.ReadAsAsync<Person>();
                string fName = person.FirstName;
                string lName = person.LastName;
                string phonenumber = person.Phonenumber;

                if (string.IsNullOrEmpty(fName) || string.IsNullOrEmpty(lName) || string.IsNullOrEmpty(phonenumber)){
                    responseMessage = "Provide full info";
                    result = new BadRequestObjectResult(responseMessage);
                }
                else{                   
                    var db = new DatabaseContext(connectionString);
                    db.AddPerson(fName, lName, phonenumber);          
                    responseMessage = $"{fName} {lName} {phonenumber} created succesfully.";
                    result = new OkObjectResult(responseMessage);
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