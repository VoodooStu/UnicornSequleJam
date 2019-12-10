using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VoodooPackages.Tech
{

    
    public class DemoWeightedRandomizer : MonoBehaviour
    {
        public TextMeshProUGUI result;
        
        private WeightedRandomizer<string> randomizer;

        // Start is called before the first frame update
        void Start()
        {
            randomizer = new WeightedRandomizer<string>();

            randomizer.AddElement("A", 2);
            randomizer.AddElement("B", 3);
            randomizer.AddElement("C", 4);
            randomizer.AddElement("D", 8);
        }

        void NewRandom()
        {
            result.text = randomizer.TakeOne();
        }
        
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                NewRandom();
            }
        }
    }
}
