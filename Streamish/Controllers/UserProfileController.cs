using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streamish.Models;
using Streamish.Repositories;

namespace Streamish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepo;
        public UserProfileController(IUserProfileRepository userProfileRepo)
        {
            _userProfileRepo = userProfileRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userProfileRepo.GetAllUserProfiles());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var profile = _userProfileRepo.GetUserProfileById(id);
            if(profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpPost]
        public IActionResult Post(UserProfile userProfile)
        {
            _userProfileRepo.Add(userProfile);
            return CreatedAtAction("Get", new { id = userProfile.Id }, userProfile);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile userProfile)
        {
            if(id != userProfile.Id)
            {
                return BadRequest();
            }

            _userProfileRepo.Update(userProfile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userProfileRepo.Delete(id);
            return NoContent();
        }

        [HttpGet("GetIdWithVideos{id}")]
        public IActionResult GetIdWithVideos(int id)
        {
            var videos = _userProfileRepo.GetUserProfileByIdWithVideos(id);
            if (videos == null)
            {
                return NotFound();
            }
            return Ok(videos);
        }
    }
}
