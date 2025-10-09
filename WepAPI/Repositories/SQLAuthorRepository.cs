using Microsoft.EntityFrameworkCore;
using WepAPI.Data;
using WepAPI.Models.Domain;
using WepAPI.Models.DTO;

namespace WepAPI.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AuthorDTO> GetAllAuthors()
        {
            var allAuthor = _dbContext.Authors.Select(Authors => new AuthorDTO()
            {
                Id = Authors.Id,
                FullName = Authors.FullName,
                
            }).ToList();return allAuthor;
        }
        public AuthorNoIdDTO GetAuthorById(int id)
        {
            var authorWithDomain = _dbContext.Authors.Where(n => n.Id == id);
            var authorWithDTO = authorWithDomain.Select(author => new AuthorNoIdDTO()
            {
                
                FullName = author.FullName,
            }).FirstOrDefault();return authorWithDTO;
        }
        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorDomainModle = new Authors
            {
                FullName = addAuthorRequestDTO.FullName,
            };
            _dbContext.Authors.Add(authorDomainModle);
            _dbContext.SaveChanges();

            return addAuthorRequestDTO;
        }
        public AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var authorDomain =_dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authorDomain != null)
            {
                authorDomain.FullName = authorNoIdDTO.FullName;
                _dbContext.SaveChanges();
            }
            return authorNoIdDTO;
        }
        public Authors? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(n =>n.Id == id);
            if(authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return authorDomain;
        }
    }
}
