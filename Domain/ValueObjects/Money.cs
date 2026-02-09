using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    /// <summary>
    /// Value Object for monetary amounts to ensure consistency
    /// </summary>
    public record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "VND";

        public Money(decimal amount, string currency = "VND")
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            Amount = Math.Round(amount, 2);
            Currency = currency?.ToUpperInvariant() ?? "VND";
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies");

            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies");

            return new Money(a.Amount - b.Amount, a.Currency);
        }

        public static Money operator *(Money money, int multiplier)
            => new(money.Amount * multiplier, money.Currency);

        public static Money operator *(Money money, decimal multiplier)
            => new(money.Amount * multiplier, money.Currency);

        public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
        public static bool operator <(Money a, Money b) => a.Amount < b.Amount;
        public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;
        public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;

        public override string ToString() => $"{Amount:N0} {Currency}";
    }
}
