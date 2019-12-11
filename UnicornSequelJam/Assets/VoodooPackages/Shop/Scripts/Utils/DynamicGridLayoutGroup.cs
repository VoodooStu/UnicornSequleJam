using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tool.Shop
{
	[ExecuteInEditMode]
	public class DynamicGridLayoutGroup : MonoBehaviour
	{
		public uint line;
		public uint column;
		public GridLayoutGroup gridLayoutGroup;

		private RectTransform rectTransform;

		private void Start()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		void Update()
		{
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			int paddingHorizontal = gridLayoutGroup.padding.right + gridLayoutGroup.padding.left;
			int paddingVertical = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

			float Xsize = (rectTransform.rect.width - ((column - 1) * gridLayoutGroup.spacing.x + paddingHorizontal)) / column;
			float Ysize = (rectTransform.rect.height - ((line - 1) * gridLayoutGroup.spacing.y + paddingVertical)) / line;
			gridLayoutGroup.cellSize = new Vector2(Xsize, Ysize);
		}
	}
}
