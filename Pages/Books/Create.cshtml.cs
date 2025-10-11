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
    public class CreateModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;

        public CreateModel(Sician_Diana_Lab2Context context)
        {
            _context = context;
        }

        // IMPORTANT: modelul legat de formular
        [BindProperty]
        public Book Book { get; set; } = new Book();

        public IActionResult OnGet()
        {
            ViewData["AuthorID"] = new SelectList(_context.Author.OrderBy(a => a.LastName), "ID", "LastName");
            ViewData["PublisherID"] = new SelectList(_context.Publisher.OrderBy(p => p.PublisherName), "ID", "PublisherName");
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

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}