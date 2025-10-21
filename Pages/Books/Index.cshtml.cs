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

    // constructor – doar păstrez contextul
    public IndexModel(Sician_Diana_Lab2Context context) => _context = context;

    // rămâne pentru compatibilitate cu .cshtml (dacă era legat de asta)
    public IList<Book> Book { get; set; } = new List<Book>();

    // ce cere lab-ul – „view model”-ul cu listele ce-mi trebuie
    public BookData BookD { get; set; } = default!;
        public string CurrentFilter { get; set; }

        // id carte selectată (din lab)
        public int? BookID { get; set; }

    // id categorie selectată (din lab)
    public int? CategoryID { get; set; }

     public string TitleSort { get; set; }
     public string AuthorSort { get; set; }

        // păstrez filtrarea ta existentă după autor (nu o stric)
        [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    // dropdown cu autori – îl populăm ca înainte
    public SelectList AuthorsSelect { get; set; } = default!;

        // am unificat semnătura din lab cu a ta: primesc id + categoryID din querystring
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string
    searchString)
        {
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentFilter = searchString;
            // 1) Populez dropdown-ul de autori (ca înainte)
            var authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ToListAsync();


            // 2) Construiesc query-ul de cărți cu includerile cerute în lab
            //    + păstrez filtrarea ta după AuthorID dacă e setată
            IQueryable<Book> query = _context.Book
                .Include(b => b.Publisher)              // lab: includ editorul
                .Include(b => b.BookCategories)         // lab: includ legătura many-to-many
                    .ThenInclude(bc => bc.Category)     // lab: includ categoriile efective
                .Include(b => b.Author)                 // păstrez și Author (tu îl foloseai)
                .AsNoTracking()
                .OrderBy(b => b.Title);                 // lab: sortez după titlu

            // dacă s-a selectat un autor din dropdown, filtrez (comportamentul tău inițial)

            if (AuthorID.HasValue)
            {
                query = query.Where(b => b.AuthorID == AuthorID.Value);
            }
            // 3) Umplu BookData conform lab-ului
            BookD = new BookData
            {
                Books = await query.ToListAsync()
            };
            if (!string.IsNullOrEmpty(searchString))
            {
                var term = searchString.Trim();

                BookD.Books = BookD.Books
                    .Where(s =>
                        (!string.IsNullOrEmpty(s.Title) &&
                         s.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
                        ||
                        (s.Author != null && (
                            (!string.IsNullOrEmpty(s.Author.FirstName) &&
                             s.Author.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(s.Author.LastName) &&
                             s.Author.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
                            ($"{s.Author.FirstName ?? ""} {s.Author.LastName ?? ""}"
                                .Contains(term, StringComparison.OrdinalIgnoreCase))
                        ))
                    )
                    .ToList();
            }
            AuthorsSelect = new SelectList(authors, "ID", "LastName");
            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Title);
                    break;
                case "author_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s =>
                   s.Author.FullName);
                    break;
                case "author":
                    BookD.Books = BookD.Books.OrderBy(s =>
                   s.Author.FullName);
                    break;
                default:
                    BookD.Books = BookD.Books.OrderBy(s => s.Title);
                    break;

            }
            


            // 4) Dacă mi-a venit un id de carte, scot categoriile cărții selectate (lab)
            if (id != null)
        {
            BookID = id.Value;
            var book = BookD.Books.Single(i => i.ID == id.Value); // știu sigur că e una
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }

        // 5) Dacă mi-a venit categoryID, mai aplic o filtrare în memorie (lab)
        if (categoryID != null)
        {
            CategoryID = categoryID.Value;
            BookD.Books = BookD.Books
                .Where(b => b.BookCategories.Any(bc => bc.CategoryID == CategoryID))
                .ToList();
        }

        // 6) Pentru .cshtml care poate folosi încă `Model.Book`, copiez lista (nu stric nimic)
        Book = BookD.Books.ToList();
    }
}
}
