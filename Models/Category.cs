namespace futapi.Models;

public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    // public ICollection<Post> Posts { get; set; }
    }


public class CategoryDTO
{
    public string Name { get; set; }
}
