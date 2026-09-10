using System;
using System.Collections.Generic;
using WordVista.Utilities;

namespace WordVista.Levels
{
    public class LevelValidationResult
    {
        public int LevelId { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; } = new List<string>();

        public override string ToString()
        {
            return IsValid
                ? $"Level {LevelId}: PASS"
                : $"Level {LevelId}: FAIL ({string.Join(", ", Errors)})";
        }
    }

    public static class LevelValidator
    {
        public static LevelValidationResult ValidateLevel(LevelData level, HashSet<int> seenLevelIds = null)
        {
            var result = new LevelValidationResult { LevelId = level?.levelId ?? -1, IsValid = true };

            if (level == null)
            {
                result.IsValid = false;
                result.Errors.Add("LevelData is null");
                return result;
            }

            // 1. Level ID checks
            if (level.levelId <= 0)
            {
                result.IsValid = false;
                result.Errors.Add($"Invalid levelId: {level.levelId}");
            }

            if (seenLevelIds != null)
            {
                if (seenLevelIds.Contains(level.levelId))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Duplicate levelId detected: {level.levelId}");
                }
                else
                {
                    seenLevelIds.Add(level.levelId);
                }
            }

            // 2. Letters check
            if (string.IsNullOrEmpty(level.letters) || level.letters.Length < 3 || level.letters.Length > 7)
            {
                result.IsValid = false;
                result.Errors.Add($"Invalid letters '{level.letters}' (must be 3 to 7 letters)");
            }

            // 3. Answer words existence
            if (level.answerWords == null || level.answerWords.Count == 0)
            {
                result.IsValid = false;
                result.Errors.Add("No answer words in level");
                return result;
            }

            // 4. Word formation & physical letter availability
            foreach (var wp in level.answerWords)
            {
                if (string.IsNullOrEmpty(wp.word))
                {
                    result.IsValid = false;
                    result.Errors.Add("Contains empty word placement");
                    continue;
                }

                if (!WordTrie.CanFormWord(wp.word, level.letters))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Word '{wp.word}' cannot be formed from available letters '{level.letters}'");
                }
            }

            // 5. Crossword grid intersections and coordinate bounds
            var grid = new Dictionary<(int x, int y), (char letter, string word)>();

            foreach (var wp in level.answerWords)
            {
                if (wp.startX < 0 || wp.startY < 0)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Negative coordinates for word '{wp.word}': ({wp.startX}, {wp.startY})");
                }

                string word = wp.word.ToUpperInvariant();
                for (int i = 0; i < word.Length; i++)
                {
                    int cx = wp.isHorizontal ? wp.startX + i : wp.startX;
                    int cy = wp.isHorizontal ? wp.startY : wp.startY + i;
                    char letter = word[i];
                    var coord = (cx, cy);

                    if (grid.TryGetValue(coord, out var existing))
                    {
                        if (existing.letter != letter)
                        {
                            result.IsValid = false;
                            result.Errors.Add($"Intersection mismatch at ({cx}, {cy}): '{existing.letter}' in '{existing.word}' vs '{letter}' in '{word}'");
                        }
                    }
                    else
                    {
                        grid[coord] = (letter, word);
                    }
                }
            }

            return result;
        }

        public static List<LevelValidationResult> ValidateAllLevels(IEnumerable<LevelData> levels)
        {
            var results = new List<LevelValidationResult>();
            var seenIds = new HashSet<int>();

            foreach (var level in levels)
            {
                results.Add(ValidateLevel(level, seenIds));
            }

            return results;
        }
    }
}
