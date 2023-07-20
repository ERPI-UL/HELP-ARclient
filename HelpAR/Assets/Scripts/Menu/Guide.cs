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

    static Dictionary<string, string[]> guides = new Dictionary<string, string[]>();

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

    public void say(string text, string lang = null)
    {
        if (string.IsNullOrEmpty(lang)) lang = Lang.Instance.getCode();

        TTS tts = new TTS();
        tts.speak(text, Language.fromString(lang), getAudioSource());
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    private void createGuideSentences() {
        var user = AimXRManager.Instance.GetUser();
        var mode = AimXRManager.Instance.mode;
        guides.Add("fr", new string[] {
            "Bienvenue " + user.firstname + " " + user.lastname + ". " +
            (mode == AimXRManager.MODE.MIXED_REALITY? "Scannez l'ancre de" : "sélectionnez") + " vôtre environnement de travail avant de sélectionner une activité.",
            "Bien. Sélectionnez maintenant l'activité que vous souhaitez réaliser.",
            "Tout est prêt. Cliquez sur le bouton commencer pour lancer l'activité !"
        });
        guides.Add("en", new string[] {
            "Welcome " + user.firstname + " " + user.lastname + ". " +
            (mode == AimXRManager.MODE.MIXED_REALITY? "Scan the anchor of" : "select") + " your working environment before selecting an activity.",
            "Good. Now select the activity you want to do.",
            "Everything is ready. Click on the start button to launch the activity !"
        });
        guides.Add("de", new string[] {
            "Willkommen " + user.firstname + " " + user.lastname + ". " +
            (mode == AimXRManager.MODE.MIXED_REALITY? "Scanne das Anker von" : "wähle") + " deine Arbeitsumgebung, bevor du eine Aktivität auswählst.",
            "Gut. Wählen Sie jetzt die Aktivität aus, die Sie ausführen möchten.",
            "Alles ist bereit. Klicken Sie auf die Schaltfläche Start, um die Aktivität zu starten !"
        });
        guides.Add("es", new string[] {
            "Bienvenido " + user.firstname + " " + user.lastname + ". " +
            (mode == AimXRManager.MODE.MIXED_REALITY? "Escanea el ancla de" : "selecciona") + " tu entorno de trabajo antes de seleccionar una actividad.",
            "Bien. Ahora selecciona la actividad que quieres hacer.",
            "Todo está listo. ¡Haz clic en el botón de inicio para iniciar la actividad!"
        });
        guides.Add("it", new string[] {
            "Benvenuto " + user.firstname + " " + user.lastname + ". " +
            (mode == AimXRManager.MODE.MIXED_REALITY? "Scansiona l'ancora di" : "seleziona") + " il tuo ambiente di lavoro prima di selezionare un'attività.",
            "Bene. Ora seleziona l'attività che vuoi fare.",
            "Tutto è pronto. Fai clic sul pulsante di avvio per avviare l'attività!"
        });
    }

    private string getGuideSentence(string code, int index) {
        if (guides.Values.Count == 0) createGuideSentences();

        bool success = guides.TryGetValue(code, out string[] sentences);
        if (!success) return "";
        if (index < 0 || index >= sentences.Length) return "";
        return sentences[index];
    }

    private string getGuide(int index)
    {
        if (index == 0) return "";
        string langCode = Lang.Instance.getCode();
        return getGuideSentence(langCode, index);
    }
}
