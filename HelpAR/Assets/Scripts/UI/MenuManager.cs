using AimXRToolkit.Managers;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class MenuManager : MonoBehaviour
{
    [SerializeField] protected GameObject[] menus;
    [SerializeField] public UnityEvent<int> onMenuChanged;
    protected bool[] menusStates;
    protected Menu[] menusScripts;
    protected PanelManager[] menusPanels;
    private UIFollower uiFollower;

    [Range(0f, 2f)]
    [SerializeField] private float menusRadius = 1f;
    [Range(0, 90)]
    [SerializeField] private int menusAngle = 15;
    [Range(0, 2f)]
    [SerializeField] private float shift = 1f;

    // Start is called before the first frame update
    void Start()
    {
        menusStates = new bool[menus.Length];
        menusScripts = new Menu[menus.Length];
        menusPanels= new PanelManager[menus.Length];
        uiFollower = GetComponent<UIFollower>();
        
        for (int i = 0; i < menus.Length; i++)
        {
            menusStates[i] = menus[i].activeSelf;
            menusScripts[i] = menus[i].GetComponent<Menu>();
            menusPanels[i] = menus[i].GetComponent<PanelManager>();
        }

        bool userConnected     = AimXRManager.Instance.GetUser() != null;
        bool workplaceSelected = AimXRManager.Instance.GetWorkplaceId() != 0;
        bool activitySelected  = AimXRManager.Instance.GetActivityId() != 0;

        Debug.Log("userConnected :" + userConnected);
        Debug.Log("workplaceSelected :" + workplaceSelected);
        Debug.Log("activitySelected :" + activitySelected);


        setMenuProgress(0);
        if (userConnected)
        {
            menusPanels[0].showPanel(2);
            setMenuProgress(2);
        }
        if (workplaceSelected)
        {
            menusPanels[1].showPanel(1);
            setMenuProgress(3);
            menus[1].GetComponent<WorkplaceMenu>().selectWorkplace(AimXRManager.Instance.GetWorkplaceId(), AimXRManager.Instance.getWorkplacePose());
        }
        if (activitySelected)
        {
            menusPanels[2].showPanel(1);
            setMenuProgress(4);
            menus[2].GetComponent<ActivityMenu>().selectActivity(AimXRManager.Instance.GetActivityId());
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        updatePanelPos();
#endif
    }

    private void updatePanelPos()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            float angle = menusAngle * i;
            Vector3 pos = new Vector3(
                Mathf.Sin(angle * Mathf.Deg2Rad) * menusRadius,
                0,
                Mathf.Cos(angle * Mathf.Deg2Rad) * menusRadius - shift
            );
            menus[i].transform.parent.localPosition = pos;
            menus[i].transform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }

    public void setMenuProgress(int progress)
    {
        onMenuChanged.Invoke(progress);
        Debug.Log("MenuManager - setMenuProgress : " + progress);

        float margin = 30f;
        float angle = menusAngle * progress;
        if (uiFollower == null) uiFollower = GetComponent<UIFollower>();
        uiFollower.minAngle = - (margin + angle);
        uiFollower.maxAngle = margin;

        for (int i = 0; i < menus.Length; i++)
        {
            toggleMenu(i, progress);
        }
    }

    protected void toggleMenu(int index, int target)
    {
        bool state = (index <= target);
        GameObject menu = menus[index];
        Menu menuScript = menusScripts[index];
        
        if (index == target) menuScript.onFocused();
        else menuScript.onBlured();

        if (state == menusStates[index]) return;
        if (state)
        {
            menu.SetActive(true);
            DOTween.To(() => 0.1f, x =>
            {
                menu.transform.localPosition = new Vector3(-x, 0f, 0f);
            }, 0f, 0.5f).SetEase(Ease.OutExpo);
        }
        else { menu.SetActive(false); }

        
        if (state && menuScript != null)
        {
            menuScript.triggerOnShow();
        }
        menusStates[index] = state;
    }

    public void quitApp()
    {
        if (Application.isPlaying)
            Application.Quit();
    }
}
