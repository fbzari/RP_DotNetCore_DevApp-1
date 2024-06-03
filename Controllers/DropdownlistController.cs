using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RP_DotNetCore_DevApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownlistController : ControllerBase
    {
        List<string> game = new List<string>();
        [HttpGet]
        public List<string> Get()
        {
            game.Add("Badminton");
            game.Add("Basketball");
            game.Add("Cricket");
            game.Add("Golf");
            game.Add("Gymnastics");
            game.Add("Tennis");
            game.Add("Hockey");
            return game;
        }
    }
}
