using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Calbaza_Ioan_Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Calbaza_Ioan_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Calbaza_Ioan_Lab2Context _context;

    public IndexModel(Calbaza_Ioan_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;

    public async Task OnGetAsync(int? authorId)
    {
        ViewData["AuthorID"] = new SelectList(
            _context.Authors.Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            }).ToList(),
            "ID",
            "FullName",
            authorId);

        var query = _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .AsQueryable();

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorID == authorId.Value);
        }

        Book = await query.ToListAsync();
    }
}
