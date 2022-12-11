using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
	public int rows;
	public int columns;

	private Vector2 cellSize;
	public Vector2 cellSpacing;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		float sqrt = Mathf.Sqrt(transform.childCount);

		rows = Mathf.CeilToInt(sqrt);
		columns = Mathf.CeilToInt(sqrt);

		float parentWidth = rectTransform.rect.width;
		float parentHeight = rectTransform.rect.height;
		float cellWidth = (parentWidth / (float)columns) - ((cellSpacing.x / (float)columns) * (columns - 1)) - (padding.left / (float) columns) - (padding.right / (float)columns);
		float cellHeight = (parentHeight / (float)rows) - ((cellSpacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)columns) - (padding.bottom / (float)columns);

		cellSize.x = cellWidth;
		cellSize.y = cellHeight;

		int columnsCount = 0;
		int rowCount = 0;

		for (int i = 0; i < rectChildren.Count; i++)
		{
			rowCount = i / columns;
			columnsCount = i % columns;

			var item = rectChildren[i];

			var xPos = (cellSize.x * columnsCount) + (cellSpacing.x * columnsCount) + padding.left;
			var yPos = (cellSize.y * rowCount) + (cellSpacing.y * rowCount) + padding.top;

			SetChildAlongAxis(item, 0, xPos, cellSize.x);
			SetChildAlongAxis(item, 1, yPos, cellSize.y);
		}
	}

	public override void CalculateLayoutInputVertical()
	{

	}

	public override void SetLayoutHorizontal()
	{

	}

	public override void SetLayoutVertical()
	{

	}
}
