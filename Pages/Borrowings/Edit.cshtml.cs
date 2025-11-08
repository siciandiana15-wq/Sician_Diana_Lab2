using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;

        public EditModel(Sician_Diana_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing.FirstOrDefaultAsync(m => m.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            Borrowing = borrowing;

            ViewData["MemberID"] = new SelectList(
    _context.Member.OrderBy(m => m.FirstName),
    "ID",
    "FullName",
    Borrowing.MemberID);

            ViewData["BookID"] = new SelectList(
                _context.Book.OrderBy(b => b.Title),
                "ID",
                "Title",
                Borrowing.BookID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["MemberID"] = new SelectList(
    _context.Member.OrderBy(m => m.FirstName),
    "ID",
    "FullName",
    Borrowing.MemberID);

                ViewData["BookID"] = new SelectList(
                    _context.Book.OrderBy(b => b.Title),
                    "ID",
                    "Title",
                    Borrowing.BookID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}