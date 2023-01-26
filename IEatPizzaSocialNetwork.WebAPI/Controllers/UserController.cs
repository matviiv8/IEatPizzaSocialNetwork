using IEatPizzaSocialNetwork.Domain.Core.Entities;
using IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IEatPizzaSocialNetwork.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            this._repository = repository;
        }

        /// <summary>  
        /// return the users list
        /// </summary>  
        /// <param></param> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            try
            {
                var users = _repository.GetUserList();

                if (users == null)
                {
                    return NotFound();
                }

                return Ok(users);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }

        /// <summary>  
        /// creates a new user
        /// </summary>  
        /// <param name="user">object user</param> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                _repository.Create(user);
                _repository.Save();

                return Created("Create", user);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }
    }
}
