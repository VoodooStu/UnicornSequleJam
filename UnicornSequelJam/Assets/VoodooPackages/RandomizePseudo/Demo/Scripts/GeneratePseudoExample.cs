using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VoodooPackages.Tech;
using UnityEngine;
using UnityEngine.Serialization;

namespace VoodooPackages.Tech
{
	public class GeneratePseudoExample : MonoBehaviour
	{
		public PseudoPool pseudosPool;

		List<string> names = new List<string>();

		// Use this for initialization
		void Start()
		{

			// Get a list of 10 names from namesPool without duplicate
			names = RandomizePseudo.instance.GetRandomPseudos(pseudosPool, 10, false);

			foreach (string _name in names)
			{
				Debug.Log(_name);
			}

		}

	}
}