using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.Items
{
    public class ItemAnimation
    {
        public Guid     guid;
        public int      itemId;
        public Sprite   sprite;
        public Color    color = Color.white;
        public ItemAnimationData animationData;
        public int      quantity;
        public Vector3  destination;
        public int      compt = 0;
    }

    public class ItemParticlesAnimation : MonoBehaviour
	{
        public bool isUsed = false;
        public ItemAnimation itemAnimationData;

		public Image image;
		
		private RectTransform rectTransform;

		private Vector3 fromPosition;
		private Vector3 toPosition;
		
		public void SetUp()
		{
			rectTransform   = GetComponent<RectTransform>();
			image.sprite    = itemAnimationData.sprite;
			image.color     = itemAnimationData.color;
			gameObject.name = itemAnimationData.itemId.ToString();
            isUsed          = true;
		}

		public void SetAppearingToPosition(Vector3 _to)
		{
			toPosition = _to;
		}

		public Vector3 GetToPosition()
		{
			return toPosition;
		}
		
		public void StartAppearing()
		{
			fromPosition = transform.position;
			transform.localScale = itemAnimationData.animationData.startScaleAppearing;
			
			StartCoroutine(Appearing());
		}
		
		private IEnumerator Appearing()
		{
			float _time = 0f;
			bool _customCurveSpeed = itemAnimationData.animationData.speedAppearing.curveTypeIndex == 0;
			bool _customCurveScale = itemAnimationData.animationData.scaleAppearing.curveTypeIndex == 0;

			CalculationEasing.Function _functionSpeed = CalculationEasing.GetEasingFunction(Ease.Linear);
			CalculationEasing.Function _functionScale = CalculationEasing.GetEasingFunction(Ease.Linear);
			
			if (_customCurveSpeed == false)
			{
				_functionSpeed = CalculationEasing.GetEasingFunction((Ease) itemAnimationData.animationData.speedAppearing.curveTypeIndex-1);
			}
			
			if (_customCurveScale == false)
			{
				_functionScale = CalculationEasing.GetEasingFunction((Ease) itemAnimationData.animationData.scaleAppearing.curveTypeIndex-1);
			}

			while (_time<itemAnimationData.animationData.delayAppearing)
			{
				if (itemAnimationData.animationData.speedAppearing.useCurve)
				{
					if (_customCurveSpeed)
					{
						transform.position = CalculateVectorInterpolationCustom(itemAnimationData.animationData.speedAppearing.curve,
							itemAnimationData.animationData.speedAppearing.reversed,
							fromPosition, toPosition, _time / itemAnimationData.animationData.delayAppearing);
					}
					else
					{
						transform.position = CalculateVectorInterpolation(_functionSpeed,
							itemAnimationData.animationData.speedAppearing.reversed, fromPosition, toPosition,
							_time / itemAnimationData.animationData.delayAppearing);
					}
				}


				if (itemAnimationData.animationData.scaleAppearing.useCurve)
				{
					if (_customCurveScale)
					{
						
						transform.localScale = CalculateVectorInterpolationCustom(itemAnimationData.animationData.scaleAppearing.curve,
							itemAnimationData.animationData.scaleAppearing.reversed,
							itemAnimationData.animationData.startScaleAppearing, itemAnimationData.animationData.endScaleAppearing,
							_time / itemAnimationData.animationData.delayAppearing);
					}
					else
					{
						transform.localScale = CalculateVectorInterpolation(_functionScale,
							itemAnimationData.animationData.scaleAppearing.reversed, itemAnimationData.animationData.startScaleAppearing,
							itemAnimationData.animationData.endScaleAppearing, _time / itemAnimationData.animationData.delayAppearing);
					}
				}


				_time += Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime);
				
			}

			if (itemAnimationData.animationData.speedAppearing.useCurve)
			{
				rectTransform.position = toPosition;
				fromPosition = toPosition;
			}

			if (itemAnimationData.animationData.scaleAppearing.useCurve)
			{
				transform.localScale = itemAnimationData.animationData.endScaleAppearing;
			}

			ItemParticlesAnimator.Instance.OneItemAppearingCompleted(this);

		}
		
		public void StartGrouping(Vector3 _to)
		{
			fromPosition = transform.position;
			toPosition = _to;
			
			transform.localScale = itemAnimationData.animationData.endScaleAppearing;
			
			StartCoroutine(Grouping());
		}

		private IEnumerator Grouping()
		{
			float _time = 0;
		
			bool _customCurveSpeed = itemAnimationData.animationData.speedGrouping.curveTypeIndex == 0;
			bool _customCurveScale = itemAnimationData.animationData.scaleGrouping.curveTypeIndex == 0;

			CalculationEasing.Function _functionSpeed = CalculationEasing.GetEasingFunction((Ease) itemAnimationData.animationData.speedGrouping.curveTypeIndex-1);
			CalculationEasing.Function _functionScale = CalculationEasing.GetEasingFunction((Ease) itemAnimationData.animationData.scaleGrouping.curveTypeIndex-1);

			Vector3 _scaleStart = transform.localScale;
			Vector3 _position;

			while (_time<itemAnimationData.animationData.delayGrouping)
			{
				if (itemAnimationData.animationData.speedGrouping.useCurve)
				{
					if (_customCurveSpeed)
					{
						_position = CalculateVectorInterpolationCustom(itemAnimationData.animationData.speedGrouping.curve,
							itemAnimationData.animationData.speedGrouping.reversed,
							fromPosition, toPosition, _time / itemAnimationData.animationData.delayGrouping);
					}
					else
					{
						_position = CalculateVectorInterpolation(_functionSpeed,
							itemAnimationData.animationData.speedGrouping.reversed, fromPosition, toPosition,
							_time / itemAnimationData.animationData.delayGrouping);
					}
					
					
					transform.position = _position;
				}


				if (itemAnimationData.animationData.scaleGrouping.useCurve)
				{
					if (_customCurveScale)
					{
						
						transform.localScale = CalculateVectorInterpolationCustom(itemAnimationData.animationData.scaleGrouping.curve,
							itemAnimationData.animationData.scaleGrouping.reversed,
							_scaleStart, itemAnimationData.animationData.endScaleGrouping,
							_time / itemAnimationData.animationData.delayGrouping);
					}
					else
					{
						transform.localScale = CalculateVectorInterpolation(_functionScale,
							itemAnimationData.animationData.scaleGrouping.reversed, _scaleStart,
							itemAnimationData.animationData.endScaleGrouping, _time / itemAnimationData.animationData.delayGrouping);
					}
				}


				_time += Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime);
				
			}

			if (itemAnimationData.animationData.speedGrouping.useCurve)
			{
				rectTransform.position = toPosition;
				fromPosition = toPosition;
			}

			if (itemAnimationData.animationData.scaleGrouping.useCurve)
			{
				transform.localScale = itemAnimationData.animationData.endScaleGrouping;
			}

			ItemParticlesAnimator.Instance.OneItemGroupingCompleted(this);
			
			//ItemParticlesAnimator.Instance.BecomeReady(this);

		}
		
		private Vector3 CalculateVectorInterpolation(CalculationEasing.Function _function, bool _reversed, Vector3 _start, Vector3 _end, float _time)
		{
			Vector3 _vectorInterpolation = new Vector3();

			if (_reversed)
			{
				_vectorInterpolation = Vector3.Lerp(_end, _start, _time);
			}
			else
			{
				_vectorInterpolation = Vector3.Lerp(_start, _end, _time);
			}

			return _vectorInterpolation;
		}
		
		private Vector3 CalculateVectorInterpolationCustom(AnimationCurve _curve, bool _reversed, Vector3 _start, Vector3 _end, float _time)
		{
			Vector3 _position = new Vector3();

			if (_reversed)
			{
				_position = Vector3.Lerp(_end, _start, _curve.Evaluate(_time));
			}
			else
			{
				_position = Vector3.Lerp(_start, _end, _curve.Evaluate(_time));
			}
			
			return _position;

		}		
	}
}