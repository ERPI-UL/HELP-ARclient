using AimXRToolkit;
using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityEndMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject button;
    [SerializeField] private AudioSource _audio_source;

    // Start is called before the first frame update
    void Start()
    {
        TTS tts = new TTS();
        tts.speak("L'activité est maintenant terminée. Vous pouvez revenir à l'écran d'accueil", Language.FR, getAudioSource());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goHome()
    {
        SceneManager.LoadSceneAsync(0);
        button.SetActive(false);
        loading.SetActive(true);
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }
}
