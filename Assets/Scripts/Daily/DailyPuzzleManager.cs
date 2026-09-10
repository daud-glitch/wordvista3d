using System;
using System.Collections.Generic;
using UnityEngine;
using WordVista.Levels;
using WordVista.Save;
using WordVista.Economy;

namespace WordVista.Daily
{
    public class DailyPuzzleManager : MonoBehaviour
    {
        public static DailyPuzzleManager Instance { get; private set; }

        public LevelData CurrentDailyLevel { get; private set; }
        public bool IsTodayCompleted { get; private set; }
        public int CurrentStreak => SaveManager.Instance.Data.dailyStreak;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            CheckTodayStatus();
        }

        public void CheckTodayStatus()
        {
            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            var data = SaveManager.Instance.Data;
            IsTodayCompleted = string.Equals(data.lastDailyCompletedDate, todayStr, StringComparison.OrdinalIgnoreCase);

            // Generate deterministic daily puzzle based on today's date
            int dayOfYear = DateTime.Now.DayOfYear;
            int levelIndex = (dayOfYear % 250) + 1;
            CurrentDailyLevel = LevelRepository.GetLevel(levelIndex);
        }

        public void MarkDailyCompleted()
        {
            if (IsTodayCompleted) return;

            string todayStr = DateTime.Now.ToString("yyyy-MM-dd");
            var data = SaveManager.Instance.Data;

            // Check streak
            DateTime yesterday = DateTime.Now.AddDays(-1);
            string yesterdayStr = yesterday.ToString("yyyy-MM-dd");

            if (string.Equals(data.lastDailyCompletedDate, yesterdayStr, StringComparison.OrdinalIgnoreCase))
            {
                data.dailyStreak++;
            }
            else
            {
                data.dailyStreak = 1;
            }

            data.lastDailyCompletedDate = todayStr;
            data.totalDailyPuzzlesSolved++;

            // Extra generous daily bonus: 50 coins + streak bonus
            int bonus = 50 + (data.dailyStreak * 5);
            EconomyManager.Instance.AddCoins(bonus);

            SaveManager.Instance.Save();
            IsTodayCompleted = true;
        }
    }
}
