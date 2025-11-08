using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;

namespace Sician_Diana_Lab2.Pages.Borrowings
{
    public class DetailsModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;

        public DetailsModel(Sician_Diana_Lab2Context context)
        {
            _context = context;
        }

        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            Borrowing = borrowing;

            return Page();
        }
    }
}
