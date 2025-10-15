using Library_Wep.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Library_Wep.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BooksController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index([FromQuery] string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            List<BookDTO> response = new List<BookDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync("https://localhost:7192/api/Books/get-allbooks?filteron="+filterOn+"&filterQuery="+filterQuery+"&sortBy="+sortBy+"&isAscending="+isAscending);
                httpResponseMess.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMess.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>());

            }
            catch (Exception ex)
            {
                return View("Error");
            }
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> addBook(addBookDTO AddBookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7192/api/Books/add-book"),
                    Content = new StringContent(JsonSerializer.Serialize(AddBookDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                //Console.WriteLine(JsonSerializer.Serialize(addBookDTO));
                var httpResponseMessage = await client.SendAsync(httpRequestMess);
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<addBookDTO>();
                if (response != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public async Task<IActionResult> listBook(int id)
        {
            BookDTO response = new BookDTO();
            try
            {
                
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7192/api/Books/get-book-by-id/" + id);
                httpResponseMessage.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                response = await httpResponseMessage.Content.ReadFromJsonAsync<BookDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            BookDTO responseBook = new BookDTO();
            var client = _httpClientFactory.CreateClient();

            // 1. Get Book by ID
            var httpResponseMessage = await client.GetAsync("https://localhost:7192/api/Books/get-book-by-id/" + id);
            httpResponseMessage.EnsureSuccessStatusCode();
            responseBook = await httpResponseMessage.Content.ReadFromJsonAsync<BookDTO>();
            ViewBag.Book = responseBook;

            // 2. Get All Authors
            List<authorDTO> responseAu = new List<authorDTO>();
            var httpResponseMessageAu = await client.GetAsync("https://localhost:7192/api/Authors/get-all-author");
            httpResponseMessageAu.EnsureSuccessStatusCode();
            responseAu.AddRange(await httpResponseMessageAu.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>());
            ViewBag.ListAuthor = responseAu;

            // 3. Get All Publishers
            List<publisherDTO> responsePu = new List<publisherDTO>();
            var httpResponseMessagePu = await client.GetAsync("https://localhost:7192/api/Publishers/get-all-publisher");
            httpResponseMessagePu.EnsureSuccessStatusCode();
            responsePu.AddRange(await httpResponseMessagePu.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>());
            ViewBag.ListPublisher = responsePu;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> editBook([FromRoute] int id, editDTO bookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("https://localhost:7192/api/Books/update-book-by-id/" + id),
                    Content = new StringContent(JsonSerializer.Serialize(bookDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMess);
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<addBookDTO>();
                if (response != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> delBook([FromRoute] int id)
        {
            try
            {
                // lấy dữ liệu books from API
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync("https://localhost:7192/api/Books/delete-book-by-"
                    + "id/" + id);
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("Index");
        }
    } 
       

    
}
