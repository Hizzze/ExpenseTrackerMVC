using System.Globalization;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Syncfusion.EJ2.Charts;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<ActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransaction = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            
            int TotalIncome = SelectedTransaction
                .Where(i => i.Category != null && i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

// Expense
            int TotalExpense = SelectedTransaction
                .Where(i => i.Category != null && i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            int Balance = TotalIncome - TotalExpense;
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            cultureInfo.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(cultureInfo, "{0:C0}", Balance);
            
            // Donut chart - Expense 
            ViewBag.DoughnutChartData = SelectedTransaction
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();
            
            // Spline Chart - Income vs Expense
            // Income


            List<SplineChartData> IncomeSummary = SelectedTransaction
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MM"),
                    income = k.Sum(l => l.Amount)

                }).ToList();
            
            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransaction
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MM"),
                    expense = k.Sum(l => l.Amount)

                }).ToList();
            
            // Combine Income & Expense

            string[] last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MM"))
                .ToArray();

            ViewBag.SplineChartData = from day in last7Days
                join income in IncomeSummary on day equals income.day into dayIncomeJoined
                from income in dayIncomeJoined.DefaultIfEmpty()
                join expense in ExpenseSummary on day equals expense.day into expenseJoined
                from expense in expenseJoined.DefaultIfEmpty()
                select new
                {
                    day = day,
                    income = income == null ? 0 : income.income,
                    expense = expense == null ? 0 : expense.expense
                };
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}