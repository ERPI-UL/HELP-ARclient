using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using DG.Tweening;
using KtxUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject btn_restart;
    [SerializeField] private GameObject btn_home;
    [SerializeField] private GameObject btn_menu;
    [SerializeField] private GameObject res_panel;
    [SerializeField] private GameObject cam;
    [SerializeField] private TMPro.TextMeshPro title;
    [SerializeField] private WorkPlaceManager workplaceManager;
    [SerializeField] private ActivityManager activityManager;
    [SerializeField] private RessourcePanel ressourcePanel;
    [SerializeField] private GameObject btn_panel;
    [SerializeField] private GameObject choice_panel;
    [SerializeField] private TextButton choice_left;
    [SerializeField] private TextButton choice_right;
    private AudioSource _audio_source;
    private Vector3 targetPos;
    private Vector3 camPos = Vector3.zero;
    private Action _current_action;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (targetPos - transform.position) * Time.deltaTime * 4;
        camPos += (cam.transform.position - camPos) * Time.deltaTime * 4;
        transform.LookAt(transform.position * 2 - camPos);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
    
    public void toggleMenuButtons()
    {
        if (btn_home.activeSelf)
        {
            DOTween.To(() => 1f, menuAnimation, 0f, 0.25f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                btn_restart.SetActive(false);
                btn_home.SetActive(false);
            });
        } else
        {
            btn_restart.SetActive(true);
            btn_home.SetActive(true);
            DOTween.To(() => 0f, menuAnimation, 1f, 0.25f).SetEase(Ease.OutExpo);
        }
    }

    public void menuAnimation(float x)
    {
        btn_restart.transform.localPosition = new Vector3(-0.06f + x * 0.03f, 0f, 0f);
        btn_home.transform.localPosition = new Vector3(0.06f - x * 0.03f, 0f, 0f);
        res_panel.transform.localPosition = new Vector3(0f, x * 0.06f, 0f);
    }

    public void speak(string txt)
    {
        TTS tts = new TTS();
        tts.speak(txt, Language.fromString("fr"), getAudioSource());
    }

    public void speakActionDescription()
    {
        if (_current_action == null) return;
        speak(_current_action.GetDescription());
    }

    public void speakActionHint()
    {
        if (_current_action == null) return;
        speak(_current_action.GetHint());
    }

    public void onActionChanged(Action a)
    {
        if (a == null) return;
        Debug.Log("ControlPanel > OnActionChanged (" + a.GetId() + ")");
        _current_action = a;
        speakActionDescription();
        gotoActionPos(a);
        ressourcePanel.loadActionRessource(a);
        title.text = a.GetName();

        string type = a.GetActionType();
        if (type == "choice")
        {
            choice_left.setLabel(a.GetChoice().GetLeftText());
            choice_right.setLabel(a.GetChoice().GetRightText());
            choice_panel.gameObject.SetActive(true);
            btn_panel.gameObject.SetActive(false);
        } else
        {
            choice_panel.gameObject.SetActive(false);
            btn_panel.gameObject.SetActive(true);
        }
    }

    public void gotoActionPos(Action a)
    {
        bool success = workplaceManager.tryGetArtifactInstance(a.GetArtifact(), out ArtifactManager instance);
        if (!success)
        {
            Debug.LogError("Error : Cannot find instance for artifact " + a.GetArtifact());
            return;
        }

        Vector3 localPos = Utils.toUnityCoord(a.GetPosition());
        targetPos = instance.transform.TransformPoint(localPos);
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    public void goHome()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void restartActivity()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public async void makeLeftChoice()
    {
        choice_panel.gameObject.SetActive(false);
        btn_panel.gameObject.SetActive(true);
        await activityManager.JumpTo(_current_action.GetChoice().GetLeftActionId());
    }

    public async void makeRightChoice()
    {
        choice_panel.gameObject.SetActive(false);
        btn_panel.gameObject.SetActive(true);
        await activityManager.JumpTo(_current_action.GetChoice().GetRightActionId());
    }
}
