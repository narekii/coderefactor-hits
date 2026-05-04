namespace PersonalFinanceCli.Presentation.Parsing.Commands
{
    public sealed record CardAddCommand(
        string Name,
        string Currency,
        decimal? InitialBalance) : ParsedCommand;

}
