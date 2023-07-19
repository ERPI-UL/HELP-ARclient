using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorHighlighter : MonoBehaviour
{
    [SerializeField] private AudioClip soundNotif;
    [SerializeField] private AudioClip soundScan;
    [SerializeField] private AudioClip soundScanned;
    private AudioSource _audio_source;
    private bool spinning = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void explode()
    {
        Vector3 defaultScale = transform.localScale;
        DOTween.To(() => 1f, x =>
        {
            transform.localScale = defaultScale * x;
        }, 1.3f, 2f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void spin()
    {
        DOTween.To(() => 0f, x =>
        {
            transform.localRotation = Quaternion.Euler(0f, x, 0f);
        }, 360f, 2f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            if (spinning) spin();
        });
    }

    private void stopSpin()
    {
        spinning = false;
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    public void onCreated()
    {
        spin();
        AudioSource source = getAudioSource();
        source.clip = soundNotif;
        source.time = 0;
        source.loop = true;
        source.Play();
    }

    public void onScanBegin()
    {
        stopSpin();
        AudioSource source = getAudioSource();
        source.clip = soundScan;
        source.time = 0;
        source.loop = true;
        source.Play();
    }

    public void onScanEnd()
    {
        stopSpin();
        explode();
        AudioSource source = getAudioSource();
        source.clip = soundScanned;
        source.time = 0;
        source.loop = false;
        source.Play();
    }

    public void onScanCancel()
    {
        Destroy(gameObject);
    }
}
