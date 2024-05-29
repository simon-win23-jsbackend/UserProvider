using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions
{
    public class GetUser(ILogger<GetUser> logger, DataContext dataContext)
    {
        private readonly ILogger<GetUser> _logger = logger;
        private readonly DataContext _dataContext = dataContext;

        [Function("GetUser")]
        public async Task <IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get" , Route  = "getUser/{id}")] HttpRequestData req, string id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user == null) {
                return new NotFoundObjectResult("user not found");
            }
            return new OkObjectResult(user);
        }
    }
}
