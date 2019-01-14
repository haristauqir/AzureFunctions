using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;     
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;


namespace AzureFunctions
{
    public static class RegisterUser
    {
        [FunctionName("RegisterUser")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, 
            TraceWriter log, [Table("tblUserProfile")] CloudTable objUserProfileTable)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string firstname = req.Query["firstname"];
            string lastname = req.Query["lastname"];

            UserProfile objUserProfile = new UserProfile(firstname,
                     lastname);
            TableOperation objTblOperationInsert =
             TableOperation.Insert(objUserProfile);
            objUserProfileTable.ExecuteAsync(objTblOperationInsert);

            return (ActionResult)new OkObjectResult("Thank you for Registering..."); 
        }
    }

    public class UserProfile : TableEntity
    {
        public UserProfile(string firstName, string lastName)
        {
            this.PartitionKey = "p1";
            this.RowKey = Guid.NewGuid().ToString(); ;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        public UserProfile() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
