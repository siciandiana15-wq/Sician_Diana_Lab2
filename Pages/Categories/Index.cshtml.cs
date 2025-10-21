using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sician_Diana_Lab2.Data;
using Sician_Diana_Lab2.Models;
using Sician_Diana_Lab2.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Sician_Diana_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Sician_Diana_Lab2Context _context;

        public IndexModel(Sician_Diana_Lab2Context context)
        {
            _context = context;
        }

        public CategoryIndexData CategoryData { get; set; }
        public int CategoryID { get; set; }
        public int BookID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            CategoryData = new CategoryIndexData();

            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .OrderBy(c => c.CategoryName)
                .AsNoTracking()
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                var category = CategoryData.Categories
                    .SingleOrDefault(c => c.ID == id.Value);
                CategoryData.Books = category != null
                    ? category.BookCategories.Select(bc => bc.Book)
                    : Enumerable.Empty<Book>();
            }
        }
    }
}