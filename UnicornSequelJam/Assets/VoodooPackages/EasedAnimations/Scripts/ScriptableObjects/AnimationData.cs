using System;
using UnityEngine;

namespace VoodooPackages.Tech.EasedAnimations
{
    [CreateAssetMenu(fileName = "New Animation Data", menuName = "VoodooPackages/Eased Animation/New Animation Data")]
    public class AnimationData : ScriptableObject
    {
        public Vector3Data position = new Vector3Data();
        public Vector3Data rotation = new Vector3Data();
        public Vector3Data scale    = new Vector3Data();

        public FloatData   alpha    = new FloatData();

        public RotateData  rotate   = new RotateData();

        public float       duration = 1;
        public float       beforeDelay;
        public float       afterDelay;

        public bool        loop;

        public float       percentage;

        public void Reset()
        {
            percentage = 0;
        }

    }
    
    [Serializable]
    public class Vector3Data
    {
        public bool toggle;

        public FloatData xyzData = new FloatData();
        public FloatData xData   = new FloatData();
        public FloatData yData   = new FloatData();
        public FloatData zData   = new FloatData();
    }

    [Serializable]
    public class FloatData
    {
        public bool toggle;

        public int  methodChoice = 0;
        public AnimationCurve curve;

        public float multiplier = 1;
        
        public float startValue;
        public float endValue;
        
    }

    [Serializable]
    public class RotateData
    {
        public bool     toggle;
        public Vector3  rotate;
        public float    multiplier = 1;
    }

}