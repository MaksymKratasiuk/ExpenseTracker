using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using teamProject.Data;
using teamProject.Models;

namespace teamProject.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string TotalIncome { get; set; }
        public string TotalExpense { get; set; }
        public string Balance { get; set; }
        public List<object> DoughnutChartData { get; set; }
        public List<object> SplineChartData { get; set; }
        public List<teamProject.Models.Transaction> RecentTransactions { get; set; }

        public async Task OnGetAsync()
        {
            // Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<teamProject.Models.Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            // Total Income
            int TotalIncomeAmount = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            TotalIncome = TotalIncomeAmount.ToString("C0");

            // Total Expense
            int TotalExpenseAmount = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            TotalExpense = TotalExpenseAmount.ToString("C0");

            // Balance
            int BalanceAmount = TotalIncomeAmount - TotalExpenseAmount;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            Balance = String.Format(culture, "{0:C0}", BalanceAmount);

            // Doughnut Chart - Expense By Category
            DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList<object>();

            // Spline Chart - Income vs Expense

            // Income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            // Expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            // Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            SplineChartData = (from day in Last7Days
                               join income in IncomeSummary on day equals income.day into dayIncomeJoined
                               from income in dayIncomeJoined.DefaultIfEmpty()
                               join expense in ExpenseSummary on day equals expense.day into expenseJoined
                               from expense in expenseJoined.DefaultIfEmpty()
                               select new
                               {
                                   day = day,
                                   income = income == null ? 0 : income.income,
                                   expense = expense == null ? 0 : expense.expense,
                               }).ToList<object>();

            // Recent Transactions
            RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();
        }
    }

    public class SplineChartData
    {
        public string day { get; set; }
        public int income { get; set; }
        public int expense { get; set; }
    }
}
