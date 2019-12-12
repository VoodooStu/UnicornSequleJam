using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class Viewport : MonoBehaviour
{
    CanvasGroup m_CanvasGroup;

    float   m_StartAlpha;

    public virtual void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_StartAlpha = m_CanvasGroup.alpha;
    }

    public virtual void Start()
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }

    public virtual void Show(float delay = 1)
    {
        m_CanvasGroup.DOKill(true);
        m_CanvasGroup.DOFade(m_StartAlpha, delay).OnComplete(() =>
        {
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;
        });
    }    

    public void CloseOption()
    {
        Hide(0.5f);
    }

    public virtual void Hide(float delay = 1, System.Action callback = null)
    {
        m_CanvasGroup.DOKill(true);
        m_CanvasGroup.DOFade(0, delay).OnComplete(() =>
        {
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;

            if(callback != null)
                callback.Invoke();
        });
    }
}
