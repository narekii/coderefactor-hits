using PersonalFinanceCli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceCli.Infrastructure.Persistence.Models
{
    public sealed class DataFile
    {
        public List<Card> Cards { get; set; } = new();

        public List<Transaction> Transactions { get; set; } = new();

        public List<DailyLimit> DailyLimits { get; set; } = new();

        public DateOnly? LastCushionDeclinedDate { get; set; }

        public bool HasSeenOnboarding { get; set; }
    }
}
