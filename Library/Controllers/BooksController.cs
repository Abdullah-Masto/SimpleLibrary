using AutoMapper;
using Library.Models.ForCreate;
using Library.Models.ViewModels;
using Library_Domain.Interfaces;
using Library_Domain.Modles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.Controllers
{
    [Route("api/Authers/{autherId}/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Auther> autherRepository;
        private readonly IRepository<Book> bookRepository;
        private readonly IMapper mapper;

        public BooksController(IRepository<Auther> autherRepository, IRepository<Book> bookRepository,IMapper mapper )
        {
            this.autherRepository = autherRepository;
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("{bookId}", Name = "GetBook")]
        public async Task<ActionResult<Book>> GetBook(int autherId, int bookId)
        {
            var auther = await autherRepository.GetByIdAsync(autherId);

            if (auther == null)
            {
                return NotFound();
            }

            var book = await bookRepository.GetByIdAsync(bookId);

            if (book == null)
            {
                return NotFound();
            }

            if (book.AutherId != autherId)
                return BadRequest("autherId does not match the books auther!");

            return Ok(mapper.Map<BookWithoutAuther>(book));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks(int autherId, int pageNumber = 1, int pageSize = 10)
        {
            var (books, paginationData) = await bookRepository.GetAllAsync(pageNumber, pageSize, (e => e.AutherId == autherId));

            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(paginationData));

            return Ok(mapper.Map<List<BookWithoutAuther>>(books));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(int autherId, Book book)
        {
            if (book == null)
                return BadRequest("no book inserted!");

            var auther = await autherRepository.GetByIdAsync(autherId);

            if (auther == null)
                return NotFound();

            book.Auther = auther;

            book.AutherId = autherId;

            bookRepository.AddAsync(book);

            return CreatedAtRoute("GetBook", new { autherId = autherId, bookId = bookRepository.GetLastIDAsync() }, mapper.Map<BookWithoutAuther>(book));
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<Book>> UpdateBook(int autherId, int bookId, Book newBook)
        {
            var book = await bookRepository.GetByIdAsync(bookId);

            if (book == null)
                return NotFound();

            var newAuther = await autherRepository.GetByIdAsync(autherId);

            if (newAuther == null)
                return NotFound("auther not found!");

            book.Title = newBook.Title;
            book.ReleaseDate = newBook.ReleaseDate;
            book.Auther = newAuther;
            book.AutherId = autherId;
            book.Pages = newBook.Pages;
            book.Language = newBook.Language;
            book.Price = newBook.Price;
            book.MainCategory = newBook.MainCategory;
            book.SubCategory = newBook.SubCategory;

            bookRepository.Update();

            return NoContent();
        }

        [HttpPatch("{bookId}")]
        public async Task<ActionResult<Book>> PatiallyUpdateBook(int autherId, int bookId, JsonPatchDocument<BookForCreate> document)
        {
            var book = await bookRepository.GetByIdAsync(bookId);

            if (book == null)
                return NotFound();

            var newAuther = await autherRepository.GetByIdAsync(autherId);

            if (newAuther == null)
                return NotFound("auther not found!");

            var bookToPatch = new BookForCreate()
            {
                Title = book.Title,
                ReleaseDate = book.ReleaseDate,
                Pages = book.Pages,
                AutherId = autherId,
                Auther = newAuther,
                Language = book.Language,
                Price = book.Price,
                MainCategory = book.MainCategory,
                SubCategory = book.SubCategory,
            };

            document.ApplyTo(bookToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            book.Title = bookToPatch.Title;
            book.ReleaseDate = bookToPatch.ReleaseDate;
            book.Pages = bookToPatch.Pages;
            book.AutherId = bookToPatch.AutherId;
            book.Auther = newAuther;
            book.Language = bookToPatch.Language;
            book.Price = bookToPatch.Price;
            book.MainCategory = bookToPatch.MainCategory;
            book.SubCategory = bookToPatch.SubCategory;

            bookRepository.Update();

            return NoContent();
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult<Auther>> DeleteAuther(int autherId, int bookId)
        {
            var auther = await autherRepository.GetByIdAsync(autherId, e => e.Books);

            if (auther == null) return NotFound();

            var book = await bookRepository.GetByIdAsync(bookId);

            if (book == null) return NotFound();

            auther.Books.Remove(book);

            autherRepository.Update();

            bookRepository.Delete(book);

            return NoContent();
        }
    }
}
