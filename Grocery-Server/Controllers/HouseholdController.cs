using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Route("api/household")]
    public class HouseholdController : ControllerBase
    {
        private readonly DbContext _dbContext;
        public HouseholdController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] Guid id)
        {
            HouseHold? household = _dbContext.HouseHolds.FirstOrDefault(house => house.Id == id);
            return Ok(household);
        }

        //[HttpPost()]
        // todo: make household creation, leaving etc
        [HttpPost("create")]
        public IActionResult CreateHouseHold([FromBody] NewHouseHoldDTO creationModel)
        {
            HouseHold houseHold = new(creationModel);
            User? user = _dbContext.Users.FirstOrDefault(user => user.Id == creationModel.UserId);
            if (user == null)
                return BadRequest();
            if (_dbContext.HouseHolds.Any(house => house.Owner == user))
                return Conflict(); // can't create new group if already owner of a group
            _dbContext.HouseHolds.Add(houseHold);
            user.HouseHoldId = houseHold.Id;
            _dbContext.SaveChanges();
            return Ok(houseHold.GetString());
        }

        [HttpDelete]
        public IActionResult DeleteHousehold([FromQuery] Guid id)
        {
            HouseHold? houseHold = _dbContext.HouseHolds.FirstOrDefault(household => household.Id == id);
            if (houseHold == null)
                return NotFound();
            _dbContext.HouseHolds.Remove(houseHold);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
