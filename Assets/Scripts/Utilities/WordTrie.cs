using System;
using System.Collections.Generic;

namespace WordVista.Utilities
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();
        public bool IsEndOfWord;
    }

    public class WordTrie
    {
        private readonly TrieNode _root = new TrieNode();
        public int Count { get; private set; }

        public void Insert(string word)
        {
            if (string.IsNullOrEmpty(word)) return;
            string normalized = word.Trim().ToUpperInvariant();
            var current = _root;

            foreach (char c in normalized)
            {
                if (!current.Children.TryGetValue(c, out var child))
                {
                    child = new TrieNode();
                    current.Children[c] = child;
                }
                current = child;
            }

            if (!current.IsEndOfWord)
            {
                current.IsEndOfWord = true;
                Count++;
            }
        }

        public bool Contains(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;
            string normalized = word.Trim().ToUpperInvariant();
            var current = _root;

            foreach (char c in normalized)
            {
                if (!current.Children.TryGetValue(c, out var child))
                {
                    return false;
                }
                current = child;
            }

            return current.IsEndOfWord;
        }

        public bool StartsWith(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) return false;
            string normalized = prefix.Trim().ToUpperInvariant();
            var current = _root;

            foreach (char c in normalized)
            {
                if (!current.Children.TryGetValue(c, out var child))
                {
                    return false;
                }
                current = child;
            }

            return true;
        }

        public static bool CanFormWord(string word, string availableLetters)
        {
            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(availableLetters)) return false;
            if (word.Length > availableLetters.Length) return false;

            var letterCounts = new Dictionary<char, int>();
            foreach (char c in availableLetters.ToUpperInvariant())
            {
                if (!letterCounts.ContainsKey(c)) letterCounts[c] = 0;
                letterCounts[c]++;
            }

            foreach (char c in word.ToUpperInvariant())
            {
                if (!letterCounts.TryGetValue(c, out int count) || count <= 0)
                {
                    return false;
                }
                letterCounts[c]--;
            }

            return true;
        }
    }
}
