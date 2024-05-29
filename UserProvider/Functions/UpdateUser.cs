using Grpc.Core;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UserProvider.Functions
{
    public class UpdateUser(ILogger<UpdateUser> logger, DataContext dataContext)
    {
        private readonly ILogger<UpdateUser> _logger = logger;
        private readonly DataContext _dataContext = dataContext;

        [Function("UpdateUser")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function,"put" , Route = "updateUser/{id}")] HttpRequestData req, string id)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var updateUser = JsonConvert.DeserializeObject<ApplicationUser>(requestBody);

                if (updateUser == null || id != updateUser.Id)
                {
                    return new BadRequestObjectResult("Data provided is invalid");

                }
                
                var user = await _dataContext.Users.FindAsync(id);
                if (user == null) 
                {
                    return new NotFoundObjectResult("user not found");
                }
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                    
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();
                return new OkObjectResult("User was updated");


            }
            catch (Exception ex) {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

 
    }
}
