using UnityEngine;

namespace VoodooPackages.Tool.Shop
{
    public class RotateObject : MonoBehaviour
    {
        public float speed = 25;
        public Vector3 rotateAxis = new Vector3(-1,1,1);
        
        public void Update()
        {
            transform.Rotate(rotateAxis, speed*Time.deltaTime);
        }
    }
}