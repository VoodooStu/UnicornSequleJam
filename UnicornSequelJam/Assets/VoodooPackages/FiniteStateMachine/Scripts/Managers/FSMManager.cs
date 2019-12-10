using UnityEngine;

namespace VoodooPackages.Tech
{
	public enum GamePhase
	{
		MAIN_MENU,
		GAME,
		SUCCESS,
		FAILURE
	}

	public class FSMManager : SingletonMB<FSMManager>
	{
		public delegate void OnGamePhaseChanged(GamePhase _oldPhase, GamePhase _newPhase);
		public static event OnGamePhaseChanged onGamePhaseChanged;

		public GamePhase CurrentPhase { get; private set; }

		private void Start()
		{
			ChangePhase(GamePhase.MAIN_MENU);
		}

#if UNITY_EDITOR

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.M))
				ChangePhase(GamePhase.MAIN_MENU);
			else if (Input.GetKeyDown(KeyCode.G))
				ChangePhase(GamePhase.GAME);
			else if (Input.GetKeyDown(KeyCode.S))
				ChangePhase(GamePhase.SUCCESS);
			else if (Input.GetKeyDown(KeyCode.F))
				ChangePhase(GamePhase.FAILURE);
		}

#endif

		/// <summary>
		/// Changes the phase.
		/// </summary>
		/// <param name="_GamePhase">Game phase.</param>
		public void ChangePhase(GamePhase _GamePhase)
		{
			if (onGamePhaseChanged != null)
				onGamePhaseChanged.Invoke(CurrentPhase, _GamePhase);

			CurrentPhase = _GamePhase;
		}
	}
}
