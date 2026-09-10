using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordVista.InputSystem
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : MaskableGraphic
    {
        [SerializeField] private float thickness = 14f;
        [SerializeField] private Color lineColor = new Color(1f, 0.85f, 0.35f, 0.85f);

        private readonly List<Vector2> _points = new List<Vector2>();

        public void SetPoints(List<Vector2> points)
        {
            _points.Clear();
            if (points != null)
            {
                _points.AddRange(points);
            }
            SetVerticesDirty();
        }

        public void ClearPoints()
        {
            _points.Clear();
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (_points == null || _points.Count < 2) return;

            float halfThickness = thickness * 0.5f;

            for (int i = 0; i < _points.Count - 1; i++)
            {
                Vector2 start = _points[i];
                Vector2 end = _points[i + 1];

                Vector2 dir = (end - start).normalized;
                Vector2 normal = new Vector2(-dir.y, dir.x) * halfThickness;

                int baseIndex = vh.currentVertCount;

                UIVertex v1 = UIVertex.simpleVert;
                v1.color = lineColor;
                v1.position = start + normal;

                UIVertex v2 = UIVertex.simpleVert;
                v2.color = lineColor;
                v2.position = start - normal;

                UIVertex v3 = UIVertex.simpleVert;
                v3.color = lineColor;
                v3.position = end - normal;

                UIVertex v4 = UIVertex.simpleVert;
                v4.color = lineColor;
                v4.position = end + normal;

                vh.AddVert(v1);
                vh.AddVert(v2);
                vh.AddVert(v3);
                vh.AddVert(v4);

                vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
                vh.AddTriangle(baseIndex + 2, baseIndex + 3, baseIndex);
            }
        }
    }
}
