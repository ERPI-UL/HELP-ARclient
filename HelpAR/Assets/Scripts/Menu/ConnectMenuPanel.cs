using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using UnityEngine;
using UnityEngine.Events;

public class ConnectMenuPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro easyCode;
    public UnityEvent onConnected;

    // Start is called before the first frame update
    void Start()
    {

    }

    public async void init()
    {
        Debug.Log("Asking for easy code");
        EasyConnect easy = new EasyConnect();
        easy.OnCodeChanged.AddListener(code =>
        {
            Debug.Log("Got easy code " + code);
            easyCode.text = code;
        });
        easy.OnTokenReceived.AddListener(async token =>
        {
            Debug.Log("Got user token : " + token);
            var user = await User.GetUserFromToken(token);
            AimXRManager.Instance.SetUser(user);
            AimXRManager.Instance.GetUser().language = Lang.Instance.getCode();
            onConnected.Invoke();
        });
        await easy.LaunchAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disconnectUser()
    {
        AimXRManager.Instance.SetUser(null);
    }
}
