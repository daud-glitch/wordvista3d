using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WordVista.Crossword
{
    public class CrosswordCellView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI letterText;
        [SerializeField] private Image backgroundTile;
        [SerializeField] private Image borderOutline;
        [SerializeField] private ParticleSystem shimmerParticles;

        [Header("Color Palette")]
        [SerializeField] private Color unsolvedTileColor = new Color(0.2f, 0.28f, 0.35f, 0.75f);
        [SerializeField] private Color solvedTileColor = new Color(0.95f, 0.92f, 0.85f, 1f);
        [SerializeField] private Color solvedBorderColor = new Color(0.98f, 0.8f, 0.25f, 1f);
        [SerializeField] private Color hintBorderColor = new Color(0.35f, 0.85f, 1.0f, 1f);

        public int GridX { get; private set; }
        public int GridY { get; private set; }
        public char Letter { get; private set; }
        public bool IsRevealed { get; private set; }
        public RectTransform RectTransform => (RectTransform)transform;

        public void Initialize(int x, int y, char letter)
        {
            GridX = x;
            GridY = y;
            Letter = char.ToUpperInvariant(letter);
            IsRevealed = false;

            if (letterText != null)
            {
                letterText.text = "";
            }

            if (backgroundTile != null)
            {
                backgroundTile.color = unsolvedTileColor;
            }

            if (borderOutline != null)
            {
                borderOutline.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }

        public void RevealLetter(bool isHint = false, float delay = 0f)
        {
            if (IsRevealed) return;
            IsRevealed = true;
            StartCoroutine(AnimateRevealRoutine(isHint, delay));
        }

        private IEnumerator AnimateRevealRoutine(bool isHint, float delay)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);

            if (letterText != null)
            {
                letterText.text = Letter.ToString();
            }

            if (backgroundTile != null)
            {
                backgroundTile.color = solvedTileColor;
            }

            if (borderOutline != null)
            {
                borderOutline.color = isHint ? hintBorderColor : solvedBorderColor;
            }

            if (shimmerParticles != null)
            {
                shimmerParticles.Play();
            }

            // Punch scale bounce animation
            float elapsed = 0f;
            float duration = 0.35f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float scale = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.3f;
                transform.localScale = Vector3.one * scale;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }
    }
}
