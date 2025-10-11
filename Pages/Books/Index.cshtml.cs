using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;
        public IndexModel(Sician_Diana_Lab2Context context) => _context = context;

        public IList<Book> Book { get; set; } = new List<Book>();

        
        [BindProperty(SupportsGet = true)]
        public int? AuthorID { get; set; }

        public SelectList AuthorsSelect { get; set; } = default!;

        public async Task OnGetAsync()
        {
            
            var authors = await _context.Author
                .OrderBy(a => a.LastName)
                .ToListAsync();
            AuthorsSelect = new SelectList(authors, "ID", "LastName");

            
            IQueryable<Book> query = _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher);

            if (AuthorID.HasValue)
                query = query.Where(b => b.AuthorID == AuthorID.Value);

            Book = await query.ToListAsync();
        }
    }
}
