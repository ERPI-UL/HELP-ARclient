using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QRScanner : MonoBehaviour, IQRScanProvider
{
    private const int PLAYING_SCAN = 1;
    private const int PLAYING_SCANNED = 2;
    private int soundPlaying = 0;
    private AudioSource _audio_source = null;
    [SerializeField] AudioClip scanClip;
    [SerializeField] AudioClip scannedClip;

    [SerializeField] TMPro.TextMeshPro _title;
    [SerializeField] TMPro.TextMeshPro _desc;
    private QRCodeDetection detect;
    [SerializeField] private DetectedQRInfos detectedQRPrefab;
    private Dictionary<string, DetectedQRInfos> detectedQR = new Dictionary<string, DetectedQRInfos>();
    private static readonly int NB_QR_POSES = 12;

    // Start is called before the first frame update
    void Start()
    {
        QRScanProvider.registerProvider(this);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTitle(string title)
    {
        _title.text = title;
    }

    public void setDescription(string description)
    {
        _desc.text = description;
    }

    public void beginUnknownQRScan(QRCodeType type, System.Action<QRInfos> onResult, System.Action<QRInfos, float> onProgress)
    {
        beginKnownQRScan(type, -1, onResult, onProgress);
    }

    public void beginKnownQRScan(QRCodeType type, int id, System.Action<QRInfos> onResult, System.Action<QRInfos, float> onProgress)
    {
        if (gameObject == null) return;

        gameObject.SetActive(true);

        if (detect != null)
        {
            detect.end();
            detect = null;
        }
        detect = QRCodesManager.Instance.scanForQR();
        Dictionary<string, List<Pose>> qrCodePoses = new Dictionary<string, List<Pose>>();
   
        UnityAction<QRInfos> OnInfos = (infos) =>
        {
            playSound(PLAYING_SCAN);
            bool gotPoses = qrCodePoses.TryGetValue(infos.content, out List<Pose> qrPoses);
            if (!gotPoses)
            {
                qrPoses = new List<Pose>();
                qrCodePoses.Add(infos.content, qrPoses);
            }

            if (detectedQRPrefab != null)
            {
                if (!detectedQR.ContainsKey(infos.content))
                {
                    GameObject instance = Instantiate(detectedQRPrefab.gameObject);
                    DetectedQRInfos popupInfos = instance.GetComponent<DetectedQRInfos>();
                    popupInfos.displayQRInfos(infos);
                    popupInfos.setQRScanProgress(0);
                    if (qrPoses.Count > 0)
                    {
                        Pose pose = getAvgPose(qrPoses);
                        popupInfos.setPosition(pose.position);
                        popupInfos.setRotation(pose.rotation);
                    }
                    else
                    {
                        popupInfos.setPosition(infos.position);
                        popupInfos.setRotation(infos.rotation);
                    }
                    detectedQR.Add(infos.content, popupInfos);
                    DOTween.To(() => 0f, x =>
                    {
                        instance.transform.localScale = Vector3.one * x;
                    }, 1f, 1f).SetEase(Ease.OutBounce);
                } else
                {
                    bool success = detectedQR.TryGetValue(infos.content, out DetectedQRInfos popup);
                    if (success)
                    {
                        popup.setPosition(infos.position);
                        popup.setRotation(infos.rotation);
                        popup.setQRScanProgress(qrPoses.Count / (float) NB_QR_POSES);
                    }
                }
            }

            if (infos.type != type) return;
            if (id == -1 || infos.id == id)
            {
                qrPoses.Add(new Pose(infos.position, infos.rotation));
                
                if (qrPoses.Count >= NB_QR_POSES)
                {
                    Pose pose = getAvgPose(filterPoses(qrPoses));

                    if (detect != null)
                    {
                        detect.end();
                        detect = null;
                    }

                    playSound(PLAYING_SCANNED);
                    deleteAllPopups();
                    StartCoroutine(disableWhenDone());
                    if (onResult != null)
                        onResult(new QRInfos(infos.content, pose.position, pose.rotation));
                } else
                {
                    if (onProgress != null)
                        onProgress(new QRInfos(infos.content, infos.position, infos.rotation), qrPoses.Count / NB_QR_POSES);
                }
            }
        };
        
        detect.onQRUpdated.AddListener(OnInfos);
    }

    private IEnumerator disableWhenDone()
    {
        yield return new WaitUntil(() => !getAudioSource().isPlaying);
    }

    private Pose getAvgPose(List<Pose> poses)
    {
        if (poses.Count <= 0)
            return new Pose();

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        poses.ForEach(pose =>
        {
            position += pose.position;
            rotation *= pose.rotation;
        });
        position /= poses.Count;
        rotation = poses[0].rotation;
        return new Pose(position, rotation);
    }

    private List<Pose> filterPoses(List<Pose> list, int maxRound = 5, float eliminationFactor = 2f)
    {
        List<Pose> result = new List<Pose>();
        list.ForEach(el => result.Add(el));

        bool valid = false;
        int nbRounds = 0;
        while (!valid && nbRounds >= maxRound && result.Count > 1)
        {
            valid = true;
            nbRounds++;

            Pose avgPose = getAvgPose(result);
            List<float> distances = new List<float>(result.Count);
            float avgAvgPose = 0;
            for (int i = 0;  i < result.Count; i++)
            {
                float dist = Vector3.Distance(avgPose.position, result[i].position);
                distances.Add(dist);
                avgAvgPose += dist;
            }
            avgAvgPose /= result.Count;
            if (avgAvgPose == 0f) break;

            for (int i = 0; i < distances.Count; i++)
            {
                if (distances[i] >= avgAvgPose * eliminationFactor)
                {
                    result.RemoveAt(i);
                    valid = false;
                    break;
                }
            }
        }

        return result;
    }

    public void cancelScan()
    {
        if (detect != null) detect.end();
        detect = null;
        deleteAllPopups();
        gameObject.SetActive(false);
    }


    public void deleteAllPopups()
    {
        foreach (KeyValuePair<string, DetectedQRInfos> entry in detectedQR)
            Destroy(entry.Value.gameObject);
        detectedQR.Clear();
    }

    public AudioSource getAudioSource()
    {
        if (_audio_source == null) _audio_source = GetComponent<AudioSource>();
        if (_audio_source == null) _audio_source = gameObject.AddComponent<AudioSource>();
        return _audio_source;
    }

    public void playSound(int sound)
    {
        if (sound == soundPlaying) return;

        AudioSource audio = getAudioSource();
        switch (sound)
        {
            case PLAYING_SCAN:
                audio.clip = scanClip;
                audio.loop = true;
                break;
            case PLAYING_SCANNED:
                audio.clip = scannedClip;
                audio.loop = false;
                break;
            default: break;
        }
        audio.time = 0f;
        audio.Play();
    }
}
