using System;
using System.Collections.Generic;
using WordVista.Levels;

namespace WordVista.Crossword
{
    public enum CellState
    {
        Empty,        // No letter here
        Unsolved,     // Hidden crossword tile
        Highlighted,  // Actively swiped/matched preview
        Solved,       // Word successfully guessed
        HintRevealed  // Revealed through hint
    }

    public class CrosswordCell
    {
        public int X { get; }
        public int Y { get; }
        public char Letter { get; set; }
        public CellState State { get; set; }
        public List<int> WordIndices { get; } = new List<int>();

        public CrosswordCell(int x, int y, char letter)
        {
            X = x;
            Y = y;
            Letter = char.ToUpperInvariant(letter);
            State = CellState.Unsolved;
        }

        public bool IsRevealed => State == CellState.Solved || State == CellState.HintRevealed;
    }

    public class CrosswordGridModel
    {
        private readonly Dictionary<(int x, int y), CrosswordCell> _cells = new Dictionary<(int x, int y), CrosswordCell>();
        private readonly List<WordPlacement> _words = new List<WordPlacement>();
        private readonly HashSet<string> _solvedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TotalCellCount => _cells.Count;
        public int SolvedWordCount => _solvedWords.Count;
        public int TotalWordCount => _words.Count;
        public bool IsComplete => _words.Count > 0 && _solvedWords.Count == _words.Count;

        public IReadOnlyDictionary<(int x, int y), CrosswordCell> Cells => _cells;
        public IReadOnlyList<WordPlacement> Words => _words;
        public IReadOnlyCollection<string> SolvedWords => _solvedWords;

        public void LoadLevel(LevelData level)
        {
            _cells.Clear();
            _words.Clear();
            _solvedWords.Clear();

            if (level == null || level.answerWords == null) return;

            int maxX = 0;
            int maxY = 0;

            for (int wIdx = 0; wIdx < level.answerWords.Count; wIdx++)
            {
                var wp = level.answerWords[wIdx];
                _words.Add(wp);
                string word = wp.word.ToUpperInvariant();

                for (int i = 0; i < word.Length; i++)
                {
                    int cx = wp.isHorizontal ? wp.startX + i : wp.startX;
                    int cy = wp.isHorizontal ? wp.startY : wp.startY + i;

                    if (cx > maxX) maxX = cx;
                    if (cy > maxY) maxY = cy;

                    var key = (cx, cy);
                    if (!_cells.TryGetValue(key, out var cell))
                    {
                        cell = new CrosswordCell(cx, cy, word[i]);
                        _cells[key] = cell;
                    }
                    else
                    {
                        // Validate intersection character consistency
                        if (cell.Letter != word[i])
                        {
                            throw new InvalidOperationException(
                                $"Crossword conflict at ({cx}, {cy}): '{cell.Letter}' vs '{word[i]}' in word '{word}'");
                        }
                    }

                    if (!cell.WordIndices.Contains(wIdx))
                    {
                        cell.WordIndices.Add(wIdx);
                    }
                }
            }

            Width = maxX + 1;
            Height = maxY + 1;
        }

        public bool TrySolveWord(string guess, out WordPlacement matchedPlacement)
        {
            matchedPlacement = null;
            if (string.IsNullOrEmpty(guess)) return false;

            string normalized = guess.Trim().ToUpperInvariant();
            if (_solvedWords.Contains(normalized)) return false; // Already solved

            for (int i = 0; i < _words.Count; i++)
            {
                if (string.Equals(_words[i].word, normalized, StringComparison.OrdinalIgnoreCase))
                {
                    matchedPlacement = _words[i];
                    _solvedWords.Add(normalized);

                    // Reveal cells for this word
                    for (int c = 0; c < normalized.Length; c++)
                    {
                        int cx = matchedPlacement.isHorizontal ? matchedPlacement.startX + c : matchedPlacement.startX;
                        int cy = matchedPlacement.isHorizontal ? matchedPlacement.startY : matchedPlacement.startY + c;

                        if (_cells.TryGetValue((cx, cy), out var cell))
                        {
                            cell.State = CellState.Solved;
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        public bool RevealRandomLetter(out CrosswordCell revealedCell)
        {
            revealedCell = null;
            var unsolved = new List<CrosswordCell>();
            foreach (var cell in _cells.Values)
            {
                if (!cell.IsRevealed) unsolved.Add(cell);
            }

            if (unsolved.Count == 0) return false;

            var rnd = new Random();
            revealedCell = unsolved[rnd.Next(unsolved.Count)];
            revealedCell.State = CellState.HintRevealed;
            CheckAutoWordCompletions();
            return true;
        }

        public bool RevealSmartLetter(out CrosswordCell revealedCell)
        {
            revealedCell = null;
            CrosswordCell bestCell = null;
            int maxUnsolvedIntersections = -1;

            foreach (var cell in _cells.Values)
            {
                if (cell.IsRevealed) continue;

                // Priority to intersection cells connecting multiple unsolved words
                int intersectionWeight = cell.WordIndices.Count;
                if (intersectionWeight > maxUnsolvedIntersections)
                {
                    maxUnsolvedIntersections = intersectionWeight;
                    bestCell = cell;
                }
            }

            if (bestCell == null) return false;

            bestCell.State = CellState.HintRevealed;
            revealedCell = bestCell;
            CheckAutoWordCompletions();
            return true;
        }

        public bool RevealEntireWord(out WordPlacement revealedWord)
        {
            revealedWord = null;
            foreach (var wp in _words)
            {
                if (!_solvedWords.Contains(wp.word))
                {
                    TrySolveWord(wp.word, out revealedWord);
                    return true;
                }
            }
            return false;
        }

        private void CheckAutoWordCompletions()
        {
            foreach (var wp in _words)
            {
                if (_solvedWords.Contains(wp.word)) continue;

                bool allRevealed = true;
                for (int i = 0; i < wp.word.Length; i++)
                {
                    int cx = wp.isHorizontal ? wp.startX + i : wp.startX;
                    int cy = wp.isHorizontal ? wp.startY : wp.startY + i;
                    if (_cells.TryGetValue((cx, cy), out var cell) && !cell.IsRevealed)
                    {
                        allRevealed = false;
                        break;
                    }
                }

                if (allRevealed)
                {
                    _solvedWords.Add(wp.word);
                }
            }
        }
    }
}
