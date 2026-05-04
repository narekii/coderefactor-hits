namespace PersonalFinanceCli.Presentation.Parsing.Commands
{
    public sealed record LimitSetCommand(decimal Amount) : ParsedCommand;
}
