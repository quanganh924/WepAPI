using System.ComponentModel.DataAnnotations;

namespace WepAPI.Models.Domain
{
    public class Authors
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<Book_Author> Book_Author {  get; set; }
    }
}
