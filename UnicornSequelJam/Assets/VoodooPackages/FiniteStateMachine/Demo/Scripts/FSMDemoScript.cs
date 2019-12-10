using UnityEngine;

namespace VoodooPackages.Tech
{
	public class FSMDemoScript : MonoBehaviour
	{
		private FSMManager finiteStateMachineManager;
		public GameObject views;

		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			finiteStateMachineManager = FSMManager.Instance;
			FSMManager.onGamePhaseChanged += GamePhaseChanged;
		}

		private void GamePhaseChanged(GamePhase oldPhase, GamePhase newPhase)
		{
			Debug.Log(oldPhase + " => " + newPhase);

			if (views != null)
			{
				for (int i = 0; i < views.transform.childCount; i++)
				{
					Transform childView = views.transform.GetChild(i);
					bool isNewView = i == (int)newPhase;

					childView.gameObject.SetActive(isNewView);
				}
			}
		}

		public void ChangePhase(int gamePhase)
		{
			// 0 = GamePhase.MAIN_MENU
			// 1 = GamePhase.GAME
			// 2 = GamePhase.SUCCESS
			// 3 = GamePhase.FAILURE
			finiteStateMachineManager.ChangePhase((GamePhase)gamePhase);
		}
	}
}
