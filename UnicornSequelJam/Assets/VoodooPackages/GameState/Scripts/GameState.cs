using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoodooPackages.Tech;

namespace VoodooPackages.Tech
{

    public class GameState : SingletonMB<GameState>
    {
        void OnEnable()
        {
            FSMManager.onGamePhaseChanged += ChangePhase;
        }

        void OnDisable()
        {
            FSMManager.onGamePhaseChanged -= ChangePhase;
        }

        public void ChangePhase(GamePhase _oldPhase, GamePhase _newPhase)
        {
            switch (_newPhase)
            {
                case GamePhase.MAIN_MENU:
                    MainMenuView.Instance.Show();
                    break;

                case GamePhase.GAME:
                    MainMenuView.Instance.Hide();
                    break;

                case GamePhase.SUCCESS:
                    SuccessView.Instance.Show();
                    break;

                case GamePhase.FAILURE:

                    FailureView.Instance.Show();
                    break;
            }
        }

    }
}
