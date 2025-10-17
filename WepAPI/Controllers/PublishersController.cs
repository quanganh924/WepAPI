using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WepAPI.Models.DTO;
using WepAPI.Repositories;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PublishersController : ControllerBase
    {
       
            private readonly IPublisherRepository _publisherRepository;

            public PublishersController(IPublisherRepository publisherRepository)
            {
                _publisherRepository = publisherRepository;
            }
            
            [HttpGet("get-all")]
            [AllowAnonymous]
            public IActionResult GetAll()
            {
                return Ok(_publisherRepository.GetAllPublishers());
            }

            [HttpGet("get-by-id/{id}")]
            public IActionResult GetById(int id)
            {
                return Ok(_publisherRepository.GetPublisherById(id));
            }

            [HttpPost("add")]
            public IActionResult Add([FromBody] AddPublisherRequestDTO dto)
            {
                return Ok(_publisherRepository.AddPublisher(dto));
            }

            [HttpPut("update/{id}")]
            public IActionResult Update(int id, [FromBody] PublisherNoIdDTO dto)
            {
                return Ok(_publisherRepository.UpdatePublisherById(id, dto));
            }

            [HttpDelete("delete/{id}")]
            public IActionResult Delete(int id)
            {
                return Ok(_publisherRepository.DeletePublisherById(id));
            }
    }
}
