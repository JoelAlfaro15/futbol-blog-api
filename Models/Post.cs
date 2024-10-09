namespace futapi.Models;

public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
    }

    public class PostDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; } // La ruta de la imagen recibida
    }
