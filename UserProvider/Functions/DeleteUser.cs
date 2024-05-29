using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions
{
    public class DeleteUser(ILogger<DeleteUser> logger, DataContext dataContext)
    {
        private readonly ILogger<DeleteUser> _logger = logger;
        private readonly DataContext _dataContext = dataContext;
        [Function("DeleteUser")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route ="deleteUser/{id}" )] HttpRequestData req, string id)
        {
            try
            {
                var user = await _dataContext.Users.FindAsync(id);
                if (user == null) 
                {
                    _logger.LogInformation("User with Id {Id} not found", id);
                    return new NotFoundObjectResult("user not found");
                }
                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation("User with ID {Id} deleted successfully", id);
                return new OkObjectResult("User deleted successfully");
            }
            catch(System.Exception ex) {
                _logger.LogError(ex, "an error has occured while trying to delete the user");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            
            }
        }
    }
}
