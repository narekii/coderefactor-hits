using PersonalFinanceCli.Application.Repositories;
using PersonalFinanceCli.Domain.Entities;

namespace PersonalFinanceCli.Infrastructure.Persistence;

public sealed class JsonCardRepository : ICardRepository
{
    private readonly JsonDataStore _store;

    public JsonCardRepository(JsonDataStore store)
    {
        _store = store;
    }

    public IReadOnlyList<Card> GetAll()
    {
        return _store.Load().Cards.OrderBy(c => c.Id).ToList();
    }

    public Card? GetById(int id)
    {
        return _store.Load().Cards.FirstOrDefault(c => c.Id == id);
    }

    public Card? GetDefault()
    {
        return _store.Load().Cards.FirstOrDefault(c => c.IsDefault);
    }

    public Card? GetFirst()
    {
        return _store.Load().Cards.OrderBy(c => c.Id).FirstOrDefault();
    }

    public Card Add(Card card)
    {
        var data = _store.Load();
        card.Id = data.Cards.Count == 0 ? 1 : data.Cards.Max(c => c.Id) + 1;
        if (data.Cards.Count == 0)
        {
            card.IsDefault = true;
        }

        data.Cards.Add(card);
        _store.Save(data);
        return card;
    }

    public void SetDefault(int cardId)
    {
        var data = _store.Load();
        foreach (var card in data.Cards)
        {
            card.IsDefault = card.Id == cardId;
        }

        _store.Save(data);
    }
}
