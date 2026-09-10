using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WordVista.Levels
{
    public static class LevelRepository
    {
        private static readonly Dictionary<int, LevelData> _cachedLevels = new Dictionary<int, LevelData>();
        private static LevelManifest _manifest;

        public static LevelData GetLevel(int levelId)
        {
            if (_cachedLevels.TryGetValue(levelId, out var cached))
            {
                return cached;
            }

            // Attempt to load from Unity Resources
            string resourcePath = $"Levels/Level_{levelId:D3}";
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
            {
                var level = JsonUtility.FromJson<LevelData>(textAsset.text);
                if (level != null)
                {
                    _cachedLevels[levelId] = level;
                    return level;
                }
            }

            // Fallback for file system / runtime outside Resources
            string diskPath = Path.Combine(Application.dataPath, "Resources", "Levels", $"Level_{levelId:D3}.json");
            if (File.Exists(diskPath))
            {
                string json = File.ReadAllText(diskPath);
                var level = JsonUtility.FromJson<LevelData>(json);
                if (level != null)
                {
                    _cachedLevels[levelId] = level;
                    return level;
                }
            }

            return null;
        }

        public static void CacheLevel(LevelData level)
        {
            if (level != null && level.levelId > 0)
            {
                _cachedLevels[level.levelId] = level;
            }
        }

        public static LevelManifest GetManifest()
        {
            if (_manifest != null) return _manifest;

            var textAsset = Resources.Load<TextAsset>("LevelManifest");
            if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
            {
                _manifest = JsonUtility.FromJson<LevelManifest>(textAsset.text);
            }
            return _manifest;
        }

        public static void ClearCache()
        {
            _cachedLevels.Clear();
            _manifest = null;
        }
    }
}
