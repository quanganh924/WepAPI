﻿using WepAPI.Data;
using WepAPI.Models.DTO;
using WepAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

    
namespace WepAPI.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BookDTO> GetAllBooks(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var allBooks = _dbContext.Books.Select(Books => new BookDTO()
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.DateRead.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.Book_Author.Select(n => n.Authors.FullName).ToList()
            }).AsQueryable();
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
            }
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending ? allBooks.OrderBy(x => x.Title) : allBooks.OrderByDescending(x => x.Title);
                }
            }
            var skipResults = (pageNumber - 1) * pageSize;
            return allBooks.Skip(skipResults).Take(pageSize).ToList();
        }
        public BookDTO GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books.Where(n => n.Id == id);
          
            var bookWithIdDTO = bookWithDomain.Select(book => new BookDTO()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Author.Select(n => n.Authors.FullName).ToList()
            }).FirstOrDefault(); return bookWithIdDTO;

        }
        public addBookRequestDTO AddBook(addBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherId

            };
            _dbContext.Books.Add(bookDomainModel);
                _dbContext.SaveChanges();
            foreach(var id in addBookRequestDTO.AuthorId)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                _dbContext.Books_Author.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return addBookRequestDTO;

        }
        public addBookRequestDTO? UpdateBookById(int id, addBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = bookDTO.Title;
                bookDomain.Description = bookDTO.Description; 
                bookDomain.IsRead = bookDTO.IsRead; 
                bookDomain.DateRead = bookDTO.DateRead; 
                bookDomain.Rate = bookDTO.Rate; 
                bookDomain.Genre = bookDTO.Genre; 
                bookDomain.CoverUrl = bookDTO.CoverUrl; 
                bookDomain.DateAdded = bookDTO.DateAdded; 
                bookDomain.PublisherID = bookDTO.PublisherId;
                _dbContext.SaveChanges();

            }
            var authorDomain = _dbContext.Books_Author.Where(a => a.BookId == id).ToList(); 
            if (authorDomain != null)
            {
                _dbContext.Books_Author.RemoveRange(authorDomain);
                _dbContext.SaveChanges();

            }
            foreach (var authorid in bookDTO.AuthorId)
            {
                var _book_author = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorid
                };

                _dbContext.Books_Author.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return bookDTO;
        }
        public Books? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id); if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;

        }
    }

}
    



