using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech
{
	public class WeightedRandomizer<T>
	{
		private Dictionary<T, int> m_Weights;

		private int m_Sum;

		public int Count => m_Weights.Count;
		
		/// <summary>
		/// Create a WeightedRandomizer
		/// </summary>
		public WeightedRandomizer()
		{
			m_Weights = new Dictionary<T, int>();
		}

		/// <summary>
		/// Create a WeightedRandomizer
		/// </summary>
		/// <param name="weights"> Dictionary of elements weighted</param>
		public WeightedRandomizer(Dictionary<T, int> weights)
		{
			m_Weights = weights;
			m_Sum = SumSpawnRates();
		}

		/// <summary>
		/// Add a new element to the actual dictionary
		/// </summary>
		/// <param name="_Item"></param>
		/// <param name="_Weight"></param>
		public void AddElement(T _Item, int _Weight)
		{
			m_Weights[_Item] = _Weight;
			m_Sum = SumSpawnRates();
		}

		/// <summary>
		/// Get a value from the Weighted Randomizer
		/// </summary>
		/// <returns></returns>
		public T TakeOne()
		{
			// Randomizes a number from Zero to Sum
			int roll = Random.Range(1, m_Sum+1);
			// Finds chosen item based on spawn rate
			T selected = default;
			foreach (KeyValuePair<T, int> spawn in m_Weights)
			{
				if (roll <= spawn.Value)
				{
					selected = spawn.Key;
					break;
				}

				roll -= spawn.Value;
			}

			return selected;
		}

		private int SumSpawnRates()
		{
			// Sums all spawn rates
			int sum = 0;
			foreach (KeyValuePair<T, int> spawn in m_Weights)
			{
				sum += spawn.Value;
			}

			return sum;
		}
		
	}
}