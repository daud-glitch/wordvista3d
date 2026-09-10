using System;
using System.Collections.Generic;

namespace WordVista.Levels
{
    [Serializable]
    public class WordPlacement
    {
        public string word;
        public int startX; // 0-indexed column
        public int startY; // 0-indexed row
        public bool isHorizontal; // true = horizontal (left to right), false = vertical (top to bottom)
        public string clue = "";

        public WordPlacement() { }

        public WordPlacement(string word, int startX, int startY, bool isHorizontal, string clue = "")
        {
            this.word = word.ToUpperInvariant();
            this.startX = startX;
            this.startY = startY;
            this.isHorizontal = isHorizontal;
            this.clue = clue;
        }

        public int Length => string.IsNullOrEmpty(word) ? 0 : word.Length;
    }

    [Serializable]
    public class LevelData
    {
        public int levelId;
        public int worldId;
        public string worldName = "";
        public string letters = ""; // All available letters (e.g. "RIVER", "STONE")
        public List<WordPlacement> answerWords = new List<WordPlacement>();
        public List<string> bonusWords = new List<string>();
        public int coinReward = 20;
        public int difficulty = 1; // 1 to 5
        public int gridWidth = 8;
        public int gridHeight = 8;

        public LevelData() { }

        public LevelData(int levelId, int worldId, string worldName, string letters, int coinReward = 20, int difficulty = 1)
        {
            this.levelId = levelId;
            this.worldId = worldId;
            this.worldName = worldName;
            this.letters = letters.ToUpperInvariant();
            this.coinReward = coinReward;
            this.difficulty = difficulty;
            this.answerWords = new List<WordPlacement>();
            this.bonusWords = new List<string>();
        }
    }

    [Serializable]
    public class LevelManifest
    {
        public int totalLevels;
        public List<LevelSummary> levels = new List<LevelSummary>();
    }

    [Serializable]
    public class LevelSummary
    {
        public int levelId;
        public int worldId;
        public string worldName;
        public int wordCount;
        public int letterCount;
        public int coinReward;
    }
}
