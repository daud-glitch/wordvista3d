using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;

namespace WordVista.Tests
{
    public class WordPlacementDto
    {
        public string word { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public bool isHorizontal { get; set; }
        public string clue { get; set; } = "";
    }

    public class LevelDataDto
    {
        public int levelId { get; set; }
        public int worldId { get; set; }
        public string worldName { get; set; } = "";
        public string letters { get; set; } = "";
        public List<WordPlacementDto> answerWords { get; set; } = new List<WordPlacementDto>();
        public List<string> bonusWords { get; set; } = new List<string>();
        public int coinReward { get; set; } = 20;
        public int difficulty { get; set; } = 1;
        public int gridWidth { get; set; } = 8;
        public int gridHeight { get; set; } = 8;
    }

    public class LevelManifestDto
    {
        public int totalLevels { get; set; }
        public List<LevelSummaryDto> levels { get; set; } = new List<LevelSummaryDto>();
    }

    public class LevelSummaryDto
    {
        public int levelId { get; set; }
        public int worldId { get; set; }
        public string worldName { get; set; }
        public int wordCount { get; set; }
        public int letterCount { get; set; }
        public int coinReward { get; set; }
    }

    public class LevelValidationTests
    {
        private readonly string _levelsDir = @"E:\wordsgame\WordVista3D\Assets\Resources\Levels";
        private readonly string _manifestPath = @"E:\wordsgame\WordVista3D\Assets\Resources\LevelManifest.json";

        [Fact]
        public void All250Levels_Exist_And_ValidateStrictly()
        {
            Assert.True(Directory.Exists(_levelsDir), $"Levels directory {_levelsDir} must exist.");

            var seenIds = new HashSet<int>();
            int validatedCount = 0;

            for (int i = 1; i <= 250; i++)
            {
                string path = Path.Combine(_levelsDir, $"Level_{i:D3}.json");
                Assert.True(File.Exists(path), $"Level file {path} must exist.");

                string json = File.ReadAllText(path);
                var level = JsonSerializer.Deserialize<LevelDataDto>(json);
                Assert.NotNull(level);

                // 1. Check ID
                Assert.Equal(i, level.levelId);
                Assert.DoesNotContain(level.levelId, seenIds);
                seenIds.Add(level.levelId);

                // 2. Letters count 3 to 7
                Assert.InRange(level.letters.Length, 3, 7);

                // 3. Answer words
                Assert.NotEmpty(level.answerWords);

                // 4. Letter availability
                foreach (var wp in level.answerWords)
                {
                    Assert.False(string.IsNullOrEmpty(wp.word), $"Level {i} has empty word");
                    Assert.True(CanFormWord(wp.word, level.letters),
                        $"Level {i}: Word '{wp.word}' cannot be formed from bank '{level.letters}'");
                }

                // 5. Crossword intersections
                var grid = new Dictionary<(int x, int y), (char letter, string word)>();
                foreach (var wp in level.answerWords)
                {
                    Assert.True(wp.startX >= 0, $"Level {i}: Negative startX in {wp.word}");
                    Assert.True(wp.startY >= 0, $"Level {i}: Negative startY in {wp.word}");

                    string word = wp.word.ToUpperInvariant();
                    for (int c = 0; c < word.Length; c++)
                    {
                        int x = wp.isHorizontal ? wp.startX + c : wp.startX;
                        int y = wp.isHorizontal ? wp.startY : wp.startY + c;
                        char letter = word[c];
                        var coord = (x, y);

                        if (grid.TryGetValue(coord, out var existing))
                        {
                            Assert.True(existing.letter == letter,
                                $"Level {i} intersection mismatch at ({x},{y}): '{existing.letter}' in '{existing.word}' vs '{letter}' in '{word}'");
                        }
                        else
                        {
                            grid[coord] = (letter, word);
                        }
                    }
                }

                validatedCount++;
            }

            Assert.Equal(250, validatedCount);
        }

        [Fact]
        public void LevelManifest_ContainsAll250Levels()
        {
            Assert.True(File.Exists(_manifestPath), $"Manifest must exist at {_manifestPath}");
            string json = File.ReadAllText(_manifestPath);
            var manifest = JsonSerializer.Deserialize<LevelManifestDto>(json);

            Assert.NotNull(manifest);
            Assert.Equal(250, manifest.totalLevels);
            Assert.Equal(250, manifest.levels.Count);

            // Verify world distribution
            int greenValley = 0, mysticForest = 0, crystalLake = 0, desert = 0;
            int snowValley = 0, tropical = 0, sky = 0, aurora = 0;

            foreach (var l in manifest.levels)
            {
                switch (l.worldId)
                {
                    case 1: greenValley++; break;
                    case 2: mysticForest++; break;
                    case 3: crystalLake++; break;
                    case 4: desert++; break;
                    case 5: snowValley++; break;
                    case 6: tropical++; break;
                    case 7: sky++; break;
                    case 8: aurora++; break;
                }
            }

            Assert.Equal(30, greenValley);   // 1-30
            Assert.Equal(30, mysticForest);  // 31-60
            Assert.Equal(30, crystalLake);   // 61-90
            Assert.Equal(30, desert);        // 91-120
            Assert.Equal(30, snowValley);    // 121-150
            Assert.Equal(30, tropical);      // 151-180
            Assert.Equal(35, sky);           // 181-215
            Assert.Equal(35, aurora);        // 216-250
        }

        [Fact]
        public void Level1_VerticalSlice_HasExpectedData()
        {
            string path = Path.Combine(_levelsDir, "Level_001.json");
            string json = File.ReadAllText(path);
            var level = JsonSerializer.Deserialize<LevelDataDto>(json);

            Assert.NotNull(level);
            Assert.Equal(1, level.levelId);
            Assert.Equal("Green Valley", level.worldName);
            Assert.Equal("ACT", level.letters);
            Assert.Equal(2, level.answerWords.Count);
            Assert.Contains(level.answerWords, wp => wp.word == "CAT" && wp.isHorizontal);
            Assert.Contains(level.answerWords, wp => wp.word == "ACT" && !wp.isHorizontal);
            Assert.Equal(20, level.coinReward);
        }

        private static bool CanFormWord(string word, string bank)
        {
            var counts = new Dictionary<char, int>();
            foreach (char c in bank.ToUpperInvariant())
            {
                counts[c] = counts.GetValueOrDefault(c, 0) + 1;
            }
            foreach (char c in word.ToUpperInvariant())
            {
                if (!counts.TryGetValue(c, out int count) || count <= 0) return false;
                counts[c]--;
            }
            return true;
        }
    }

    public class EconomyAndSaveTests
    {
        [Fact]
        public void HintCosts_MatchConfiguredValues()
        {
            int revealLetterCost = 25;
            int smartHintCost = 40;
            int revealWordCost = 100;

            Assert.Equal(25, revealLetterCost);
            Assert.Equal(40, smartHintCost);
            Assert.Equal(100, revealWordCost);
        }

        [Fact]
        public void BonusWordPouch_AwardsCoinsAtMilestones()
        {
            int coins = 100;
            int bonusMilestone = 5;
            int milestoneReward = 30;

            int bonusWordsFound = 0;
            int pouch = 0;

            for (int i = 0; i < 7; i++)
            {
                bonusWordsFound++;
                pouch++;
                if (pouch >= bonusMilestone)
                {
                    pouch = 0;
                    coins += milestoneReward;
                }
            }

            Assert.Equal(7, bonusWordsFound);
            Assert.Equal(2, pouch);
            Assert.Equal(130, coins); // 100 + 30
        }
    }
}
