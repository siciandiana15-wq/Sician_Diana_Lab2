using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Books
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Sician_Diana_Lab2Context _context;
        public CreateModel(Sician_Diana_Lab2Context context) => _context = context;

        [BindProperty] public Book Book { get; set; } = new();

        public IActionResult OnGet()
        {
            // dropdown Autori + Publisher
            var authorList = _context.Author
                .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                .OrderBy(a => a.FullName)
                .ToList();

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName), "ID", "PublisherName");

            // bife categorii
            Book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, Book);

            return Page();
        }

        // primește categoriile bifate din formular
        public async Task<IActionResult> OnPostAsync(string[]? selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                // re-populează UI-ul când sunt erori de validare
                var authorList = _context.Author
                    .Select(a => new { a.ID, FullName = a.LastName + " " + a.FirstName })
                    .OrderBy(a => a.FullName)
                    .ToList();

                ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName", Book.AuthorID);
                ViewData["PublisherID"] = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName), "ID", "PublisherName", Book.PublisherID);

                PopulateAssignedCategoryData(_context, Book);
                return Page();
            }

            Book.BookCategories = new List<BookCategory>();

            if (selectedCategories != null)
            {
                foreach (var s in selectedCategories.Distinct())
                {
                    if (int.TryParse(s, out var id))
                        Book.BookCategories.Add(new BookCategory { CategoryID = id });
                }
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}