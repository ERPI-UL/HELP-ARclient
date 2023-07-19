using AimXRToolkit;
using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ArtifactScannerMenu : MonoBehaviour
{
    bool firstScan = false;
    string startTTSsentence = "";
    string changeTTSsentence = "";
    AudioSource _audio_source;

    [SerializeField] private TMPro.TextMeshPro title;
    [SerializeField] private TMPro.TextMeshPro description;
    [SerializeField] private ActivityManager activityManager;
    private List<AnchorHighlighter> anchorHighlighters = new List<AnchorHighlighter>();
    [SerializeField] private AnchorHighlighter highlighterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onScanStarted()
    {
        firstScan = true;
    }

    public void onScanChanged(int artifactId)
    {
        // clearHighlighters();
        // AnchorHighlighter highlighter = Instantiate(highlighterPrefab);
        // highlighter.onCreated();

        sayScanTTS(artifactId);
        setScanPanelInfo(artifactId);
    }

    public void onScanEnded()
    {
        
        sayEndTTS();
    }

    void clearHighlighters()
    {
        for (int i = anchorHighlighters.Count - 1; i >= 0; i--)
        {
            if (anchorHighlighters[i] != null)
                anchorHighlighters[i].onScanEnd();
            anchorHighlighters.RemoveAt(i);
        }
    }

    private AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    private async void sayScanTTS(int artifactId)
    {
        Artifact art = await DataManager.GetInstance().GetArtifactAsync(artifactId);
        startTTSsentence = "Avant de commencer nous allons faire le tour des artéfacts de l'activité. " +
                           "Tout d'abord scannez l'artéfact " + art.GetName() + ".";

        string[] goodBoyWords = { "Bien", "Super", "Parfait" };
        string goodBoyWord = goodBoyWords[Random.Range(0, goodBoyWords.Length)];
        changeTTSsentence = goodBoyWord + ". Scannez maintenant l'artéfact " + art.GetName() + ".";

        TTS tts = new TTS();
        tts.speak(firstScan? startTTSsentence: changeTTSsentence, Language.FR, getAudioSource());
        firstScan = false;
    }

    private async void setScanPanelInfo(int artifactId)
    {
        Artifact art = await DataManager.GetInstance().GetArtifactAsync(artifactId);
        title.text = "Artéfact " + art.GetName();
        description.text = "Scannez l'ancre de l'artéfact " + art.GetName();
    }
    
    private async void sayEndTTS()
    {
        TTS tts = new TTS();
        tts.speak("Tout est prêt. Nous allons commencer l'activité.", Language.FR, getAudioSource());
        await Task.Delay(5000);
        gameObject.SetActive(false);
        await activityManager.NextAction();
    }

    public void goToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
