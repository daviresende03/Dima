using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Net;
using System.Net.Http.Json;

namespace Dima.Web.Handlers;

public class ReportHandler(IHttpClientFactory httpClientFactory) : IReportHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(
        GetIncomesAndExpensesRequest request)
        => await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>($"v1/reports/incomes-expenses") ??
               new Response<List<IncomesAndExpenses>?>(null, (int)HttpStatusCode.BadRequest, "An error occurred while generating a report");

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(
        GetIncomesByCategoryRequest request)
        => await _client.GetFromJsonAsync<Response<List<IncomesByCategory>?>>($"v1/reports/incomes") ??
               new Response<List<IncomesByCategory>?>(null, (int)HttpStatusCode.BadRequest, "An error occurred while generating a report");

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(
        GetExpensesByCategoryRequest request)
        => await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>($"v1/reports/expenses") ??
               new Response<List<ExpensesByCategory>?>(null, (int)HttpStatusCode.BadRequest, "An error occurred while generating a report");

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
        => await _client.GetFromJsonAsync<Response<FinancialSummary?>>($"v1/reports/summary") ??
               new Response<FinancialSummary?>(null, (int)HttpStatusCode.BadRequest, "An error occurred while generating a report");
}