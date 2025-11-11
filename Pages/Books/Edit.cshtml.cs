using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Sician_Diana_Lab2Context _context;
        public EditModel(Sician_Diana_Lab2Context context) => _context = context;

        [BindProperty]
        public Book Book { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories).ThenInclude(b => b.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            // pregătesc lista de categorii care vor apărea cu bifele corespunzătoare
            PopulateAssignedCategoryData(_context, Book);

            // fac lista de autori cu numele complet pentru dropdown
            var authorList = _context.Author
                .Select(x => new
                {
                    x.ID,
                    FullName = x.LastName + " " + x.FirstName
                });

            // încarc dropdown-urile pentru autor și publisher
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            return Page();
        }

        // am pus ? la selectedCategories ca să nu dea eroare dacă nu bifez nicio categorie
        public async Task<IActionResult> OnPostAsync(int? id, string[]? selectedCategories)
        {
            if (id == null) return NotFound();

            var bookToUpdate = await _context.Book
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null) return NotFound();

            // actualizez restul datelor despre carte (titlu, autor, preț, etc.)
            if (await TryUpdateModelAsync<Book>(
                    bookToUpdate,
                    "Book",
                    b => b.Title,
                    b => b.AuthorID,
                    b => b.Price,
                    b => b.PublishingDate,
                    b => b.PublisherID))
            {
                // sincronizez categoriile selectate și apoi salvez
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // dacă apar erori, refac bifele și dropdown-urile pentru reafișare
            PopulateAssignedCategoryData(_context, bookToUpdate);

            var authorList = _context.Author
                .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                .OrderBy(a => a.FullName);

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName", bookToUpdate.AuthorID);
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher.OrderBy(p => p.PublisherName),
                "ID", "PublisherName", bookToUpdate.PublisherID);

            return Page();
        }
    }
}
