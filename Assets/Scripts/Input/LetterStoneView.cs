using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WordVista.InputSystem
{
    public class LetterStoneView : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI letterText;
        [SerializeField] private Image stoneBackground;
        [SerializeField] private Image glowHighlight;

        [Header("Styling")]
        [SerializeField] private Color normalColor = new Color(0.95f, 0.93f, 0.88f);
        [SerializeField] private Color selectedColor = new Color(1.0f, 0.85f, 0.4f);
        [SerializeField] private float elevationScale = 1.15f;

        public char Letter { get; private set; }
        public int StoneIndex { get; private set; }
        public bool IsSelected { get; private set; }
        public RectTransform RectTransform => (RectTransform)transform;

        public void Initialize(char letter, int index)
        {
            Letter = char.ToUpperInvariant(letter);
            StoneIndex = index;
            if (letterText != null)
            {
                letterText.text = Letter.ToString();
            }
            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            if (glowHighlight != null)
            {
                glowHighlight.gameObject.SetActive(selected);
            }

            transform.localScale = selected ? Vector3.one * elevationScale : Vector3.one;

            if (stoneBackground != null)
            {
                stoneBackground.color = selected ? selectedColor : normalColor;
            }
        }

        public void PlayShakeAnimation()
        {
            // Simple gentle punch animation
            StartCoroutine(ShakeRoutine());
        }

        private System.Collections.IEnumerator ShakeRoutine()
        {
            Vector3 origin = transform.localPosition;
            float duration = 0.25f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float offset = Mathf.Sin(elapsed * 40f) * 6f;
                transform.localPosition = origin + new Vector3(offset, 0, 0);
                yield return null;
            }
            transform.localPosition = origin;
        }
    }
}
