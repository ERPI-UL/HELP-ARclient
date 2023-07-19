using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ActivityBootup bootup;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private async void Start()
    {
        if (AimXRManager.Instance.GetUser() == null && bootup != null)
        {
            EasyConnect ease = new EasyConnect();
            ease.OnCodeChanged.AddListener(code => Debug.Log("EASYCONNECT CODE : " + code));
            ease.OnTokenReceived.AddListener(async token =>
            {
                AimXRManager.Instance.SetUser(await User.GetUserFromToken(token));
                bootup.init();
            });
            await ease.LaunchAsync();
        }
    }
}
