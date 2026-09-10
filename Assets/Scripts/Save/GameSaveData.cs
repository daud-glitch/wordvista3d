using System;
using System.Collections.Generic;

namespace WordVista.Save
{
    [Serializable]
    public class WorldProgress
    {
        public int worldId;
        public int highestLevelCompleted;
        public int unlockedStage; // 0 to 5 transformation stage
        public bool isCompleted;
    }

    [Serializable]
    public class GameSaveData
    {
        public int saveVersion = 1;
        public string lastSaveTimestamp = "";

        // Campaign Progression
        public int currentLevelIndex = 1;      // 1 to 250
        public int highestUnlockedLevel = 1;  // 1 to 250
        public List<int> completedLevelIds = new List<int>();
        public List<WorldProgress> worldProgressList = new List<WorldProgress>();

        // Economy
        public int coins = 150; // Starting gift
        public int totalCoinsEarned = 150;
        public int totalCoinsSpent = 0;

        // Bonus Words
        public int totalBonusWordsFound = 0;
        public int bonusWordPouchProgress = 0; // towards next milestone (e.g. 5)

        // Settings
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool hapticsEnabled = true;
        public float musicVolume = 0.8f;
        public float sfxVolume = 1.0f;
        public int qualityLevel = 1; // 0=Fast, 1=Balanced, 2=High

        // Daily Puzzle
        public string lastDailyCompletedDate = "";
        public int dailyStreak = 0;
        public int totalDailyPuzzlesSolved = 0;

        // Achievements
        public List<string> unlockedAchievementIds = new List<string>();

        // Statistics
        public int totalWordsFound = 0;
        public int totalHintsUsed = 0;
        public int perfectLevelsWithoutHints = 0;
    }
}
