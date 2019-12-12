using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportController : MonoBehaviour
{
    public static ViewportController Instance;

    [SerializeField]
    Viewport[] m_ViewportsArray = new Viewport[0];

    Dictionary<string, Viewport> m_Viewports = new Dictionary<string, Viewport>();

    public Viewport GetViewport(string name)
    {
        if (m_Viewports.ContainsKey(name))
            return m_Viewports[name];
        else
            return null;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        foreach(Viewport viewport in m_ViewportsArray)
        {
            m_Viewports.Add(viewport.name, viewport);
        }
    }
}
