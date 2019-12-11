using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tool.Shop
{
	[ExecuteInEditMode]
	public class DynamicGridLayoutGroupCanvas : MonoBehaviour
	{
		public uint line;
		public uint column;
		public GridLayoutGroup gridLayoutGroup;
		public RectTransform containerRectTransform;

		private void Start()
		{
			if (containerRectTransform == null)
				containerRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		}

		void Update()
		{
			if (gridLayoutGroup == null)
				return;

			if (containerRectTransform == null)
				containerRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

			int paddingHorizontal = gridLayoutGroup.padding.right + gridLayoutGroup.padding.left;
			int paddingVertical = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

			float Xsize = (containerRectTransform.rect.width - ((column - 1) * gridLayoutGroup.spacing.x + paddingHorizontal)) / column;
			float Ysize = (containerRectTransform.rect.height - ((line - 1) * gridLayoutGroup.spacing.y + paddingVertical)) / line;
			float uniformedCellSize = Mathf.Min(Xsize, Ysize);
			gridLayoutGroup.cellSize = new Vector2(uniformedCellSize, uniformedCellSize);
		}
	}
}
