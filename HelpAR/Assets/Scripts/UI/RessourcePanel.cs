using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using AimXRToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using MoonSharp.Interpreter;
using DG.Tweening;

public class RessourcePanel : MonoBehaviour, IRessourceVisitor
{
    [SerializeField] private Material screen_material;
    [SerializeField] private MeshRenderer screen_renderer;
    [SerializeField] private VideoPlayer video_player;
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private Vector2 panel_size;
    Action _current_action = null;
    private bool showing = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void hide()
    {
        if (!showing) return;
        showing = false;
        Debug.Log("Hiding panel");
        DOTween.To(() => 1f, x =>
        {
            screen_renderer.transform.parent.localScale = Vector3.one * x;
        }, 0f, 0.4f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            screen_renderer.transform.parent.localScale = Vector2.zero;
        });
    }

    void show()
    {
        if (showing) return;
        showing = true;
        Debug.Log("Showing panel");
        DOTween.To(() => 0f, x =>
        {
            screen_renderer.transform.parent.localScale = Vector3.one * x;
        }, 1f, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            screen_renderer.transform.parent.localScale = Vector3.one;
        });
    }

    public async void loadActionRessource(Action a)
    {
        if (string.IsNullOrEmpty(a.GetRessourceName()))
        {
            Debug.Log("No ressource for action " + a.GetId());
            hide();
            audio_source.Stop();
            video_player.Stop();
        } else
        {
            try
            {
                _current_action = a;
                Debug.Log("Loading action " + a.GetId() + " ressource ...");
                Ressource res = await DataManager.GetInstance().GetActionRessourceAsync(a.GetId());
                Debug.Log("Adding visitor listener to ressource ...");
                video_player.Stop();
                audio_source.Stop();
                res.accept(this);
            }
            catch (RessourceNotFoundException)
            {
                Debug.Log("Ressource not found for action " + a.GetId());
                hide();
            }
        }
    }

    public void visit(AudioRessource res)
    {
        if (res.getActionId() != _current_action.GetId()) return; // action changed when loading
        Debug.Log("Got AudioRessource ressource call ! Processing ...");
        hide(); 
        audio_source.clip = res.GetAudioClip();
        audio_source.time = 0;
        audio_source.Play();
    }

    public void visit(ImageRessource res)
    {
        if (res.getActionId() != _current_action.GetId()) return; // action changed when loading
        Debug.Log("Got ImageRessource ressource call ! Processing ...");
        show();
        Texture tex = res.GetTexture();
        screen_material.mainTexture = tex;
        resizeScreenTo(tex.width, tex.height);
    }

    public void visit(VideoRessource res)
    {
        if (res.getActionId() != _current_action.GetId()) return; // action changed when loading
        Debug.Log("Got VideoRessource ressource call ! Processing ...");
        video_player.url = res.GetVideoUrl();
        video_player.isLooping = true;
        video_player.Play();
        StartCoroutine(waitForPlayer(video_player, () =>
        {
            if (res.getActionId() != _current_action.GetId()) // action changed when loading
            {
                video_player.Stop();
                return;
            }
            show();
            Vector2 dims = new Vector2(video_player.width, video_player.height);
            RenderTexture tex = new RenderTexture((int)dims.x, (int)dims.y, 24);
            video_player.targetTexture = tex;
            screen_material.mainTexture = tex;
            resizeScreenTo(dims.x, dims.y);
        }));
    }

    private IEnumerator waitForPlayer(VideoPlayer player, System.Action callback)
    {
        yield return new WaitUntil(() =>
        {
            if (player.isPrepared && callback != null) callback();
            return player.isPrepared;
        });
    }
    
    private void resizeScreenTo(float width, float height)
    {
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Error : Invalid ressource size (" + width + "x" + height + ")");
            return;
        }

        float resRatio = width / height;
        float screenRatio = panel_size.x / panel_size.y;
        Vector2 finalSize;
        
        if (resRatio >= screenRatio)
        {
            finalSize = new Vector2(panel_size.x, panel_size.y / resRatio);
        } else
        {
            finalSize = new Vector2(panel_size.x * resRatio, panel_size.y);
        }

        screen_renderer.transform.localScale = new Vector3(finalSize.x, finalSize.y, 1f);
        screen_renderer.transform.localPosition = new Vector3(0f, finalSize.y / 2, 0f);
    }
}
