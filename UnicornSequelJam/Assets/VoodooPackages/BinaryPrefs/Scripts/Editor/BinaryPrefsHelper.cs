using UnityEditor;

namespace VoodooPackages.Tech
{
    public static class BinaryPrefsHelper
    {
        [MenuItem("VoodooPackages/BinaryPrefs/Delete All")]
        public static void DeleteAll()
        {
            BinaryPrefs.DeleteAll();
        }
    }
}
