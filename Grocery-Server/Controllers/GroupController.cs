using Grocery_Server.ControllerModels;
using Grocery_Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Grocery_Server.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/group")]
    public class GroupController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public GroupController(DbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets your own group's information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            User? user = await GetCurrentUser();
            if (user == null)
                return Unauthorized();
            if (user.GroupId == null)
                return Ok();
            Group? group = _dbContext.Groups.FirstOrDefault(house => house.Id == user.GroupId);
            if (group == null)
                return NotFound();
            return Ok(new GroupDisplayDTO(group));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] NewGroupDTO creationModel)
        {
            User? user = await GetCurrentUser();
            if (user == null)
                return Unauthorized();

            Group group = new(user, creationModel.Name);

            if (_dbContext.Groups.Any(house => house.Owner == user))
                return Conflict("You are already the owner of a group"); // can't create new group if already owner of a group

            _dbContext.Groups.Add(group);
            user.GroupId = group.Id;
            _dbContext.SaveChanges();

            return Ok(group.GetString());
        }

        [HttpDelete]
        public IActionResult DeleteGroup([FromQuery] Guid id)
        {
            Group? group = _dbContext.Groups.FirstOrDefault(group => group.Id == id);
            if (group == null)
                return NotFound();
            _dbContext.Groups.Remove(group);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("send-invite")]
        public async Task<IActionResult> SendInvite([FromBody] NewInviteDTO invite)
        {
            User? user = await GetCurrentUser();
            if (user == null)
                return Unauthorized();
            if (user.GroupId != invite.GroupId)
                return BadRequest();

            User? addressee = _dbContext.Users.FirstOrDefault(user => user.Id == invite.UserId);
            if (addressee.GroupId == invite.GroupId)
                return BadRequest("Already a member");

            GroupInvite? existingInvite = _dbContext.GroupInvites.FirstOrDefault(existingInvite => existingInvite.GroupId == invite.GroupId && existingInvite.UserId == invite.UserId);
            if (existingInvite != null)
            {
                if (!existingInvite.IsExpired()) // if a valid invite still exists, return conflict
                    return Conflict("You have already sent an invite to this person");
                else // if existing invite is expired, remove it and continue
                    _dbContext.Remove(existingInvite);
            }

            _dbContext.GroupInvites.Add(invite.GetInvite());
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPatch("retract-invite")]
        public async Task<IActionResult> RetractInvite([FromBody] NewInviteDTO invite)
        {
            User? user = await GetCurrentUser();
            if (user == null || user.GroupId != invite.GroupId)
                // will swap to NotFound once done
                return Unauthorized();

            GroupInvite? existingInvite = _dbContext.GroupInvites.FirstOrDefault(oldInvite => oldInvite.UserId == invite.UserId && oldInvite.GroupId == invite.GroupId);
            if (existingInvite == null)
                return NotFound();

            // expired or not, remove it from db
            _dbContext.Remove(existingInvite);
            _dbContext.SaveChanges();

            if (existingInvite.IsExpired())
                return NotFound();
            return Ok();
        }

        [HttpGet("my-invites")]
        public async Task<IActionResult> GetMyInvites()
        {
            User? user = await GetCurrentUser();
            if (user == null)
                return Unauthorized();

            List<NewInviteDTO> invites = user.Invites.Select(invite => new NewInviteDTO(invite)).ToList();
            return Ok(invites);
        }

        [HttpGet("sent-invites")]
        public async Task<IActionResult> GetSentInvites()
        {
            User? user = await GetCurrentUser();
            if (user == null)
                return Unauthorized();
            if (user.Group == null)
                return NotFound();

            List<NewInviteDTO> invites = user.Group.Invites.Select(invite => new NewInviteDTO(invite)).ToList();
            return Ok(invites);
        }


        [HttpPost("accept-invite")]
        public async Task<IActionResult> AcceptInvite([FromBody] NewInviteDTO invite)
        {
            User? user = await GetCurrentUser();
            if (user == null || user.Id != invite.UserId)
                // will swap to NotFound once done
                return Unauthorized();

            if (user.Group != null && user.Group.Owner == user)
                return BadRequest("Cannot join a group while owner of another");

            GroupInvite? foundInvite = _dbContext.GroupInvites.FirstOrDefault(foundInvite => foundInvite.UserId == invite.UserId && foundInvite.GroupId == invite.GroupId);
            if (foundInvite == null)
                return NotFound();
            else if (foundInvite.IsExpired())
            {
                _dbContext.Remove(foundInvite);
                _dbContext.SaveChanges();
                return NotFound();
            }
            Group group = _dbContext.Groups.First(group => group.Id == invite.GroupId);
            group.Members.Add(user);
            user.Group = group;
            _dbContext.GroupInvites.Remove(foundInvite);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("deny-invite")]
        public async Task<IActionResult> DenyInvite([FromBody] NewInviteDTO invite)
        {
            User? user = await GetCurrentUser();
            if (user == null || user.Id != invite.UserId)
                // will swap to NotFound once done
                return Unauthorized();

            GroupInvite? foundInvite = _dbContext.GroupInvites.FirstOrDefault(foundInvite => foundInvite.UserId == invite.UserId && foundInvite.GroupId == invite.GroupId);
            if (foundInvite == null)
                return NotFound();

            _dbContext.Remove(foundInvite);
            _dbContext.SaveChanges();

            if (foundInvite.IsExpired())
                return NotFound();
            return Ok();
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveGroup()
        {
            User? user = await GetCurrentUser();
            if (user == null)
                // will swap to NotFound once done
                return Unauthorized();

            if (user.Group == null)
                return BadRequest("You are not in a group right now");

            user.Group.Members.Remove(user);
            _dbContext.SaveChanges();
            return Ok();
        }


        private async Task<User?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
