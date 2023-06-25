using APi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace APi.Controllers
{
    [Authorize]
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("auth")]
        public  ActionResult<string> GetSecrt()
        {
            return "secret text"; 
        }

    
        [HttpGet("not-found")]
        public ActionResult<string> NotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();

            return Ok(thing);
        }

     
        [HttpGet("Server-error")]
        public ActionResult<string> GetServererror()
        {
            
                var thing = _context.Users.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;

            
        }
      
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest ()
        {
            return BadRequest("this was not a good Request");
        }
        

    }
}
