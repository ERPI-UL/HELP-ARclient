using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] protected Panel[] panels;
    protected bool[] panelsStates;

    void Start()
    {
        initStates();
    }

    public void showPanel(int index)
    {
        initStates();
        Debug.Log("showPanel(" + index + ") on GameObject " + gameObject.name);
        
        for (int i = 0; i < panels.Length; i++)
        {
            if ((i == index) != panelsStates[i])
                togglePanel(i, i == index);
        }
    }

    public void togglePanel(int index, bool state)
    {
        panelsStates[index] = state;
        Panel panel = panels[index];
        if (state)
        {
            panel.gameObject.SetActive(true);
            Vector3 defaultPos = panel.transform.localPosition;
            DOTween.To(() => 0.1f, x =>
            {
                panel.transform.localPosition = defaultPos - new Vector3(0f, x, 0f);
            }, 0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                panel.transform.localPosition = defaultPos;
            });
            panel.triggerOnShow();
        } else
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void initStates()
    {
        if (panelsStates != null) return;
            
        panelsStates = new bool[panels.Length];
        for (int i = 0; i < panels.Length; i++)
        {
            panelsStates[i] = panels[i].gameObject.activeSelf ? true : false;
        }
    }
}
