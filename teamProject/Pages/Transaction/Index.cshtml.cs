using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using teamProject.Data;
using teamProject.Models; // Додайте, якщо ще не додано
using System.Collections.Generic; // Додайте, якщо ще не додано

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

        // Метод для обробки запиту на видалення
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
