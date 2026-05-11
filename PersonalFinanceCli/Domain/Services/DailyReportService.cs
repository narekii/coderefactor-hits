using PersonalFinanceCli.Application.Repositories;
using PersonalFinanceCli.Domain.ValueObjects;
using PersonalFinanceCli.Domain.DTOs;

namespace PersonalFinanceCli.Domain.Services;

public sealed class DailyReportService
{
    private readonly ICardRepository _cardRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILimitRepository _limitRepository;

    public DailyReportService(
        ICardRepository cardRepository,
        ITransactionRepository transactionRepository,
        ILimitRepository limitRepository)
    {
        _cardRepository = cardRepository;
        _transactionRepository = transactionRepository;
        _limitRepository = limitRepository;
    }

    public DailyReport Generate(DateOnly date)
    {
        var cards = _cardRepository.GetAll();
        var currency = cards.FirstOrDefault(c => c.IsDefault)?.Currency
            ?? cards.FirstOrDefault()?.Currency
            ?? Currency.RUB;

        var cardIds = cards.Where(c => c.Currency == currency).Select(c => c.Id).ToHashSet();
        var allTransactions = _transactionRepository.GetAll();

        decimal income = 0m;
        decimal expense = 0m;
        var categoryTotals = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        foreach (var transaction in allTransactions)
        {
            if (!cardIds.Contains(transaction.CardId) || transaction.Date != date)
            {
                continue;
            }

            if (transaction.Type == TransactionType.Income)
            {
                income += transaction.Amount;
            }
            else
            {
                expense += transaction.Amount;
                if (categoryTotals.ContainsKey(transaction.Category))
                {
                    categoryTotals[transaction.Category] += transaction.Amount;
                }
                else
                {
                    categoryTotals[transaction.Category] = transaction.Amount;
                }
            }
        }

        var limit = _limitRepository.GetByDate(date);

        var balances = new List<CardBalanceLine>();
        foreach (var card in cards)
        {
            decimal balance = card.InitialBalance;
            foreach (var transaction in allTransactions.Where(trans => trans.CardId == card.Id))
            {
                if (transaction.Type == TransactionType.Income)
                {
                    balance += transaction.Amount;
                }
                else
                {
                    balance -= transaction.Amount;
                }
            }

            balances.Add(new CardBalanceLine(card.Id, card.Name, card.IsDefault, balance, card.Currency));
        }

        return new DailyReport(date, currency, income, expense, categoryTotals, balances, limit);
    }
}