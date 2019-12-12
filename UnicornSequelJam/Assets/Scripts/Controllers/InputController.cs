using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VoodooPackages.Tech;
using VoodooPackages.Tech.Items;

public class InputController : SingletonMB<InputController>
{
    public EventSystem _eventSystem;
    public GameObject _chestObject;
    public GameObject _plantParticle;
    public GameObject _touchParticle;
    // Start is called before the first frame update
    void Start()
    {
        dragImage.gameObject.SetActive(false);
        Input.multiTouchEnabled = false;
    }
    private void OnApplicationFocus(bool focus)
    {
        //Debug.Log("Foxus" + focus);
        //Cursor.visible = !focus;
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
            //if (_touchParticle != null)
            //{
            //    _touchParticle.SetActive(false);
            //    _touchParticle.SetActive(true);
            //    _touchParticle.transform.position = Input.mousePosition;
            //}
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
                    if (box != null && box.OnBoxSelect!=null)
                    {
                        StartCoroutine(WaitAndTrigger(() => {

                            box.OnBoxSelect?.Invoke(box);
                        }));
                        

                    }
                       
                    else if(hit.transform.gameObject == _chestObject)
                    {
                        _chestObject.SetActive(false);
                        if (ItemParticlesAnimator.Instance != null)
                        {
                            ItemParticlesAnimator.Instance.NewGenericRewardAnimation(CurrencyManager.Instance.MainCurrency, ItemParticlesAnimator.Instance.DefaultAnimation, 100, Input.mousePosition, IncrementalController.Instance.CurrencyDestination.transform.position);
                            StartCoroutine(SlowlyAddToCurrency());

                        }
                    }
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
                else if(_dragSeedBox!=null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10000f))
                    {
                       
                        PlantBox box = hit.transform.GetComponent<PlantBox>();
                        if (box != null)
                        {
                            GameObject g = Instantiate(_plantParticle, this.transform);
                            g.transform.position = box.transform.position + new Vector3(0,0.1f,0);
                            Destroy(g, 1f);
                            box.PlantCallBack?.Invoke(box, _dragSeedBox);
                            StartCoroutine(WaitAndTrigger(()=> {

                                if (ItemParticlesAnimator.Instance != null)
                                {
                                    ItemParticlesAnimator.Instance.NewGenericRewardAnimation(CurrencyManager.Instance.MainCurrency, ItemParticlesAnimator.Instance.DefaultAnimation, 100, new Vector3(Screen.width / 2, Screen.height / 2, 0)/*dragImage.gameObject.transform*/, IncrementalController.Instance.CurrencyDestination.transform.position);
                                    StartCoroutine(SlowlyAddToCurrency());

                                }
                            }));
                           
                        }
                           
                    }
                }
            }
            dragImage.gameObject.SetActive(false);
            _dragSeedBox = null;
            _nextSeedBox = null;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _chestObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (NewAreaUnlocked.Instance != null)
            {
                NewAreaUnlocked.Instance.Show();
            }
        }

    }

    private IEnumerator WaitAndTrigger(Action onEnd)
    {
        yield return new WaitForSeconds(0.5f);
        onEnd?.Invoke();
    }

    private IEnumerator SlowlyAddToCurrency()
    {
        yield return new WaitForSeconds(0.5f);
            int count = 0;
        while (count < 100)
        {
            yield return null;
            count += 2;
            CurrencyManager.Instance.AddToMainCurrency(2);
        }
        
    }
}
