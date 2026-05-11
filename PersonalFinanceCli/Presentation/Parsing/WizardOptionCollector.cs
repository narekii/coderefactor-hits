namespace PersonalFinanceCli.Presentation.Parsing;

public sealed class WizardOptionCollector
{
    public WizardOptions Collect(IReadOnlyList<string> tokens, int startIndex)
    {
        string? cardRaw = null;
        DateOnly? date = null;
        string? note = null;

        var i = startIndex;
        while (i < tokens.Count)
        {
            var option = tokens[i];
            if (option == "--card")
            {
                i++;
                cardRaw = i < tokens.Count ? tokens[i] : null;
                if (string.IsNullOrWhiteSpace(cardRaw))
                {
                    return new WizardOptions(null, null, null, "Invalid --card value.");
                }
            }
            else if (option == "--date")
            {
                i++;
                if (i >= tokens.Count || !DateOnly.TryParse(tokens[i], out var parsedDate))
                {
                    return new WizardOptions(null, null, null, "Invalid --date value. Use YYYY-MM-DD.");
                }

                date = parsedDate;
            }
            else if (option == "--note")
            {
                i++;
                if (i >= tokens.Count)
                {
                    return new WizardOptions(null, null, null, "Invalid --note value.");
                }

                note = tokens[i];
            }
            else
            {
                return new WizardOptions(null, null, null, $"Unknown option {option}.");
            }

            i++;
        }

        return new WizardOptions(cardRaw, date, note, null);
    }
}