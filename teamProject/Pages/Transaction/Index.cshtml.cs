using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using teamProject.Data;

namespace teamProject.Pages.Transaction
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<teamProject.Models.Transaction> Transactions { get; set; }

        public async Task OnGetAsync()
        {
            Transactions = await _context.Transactions.Include(t => t.Category).ToListAsync();
        }
    }
}
