namespace PersonalFinanceCli.Presentation.Parsing.DTOs
{
    public sealed record LimitSetCommand(decimal Amount) : ParsedCommand;
}
