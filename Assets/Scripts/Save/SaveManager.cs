using System;
using System.IO;
using UnityEngine;

namespace WordVista.Save
{
    public class SaveManager
    {
        private static SaveManager _instance;
        public static SaveManager Instance => _instance ??= new SaveManager();

        public GameSaveData Data { get; private set; }

        private readonly string _saveFilePath;
        private readonly object _lock = new object();

        public event Action<GameSaveData> OnSaveLoaded;
        public event Action<GameSaveData> OnDataSaved;

        public SaveManager(string customPath = null)
        {
            if (!string.IsNullOrEmpty(customPath))
            {
                _saveFilePath = customPath;
            }
            else
            {
                string dir = Application.persistentDataPath;
                if (string.IsNullOrEmpty(dir))
                {
                    dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SavedData");
                }
                _saveFilePath = Path.Combine(dir, "wordvista_save.json");
            }

            Load();
        }

        public void Load()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_saveFilePath))
                    {
                        string json = File.ReadAllText(_saveFilePath);
                        Data = JsonUtility.FromJson<GameSaveData>(json);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SaveManager] Failed to load save file: {ex.Message}. Creating default save.");
                }

                if (Data == null)
                {
                    Data = new GameSaveData();
                    Save();
                }

                EnsureWorldProgressList();
                OnSaveLoaded?.Invoke(Data);
            }
        }

        public void Save()
        {
            lock (_lock)
            {
                try
                {
                    Data.lastSaveTimestamp = DateTime.UtcNow.ToString("o");
                    string json = JsonUtility.ToJson(Data, true);
                    string dir = Path.GetDirectoryName(_saveFilePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.WriteAllText(_saveFilePath, json);
                    OnDataSaved?.Invoke(Data);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SaveManager] Failed to save game data: {ex.Message}");
                }
            }
        }

        public void MarkLevelCompleted(int levelId, int worldId, int coinsAwarded, bool usedNoHints)
        {
            if (!Data.completedLevelIds.Contains(levelId))
            {
                Data.completedLevelIds.Add(levelId);
            }

            if (levelId >= Data.highestUnlockedLevel && Data.highestUnlockedLevel < 250)
            {
                Data.highestUnlockedLevel = levelId + 1;
            }

            Data.currentLevelIndex = Math.Min(levelId + 1, 250);
            Data.coins += coinsAwarded;
            Data.totalCoinsEarned += coinsAwarded;

            if (usedNoHints)
            {
                Data.perfectLevelsWithoutHints++;
            }

            // Update world progress
            var wp = GetWorldProgress(worldId);
            if (levelId > wp.highestLevelCompleted)
            {
                wp.highestLevelCompleted = levelId;
            }

            Save();
        }

        public WorldProgress GetWorldProgress(int worldId)
        {
            EnsureWorldProgressList();
            var wp = Data.worldProgressList.Find(w => w.worldId == worldId);
            if (wp == null)
            {
                wp = new WorldProgress { worldId = worldId, highestLevelCompleted = 0, unlockedStage = 0 };
                Data.worldProgressList.Add(wp);
            }
            return wp;
        }

        private void EnsureWorldProgressList()
        {
            if (Data.worldProgressList == null)
            {
                Data.worldProgressList = new List<WorldProgress>();
            }

            for (int w = 1; w <= 8; w++)
            {
                if (!Data.worldProgressList.Exists(item => item.worldId == w))
                {
                    Data.worldProgressList.Add(new WorldProgress
                    {
                        worldId = w,
                        highestLevelCompleted = 0,
                        unlockedStage = 0,
                        isCompleted = false
                    });
                }
            }
        }

        public void ResetProgressForDebug()
        {
            Data = new GameSaveData();
            Save();
        }
    }
}
