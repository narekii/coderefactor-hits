namespace PersonalFinanceCli.Presentation.Parsing.DTOs
{
    public sealed record ReportDayCommand(DateOnly? Date) : ParsedCommand;
}
