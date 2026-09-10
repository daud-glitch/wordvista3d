using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordVista.Generator
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

    public class LevelSummaryDto
    {
        public int levelId { get; set; }
        public int worldId { get; set; }
        public string worldName { get; set; }
        public int wordCount { get; set; }
        public int letterCount { get; set; }
        public int coinReward { get; set; }
    }

    public class LevelManifestDto
    {
        public int totalLevels { get; set; }
        public List<LevelSummaryDto> levels { get; set; } = new List<LevelSummaryDto>();
    }

    public class PuzzleTemplate
    {
        public string RootLetters { get; set; }
        public List<string> Answers { get; set; }
        public List<string> Bonuses { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("Generating 250 Validated Crossword Adventure Levels");
            Console.WriteLine("==================================================");

            string outputDir = @"E:\wordsgame\WordVista3D\Assets\Resources\Levels";
            string manifestPath = @"E:\wordsgame\WordVista3D\Assets\Resources\LevelManifest.json";

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var puzzles = LevelCatalog.GetPuzzles();
            if (puzzles.Count != 250)
            {
                throw new InvalidOperationException($"Catalog contains {puzzles.Count} puzzles, expected 250!");
            }

            var manifest = new LevelManifestDto { totalLevels = 250 };
            int passCount = 0;
            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

            for (int i = 0; i < 250; i++)
            {
                int levelId = i + 1;
                var (worldId, worldName) = GetWorldInfo(levelId);
                var template = puzzles[i];

                var levelData = BuildCrosswordLevel(levelId, worldId, worldName, template);

                // Validate before saving
                var validationErrors = ValidateLevel(levelData);
                if (validationErrors.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR] Level {levelId} failed validation: {string.Join(", ", validationErrors)}");
                    Console.ResetColor();
                    throw new InvalidOperationException($"Validation failure at Level {levelId}");
                }

                passCount++;

                // Write Level JSON
                string fileName = $"Level_{levelId:D3}.json";
                string fullPath = Path.Combine(outputDir, fileName);
                string json = JsonSerializer.Serialize(levelData, jsonOptions);
                File.WriteAllText(fullPath, json);

                manifest.levels.Add(new LevelSummaryDto
                {
                    levelId = levelId,
                    worldId = worldId,
                    worldName = worldName,
                    wordCount = levelData.answerWords.Count,
                    letterCount = levelData.letters.Length,
                    coinReward = levelData.coinReward
                });
            }

            // Write Manifest
            string manifestJson = JsonSerializer.Serialize(manifest, jsonOptions);
            File.WriteAllText(manifestPath, manifestJson);

            GenerateSfxFiles();
            GenerateCalmMusicTracks();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS: All 250 levels generated and 100% validated!");
            Console.WriteLine($"Manifest written with {manifest.levels.Count} entries.");
            Console.ResetColor();
        }

        static void GenerateSfxFiles()
        {
            string dir = @"E:\wordsgame\WordVista3D\Assets\Audio\SFX";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            MakeWav(Path.Combine(dir, "button_click.wav"), 650, 0.08);
            MakeWav(Path.Combine(dir, "letter_select.wav"), 520, 0.09);
            MakeWav(Path.Combine(dir, "word_solved.wav"), 880, 0.35);
            MakeWav(Path.Combine(dir, "bonus_word.wav"), 1046, 0.4);
            MakeWav(Path.Combine(dir, "invalid_shake.wav"), 220, 0.2);
            MakeWav(Path.Combine(dir, "hint_purchased.wav"), 784, 0.3);
            MakeWav(Path.Combine(dir, "level_complete.wav"), 987, 0.6);

            Console.WriteLine("Procedural game SFX WAV clips created in " + dir);
        }

        static void MakeWav(string path, double freq, double duration, int sampleRate = 22050)
        {
            int samples = (int)(sampleRate * duration);
            byte[] bytes = new byte[44 + samples * 2];
            var enc = System.Text.Encoding.ASCII;

            Array.Copy(enc.GetBytes("RIFF"), 0, bytes, 0, 4);
            BitConverter.GetBytes(36 + samples * 2).CopyTo(bytes, 4);
            Array.Copy(enc.GetBytes("WAVE"), 0, bytes, 8, 4);
            Array.Copy(enc.GetBytes("fmt "), 0, bytes, 12, 4);
            BitConverter.GetBytes(16).CopyTo(bytes, 16);
            BitConverter.GetBytes((short)1).CopyTo(bytes, 20); // PCM
            BitConverter.GetBytes((short)1).CopyTo(bytes, 22); // Mono
            BitConverter.GetBytes(sampleRate).CopyTo(bytes, 24);
            BitConverter.GetBytes(sampleRate * 2).CopyTo(bytes, 28);
            BitConverter.GetBytes((short)2).CopyTo(bytes, 32);
            BitConverter.GetBytes((short)16).CopyTo(bytes, 34);
            Array.Copy(enc.GetBytes("data"), 0, bytes, 36, 4);
            BitConverter.GetBytes(samples * 2).CopyTo(bytes, 40);

            for (int i = 0; i < samples; i++)
            {
                double t = (double)i / sampleRate;
                double decay = Math.Exp(-3.0 * t / duration);
                short val = (short)(Math.Sin(2.0 * Math.PI * freq * t) * 16000 * decay);
                BitConverter.GetBytes(val).CopyTo(bytes, 44 + i * 2);
            }

            File.WriteAllBytes(path, bytes);
        }

        static void GenerateCalmMusicTracks()
        {
            string dir = @"E:\wordsgame\WordVista3D\Assets\Audio\Music";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            // Track 1: Peaceful Green Valley Melody (Cmaj - Gmaj - Am - Fmaj)
            SynthesizeAmbientLoop(Path.Combine(dir, "valley_calm_ambient.wav"), 12.0, new double[][]
            {
                new double[] { 261.63, 329.63, 392.00, 523.25 }, // C4, E4, G4, C5
                new double[] { 196.00, 246.94, 293.66, 392.00 }, // G3, B3, D4, G4
                new double[] { 220.00, 261.63, 329.63, 440.00 }, // A3, C4, E4, A4
                new double[] { 174.61, 220.00, 261.63, 349.23 }  // F3, A3, C4, F4
            });

            // Track 2: Mystic Forest Serenity (Em - Cmaj - Gmaj - Dmaj)
            SynthesizeAmbientLoop(Path.Combine(dir, "mystic_ambient_peace.wav"), 12.0, new double[][]
            {
                new double[] { 164.81, 246.94, 329.63, 392.00 }, // E3, B3, E4, G4
                new double[] { 130.81, 261.63, 329.63, 523.25 }, // C3, C4, E4, C5
                new double[] { 196.00, 293.66, 392.00, 493.88 }, // G3, D4, G4, B4
                new double[] { 146.83, 220.00, 293.66, 440.00 }  // D3, A3, D4, A4
            });

            // Track 3: Crystal Serenity Chimes (Dmaj - A/C# - Bm - Gmaj)
            SynthesizeAmbientLoop(Path.Combine(dir, "crystal_serenity.wav"), 12.0, new double[][]
            {
                new double[] { 293.66, 369.99, 440.00, 587.33 }, // D4, F#4, A4, D5
                new double[] { 277.18, 329.63, 440.00, 554.37 }, // C#4, E4, A4, C#5
                new double[] { 246.94, 293.66, 369.99, 493.88 }, // B3, D4, F#4, B4
                new double[] { 196.00, 293.66, 392.00, 587.33 }  // G3, D4, G4, D5
            });

            Console.WriteLine("Calming ambient background music tracks created in " + dir);
        }

        static void SynthesizeAmbientLoop(string path, double durationSeconds, double[][] chordProgression, int sampleRate = 22050)
        {
            int totalSamples = (int)(sampleRate * durationSeconds);
            byte[] bytes = new byte[44 + totalSamples * 2];
            var enc = System.Text.Encoding.ASCII;

            Array.Copy(enc.GetBytes("RIFF"), 0, bytes, 0, 4);
            BitConverter.GetBytes(36 + totalSamples * 2).CopyTo(bytes, 4);
            Array.Copy(enc.GetBytes("WAVE"), 0, bytes, 8, 4);
            Array.Copy(enc.GetBytes("fmt "), 0, bytes, 12, 4);
            BitConverter.GetBytes(16).CopyTo(bytes, 16);
            BitConverter.GetBytes((short)1).CopyTo(bytes, 20); // PCM
            BitConverter.GetBytes((short)1).CopyTo(bytes, 22); // Mono
            BitConverter.GetBytes(sampleRate).CopyTo(bytes, 24);
            BitConverter.GetBytes(sampleRate * 2).CopyTo(bytes, 28);
            BitConverter.GetBytes((short)2).CopyTo(bytes, 32);
            BitConverter.GetBytes((short)16).CopyTo(bytes, 34);
            Array.Copy(enc.GetBytes("data"), 0, bytes, 36, 4);
            BitConverter.GetBytes(totalSamples * 2).CopyTo(bytes, 40);

            int numChords = chordProgression.Length;
            double chordDuration = durationSeconds / numChords;

            for (int i = 0; i < totalSamples; i++)
            {
                double t = (double)i / sampleRate;
                int chordIdx = (int)(t / chordDuration) % numChords;
                double chordLocalTime = t - (chordIdx * chordDuration);

                // Smooth bell/ambient envelope per chord
                double env = Math.Sin(Math.PI * (chordLocalTime / chordDuration));
                env = Math.Pow(env, 0.6); // warm sustain

                double sampleSum = 0;
                var freqs = chordProgression[chordIdx];
                foreach (double f in freqs)
                {
                    // Fundamental
                    sampleSum += Math.Sin(2.0 * Math.PI * f * t) * 0.4;
                    // Warm harmonic
                    sampleSum += Math.Sin(4.0 * Math.PI * f * t) * 0.15;
                    // Sub-octave warmth
                    sampleSum += Math.Sin(1.0 * Math.PI * f * t) * 0.2;
                }

                sampleSum /= freqs.Length;
                short val = (short)(sampleSum * 14000 * env);
                BitConverter.GetBytes(val).CopyTo(bytes, 44 + i * 2);
            }

            File.WriteAllBytes(path, bytes);
        }

        static (int worldId, string worldName) GetWorldInfo(int levelId)
        {
            if (levelId <= 30) return (1, "Green Valley");
            if (levelId <= 60) return (2, "Mystic Forest");
            if (levelId <= 90) return (3, "Crystal Lake");
            if (levelId <= 120) return (4, "Desert Kingdom");
            if (levelId <= 150) return (5, "Snow Valley");
            if (levelId <= 180) return (6, "Tropical Island");
            if (levelId <= 215) return (7, "Sky Kingdom");
            return (8, "Aurora World");
        }

        static LevelDataDto BuildCrosswordLevel(int levelId, int worldId, string worldName, PuzzleTemplate template)
        {
            var answers = template.Answers;

            // Compute the minimal letter multiset needed to form every answer word
            var letterCounts = new Dictionary<char, int>();
            foreach (var word in answers)
            {
                var wordCounts = new Dictionary<char, int>();
                foreach (char c in word.ToUpperInvariant())
                {
                    wordCounts[c] = wordCounts.GetValueOrDefault(c, 0) + 1;
                }
                foreach (var kvp in wordCounts)
                {
                    if (!letterCounts.ContainsKey(kvp.Key) || letterCounts[kvp.Key] < kvp.Value)
                    {
                        letterCounts[kvp.Key] = kvp.Value;
                    }
                }
            }

            // Construct letter bank
            var bankList = new List<char>();
            foreach (var kvp in letterCounts)
            {
                for (int c = 0; c < kvp.Value; c++) bankList.Add(kvp.Key);
            }

            // If bank has fewer letters than template root, add remaining characters from template
            if (!string.IsNullOrEmpty(template.RootLetters))
            {
                var rootCounts = new Dictionary<char, int>();
                foreach (char c in template.RootLetters.ToUpperInvariant())
                {
                    rootCounts[c] = rootCounts.GetValueOrDefault(c, 0) + 1;
                }
                foreach (var kvp in rootCounts)
                {
                    int current = letterCounts.GetValueOrDefault(kvp.Key, 0);
                    if (kvp.Value > current)
                    {
                        for (int k = 0; k < kvp.Value - current; k++) bankList.Add(kvp.Key);
                    }
                }
            }

            // Ensure bank is at least 3 characters
            char[] padChars = new[] { 'E', 'A', 'R', 'S', 'T' };
            int padIdx = 0;
            while (bankList.Count < 3 && padIdx < padChars.Length)
            {
                bankList.Add(padChars[padIdx++]);
            }

            // If bank exceeds 7 characters, keep it within 7
            if (bankList.Count > 7)
            {
                bankList = bankList.GetRange(0, 7);
            }

            // Shuffle bank for interesting letter wheel presentation
            var rnd = new Random(levelId * 7919);
            for (int i = bankList.Count - 1; i > 0; i--)
            {
                int k = rnd.Next(i + 1);
                (bankList[i], bankList[k]) = (bankList[k], bankList[i]);
            }

            string letters = new string(bankList.ToArray());

            // Filter answer words to only those that can strictly be formed by the final bank
            var validAnswers = new List<string>();
            foreach (var w in answers)
            {
                if (CanForm(w, letters)) validAnswers.Add(w);
            }

            if (validAnswers.Count == 0)
            {
                // Fallback: use first answer word as root
                letters = answers[0].ToUpperInvariant();
                validAnswers.Add(letters);
            }

            var layout = CrosswordArranger.Arrange(validAnswers);

            // Filter bonuses that can be formed
            var validBonuses = new List<string>();
            if (template.Bonuses != null)
            {
                foreach (var b in template.Bonuses)
                {
                    if (CanForm(b, letters) && !validAnswers.Contains(b))
                    {
                        validBonuses.Add(b.ToUpperInvariant());
                    }
                }
            }

            int difficulty = 1;
            if (levelId > 20) difficulty = 2;
            if (levelId > 70) difficulty = 3;
            if (levelId > 150) difficulty = 4;
            if (levelId > 220) difficulty = 5;

            int coinReward = 15 + (difficulty * 5); // 20 to 40 coins

            return new LevelDataDto
            {
                levelId = levelId,
                worldId = worldId,
                worldName = worldName,
                letters = letters,
                answerWords = layout.Placements,
                bonusWords = validBonuses,
                coinReward = coinReward,
                difficulty = difficulty,
                gridWidth = layout.Width,
                gridHeight = layout.Height
            };
        }

        static List<string> ValidateLevel(LevelDataDto level)
        {
            var errors = new List<string>();

            if (level.levelId <= 0) errors.Add("Invalid levelId");
            if (string.IsNullOrEmpty(level.letters) || level.letters.Length < 3 || level.letters.Length > 7)
                errors.Add($"Invalid letters count ({level.letters?.Length})");
            if (level.answerWords == null || level.answerWords.Count == 0)
                errors.Add("No answer words");

            // Check word formation from letters
            foreach (var wp in level.answerWords)
            {
                if (!CanForm(wp.word, level.letters))
                {
                    errors.Add($"Word '{wp.word}' cannot be formed from '{level.letters}'");
                }
            }

            // Check intersections
            var grid = new Dictionary<(int x, int y), (char letter, string word)>();
            foreach (var wp in level.answerWords)
            {
                for (int i = 0; i < wp.word.Length; i++)
                {
                    int x = wp.isHorizontal ? wp.startX + i : wp.startX;
                    int y = wp.isHorizontal ? wp.startY : wp.startY + i;
                    char letter = wp.word[i];

                    if (grid.TryGetValue((x, y), out var existing))
                    {
                        if (existing.letter != letter)
                        {
                            errors.Add($"Conflict at ({x},{y}): '{existing.letter}' in '{existing.word}' vs '{letter}' in '{wp.word}'");
                        }
                    }
                    else
                    {
                        grid[(x, y)] = (letter, wp.word);
                    }
                }
            }

            return errors;
        }

        static bool CanForm(string word, string bank)
        {
            var counts = new Dictionary<char, int>();
            foreach (char c in bank)
            {
                counts[c] = counts.GetValueOrDefault(c, 0) + 1;
            }
            foreach (char c in word)
            {
                if (!counts.TryGetValue(c, out int count) || count <= 0) return false;
                counts[c]--;
            }
            return true;
        }
    }

    public class CrosswordLayoutResult
    {
        public List<WordPlacementDto> Placements { get; set; } = new List<WordPlacementDto>();
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static class CrosswordArranger
    {
        public static CrosswordLayoutResult Arrange(List<string> words)
        {
            // Place first word horizontally at origin
            var placements = new List<WordPlacementDto>();
            var grid = new Dictionary<(int x, int y), char>();

            string first = words[0].ToUpperInvariant();
            placements.Add(new WordPlacementDto { word = first, startX = 0, startY = 0, isHorizontal = true });
            for (int i = 0; i < first.Length; i++) grid[(i, 0)] = first[i];

            for (int w = 1; w < words.Count; w++)
            {
                string word = words[w].ToUpperInvariant();
                bool placed = false;

                // Try to find a valid intersection with already placed words
                for (int pIdx = 0; pIdx < placements.Count && !placed; pIdx++)
                {
                    var existing = placements[pIdx];
                    bool targetOrientation = !existing.isHorizontal;

                    for (int i = 0; i < existing.word.Length && !placed; i++)
                    {
                        char matchChar = existing.word[i];
                        int matchX = existing.isHorizontal ? existing.startX + i : existing.startX;
                        int matchY = existing.isHorizontal ? existing.startY : existing.startY + i;

                        for (int j = 0; j < word.Length && !placed; j++)
                        {
                            if (word[j] == matchChar)
                            {
                                int testStartX = targetOrientation ? matchX : matchX - j;
                                int testStartY = targetOrientation ? matchY - j : matchY;

                                if (CanPlace(word, testStartX, testStartY, targetOrientation, grid))
                                {
                                    PlaceWord(word, testStartX, testStartY, targetOrientation, grid);
                                    placements.Add(new WordPlacementDto
                                    {
                                        word = word,
                                        startX = testStartX,
                                        startY = testStartY,
                                        isHorizontal = targetOrientation
                                    });
                                    placed = true;
                                }
                            }
                        }
                    }
                }

                if (!placed)
                {
                    // If no direct intersection, place parallel in clean offset row
                    int maxY = 0;
                    foreach (var k in grid.Keys) if (k.y > maxY) maxY = k.y;
                    int startY = maxY + 2;
                    placements.Add(new WordPlacementDto { word = word, startX = 0, startY = startY, isHorizontal = true });
                    PlaceWord(word, 0, startY, true, grid);
                }
            }

            // Normalize coordinates so minX = 0, minY = 0
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxMaxY = int.MinValue;
            foreach (var k in grid.Keys)
            {
                if (k.x < minX) minX = k.x;
                if (k.y < minY) minY = k.y;
                if (k.x > maxX) maxX = k.x;
                if (k.y > maxMaxY) maxMaxY = k.y;
            }

            foreach (var p in placements)
            {
                p.startX -= minX;
                p.startY -= minY;
            }

            return new CrosswordLayoutResult
            {
                Placements = placements,
                Width = (maxX - minX) + 1,
                Height = (maxMaxY - minY) + 1
            };
        }

        private static bool CanPlace(string word, int startX, int startY, bool isHorizontal, Dictionary<(int x, int y), char> grid)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int x = isHorizontal ? startX + i : startX;
                int y = isHorizontal ? startY : startY + i;

                if (grid.TryGetValue((x, y), out char existing))
                {
                    if (existing != word[i]) return false;
                }
                else
                {
                    // Check adjacent cells so words do not inappropriately collide or touch parallel
                    if (isHorizontal)
                    {
                        if (grid.ContainsKey((x, y - 1)) && !IsIntersection(x, y - 1, grid, word[i])) return false;
                        if (grid.ContainsKey((x, y + 1)) && !IsIntersection(x, y + 1, grid, word[i])) return false;
                    }
                    else
                    {
                        if (grid.ContainsKey((x - 1, y)) && !IsIntersection(x - 1, y, grid, word[i])) return false;
                        if (grid.ContainsKey((x + 1, y)) && !IsIntersection(x + 1, y, grid, word[i])) return false;
                    }
                }
            }

            // Check boundaries right before start and right after end
            int beforeX = isHorizontal ? startX - 1 : startX;
            int beforeY = isHorizontal ? startY : startY - 1;
            int afterX = isHorizontal ? startX + word.Length : startX;
            int afterY = isHorizontal ? startY : startY + word.Length;

            if (grid.ContainsKey((beforeX, beforeY))) return false;
            if (grid.ContainsKey((afterX, afterY))) return false;

            return true;
        }

        private static bool IsIntersection(int x, int y, Dictionary<(int x, int y), char> grid, char c)
        {
            return false;
        }

        private static void PlaceWord(string word, int startX, int startY, bool isHorizontal, Dictionary<(int x, int y), char> grid)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int x = isHorizontal ? startX + i : startX;
                int y = isHorizontal ? startY : startY + i;
                grid[(x, y)] = word[i];
            }
        }
    }
}
