using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VoodooPackages.Tech
{
	public class ScrollRectSwipe : ScrollRect
	{
		private bool routeToParent = false;
		private Vector2 currentItemPositionIndex;
		private Queue<(Vector2, float)> previousTimePositions;
		private List<ScrollRectSwipe> m_ScrollRectSwipeChilds;

		public bool m_Interactable = true;
		public bool m_Swipe = true;
		public float m_AccuracyRatio = 0.025f;
		public int m_QueueSize = 10;
		public bool m_ResetVerticalScrollOnSwipe = true;
		public int CurrentPage
		{
			get;
			private set;
		}

		public delegate void PageEvent(int _pageIndex);
		public PageEvent OnPageChanged;

		/// <summary>
		/// Do action for all parents
		/// </summary>
		private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
		{
			Transform parent = transform.parent;
			while (parent != null)
			{
				foreach (var component in parent.GetComponents<Component>())
				{
					if (component is T)
						action((T)(IEventSystemHandler)component);
				}
				parent = parent.parent;
			}
		}

		/// <summary>
		/// Always route initialize potential drag event to parents
		/// </summary>
		public override void OnInitializePotentialDrag(PointerEventData eventData)
		{
			DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
			base.OnInitializePotentialDrag(eventData);
		}

		/// <summary>
		/// Drag event
		/// </summary>
		public override void OnDrag(PointerEventData eventData)
		{
			if (routeToParent)
				DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
			else
				base.OnDrag(eventData);

			if (m_Swipe)
			{
				if (previousTimePositions != null && previousTimePositions.Count == m_QueueSize)
					previousTimePositions.Dequeue();

				previousTimePositions.Enqueue((content.anchoredPosition, Time.time));
			}
		}

		/// <summary>
		/// Begin drag event
		/// </summary>
		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (m_Interactable)
			{
				if (!horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
					routeToParent = true;
				else if (!vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
					routeToParent = true;
				else
					routeToParent = false;

				if (routeToParent)
					DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
				else
					base.OnBeginDrag(eventData);

				if (m_Swipe)
				{
					currentItemPositionIndex =
						content.anchoredPosition * content.transform.childCount / content.sizeDelta;
					previousTimePositions = new Queue<(Vector2, float)>();
				}
			}
		}

		/// <summary>
		/// End drag event
		/// </summary>
		public override void OnEndDrag(PointerEventData eventData)
		{
			if (routeToParent)
				DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData);});
			else
				base.OnEndDrag(eventData);

			routeToParent = false;

			if (m_Swipe)
			{
				Swipe();
			}
		}

		/// <summary>
		/// Swipe and snap for horizontal scrolling
		/// </summary>
		private void Swipe()
		{
			List<(Vector2, float)> _previousTimePositions = new List<(Vector2, float)>();
			while (previousTimePositions.Count > 0)
			{
				_previousTimePositions.Add(previousTimePositions.Dequeue());
			}

			Vector2 delta = _previousTimePositions.Last().Item1 - _previousTimePositions.First().Item1;
			float deltaTime = _previousTimePositions.Last().Item2 - _previousTimePositions.First().Item2;

			if (horizontal)
				HorizontalSwipe(_previousTimePositions, delta, deltaTime);
		}

		/// <summary>
		/// Horizontal Swipe
		/// </summary>
		/// <param name="_previousTimePositions"></param>
		/// <param name="_delta"></param>
		/// <param name="_deltaTime"></param>
		private void HorizontalSwipe(List<(Vector2, float)> _previousTimePositions, Vector2 _delta, float _deltaTime)
		{
			float _velocity = 0;
			for (int i = _previousTimePositions.Count - 1; i > 0; i--)
			{
				_velocity += (_previousTimePositions[i].Item1.x - _previousTimePositions[i - 1].Item1.x) / (_previousTimePositions[i].Item2 - _previousTimePositions[i - 1].Item2);
			}

			_velocity = _velocity * Mathf.Pow(decelerationRate, _deltaTime) / (previousTimePositions.Count - 1);

			if (Mathf.Abs(_velocity) < 1)
				_velocity = 0;

			float itemSize = content.sizeDelta.x / content.transform.childCount;
			float newItemPositionIndex = Mathf.RoundToInt(content.anchoredPosition.x / itemSize);

			float newPosition = content.anchoredPosition.x + _velocity * _deltaTime;
			int iteration = 0;
			float remapedAccuracyRatio = Arithmetic.Remap(m_AccuracyRatio, 0, 1, 1, 480);
			while (Math.Abs(_velocity) > 1 && iteration < remapedAccuracyRatio)
			{
				iteration++;
				_velocity *= Mathf.Pow(decelerationRate, _deltaTime);
				if (Mathf.Abs(_velocity) < 1)
					_velocity = 0;

				newPosition += _velocity * _deltaTime;
			}

			bool isQuickEnough = Math.Abs(content.anchoredPosition.x - newPosition) > itemSize;

			if (isQuickEnough)
			{
				newItemPositionIndex += _delta.x > 0 ? 1 : -1;
			}

			int minValue = Math.Max(Mathf.RoundToInt(currentItemPositionIndex.x) - 1, 1 - content.childCount);
			int maxValue = Mathf.Min(Mathf.RoundToInt(currentItemPositionIndex.x + 1), 0);
			newItemPositionIndex = Mathf.Clamp(newItemPositionIndex, minValue, maxValue);

			ChangePage((int)newItemPositionIndex * -1);
		}

		/// <summary>
		/// Lerp from current position to _aimPosition
		/// </summary>
		/// <param name="_aimPosition"></param>
		/// <returns></returns>
		private IEnumerator LerpTo(Vector2 _aimPosition)
		{
			StopMovement();

			while (Vector2.Distance(content.anchoredPosition, _aimPosition) > 0.05f)
			{
				if (Vector2.Distance(content.anchoredPosition, _aimPosition) < 5f)
				{
					content.anchoredPosition = _aimPosition;
				}
				else
				{
					content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, _aimPosition, decelerationRate);
				}

				yield return null;
			}

			if (m_ResetVerticalScrollOnSwipe)
			{
				Vector2 newItemPositionIndex = _aimPosition * content.transform.childCount / content.sizeDelta;

				if (Mathf.Abs(currentItemPositionIndex.x - newItemPositionIndex.x) > 0f)
				{
					ResetVerticalNormalizedPosition(newItemPositionIndex);
				}
			}
		}

		/// <summary>
		/// Get all vertical ScrollRectSwipe childs and reset their verticalNormalizedPosition
		/// if it's not the current one.
		/// </summary>
		private void ResetVerticalNormalizedPosition(Vector2 _newItemPositionIndex)
		{
			if (m_ScrollRectSwipeChilds == null || m_ScrollRectSwipeChilds.Count == 0)
			{
				m_ScrollRectSwipeChilds = GetComponentsInChildren<ScrollRectSwipe>(true).ToList();
				m_ScrollRectSwipeChilds.Remove(this);
			}

			for (int i = 0; i < m_ScrollRectSwipeChilds.Count; i++)
			{
				ScrollRectSwipe scrollRectSwipeChild = m_ScrollRectSwipeChilds[i];

				if (Math.Abs(i - Mathf.Abs(_newItemPositionIndex.x)) > Mathf.Epsilon)
				{
					scrollRectSwipeChild.verticalNormalizedPosition = 1;
				}
			}
		}

		public void ChangePage(int _index, bool _instant = false)
		{
			if (horizontal)
			{
				_index = Mathf.Clamp(_index, 0, content.transform.childCount-1);
				float itemSize = content.sizeDelta.x / content.transform.childCount;
				Vector2 aimPosition = new Vector2(_index * itemSize * -1, content.anchoredPosition.y);

				CurrentPage = _index;

				StopAllCoroutines();
				if (!_instant)
				{
					StartCoroutine(LerpTo(aimPosition));
				}
				else
				{
					content.anchoredPosition = aimPosition;

					if (m_ResetVerticalScrollOnSwipe)
					{
						Vector2 newItemPositionIndex = aimPosition * content.transform.childCount / content.sizeDelta;

						if (Mathf.Abs(currentItemPositionIndex.x - newItemPositionIndex.x) > 0f)
						{
							ResetVerticalNormalizedPosition(newItemPositionIndex);
						}
					}
				}
			}
			OnPageChanged?.Invoke(_index);
		}
	}
}