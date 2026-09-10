using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using WordVista.Save;

namespace WordVista.UI
{
    public class WorldMapUI : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform nodesContainer;
        [SerializeField] private GameObject levelNodePrefab;

        [Header("Top Navigation")]
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI worldTitleText;
        [SerializeField] private TextMeshProUGUI coinsText;

        [Header("Styling")]
        [SerializeField] private Color completedColor = new Color(0.98f, 0.82f, 0.25f);
        [SerializeField] private Color currentColor = new Color(0.35f, 0.85f, 1f);
        [SerializeField] private Color lockedColor = new Color(0.45f, 0.5f, 0.55f);

        private void Start()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
            }

            if (coinsText != null)
            {
                coinsText.text = SaveManager.Instance.Data.coins.ToString();
            }

            GenerateNodes();
        }

        private void GenerateNodes()
        {
            if (nodesContainer == null) return;

            int highestUnlocked = SaveManager.Instance.Data.highestUnlockedLevel;
            var completedList = SaveManager.Instance.Data.completedLevelIds;

            for (int i = 1; i <= 250; i++)
            {
                int levelNum = i;
                GameObject nodeObj;
                if (levelNodePrefab != null)
                {
                    nodeObj = Instantiate(levelNodePrefab, nodesContainer);
                }
                else
                {
                    nodeObj = new GameObject($"Node_{levelNum}", typeof(RectTransform), typeof(Button), typeof(Image));
                    nodeObj.transform.SetParent(nodesContainer, false);
                }

                var btn = nodeObj.GetComponent<Button>();
                var img = nodeObj.GetComponent<Image>();
                var txt = nodeObj.GetComponentInChildren<TextMeshProUGUI>();

                if (txt != null)
                {
                    txt.text = levelNum.ToString();
                }

                bool isCompleted = completedList.Contains(levelNum);
                bool isCurrent = levelNum == highestUnlocked;
                bool isLocked = levelNum > highestUnlocked;

                if (img != null)
                {
                    img.color = isCompleted ? completedColor : (isCurrent ? currentColor : lockedColor);
                }

                if (btn != null)
                {
                    btn.interactable = !isLocked;
                    btn.onClick.AddListener(() =>
                    {
                        SaveManager.Instance.Data.currentLevelIndex = levelNum;
                        SceneManager.LoadScene("Gameplay");
                    });
                }
            }

            // Scroll to current level
            if (scrollRect != null)
            {
                float normalizedProgress = Mathf.Clamp01((float)highestUnlocked / 250f);
                scrollRect.verticalNormalizedPosition = 1f - normalizedProgress;
            }
        }
    }
}
