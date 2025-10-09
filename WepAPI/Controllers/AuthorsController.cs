using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WepAPI.Data;
using WepAPI.Models.DTO;
using WepAPI.Repositories;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }
        [HttpGet("get-all-author")]
        public IActionResult GetAll()
        {

            var allAuthor = _authorRepository.GetAllAuthors();
            return Ok(allAuthor);

        }
        [HttpGet]
        [Route("get-author-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var authorWithIdDTO = _authorRepository.GetAuthorById(id); return Ok(authorWithIdDTO);

        }
        [HttpPost("add-author")]
        public IActionResult AddBook([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO); return Ok(authorAdd);

        }
        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorNoIdDTO)
        {
            var updateAuthor = _authorRepository.UpdateAuthorById(id, authorNoIdDTO); return Ok(updateAuthor);

        }
        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var deleteAuthor = _authorRepository.DeleteAuthorById(id); return Ok(deleteAuthor);
        }
    }
}
