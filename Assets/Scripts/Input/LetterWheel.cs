using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace WordVista.InputSystem
{
    public class LetterWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Wheel Configuration")]
        [SerializeField] private float wheelRadius = 140f;
        [SerializeField] private GameObject letterStonePrefab;
        [SerializeField] private Transform stonesContainer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private UILineRenderer uiLineRenderer;

        [Header("Preview UI")]
        [SerializeField] private TextMeshProUGUI wordPreviewText;
        [SerializeField] private CanvasGroup previewCanvasGroup;

        [Header("Audio & Haptics")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip stoneSelectClip;
        [SerializeField] private AudioClip wordSubmitClip;
        [SerializeField] private AudioClip invalidShakeClip;

        private readonly List<LetterStoneView> _spawnedStones = new List<LetterStoneView>();
        private readonly List<LetterStoneView> _selectedStones = new List<LetterStoneView>();
        private string _activeLetters = "";
        private bool _isDragging;
        private Camera _uiCamera;

        public event Action<string> OnWordSubmitted;
        public event Action<string> OnWordPreviewChanged;

        private void Awake()
        {
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                _uiCamera = canvas.worldCamera;
            }
        }

        public void SetupWheel(string letters)
        {
            _activeLetters = letters.ToUpperInvariant();
            ClearStones();

            int count = _activeLetters.Length;
            if (count < 3 || count > 7)
            {
                Debug.LogWarning($"[LetterWheel] Setup with {count} letters. Expected 3 to 7.");
            }

            float angleStep = 360f / count;
            float startAngle = -90f; // Start top

            for (int i = 0; i < count; i++)
            {
                float angle = (startAngle + i * angleStep) * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * wheelRadius, Mathf.Sin(angle) * wheelRadius, 0);

                GameObject stoneObj;
                if (letterStonePrefab != null)
                {
                    stoneObj = Instantiate(letterStonePrefab, stonesContainer != null ? stonesContainer : transform);
                }
                else
                {
                    stoneObj = new GameObject($"Stone_{_activeLetters[i]}", typeof(RectTransform), typeof(LetterStoneView));
                    stoneObj.transform.SetParent(stonesContainer != null ? stonesContainer : transform, false);
                }

                stoneObj.transform.localPosition = pos;
                var view = stoneObj.GetComponent<LetterStoneView>();
                view.Initialize(_activeLetters[i], i);
                _spawnedStones.Add(view);
            }

            ResetSelection();
        }

        public void ShuffleStones()
        {
            if (_isDragging || _spawnedStones.Count == 0) return;

            // Fisher-Yates shuffle positions
            var positions = new List<Vector3>();
            foreach (var stone in _spawnedStones)
            {
                positions.Add(stone.transform.localPosition);
            }

            var rnd = new System.Random();
            for (int i = positions.Count - 1; i > 0; i--)
            {
                int k = rnd.Next(i + 1);
                var temp = positions[i];
                positions[i] = positions[k];
                positions[k] = temp;
            }

            for (int i = 0; i < _spawnedStones.Count; i++)
            {
                StartCoroutine(AnimateStoneToPos(_spawnedStones[i].transform, positions[i], 0.25f));
            }
        }

        private IEnumerator AnimateStoneToPos(Transform target, Vector3 endPos, float duration)
        {
            Vector3 startPos = target.localPosition;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                target.localPosition = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
            target.localPosition = endPos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragging = true;
            _selectedStones.Clear();
            CheckHitStone(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;
            CheckHitStone(eventData.position);
            UpdateLines(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDragging) return;
            _isDragging = false;

            string word = GetCurrentWord();
            if (!string.IsNullOrEmpty(word) && word.Length >= 2)
            {
                OnWordSubmitted?.Invoke(word);
            }

            ResetSelection();
        }

        private void CheckHitStone(Vector2 screenPos)
        {
            foreach (var stone in _spawnedStones)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(stone.RectTransform, screenPos, _uiCamera))
                {
                    // Check if already selected
                    int existingIdx = _selectedStones.IndexOf(stone);
                    if (existingIdx >= 0)
                    {
                        // Backtrack if player dragged backwards to previous stone
                        if (existingIdx == _selectedStones.Count - 2)
                        {
                            var removed = _selectedStones[_selectedStones.Count - 1];
                            removed.SetSelected(false);
                            _selectedStones.RemoveAt(_selectedStones.Count - 1);
                            UpdateWordPreview();
                            PlaySelectSound();
                        }
                    }
                    else
                    {
                        // New stone visited!
                        _selectedStones.Add(stone);
                        stone.SetSelected(true);
                        UpdateWordPreview();
                        PlaySelectSound();
                    }
                    break;
                }
            }
        }

        private void UpdateWordPreview()
        {
            string word = GetCurrentWord();
            if (wordPreviewText != null)
            {
                wordPreviewText.text = word;
            }
            if (previewCanvasGroup != null)
            {
                previewCanvasGroup.alpha = string.IsNullOrEmpty(word) ? 0f : 1f;
            }
            OnWordPreviewChanged?.Invoke(word);
        }

        private void UpdateLines(Vector2 pointerScreenPos)
        {
            if (uiLineRenderer == null) return;

            var points = new List<Vector2>();
            foreach (var stone in _selectedStones)
            {
                points.Add(stone.RectTransform.anchoredPosition);
            }

            if (_isDragging && points.Count > 0)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)transform, pointerScreenPos, _uiCamera, out Vector2 localPoint);
                points.Add(localPoint);
            }

            uiLineRenderer.SetPoints(points);
        }

        public void PlayInvalidFeedback()
        {
            if (wordPreviewText != null)
            {
                StartCoroutine(ShakePreviewRoutine());
            }
            if (audioSource != null && invalidShakeClip != null)
            {
                audioSource.PlayOneShot(invalidShakeClip);
            }
        }

        private IEnumerator ShakePreviewRoutine()
        {
            Vector3 origin = wordPreviewText.transform.localPosition;
            float elapsed = 0f;
            while (elapsed < 0.3f)
            {
                elapsed += Time.deltaTime;
                float offset = Mathf.Sin(elapsed * 50f) * 8f;
                wordPreviewText.transform.localPosition = origin + new Vector3(offset, 0, 0);
                yield return null;
            }
            wordPreviewText.transform.localPosition = origin;
        }

        private void ResetSelection()
        {
            foreach (var stone in _selectedStones)
            {
                stone.SetSelected(false);
            }
            _selectedStones.Clear();
            UpdateWordPreview();
            if (uiLineRenderer != null)
            {
                uiLineRenderer.ClearPoints();
            }
        }

        private string GetCurrentWord()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var stone in _selectedStones)
            {
                sb.Append(stone.Letter);
            }
            return sb.ToString();
        }

        private void PlaySelectSound()
        {
            if (audioSource != null && stoneSelectClip != null)
            {
                audioSource.pitch = 0.8f + (_selectedStones.Count * 0.1f);
                audioSource.PlayOneShot(stoneSelectClip);
            }
        }

        private void ClearStones()
        {
            foreach (var stone in _spawnedStones)
            {
                if (stone != null) Destroy(stone.gameObject);
            }
            _spawnedStones.Clear();
            _selectedStones.Clear();
        }
    }
}
