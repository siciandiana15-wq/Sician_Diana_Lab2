using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;
        public EditModel(Sician_Diana_Lab2Context context) => _context = context;

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null) return NotFound();

            ViewData["AuthorID"] = new SelectList(_context.Author.OrderBy(a => a.LastName), "ID", "LastName", Book.AuthorID);
            ViewData["PublisherID"] = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName), "ID", "PublisherName", Book.PublisherID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["AuthorID"] = new SelectList(_context.Author.OrderBy(a => a.LastName), "ID", "LastName", Book.AuthorID);
                ViewData["PublisherID"] = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName), "ID", "PublisherName", Book.PublisherID);
                return Page();
            }

            // atașăm cartea din formular și marcăm ca modificată
            _context.Attach(Book).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
