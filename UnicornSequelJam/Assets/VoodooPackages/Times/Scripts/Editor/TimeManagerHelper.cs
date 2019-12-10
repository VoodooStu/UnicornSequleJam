using UnityEditor;

namespace VoodooPackages.Tech.Times 
{ 
    public static class TimeManagerHelper
    {
        [MenuItem("VoodooPackages/Times/Next Day")]
        static void NextDay()
        {
            TimeManager.IncreaseDayCount();
        }
    }
}