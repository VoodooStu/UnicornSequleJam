using System.Collections.Generic;
using UnityEngine;

namespace VoodooPackages.Tech.Times
{
    public class DelayGameObjectActivation : Delay
    {
        public bool toActive = true;
        public List<GameObject> gameObjects = new List<GameObject>();
        protected override void Start()
        {
            DisplayGameObjects(!toActive);
            base.Start();
        }

        protected override void EndOfCountdown(Timer _timer)
        {
            DisplayGameObjects(toActive);
            base.EndOfCountdown(_timer);
        }

        /// <summary>
        /// Display/Hide referenced objects depending on _enable value
        /// </summary>
        /// <param name="_enabled">true displays objects</param>
        public void DisplayGameObjects(bool _enabled)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].SetActive(_enabled);
            }
        }
    }
}
