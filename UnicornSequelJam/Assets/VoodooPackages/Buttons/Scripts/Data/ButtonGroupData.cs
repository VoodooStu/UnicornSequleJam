using UnityEngine;

namespace VoodooPackages.Tech.Buttons
{
    [CreateAssetMenu(fileName = "Button Group Data", menuName = "VoodooPackages/Buttons/Button Group Data")]
    public class ButtonGroupData : ScriptableObject
    {
        public ButtonGroupVisual buttonGroupVisualPrefab;
        public int weight;
        public Sprite image;
        public Color color;
        public string text;
    }
}