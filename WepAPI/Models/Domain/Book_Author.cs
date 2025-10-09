namespace WepAPI.Models.Domain
{
    public class Book_Author
    {
        public int Id { get; set; } 
        public int BookId { get; set; }
        public Books Books { get; set; }
        public int AuthorId {  get; set; }
        public Authors Authors { get; set; }
    }
}
