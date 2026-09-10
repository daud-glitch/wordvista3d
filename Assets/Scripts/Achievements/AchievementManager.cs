using System;
using System.Collections.Generic;
using UnityEngine;
using WordVista.Save;
using WordVista.Economy;

namespace WordVista.Achievements
{
    [Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public int coinReward;
        public bool isUnlocked;
    }

    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }

        [SerializeField] private List<Achievement> achievements = new List<Achievement>();

        public event Action<Achievement> OnAchievementUnlocked;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            InitializeAchievements();
        }

        private void InitializeAchievements()
        {
            achievements = new List<Achievement>
            {
                new Achievement { id = "LVL_10", title = "Valley Explorer", description = "Complete 10 levels", coinReward = 50 },
                new Achievement { id = "LVL_50", title = "Forest Wanderer", description = "Complete 50 levels", coinReward = 100 },
                new Achievement { id = "LVL_100", title = "Kingdom Master", description = "Complete 100 levels", coinReward = 150 },
                new Achievement { id = "LVL_250", title = "Aurora Grand Champion", description = "Complete all 250 levels", coinReward = 500 },
                new Achievement { id = "BONUS_50", title = "Word Hunter", description = "Find 50 bonus words", coinReward = 75 },
                new Achievement { id = "BONUS_100", title = "Lexicon Seeker", description = "Find 100 bonus words", coinReward = 150 },
                new Achievement { id = "WORDS_500", title = "Wordsmith", description = "Find 500 words", coinReward = 200 },
                new Achievement { id = "PERFECT_10", title = "Pure Genius", description = "Complete 10 levels without hints", coinReward = 100 }
            };

            var unlocked = SaveManager.Instance.Data.unlockedAchievementIds;
            foreach (var ach in achievements)
            {
                ach.isUnlocked = unlocked.Contains(ach.id);
            }
        }

        public void CheckAchievements()
        {
            var data = SaveManager.Instance.Data;

            CheckRule("LVL_10", data.completedLevelIds.Count >= 10);
            CheckRule("LVL_50", data.completedLevelIds.Count >= 50);
            CheckRule("LVL_100", data.completedLevelIds.Count >= 100);
            CheckRule("LVL_250", data.completedLevelIds.Count >= 250);
            CheckRule("BONUS_50", data.totalBonusWordsFound >= 50);
            CheckRule("BONUS_100", data.totalBonusWordsFound >= 100);
            CheckRule("WORDS_500", data.totalWordsFound >= 500);
            CheckRule("PERFECT_10", data.perfectLevelsWithoutHints >= 10);
        }

        private void CheckRule(string id, bool condition)
        {
            if (!condition) return;

            var ach = achievements.Find(a => a.id == id);
            if (ach != null && !ach.isUnlocked)
            {
                ach.isUnlocked = true;
                SaveManager.Instance.Data.unlockedAchievementIds.Add(id);
                EconomyManager.Instance.AddCoins(ach.coinReward);
                SaveManager.Instance.Save();
                OnAchievementUnlocked?.Invoke(ach);
            }
        }
    }
}
