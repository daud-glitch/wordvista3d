using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordVista.Levels;

namespace WordVista.Crossword
{
    public class CrosswordRenderer : MonoBehaviour
    {
        [Header("Cell Settings")]
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float baseCellSize = 64f;
        [SerializeField] private float cellSpacing = 8f;
        [SerializeField] private RectTransform containerRect;

        [Header("Animation")]
        [SerializeField] private float staggerDelay = 0.08f;

        private readonly Dictionary<(int x, int y), CrosswordCellView> _cellViews = new Dictionary<(int x, int y), CrosswordCellView>();
        private CrosswordGridModel _gridModel;

        public void RenderGrid(CrosswordGridModel gridModel)
        {
            _gridModel = gridModel;
            ClearGrid();

            if (gridModel == null || gridModel.Cells.Count == 0) return;

            int minX = int.MaxValue, maxX = int.MinValue;
            int minY = int.MaxValue, maxY = int.MinValue;

            foreach (var key in gridModel.Cells.Keys)
            {
                if (key.x < minX) minX = key.x;
                if (key.x > maxX) maxX = key.x;
                if (key.y < minY) minY = key.y;
                if (key.y > maxY) maxY = key.y;
            }

            int cols = (maxX - minX) + 1;
            int rows = (maxY - minY) + 1;

            float step = baseCellSize + cellSpacing;
            float totalWidth = cols * step;
            float totalHeight = rows * step;

            // Auto-fit scale if larger than container
            float containerWidth = containerRect != null ? containerRect.rect.width : 700f;
            float containerHeight = containerRect != null ? containerRect.rect.height : 600f;

            float scale = 1f;
            if (totalWidth > containerWidth || totalHeight > containerHeight)
            {
                float scaleX = (containerWidth * 0.9f) / totalWidth;
                float scaleY = (containerHeight * 0.9f) / totalHeight;
                scale = Mathf.Min(scaleX, scaleY, 1f);
            }

            transform.localScale = Vector3.one * scale;

            float startX = -((cols - 1) * step) * 0.5f;
            float startY = ((rows - 1) * step) * 0.5f;

            foreach (var kvp in gridModel.Cells)
            {
                int gx = kvp.Key.x;
                int gy = kvp.Key.y;
                char letter = kvp.Value.Letter;

                float posX = startX + (gx - minX) * step;
                float posY = startY - (gy - minY) * step;

                GameObject cellObj;
                if (cellPrefab != null)
                {
                    cellObj = Instantiate(cellPrefab, transform);
                }
                else
                {
                    cellObj = new GameObject($"Cell_{gx}_{gy}", typeof(RectTransform), typeof(CrosswordCellView));
                    cellObj.transform.SetParent(transform, false);
                }

                cellObj.transform.localPosition = new Vector3(posX, posY, 0);
                var view = cellObj.GetComponent<CrosswordCellView>();
                view.Initialize(gx, gy, letter);

                if (kvp.Value.IsRevealed)
                {
                    view.RevealLetter(false, 0f);
                }

                _cellViews[(gx, gy)] = view;
            }
        }

        public void AnimateWordSolved(WordPlacement placement)
        {
            if (placement == null) return;

            string word = placement.word;
            for (int i = 0; i < word.Length; i++)
            {
                int cx = placement.isHorizontal ? placement.startX + i : placement.startX;
                int cy = placement.isHorizontal ? placement.startY : placement.startY + i;

                if (_cellViews.TryGetValue((cx, cy), out var cellView))
                {
                    cellView.RevealLetter(false, i * staggerDelay);
                }
            }
        }

        public void AnimateHintCell(CrosswordCell cell)
        {
            if (cell == null) return;
            if (_cellViews.TryGetValue((cell.X, cell.Y), out var cellView))
            {
                cellView.RevealLetter(true, 0f);
            }
        }

        private void ClearGrid()
        {
            foreach (var view in _cellViews.Values)
            {
                if (view != null) Destroy(view.gameObject);
            }
            _cellViews.Clear();
        }
    }
}
