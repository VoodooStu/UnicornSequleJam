using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VoodooPackages.Tech;

public class InputController : SingletonMB<InputController>
{
    public EventSystem _eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        Input.multiTouchEnabled = false;
    }

    public Image dragImage;
    public SeedBox _dragSeedBox;

    public SeedBox _nextSeedBox;
    public void DragSeedBegin(SeedBox _seedBox)
    {
        _dragSeedBox = _seedBox;
        dragImage.sprite = _seedBox._currentSeed._icon;
        dragImage.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_eventSystem.IsPointerOverGameObject())
            {
               
                
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 10000f))
                {
                    Debug.Log("Clicked on"+hit.transform.name);
                    PlantBox box = hit.transform.GetComponent<PlantBox>();
                    if(box!=null)
                        box.OnBoxSelect?.Invoke(box);
                }
            }
        }
        if (Input.GetMouseButton(0)&& _dragSeedBox!=null)
        {
            dragImage.rectTransform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(_nextSeedBox!=null && _dragSeedBox != null)
            {
                _nextSeedBox.DropCall?.Invoke(_nextSeedBox, _dragSeedBox);
            }
            else
            {
                if (_eventSystem.IsPointerOverGameObject())
                {


                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10000f))
                    {
                       
                        PlantBox box = hit.transform.GetComponent<PlantBox>();
                        if(box!=null)
                            box.PlantCallBack?.Invoke(box, _dragSeedBox);
                    }
                }
            }
            dragImage.gameObject.SetActive(false);
            _dragSeedBox = null;
            _nextSeedBox = null;
        }
        
    }
}
