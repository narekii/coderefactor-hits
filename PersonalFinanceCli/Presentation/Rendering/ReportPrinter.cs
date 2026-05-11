using PersonalFinanceCli.Application.Repositories;
using PersonalFinanceCli.Domain.DTOs;
using PersonalFinanceCli.Domain.ValueObjects;
using System.Globalization;

namespace PersonalFinanceCli.Presentation.Rendering;

public sealed class ReportPrinter
{
    private readonly TextWriter _writer;

    public ReportPrinter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Print(DailyReport report)
    {
        _writer.WriteLine($"Date: {report.Date:yyyy-MM-dd}");
        _writer.WriteLine($"Income: {FormatMoney(report.Income, report.Currency)}");
        _writer.WriteLine($"Expense: {FormatMoney(report.Expense, report.Currency)}");

        PrintLimit(report.Expense, report.Limit?.Amount, report.Limit?.Currency ?? report.Currency);

        _writer.WriteLine("By category:");
        foreach (var pair in report.CategoryExpenses.OrderBy(x => x.Key, StringComparer.Ordinal))
        {
            _writer.WriteLine($"  {pair.Key}: {FormatMoney(pair.Value, report.Currency)}");
        }

        _writer.WriteLine("Cards:");
        foreach (var card in report.Cards.OrderBy(c => c.CardId))
        {
            var marker = card.IsDefault ? " (default)" : string.Empty;
            _writer.WriteLine($"  {card.CardName}{marker}: {FormatMoney(card.Balance, card.Currency)}");
        }
    }

    private void PrintLimit(decimal expense, decimal? limit, Currency currency)
    {
        if (limit.HasValue && limit.Value > 0)
        {
            var percent = (int)Math.Floor((expense / limit.Value) * 100m);
            _writer.WriteLine($"Limit: {FormatMoney(limit.Value, currency)} ({percent}%)");
        }
        else
        {
            _writer.WriteLine("Limit: (not set)");
        }
    }

    public static string FormatMoney(decimal amount, Currency currency)
    {
        return string.Create(CultureInfo.InvariantCulture, $"{amount:F2} {currency}");
    }
}
