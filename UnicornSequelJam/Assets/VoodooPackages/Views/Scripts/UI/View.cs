using UnityEngine;

namespace VoodooPackages.Tech
{
	public class View<T> : SingletonMB<T> where T : MonoBehaviour
	{
		public delegate void StateEvent();
		public event StateEvent OnHide;
		public event StateEvent OnShow;
		public event StateEvent OnTransitionFinished;
	
		public float m_FadeInDuration = 0.3f;
		public float m_FadeOutDuration = 0.3f;
		public CanvasGroup m_Group;

		protected bool m_Visible;

		// Buffers
		private float m_StartTime;
		private float m_Duration;
		private bool m_InTransition;

		protected virtual void Awake()
		{
			Init();
		}

		protected virtual void Init()
		{
			m_Visible = false;
			m_Group.alpha = 0.0f;
			m_Group.interactable = false;
			m_Group.blocksRaycasts = false;
		}

		public virtual void Show()
		{
			Transition(true);
			OnShow?.Invoke();
		}

		public virtual void Hide()
		{
			if (m_Visible)
				Transition(false);
			
			OnHide?.Invoke();
		}

		private void Transition(bool _Visible)
		{
			m_Visible = _Visible;

			m_StartTime = Time.time;
			m_InTransition = true;
			m_Duration = _Visible ? m_FadeInDuration : m_FadeOutDuration;
			m_Group.interactable = false;
			m_Group.blocksRaycasts = false;
		}

		protected virtual void Update()
		{
			if (m_InTransition)
			{
				float time = Time.time - m_StartTime;
				float percent = time / m_Duration;

				if (percent < 1.0f)
				{
					m_Group.alpha = m_Visible ? percent : (1.0f - percent);
				}
				else
				{
					m_InTransition = false;
					m_Group.alpha = m_Visible ? 1.0f : 0.0f;
					m_Group.interactable = m_Visible;
					m_Group.blocksRaycasts = m_Visible;

					OnTransitionFinished?.Invoke();
				}
			}
		}
	}
}
