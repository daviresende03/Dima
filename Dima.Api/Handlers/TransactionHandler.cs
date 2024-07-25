using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Dima.Api.Handlers
{
    public class TransactionHandler(AppDbContext context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdrawal, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = new Transaction
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    CreatedAt = DateTime.Now,
                    Amount = request.Amount,
                    PaidOrReceivedAt = request.PaidOrReceivedAt,
                    CategoryId = request.CategoryId,
                    Type = request.Type
                };

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, (int)HttpStatusCode.Created, "Transaction successfully created");
            }
            catch (Exception ex)
            {
                return new Response<Transaction?>(null, (int)HttpStatusCode.InternalServerError, "An error occurred while creating a transaction");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(null, (int)HttpStatusCode.NotFound, "Transaction not found");

                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transaction successfully deleted");
            }
            catch (Exception ex)
            {
                return new Response<Transaction?>(null, (int)HttpStatusCode.InternalServerError, "An error occurred while deleting a transaction");
            }
        }

        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await context.Transactions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                return transaction is null ?
                    new Response<Transaction?>(null, (int)HttpStatusCode.NotFound, "Transaction not found") :
                    new Response<Transaction?>(transaction);
            }
            catch (Exception ex)
            {
                return new Response<Transaction?>(null, (int)HttpStatusCode.InternalServerError, "An error occurred while searching a transaction");
            }
        }

        public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();

                var query = context.Transactions
                    .AsNoTracking()
                    .Where(x => 
                        x.PaidOrReceivedAt >= request.StartDate && 
                        x.PaidOrReceivedAt <= request.EndDate && 
                        x.UserId == request.UserId)
                    .OrderBy(x => x.PaidOrReceivedAt);

                var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {

               return new PagedResponse<List<Transaction>?>(null, (int)HttpStatusCode.InternalServerError, "An error occurred while searching transactions");
            }
        }

        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            if (request is { Type: ETransactionType.Withdrawal, Amount: >= 0 })
                request.Amount *= -1;

            try
            {
                var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (transaction is null)
                    return new Response<Transaction?>(null, (int)HttpStatusCode.NotFound, "Transaction not found");

                transaction.Title = request.Title;
                transaction.Amount = request.Amount;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
                transaction.CategoryId = request.CategoryId;
                transaction.Type = request.Type;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, message: "Transaction successfully updated");
            }
            catch (Exception ex)
            {
                return new Response<Transaction?>(null, (int)HttpStatusCode.InternalServerError, "An error occurred while updating a transaction");
            }
        }
    }
}
