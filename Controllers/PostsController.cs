using futapi.Data;
using futapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace futapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly FutblogContext _context;

    public PostsController(FutblogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
    {
        return await _context.Posts.Include(p => p.Category).Include(p => p.User).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await _context.Posts.Include(p => p.Category).Include(p => p.User)
                                        .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return post;
    }

    [HttpPost("CreatePost")]
    public async Task<ActionResult> CreatePost([FromBody] PostDTO postDto)
    {
        // Mapeo
        var post = new Post
        {
            Title = postDto.Title,
            Body = postDto.Body,
            CategoryId = postDto.CategoryId,
            UserId = postDto.UserId,
        };

        // Añadir 
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Retorna el resultado exitoso junto con los detalles del post creado.
        return Ok(new {message = "Post creado"});
    }





    [HttpPost("update/{id}")]
    public async Task<IActionResult> PutPost(int id, [FromBody] Post post)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }

        _context.Entry(post).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PostExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(new {message = "Post actualizado"});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return Ok("Post eliminado");
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }
}
