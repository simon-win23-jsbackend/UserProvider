using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProvider.Functions
{
    public class GetUsers(ILogger<GetUsers> logger, DataContext dataContext)
    {
        private readonly ILogger<GetUsers> _logger = logger;
        private readonly DataContext _dataContext = dataContext;

        [Function("GetUsers")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var getUsers = await _dataContext.Users.ToListAsync();
            return new OkObjectResult(getUsers);
        }
    }
}
