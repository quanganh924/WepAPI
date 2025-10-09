using WepAPI.Data;
using WepAPI.Models.Domain;
using WepAPI.Models.DTO;

namespace WepAPI.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLPublisherRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PublisherDTO> GetAllPublishers()
        {
            return _dbContext.Publishers.Select(p => new PublisherDTO()
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            return _dbContext.Publishers
                .Where(p => p.Id == id)
                .Select(p => new PublisherNoIdDTO()
                {
                    Name = p.Name
                })
                .FirstOrDefault();
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO dto)
        {
            var publisher = new Publishers
            {
                Name = dto.Name
            };

            _dbContext.Publishers.Add(publisher);
            _dbContext.SaveChanges();

            return dto;
        }

        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO dto)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisher != null)
            {
                publisher.Name = dto.Name;
                _dbContext.SaveChanges();
            }
            return dto;
        }

        public Publishers? DeletePublisherById(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisher != null)
            {
                _dbContext.Publishers.Remove(publisher);
                _dbContext.SaveChanges();
            }
            return publisher;
        }
    }
}
