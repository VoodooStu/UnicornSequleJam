using UnityEngine;

namespace VoodooPackages.Tech.Items
{
    public class ButtonFeatures : MonoBehaviour
    {
        public RectTransform origin;
        public RectTransform destination;

#pragma warning disable 108,114
        public ItemAnimationData animation;
#pragma warning restore 108,114
        
        public Item coin;
        public int coinQuantity;

        public Item gem;
        public int gemQuantity;

        public bool showEventsCallback = true;

        private void OnEnable()
        {
            ItemParticlesAnimator.Instance.ItemAppearing += ItemAppearing;
            ItemParticlesAnimator.Instance.OnAppearingCompleted += AppearingCompleted;
            ItemParticlesAnimator.Instance.ItemGrouped += ItemGrouped;
            ItemParticlesAnimator.Instance.OnGroupedCompleted += GroupedCompleted;
        }

        private void OnDisable()
        {
            ItemParticlesAnimator.Instance.ItemAppearing -= ItemAppearing;
            ItemParticlesAnimator.Instance.OnAppearingCompleted -= AppearingCompleted;
            ItemParticlesAnimator.Instance.ItemGrouped -= ItemGrouped;
            ItemParticlesAnimator.Instance.OnGroupedCompleted -= GroupedCompleted;
        }

        void ItemAppearing(ItemParticlesAnimation _itemToAnimate)
        {
            if (showEventsCallback)
            {
                Debug.Log("Appear " + _itemToAnimate.name);
            }
        }

        void AppearingCompleted(ItemParticlesAnimation _itemToAnimate)
        {
            if (showEventsCallback)
            {
                Debug.Log("Completed " + _itemToAnimate.name);
            }
        }

        void ItemGrouped(ItemParticlesAnimation _itemToAnimate)
        {
            if (showEventsCallback)
            {
                Debug.Log("Group " + _itemToAnimate.name);
            }
        }

        void GroupedCompleted(ItemParticlesAnimation _itemToAnimate)
        {
            if (showEventsCallback)
            {
                Debug.Log("TotalCompleted " + _itemToAnimate.name);
            }
        }
        
        
        
        
        // Buttons

        public void AddCoins()
        {
            ItemParticlesAnimator.Instance.NewGenericRewardAnimation(coin, animation, coinQuantity, origin,
                destination.position);
        }

        public void AddGems()
        {
            ItemParticlesAnimator.Instance.NewGenericRewardAnimation(gem, animation, gemQuantity, origin,
                destination.position);
        }

        public void Clear()
        {
            ItemParticlesAnimator.Instance.Clear();
        }
        
        
        
    }
}