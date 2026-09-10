using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WordVista.Save;
using WordVista.Economy;

namespace WordVista.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Display Elements")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI currentWorldText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI coinCountText;

        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button journeyButton;
        [SerializeField] private Button dailyPuzzleButton;
        [SerializeField] private Button settingsButton;

        [Header("Settings Dialog")]
        [SerializeField] private GameObject settingsModal;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Toggle hapticsToggle;

        private void Start()
        {
            UpdateUI();

            if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
            if (journeyButton != null) journeyButton.onClick.AddListener(OnJourneyClicked);
            if (dailyPuzzleButton != null) dailyPuzzleButton.onClick.AddListener(OnDailyClicked);
            if (settingsButton != null) settingsButton.onClick.AddListener(ToggleSettings);

            if (musicToggle != null)
            {
                musicToggle.isOn = SaveManager.Instance.Data.musicEnabled;
                musicToggle.onValueChanged.AddListener(val =>
                {
                    SaveManager.Instance.Data.musicEnabled = val;
                    SaveManager.Instance.Save();
                });
            }

            if (sfxToggle != null)
            {
                sfxToggle.isOn = SaveManager.Instance.Data.sfxEnabled;
                sfxToggle.onValueChanged.AddListener(val =>
                {
                    SaveManager.Instance.Data.sfxEnabled = val;
                    SaveManager.Instance.Save();
                });
            }

            if (hapticsToggle != null)
            {
                hapticsToggle.isOn = SaveManager.Instance.Data.hapticsEnabled;
                hapticsToggle.onValueChanged.AddListener(val =>
                {
                    SaveManager.Instance.Data.hapticsEnabled = val;
                    SaveManager.Instance.Save();
                });
            }

            EconomyManager.Instance.OnCoinsChanged += (coins, delta) =>
            {
                if (coinCountText != null) coinCountText.text = coins.ToString();
            };
        }

        private void UpdateUI()
        {
            var data = SaveManager.Instance.Data;
            int level = data.currentLevelIndex;
            string world = GetWorldName(level);

            if (currentWorldText != null) currentWorldText.text = world;
            if (currentLevelText != null) currentLevelText.text = $"Level {level}";
            if (coinCountText != null) coinCountText.text = data.coins.ToString();
        }

        private string GetWorldName(int level)
        {
            if (level <= 30) return "Green Valley";
            if (level <= 60) return "Mystic Forest";
            if (level <= 90) return "Crystal Lake";
            if (level <= 120) return "Desert Kingdom";
            if (level <= 150) return "Snow Valley";
            if (level <= 180) return "Tropical Island";
            if (level <= 215) return "Sky Kingdom";
            return "Aurora World";
        }

        public void OnPlayClicked()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void OnJourneyClicked()
        {
            SceneManager.LoadScene("WorldMap");
        }

        public void OnDailyClicked()
        {
            SceneManager.LoadScene("DailyPuzzle");
        }

        public void ToggleSettings()
        {
            if (settingsModal != null)
            {
                settingsModal.SetActive(!settingsModal.activeSelf);
            }
        }
    }
}
