using System;
using DBManager.Models;
using DBManager.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DBManager.Controllers
{
    [Route("db-manager")]
    [ApiController]
    public class DBManagerController : ControllerBase
    {

        private readonly UserService _userService;

        public DBManagerController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("db")]
        public void Test()
        {
            var client = new MongoClient("mongodb+srv://dbAnthoAdmin:xgf1jm3gYRTpqlcP@mixup.wrwba.mongodb.net/MixUp?retryWrites=true&w=majority");
            var dbL = client.GetDatabase("sample_airbnb");
            var ll = dbL.ListCollections().ToList();
            foreach (var doc in ll)
            {
                Console.WriteLine(doc.ToString());
            }
        }

        [HttpPost]
        [Route("Token/Update")]
        public void UpdateToken([FromBody] Token newToken)
        {
            UserService service = new UserService();
            service.Update("xd", newToken);
        }

        [HttpPost]
        [Route("Token/Add")]
        public void AddToken([FromBody] Token newToken)
        {
            Console.WriteLine(newToken.RefreshToken);
            Console.WriteLine(newToken.ExpiresIn);
            _userService.Create(newToken);
        }
    }
}
