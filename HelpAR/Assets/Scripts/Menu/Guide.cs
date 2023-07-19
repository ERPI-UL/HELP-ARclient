using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Guide : MonoBehaviour
{
    static HashSet<int> guidedMenus = new HashSet<int>();

    private AudioSource _audio_source;
    [SerializeField] AudioClip introMenuClip;
    
    public void sayMenuGuide(int menu)
    {
        if (guidedMenus.Contains(menu)) return;
        guidedMenus.Add(menu);
        
        if (menu == 0)
        {
            AudioSource source = getAudioSource();
            source.clip = introMenuClip;
            source.time = 0;
            source.Play();
        } else say(getGuide(menu)); // user should be connected, it's ok to do it
    }

    public void say(string text, string lang = "fr")
    {
        TTS tts = new TTS();
        tts.speak(text, Language.fromString(lang), getAudioSource());
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    private string getGuide(int index)
    {
        if (index == 0) return "";
        switch (index)
        {
            case 1:
                {
                    var user = AimXRManager.Instance.GetUser();
                    return "Bienvenue " + user.firstname + " " + user.lastname + ". Scannez l'ancre de votre environnement de travail avant de sélectionner une activité.";
                }
            case 2:
                {
                    return "Bien. Sélectionnez maintenant l'activité que vous souhaitez réaliser.";
                }
            case 3:
                {
                    return "Tout est prêt. Cliquez sur le bouton commencer pour lancer l'activité !";
                }
            default: return "";
        }
    }
}
