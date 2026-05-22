using UnityEngine;
using System;
using System.Collections.Generic;

namespace SesiDefense.Core
{
    /// <summary>
    /// Manages player currency and economy system.
    /// Handles earning, spending, and currency events.
    /// </summary>
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private int startingCurrency = 500;
        [SerializeField] private int currencyCapacity = 999999;
        [SerializeField] private float moneyGenerationRate = 1.5f;

        private int currentCurrency;
        private int totalMoneyEarned;
        private int totalMoneySpent;
        private EventSystem eventSystem;
        private Dictionary<CurrencySource, int> currencyFromSources = new Dictionary<CurrencySource, int>();
        private List<CurrencyTransaction> transactionHistory = new List<CurrencyTransaction>();
        private const int MAX_HISTORY = 1000;

        public int CurrentCurrency => currentCurrency;
        public int TotalMoneyEarned => totalMoneyEarned;
        public int TotalMoneySpent => totalMoneySpent;

        private void Awake()
        {
            eventSystem = GameManager.Instance.GetEventSystem();
            Initialize(startingCurrency);
        }

        /// <summary>
        /// Initializes currency with starting amount.
        /// </summary>
        public void Initialize(int initial)
        {
            currentCurrency = Mathf.Min(initial, currencyCapacity);
            totalMoneyEarned = initial;
            totalMoneySpent = 0;
            currencyFromSources.Clear();
            transactionHistory.Clear();

            Logger.Log($"Currency initialized: {currentCurrency}", LogLevel.Info);
            eventSystem?.Dispatch(new CurrencyChangedEvent { amount = currentCurrency });
        }

        /// <summary>
        /// Adds currency to the player.
        /// </summary>
        public bool AddCurrency(int amount, CurrencySource source = CurrencySource.Wave)
        {
            if (amount <= 0)
            {
                Logger.Log("Cannot add negative currency", LogLevel.Warning);
                return false;
            }

            int previousAmount = currentCurrency;
            int amountToAdd = Mathf.Min(amount, currencyCapacity - currentCurrency);
            currentCurrency += amountToAdd;
            totalMoneyEarned += amountToAdd;

            // Track source
            if (!currencyFromSources.ContainsKey(source))
            {
                currencyFromSources[source] = 0;
            }
            currencyFromSources[source] += amountToAdd;

            // Log transaction
            LogTransaction(new CurrencyTransaction
            {
                timestamp = Time.time,
                type = CurrencyTransactionType.Earn,
                amount = amountToAdd,
                source = source,
                previousBalance = previousAmount,
                newBalance = currentCurrency
            });

            eventSystem?.Dispatch(new CurrencyChangedEvent
            {
                amount = currentCurrency,
                change = amountToAdd,
                source = source
            });

            return true;
        }

        /// <summary>
        /// Removes currency from the player.
        /// </summary>
        public bool RemoveCurrency(int amount, string reason = "")
        {
            if (amount <= 0)
            {
                Logger.Log("Cannot remove negative currency", LogLevel.Warning);
                return false;
            }

            if (currentCurrency < amount)
            {
                Logger.Log($"Insufficient currency. Required: {amount}, Available: {currentCurrency}", LogLevel.Warning);
                eventSystem?.Dispatch(new InsufficientCurrencyEvent { requiredAmount = amount, availableAmount = currentCurrency });
                return false;
            }

            int previousAmount = currentCurrency;
            currentCurrency -= amount;
            totalMoneySpent += amount;

            LogTransaction(new CurrencyTransaction
            {
                timestamp = Time.time,
                type = CurrencyTransactionType.Spend,
                amount = amount,
                reason = reason,
                previousBalance = previousAmount,
                newBalance = currentCurrency
            });

            eventSystem?.Dispatch(new CurrencyChangedEvent
            {
                amount = currentCurrency,
                change = -amount
            });

            return true;
        }

        /// <summary>
        /// Checks if player has enough currency.
        /// </summary>
        public bool HasCurrency(int amount)
        {
            return currentCurrency >= amount;
        }

        /// <summary>
        /// Sets currency to a specific amount (for testing/admin).
        /// </summary>
        public void SetCurrency(int amount)
        {
            int previousAmount = currentCurrency;
            currentCurrency = Mathf.Clamp(amount, 0, currencyCapacity);

            LogTransaction(new CurrencyTransaction
            {
                timestamp = Time.time,
                type = CurrencyTransactionType.Admin,
                amount = currentCurrency - previousAmount,
                reason = "Admin set",
                previousBalance = previousAmount,
                newBalance = currentCurrency
            });

            eventSystem?.Dispatch(new CurrencyChangedEvent { amount = currentCurrency });
        }

        /// <summary>
        /// Gets currency earned from a specific source.
        /// </summary>
        public int GetCurrencyFromSource(CurrencySource source)
        {
            return currencyFromSources.ContainsKey(source) ? currencyFromSources[source] : 0;
        }

        /// <summary>
        /// Gets transaction history.
        /// </summary>
        public List<CurrencyTransaction> GetTransactionHistory(int limit = 50)
        {
            int startIndex = Mathf.Max(0, transactionHistory.Count - limit);
            return transactionHistory.GetRange(startIndex, Mathf.Min(limit, transactionHistory.Count));
        }

        /// <summary>
        /// Logs a currency transaction.
        /// </summary>
        private void LogTransaction(CurrencyTransaction transaction)
        {
            transactionHistory.Add(transaction);

            // Keep history manageable
            if (transactionHistory.Count > MAX_HISTORY)
            {
                transactionHistory.RemoveAt(0);
            }
        }

        /// <summary>
        /// Resets currency manager.
        /// </summary>
        public void Reset()
        {
            Initialize(startingCurrency);
        }
    }

    /// <summary>
    /// Currency source enumeration.
    /// </summary>
    public enum CurrencySource
    {
        Wave,
        Boss,
        Special,
        Bonus,
        Passive,
        Multiplier
    }

    /// <summary>
    /// Currency transaction type.
    /// </summary>
    public enum CurrencyTransactionType
    {
        Earn,
        Spend,
        Refund,
        Bonus,
        Admin
    }

    /// <summary>
    /// Currency transaction record.
    /// </summary>
    public class CurrencyTransaction
    {
        public float timestamp { get; set; }
        public CurrencyTransactionType type { get; set; }
        public int amount { get; set; }
        public CurrencySource source { get; set; }
        public string reason { get; set; }
        public int previousBalance { get; set; }
        public int newBalance { get; set; }
    }

    /// <summary>
    /// Currency changed event.
    /// </summary>
    public class CurrencyChangedEvent
    {
        public int amount { get; set; }
        public int change { get; set; }
        public CurrencySource source { get; set; }
    }

    /// <summary>
    /// Insufficient currency event.
    /// </summary>
    public class InsufficientCurrencyEvent
    {
        public int requiredAmount { get; set; }
        public int availableAmount { get; set; }
    }
}
