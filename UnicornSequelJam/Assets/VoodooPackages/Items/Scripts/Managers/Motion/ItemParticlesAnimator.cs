using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VoodooPackages.Tech.Items
{
    public class ItemParticlesAnimator : SingletonMB<ItemParticlesAnimator>
    {
        // Pooling
        public ItemParticlesAnimation   itemToAnimatePrefab;
        
        //Parent
        public Transform                parentGameObjects;
        
        // Reward
        public List<ItemAnimationData>  animationScriptables = new List<ItemAnimationData>();
        
        private List<ItemParticlesAnimation> particles       = new List<ItemParticlesAnimation>();

        // Target
        private Transform               target;
        
        //Events
        public delegate void ItemAnimationCallback(ItemParticlesAnimation _itemToAnimate);

        public event ItemAnimationCallback ItemAppearing;
        public event ItemAnimationCallback OnAppearingCompleted;
        
        public event ItemAnimationCallback ItemGrouped;
        public event ItemAnimationCallback OnGroupedCompleted;

        public void OnDisable()
        {
            Clear();
        }

        /// <summary>
        /// Clear all prefabs into pooling.
        /// Use this only when you are sure that no animations are running anymore
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in transform) 
            {
                Destroy(child.gameObject);
            }

            particles.Clear();
        }
        
        /// <summary>
        /// Set a particle group ready to start animation
        /// </summary>
        /// <param name="_item"></param>
        public void SetUnused(ItemParticlesAnimation _item)
        {
            _item.transform.localScale = Vector3.one;
            _item.gameObject.SetActive(false);
            _item.isUsed = false;
        }

        /// <summary>
        /// Call this to start a new Generic Reward Animation
        /// </summary>
        /// <param name="_animationElement"></param>
        /// <param name="_quantity"></param>
        /// <param name="_origin"></param>
        /// <param name="_destination"></param>
        /// <param name="_delay"></param>
        public Guid NewGenericRewardAnimation(Item _item, ItemAnimationData _itemAnimationScriptable , int _quantity, Vector3 _origin, Vector3 _destination, float _delay=0)
        {
            if (_quantity <= 0)
            {
                return Guid.Empty;
            }

            ItemAnimation itemAnimationData = new ItemAnimation()
            {
                guid          = Guid.NewGuid(),
                itemId        = _item.id,
                sprite        = _item.icon,
                color         = _item.color,
                animationData = _itemAnimationScriptable,
                quantity      = _quantity,
                destination   = _destination,
                compt         = _quantity
            };
            
            List<ItemParticlesAnimation> items = new List<ItemParticlesAnimation>();

            ItemParticlesAnimation particle; 
            for (int i = 0; i < _quantity; i++)
            {
                particle = GetUnusedOrCreate();
                particle.transform.position     = _origin;
                particle.transform.localScale   = Vector3.zero;
                particle.itemAnimationData      = itemAnimationData;
                particle.SetUp();
                particle.gameObject.SetActive(true);
                items.Add(particle);
            }

            StartCoroutine(StartAppearing(itemAnimationData, items));

            return itemAnimationData.guid;
        }

        public ItemParticlesAnimation GetUnusedOrCreate() 
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                if (particles[i].isUsed == false)
                {
                    return particles[i];
                }
            }

            GameObject element = Instantiate(itemToAnimatePrefab.gameObject);
            element.transform.SetParent(parentGameObjects, false);
            
            ItemParticlesAnimation particle = element.GetComponent<ItemParticlesAnimation>();
            particles.Add(particle);
            return particle;
        }

        public Guid NewGenericRewardAnimation(Item _item, ItemAnimationData _itemAnimationScriptable, int _quantity, Transform _origin, Vector3 _destination, float _delay = 0)
        {
            Vector3 originPositon = GetAnimationSourceUIPosition(_origin);
            return NewGenericRewardAnimation(_item, _itemAnimationScriptable, _quantity, originPositon, _destination, _delay);
        }

        private Vector3 GetAnimationSourceUIPosition(Transform _source)
        {
            Canvas parentCanvas = _source.GetComponentInParent<Canvas>();

            if (parentCanvas != null && parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                return RectTransformUtility.WorldToScreenPoint(Camera.main, _source.position);
            
            return _source.position;
        }

        /// <summary>
        /// First animation sequence, appearing and getting distance from source position
        /// </summary>
        /// <param name="_itemAnimationData"></param>
        /// <param name="_items"></param>
        /// <param name="_delay"></param>
        /// <returns></returns>
        private IEnumerator StartAppearing(ItemAnimation _itemAnimationData, List<ItemParticlesAnimation> _items, float _delay = 0.0f)
        {
            yield return new WaitForSeconds(_delay);

            for (int i = 0; i < _items.Count; i++)
            {
                if (i == 0)
                {
                    Disc disc = _itemAnimationData.animationData.disc;
                    _items[i].SetAppearingToPosition(_items[i].transform.position + (Vector3)Random.insideUnitCircle *
                                                        (Random.Range(disc.radiusMin * disc.radiusFactor, disc.radiusMax * disc.radiusFactor)));
                    
                }
                else
                {
                    _items[i].SetAppearingToPosition(DispersionAroundCircle(_items[i].transform.position, _items[0].GetToPosition(), _items.Count, _itemAnimationData.animationData));
                }

                _items[i].StartAppearing();
            }

            
            yield return new WaitForSeconds(_itemAnimationData.animationData.delayAppearing);

            StartCoroutine(StartGrouping(_itemAnimationData, _items));

        }

        /// <summary>
        /// Called on appearing sequence complete
        /// </summary>
        /// <param name="_itemToAnimate"></param>
        public void OneItemAppearingCompleted(ItemParticlesAnimation _itemToAnimate)
        {
            if (_itemToAnimate.itemAnimationData.compt > 0)
            {
                ItemAppearing?.Invoke(_itemToAnimate);
            }
            
            _itemToAnimate.itemAnimationData.compt --;
            
            if(_itemToAnimate.itemAnimationData.compt <= 0)
            {
                _itemToAnimate.itemAnimationData.compt = _itemToAnimate.itemAnimationData.quantity;
                OnAppearingCompleted?.Invoke(_itemToAnimate);
            }
        }

        /// <summary>
        /// Second phase of animation, grouping toward destination
        /// </summary>
        /// <param name="_itemAnimationData"></param>
        /// <param name="_items"></param>
        /// <returns></returns>
        private IEnumerator StartGrouping(ItemAnimation _itemAnimationData, List<ItemParticlesAnimation> _items)
        {
            yield return new WaitForSeconds(_itemAnimationData.animationData.delayAfterAppear);
            
            for (int i = 0; i < _items.Count; i++)
            {
                if (_itemAnimationData.animationData.allTogether == false)
                {
                    yield return new WaitForSeconds(_itemAnimationData.animationData.delayBetweenRewards);
                }
                _items[i].StartGrouping(_itemAnimationData.destination);
            }
        }

        /// <summary>
        /// Per item callback once grouping is achieved
        /// </summary>
        /// <param name="_particle"></param>
        public void OneItemGroupingCompleted(ItemParticlesAnimation _particle)
        {
            if (_particle.itemAnimationData.compt > 0)
            {
                SetUnused(_particle);
                ItemGrouped?.Invoke(_particle);
            }
            
            _particle.itemAnimationData.compt --;
            
            if (_particle.itemAnimationData.compt <= 0)
            {
                _particle.itemAnimationData.compt = _particle.itemAnimationData.quantity;
                OnGroupedCompleted?.Invoke(_particle);
            }
        }

        private Vector3 DispersionAroundCircle(Vector3 _pointToPlace, Vector3 _lastPosition, int _numbers, ItemAnimationData _animation)
        {
            float   radAngle = (360f / _numbers) + Random.Range(0f, (_animation.dispersion * 360f)) * Mathf.Deg2Rad;
            Vector3 rotated  = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0f);
            Vector3 offset   = rotated * Random.Range(_animation.disc.radiusMin * _animation.disc.radiusFactor, _animation.disc.radiusMax * _animation.disc.radiusFactor);

            return _pointToPlace + offset;
        }

        Transform FindRootCanvasTransform(Transform _t)
        {
            Canvas parentCanvas = _t.GetComponentInParent<Canvas>();
            if (parentCanvas == null)
            {
                return _t;
            }

            return parentCanvas.rootCanvas.transform;
        }
    }
}