using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedMenuPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro username;
    [SerializeField] private PanelManager panelManager;

    // Start is called before the first frame update
    void Start()
    {
        onShow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onShow()
    {
        User user = AimXRManager.Instance.GetUser();
        if (user == null)
        {
            panelManager.showPanel(0);
        }
        else username.text = user.username;
    }
}
