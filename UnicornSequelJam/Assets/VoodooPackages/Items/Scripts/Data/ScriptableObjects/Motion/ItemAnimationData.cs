using System;
using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    [CreateAssetMenu(fileName = "ItemAnimation", menuName = "VoodooPackages/Items/Animation", order = 1)]
	public class ItemAnimationData : ScriptableObject
	{
		public float delayAppearing = 1;
		
		[SerializeField] public Disc disc = new Disc();

		public float dispersion = 1f;

		[SerializeField] public Curve scaleAppearing = new Curve();

		public Vector3 startScaleAppearing;
		public Vector3 endScaleAppearing;

		[SerializeField] public Curve speedAppearing = new Curve();

		public float delayAfterAppear;
		
		public float delayGrouping = 1;

		public bool allTogether;

		[SerializeField] public float delayBetweenRewards;

		[SerializeField] public Curve scaleGrouping = new Curve();
		
		public Vector3 endScaleGrouping;

		[SerializeField] public Curve speedGrouping = new Curve();

		public bool waveMovement = false;
		public Vector3 waveAxis = new Vector3();

	}
	
	[Serializable]
	public class Disc
	{
		public Vector3  center;
		public Vector3  normal;
		public float    range        = 360f;
		public float    radiusMin    = 1f;
		public float    radiusMax    = 1.2f;
		public float    radiusFactor = 100f;
	}

	[Serializable]
	public class Curve
	{
		public bool           useCurve = true;
		public bool           reversed = false;
		public AnimationCurve curve    = new AnimationCurve();
		public int            curveTypeIndex;
	}
}