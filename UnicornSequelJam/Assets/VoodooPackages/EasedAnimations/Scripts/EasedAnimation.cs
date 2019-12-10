using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VoodooPackages.Tech.EasedAnimations
{
    public class EasedAnimation : MonoBehaviour
	{
		public bool         animated      = true;
		public bool         launchAtStart = true;

		[Space]
		public AnimationData animationData;

		[Space]
		public CanvasGroup  canvasGroup;
		public Image        image;

		private Vector3     positionOrigin;
		private Vector3     rotationOrigin;
		private Vector3     scaleOrigin;

		private bool        isInitialized = false;
		
		private void Start()
		{
			if (launchAtStart)
				StartAnimation();
		}

		private void Initialized()
		{
			isInitialized = true;
			ResetTransformMemory();
		}

		/// <summary>
		/// Reset the origin position of the object
		/// </summary>
		public void ResetTransformMemory()
		{
			if (transform == null)
				return;
			
			positionOrigin = transform.localPosition;
			rotationOrigin = transform.localEulerAngles;
			scaleOrigin = transform.localScale;
		}
		
		/// <summary>
		/// Start the Animation
		/// </summary>
		public void StartAnimation()
		{
			if (!isInitialized)
				Initialized();
			
			if (animationData != null)
			{
				animationData.Reset();
				StartCoroutine(Animate());
			}
		}

		/// <summary>
		/// Stop the Animation
		/// </summary>
		public void StopAnimation()
		{
			if (animationData != null)
			{
				animationData.Reset();
				StopAllCoroutines();
			}
		}
		
		IEnumerator Animate()
		{
			float _timeElapsed;

			do
			{
				_timeElapsed = 0;
				animationData.percentage = 0f;

				// Before
				while (animated && animationData.beforeDelay > 0 && _timeElapsed< animationData.beforeDelay )
				{
					// Percentage
					animationData.percentage = _timeElapsed / animationData.beforeDelay;
				
					if (animationData.percentage > 1)
					{
						animationData.percentage = 1;
					}

					if (animationData.percentage < 0)
					{
						animationData.percentage = 0;
					}
				
					yield return new WaitForEndOfFrame();
					_timeElapsed += Time.deltaTime;

				}
				
				_timeElapsed = 0;
				animationData.percentage = 0f;

				// Time
				while (animated && animationData.duration > 0 && _timeElapsed< animationData.duration)
				{
					// Percentage
					animationData.percentage = _timeElapsed / animationData.duration;
				
					if (animationData.percentage > 1)
					{
						animationData.percentage = 1;
					}

					if (animationData.percentage < 0)
					{
						animationData.percentage = 0;
					}
				
					// Fields
					if (animationData.position.toggle)
					{
						PositionModification();
					}

					if (animationData.rotation.toggle)
					{
						RotationModification();
					}

					if (animationData.scale.toggle)
					{
						ScaleModification();
					}

					if (animationData.alpha.toggle)
					{
						AlphaModification();
					}

					if (animationData.rotate.toggle)
					{
						RotateModification();
					}
					
					yield return new WaitForEndOfFrame();
					_timeElapsed += Time.deltaTime;
				}

				_timeElapsed = 0;
				animationData.percentage = 0f;
				
				// After
				while (animated && animationData.afterDelay > 0 && _timeElapsed< animationData.afterDelay )
				{
					// Percentage
					animationData.percentage = _timeElapsed / animationData.afterDelay;
				
					if (animationData.percentage > 1)
					{
						animationData.percentage = 1;
					}

					if (animationData.percentage < 0)
					{
						animationData.percentage = 0;
					}
				
					yield return new WaitForEndOfFrame();
					_timeElapsed += Time.deltaTime;
				
				}
			} while (animationData.loop);
		}

		// Fields Modification
		private void PositionModification()
		{
			transform.localPosition = positionOrigin + GetVector3Value(animationData.position);
		}

		private void RotationModification()
		{
			transform.localEulerAngles = rotationOrigin + GetVector3Value(animationData.rotation);
		}

		private void ScaleModification()
		{
			Vector3 _value = GetVector3Value(animationData.scale);
            			
			transform.localScale =
				new Vector3(scaleOrigin.x * _value.x, scaleOrigin.y * _value.y, scaleOrigin.z * _value.z);
		}
		
		private void AlphaModification()
		{
			float _alpha = GetAxisValue(animationData.alpha);
			
			if (canvasGroup != null)
			{
				canvasGroup.alpha = _alpha;
			}
			else if (image != null)
			{
				image.color = new Color(image.color.r, image.color.b, image.color.g, _alpha);
			}			
		}

		private void RotateModification()
		{
			transform.Rotate(animationData.rotate.rotate * animationData.rotate.multiplier * Time.deltaTime);
		}
		
		// Data Analyse
		private Vector3 GetVector3Value(Vector3Data _data)
		{
			Vector3 _value = Vector3.zero;

			if (_data.xyzData.toggle)
			{
				_value.x += GetAxisValue(_data.xyzData);
				_value.y += GetAxisValue(_data.xyzData);
				_value.z += GetAxisValue(_data.xyzData);
			}
			else
			{
				_value.x += GetAxisValue(_data.xData);
				_value.y += GetAxisValue(_data.yData);
				_value.z += GetAxisValue(_data.zData);
			}
			

			return _value;
		}

		private float GetAxisValue(FloatData _data)
		{
			float _value = 0;

			if (_data.toggle == false)
			{
				return _value;
			}
			
			if (_data.methodChoice == 0)
			{
				_value = GetCurveValue(_data);
			}
			else
			{
				_value = GetFloatValue(_data);
			}
			
			return _value;
		}
		
		// Calcul
		private float GetCurveValue(FloatData _data)
		{
			return _data.curve.Evaluate(animationData.percentage) * _data.multiplier;
		}

		private float GetFloatValue(FloatData _data)
		{			
			if (animationData.percentage <= 0.5f)
			{
				return Mathf.Lerp(_data.startValue, _data.endValue, animationData.percentage * 2f) ;
			}

			return Mathf.Lerp(_data.endValue, _data.startValue, animationData.percentage*2f - 1f);
		}

	}
}