using System;
using System.Collections.Generic;
using WordVista.Levels;
using WordVista.Utilities;
using WordVista.Economy;

namespace WordVista.Game
{
    public enum WordValidationResult
    {
        MainAnswer,
        AlreadyFoundMain,
        BonusWord,
        AlreadyFoundBonus,
        Invalid
    }

    public class WordValidator
    {
        private LevelData _currentLevel;
        private readonly WordTrie _dictionary;
        private readonly HashSet<string> _mainAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _solvedMainWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _foundBonusWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _predefinedBonusWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<string> SolvedMainWords => _solvedMainWords;
        public IReadOnlyCollection<string> FoundBonusWords => _foundBonusWords;

        public WordValidator(WordTrie dictionary)
        {
            _dictionary = dictionary ?? new WordTrie();
        }

        public void SetLevel(LevelData level)
        {
            _currentLevel = level;
            _mainAnswers.Clear();
            _solvedMainWords.Clear();
            _foundBonusWords.Clear();
            _predefinedBonusWords.Clear();

            if (level == null) return;

            foreach (var wp in level.answerWords)
            {
                if (!string.IsNullOrEmpty(wp.word))
                {
                    _mainAnswers.Add(wp.word.Trim().ToUpperInvariant());
                }
            }

            if (level.bonusWords != null)
            {
                foreach (var b in level.bonusWords)
                {
                    if (!string.IsNullOrEmpty(b))
                    {
                        _predefinedBonusWords.Add(b.Trim().ToUpperInvariant());
                    }
                }
            }
        }

        public WordValidationResult ValidateWord(string guess, out WordPlacement matchedPlacement)
        {
            matchedPlacement = null;
            if (string.IsNullOrEmpty(guess) || _currentLevel == null) return WordValidationResult.Invalid;

            string normalized = guess.Trim().ToUpperInvariant();

            // 1. Check physical letter availability from the current level
            if (!WordTrie.CanFormWord(normalized, _currentLevel.letters))
            {
                return WordValidationResult.Invalid;
            }

            // 2. Check if it is a main crossword answer
            if (_mainAnswers.Contains(normalized))
            {
                if (_solvedMainWords.Contains(normalized))
                {
                    return WordValidationResult.AlreadyFoundMain;
                }

                _solvedMainWords.Add(normalized);
                matchedPlacement = _currentLevel.answerWords.Find(wp =>
                    string.Equals(wp.word, normalized, StringComparison.OrdinalIgnoreCase));
                return WordValidationResult.MainAnswer;
            }

            // 3. Check if it is a bonus word (either predefined or valid in English dictionary)
            bool isBonus = _predefinedBonusWords.Contains(normalized) || _dictionary.Contains(normalized);

            if (isBonus && normalized.Length >= 3)
            {
                if (_foundBonusWords.Contains(normalized))
                {
                    return WordValidationResult.AlreadyFoundBonus;
                }

                _foundBonusWords.Add(normalized);
                EconomyManager.Instance.RecordBonusWord();
                return WordValidationResult.BonusWord;
            }

            return WordValidationResult.Invalid;
        }

        public bool IsLevelComplete()
        {
            return _mainAnswers.Count > 0 && _solvedMainWords.Count == _mainAnswers.Count;
        }
    }
}
