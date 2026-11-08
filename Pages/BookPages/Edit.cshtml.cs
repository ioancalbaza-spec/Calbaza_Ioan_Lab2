using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Calbaza_Ioan_Lab2.Models;

namespace Calbaza_Ioan_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Calbaza_Ioan_Lab2Context _context;

    public EditModel(Calbaza_Ioan_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
    .Include(b => b.Publisher)
    .Include(b => b.Author)
    .Include(b => b.BookCategories)
        .ThenInclude(b => b.Category)
    .AsNoTracking()
    .FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        Book = book;
        PopulateAssignedCategoryData(_context, Book);
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(
    _context.Authors.Select(a => new
    {
        a.ID,
        FullName = a.FirstName + " " + a.LastName
    }).ToList(),
    "ID",
    "FullName",
    Book.AuthorID);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
    {
        if (id == null)
        {
            return NotFound();
        }
        var bookToUpdate = await _context.Book
    .Include(b => b.Publisher)
    .Include(b => b.Author)
    .Include(b => b.BookCategories)
        .ThenInclude(b => b.Category)
    .FirstOrDefaultAsync(m => m.ID == id);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title,
            i => i.AuthorID,
            i => i.Price,
            i => i.PublishingDate,
            i => i.PublisherID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        Book = bookToUpdate;
        PopulateAssignedCategoryData(_context, Book);

        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(), "ID", "PublisherName", Book.PublisherID);

        ViewData["AuthorID"] = new SelectList(
            _context.Authors.Select(a => new
            {
                a.ID,
                FullName = a.FirstName + " " + a.LastName
            }).ToList(),
            "ID",
            "FullName",
            Book.AuthorID);

        return Page();

    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
