using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech
{
	public static class ExtensionMethodsList
	{
		/// <summary>
		/// Swap the element at index _FirstIndex with the element at index _SecondIndex
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_List"></param>
		/// <param name="_FirstIndex"></param>
		/// <param name="_SecondIndex"></param>
		public static void Swap<T>(this List<T> _List, int _FirstIndex, int _SecondIndex)
		{
			if (_List == null)
			{
				Debug.LogError("Can't swap indexes, the list is null");
				return;
			}

			if (_List.Count <= _FirstIndex ||
				_List.Count <= _SecondIndex)
			{
				Debug.LogError("Can't swap indexes, the list is out of range : " + Mathf.Max(_FirstIndex, _SecondIndex) + " > " + _List.Count);
				return;
			}

			T element = _List[_FirstIndex];
			_List[_FirstIndex] = _List[_SecondIndex];
			_List[_SecondIndex] = element;
		}

		/// <summary>
		/// Swap a range of elements from _BaseFirstIndex to _BaseSecondIndex with the elements from _DestFirstIndex to _DestSecondIndex
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_List"></param>
		/// <param name="_BaseFirstIndex"></param>
		/// <param name="_BaseSecondIndex"></param>
		/// <param name="_DestFirstIndex"></param>
		/// <param name="_DestSecondIndex"></param>
		public static void SwapRange<T>(this List<T> _List, int _BaseFirstIndex, int _BaseSecondIndex, int _DestFirstIndex, int _DestSecondIndex)
		{
			if (_List == null)
			{
				Debug.LogError("Can't swap index range, the list is null");
				return;
			}

			if (_List.Count <= _BaseFirstIndex ||
				_List.Count <= _BaseSecondIndex ||
				_List.Count <= _DestFirstIndex ||
				_List.Count <= _DestSecondIndex)
			{
				Debug.LogError("Can't swap index range, the list is out of range : " + Mathf.Max(Mathf.Max(_BaseFirstIndex, _BaseSecondIndex), _DestFirstIndex, _DestSecondIndex) + " > " + _List.Count);
				return;
			}

			if (_BaseSecondIndex < _BaseFirstIndex ||
				_DestFirstIndex < _DestSecondIndex)
			{
				Debug.LogError("Can't swap index range, baseFirstIndex < baseSecondIndex OR destFirstIndex < destSecondIndex");
				return;
			}

			if (_BaseFirstIndex > _DestFirstIndex)
			{
				_List.RemoveRange(_BaseFirstIndex, _BaseSecondIndex - _BaseFirstIndex);
				_List.InsertRange(_DestFirstIndex, _List.GetRange(_DestFirstIndex, _DestSecondIndex - _DestFirstIndex));
				_List.RemoveRange(_DestFirstIndex, _DestSecondIndex - _DestFirstIndex);
				_List.InsertRange(_BaseFirstIndex, _List.GetRange(_BaseFirstIndex, _BaseSecondIndex - _BaseFirstIndex));
			}
			else
			{
				_List.RemoveRange(_DestFirstIndex, _DestSecondIndex - _DestFirstIndex);
				_List.InsertRange(_BaseFirstIndex, _List.GetRange(_BaseFirstIndex, _BaseSecondIndex - _BaseFirstIndex));
				_List.RemoveRange(_BaseFirstIndex, _BaseSecondIndex - _BaseFirstIndex);
				_List.InsertRange(_DestFirstIndex, _List.GetRange(_DestFirstIndex, _DestSecondIndex - _DestFirstIndex));
			}
		}

		/// <summary>
		/// Shuffles your list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_List"></param>
		public static void Shuffle<T>(this List<T> _List)
		{
			if (_List == null)
			{
				Debug.LogError("Can't shuffle, the list is null");
				return;
			}

			for (int i = 0; i < _List.Count; ++i)
				_List.Swap(i, Random.Range(0, _List.Count));
		}

		/// <summary>
		/// Shift all your elements by _Offset positions
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_List"></param>
		/// <param name="_Offset"></param>
		public static void Shift<T>(this List<T> _List, int _Offset)
		{
			if (_List == null)
			{
				Debug.LogError("Can't shift, the list is null");
				return;
			}

			int absOffset = Mathf.Abs(_Offset);
			if (absOffset >= _List.Count)
			{
				Debug.LogError("Can't shift, the list does not have enough elements");
				return;
			}

			int shiftIndex = (_Offset > 0) ? (_List.Count - _Offset) : 0;
			List<T> shiftRange = _List.GetRange(shiftIndex, absOffset);
			_List.RemoveRange(shiftIndex, absOffset);

			int insertIndex = (_Offset > 0) ? 0 : _List.Count;
			_List.InsertRange(insertIndex, shiftRange);
		}

		/// <summary>
		/// Get a random value from the list _List
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_List"></param>
		/// <returns></returns>
		public static T GetRandomValue<T>(this List<T> _List)
		{
			T result = _List.Count == 0 ? default : _List[Random.Range(0, _List.Count)];

			return result;
		}
	}
}