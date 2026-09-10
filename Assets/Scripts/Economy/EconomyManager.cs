using System;
using WordVista.Save;

namespace WordVista.Economy
{
    public enum HintType
    {
        RevealLetter,  // 25 coins
        SmartHint,     // 40 coins
        RevealWord     // 100 coins
    }

    public class EconomyManager
    {
        private static EconomyManager _instance;
        public static EconomyManager Instance => _instance ??= new EconomyManager();

        public int RevealLetterCost { get; set; } = 25;
        public int SmartHintCost { get; set; } = 40;
        public int RevealWordCost { get; set; } = 100;

        public int BonusMilestoneCount { get; set; } = 5;
        public int BonusMilestoneReward { get; set; } = 30;

        public event Action<int, int> OnCoinsChanged; // newBalance, delta
        public event Action<HintType, int> OnHintPurchased;
        public event Action<int> OnBonusMilestoneReached;

        public int Coins => SaveManager.Instance.Data.coins;

        public bool CanAfford(HintType hint)
        {
            return Coins >= GetCost(hint);
        }

        public int GetCost(HintType hint)
        {
            switch (hint)
            {
                case HintType.RevealLetter: return RevealLetterCost;
                case HintType.SmartHint: return SmartHintCost;
                case HintType.RevealWord: return RevealWordCost;
                default: return 999;
            }
        }

        public bool TryPurchaseHint(HintType hint)
        {
            int cost = GetCost(hint);
            if (SpendCoins(cost))
            {
                SaveManager.Instance.Data.totalHintsUsed++;
                OnHintPurchased?.Invoke(hint, cost);
                return true;
            }
            return false;
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            SaveManager.Instance.Data.coins += amount;
            SaveManager.Instance.Data.totalCoinsEarned += amount;
            SaveManager.Instance.Save();
            OnCoinsChanged?.Invoke(Coins, amount);
        }

        public bool SpendCoins(int amount)
        {
            if (amount <= 0) return true;
            if (Coins < amount) return false;

            SaveManager.Instance.Data.coins -= amount;
            SaveManager.Instance.Data.totalCoinsSpent += amount;
            SaveManager.Instance.Save();
            OnCoinsChanged?.Invoke(Coins, -amount);
            return true;
        }

        public void RecordBonusWord()
        {
            var data = SaveManager.Instance.Data;
            data.totalBonusWordsFound++;
            data.bonusWordPouchProgress++;

            if (data.bonusWordPouchProgress >= BonusMilestoneCount)
            {
                data.bonusWordPouchProgress = 0;
                AddCoins(BonusMilestoneReward);
                OnBonusMilestoneReached?.Invoke(BonusMilestoneReward);
            }
            else
            {
                SaveManager.Instance.Save();
            }
        }
    }
}
