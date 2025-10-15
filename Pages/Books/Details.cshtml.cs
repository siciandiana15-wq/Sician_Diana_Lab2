using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Books
{
    public class DetailsModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;
        public DetailsModel(Sician_Diana_Lab2Context context) => _context = context;

        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)        // <—
                    .ThenInclude(bc => bc.Category)    // <—
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            return Book == null ? NotFound() : Page();
        }
    }
}
