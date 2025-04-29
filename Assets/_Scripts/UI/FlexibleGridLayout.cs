using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public FitType fitType;
        public int rows;
        public int columns;
        public Vector2 spacing;

        private Vector2 cellSize;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (rows <= 0) rows = 1;
            if (columns <= 0) columns = 1;

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float availableWidth = parentWidth - padding.left - padding.right;
            float availableHeight = parentHeight - padding.top - padding.bottom;

            float potentialCellWidth = (availableWidth - (spacing.x * (columns - 1))) / columns;
            float potentialCellHeight = (availableHeight - (spacing.y * (rows - 1))) / rows;

            cellSize.x = cellSize.y = Mathf.Max(0, Mathf.Min(potentialCellWidth, potentialCellHeight)); // Ensure non-negative

            float totalGridWidth = (cellSize.x * columns) + (spacing.x * (columns - 1));
            float totalGridHeight = (cellSize.y * rows) + (spacing.y * (rows - 1));

            float startOffsetX = padding.left + (availableWidth - totalGridWidth) * 0.5f;
            float startOffsetY = padding.top + (availableHeight - totalGridHeight) * 0.5f;

            int childCount = rectChildren.Count;
            for (int i = 0; i < childCount; i++)
            {
                int rowIndex = i / columns;
                int columnIndex = i % columns;

                var item = rectChildren[i];

                var xPos = startOffsetX + (cellSize.x * columnIndex) + (spacing.x * columnIndex);
                var yPos = startOffsetY + (cellSize.y * rowIndex) + (spacing.y * rowIndex);

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical() { }
        public override void SetLayoutHorizontal() { }
        public override void SetLayoutVertical() { }
    }
}
