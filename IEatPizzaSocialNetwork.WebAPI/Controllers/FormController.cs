using IEatPizzaSocialNetwork.Domain.Core.Entities;
using IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IEatPizzaSocialNetwork.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : Controller
    {
        private readonly IFormRepository _repository;

        public FormController(IFormRepository repository)
        {
            this._repository = repository;
        }

        /// <summary>  
        /// return the forms list
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
                var forms = _repository.GetFormList();

                if (forms == null)
                {
                    return NotFound();
                }

                return Ok(forms);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }

        /// <summary>  
        /// return the form by user id
        /// </summary>  
        /// <param name="userId">user id</param> 
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(int userId)
        {
            try
            {
                var form = _repository.GetFormByUserId(userId);

                if (form == null)
                {
                    return NotFound();
                }

                return Ok(form);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }

        /// <summary>  
        /// creates a new form
        /// </summary>  
        /// <param name="form">object form</param> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create(Form form)
        {
            try
            {
                if (form == null)
                {
                    return BadRequest();
                }

                _repository.Create(form);
                _repository.Save();

                return Created("Create", form);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }

        /// <summary>  
        /// update form
        /// </summary>  
        /// <param name="form">object form</param> 
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(Form form)
        {
            try
            {
                if (form == null)
                {
                    return BadRequest();
                }

                var oldForm = _repository.GetForm(form.Id);

                if(oldForm == null)
                {
                    return NotFound();
                }

                oldForm.CountSentForm = form.CountSentForm;
                oldForm.LastDateAndTimeSentForm = form.LastDateAndTimeSentForm;

                _repository.Update(oldForm);

                _repository.Save();

                return Ok(form);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, error.Message);
            }
        }
    }
}
