namespace PersonalFinanceCli.Presentation.Parsing.DTOs
{
    public sealed record CardAddCommand(
        string Name,
        string Currency,
        decimal? InitialBalance) : ParsedCommand;

}
