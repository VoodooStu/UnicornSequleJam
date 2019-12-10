using System;
using TMPro;
using UnityEngine;

namespace VoodooPackages.Tech.Times
{
    public class ActualTime : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private Timer timer;
        
        private void OnEnable()
        {
            timer = new Timer(1f, 0, 0f, true);            
            timer.Looped += NewSecond;
            timer.Start();
        }
        
        private void OnDisable()
        {
            timer?.Stop();
        }

        private void NewSecond(Timer _timer)
        {
            text.text = DateTime.Now.ToLongTimeString();
        }
        
        private void OnDestroy()
        {
            timer?.Dispose();
        }
    }
}