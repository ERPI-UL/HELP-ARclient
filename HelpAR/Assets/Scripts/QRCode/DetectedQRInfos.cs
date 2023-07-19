using AimXRToolkit.Managers;
using AimXRToolkit.Models;
using Unity.VisualScripting;
using UnityEngine;

public class DetectedQRInfos : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro textType;
    [SerializeField] private TMPro.TextMeshPro textName;
    [SerializeField] private Transform popup;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float targetProgess;
    [SerializeField] private GameObject progressBar;

    public void displayQRInfos(QRInfos infos)
    {
        setTextsToInfos(infos);
        transform.position = infos.position;
        transform.rotation = infos.rotation;
    }

    public void setQRScanProgress(float progress)
    {
        targetProgess = progress;
    }

    private async void setTextsToInfos(QRInfos infos)
    {
        textType.text = infos.type.ToString().FirstCharacterToUpper();
        switch (infos.type)
        {
            case QRCodeType.Artifact:
                Artifact a = await DataManager.GetInstance().GetArtifactAsync(infos.id);
                textName.text = a.GetName();
                break;
            case QRCodeType.Workplace:
                Workplace w = await DataManager.GetInstance().GetWorkplaceAsync(infos.id);
                textName.text = w.GetName();
                break;
            default:
                textName.text = "- - - - -";
                break;
        }
    }

    void Update()
    {
        if (Camera.main != null)
        {
            popup.LookAt(Camera.main.transform);
            popup.localRotation *= Quaternion.Euler(-90f, 0f, 0f);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 4);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4);
        progressBar.transform.localScale = new Vector3(Mathf.Lerp(progressBar.transform.localScale.x, targetProgess, Time.deltaTime * 4), 1, 1);
    }

    public void setPosition(Vector3 pos)
    {
        if (targetPosition == null)
            transform.position = pos;
        targetPosition = pos;
    }

    public void setRotation(Quaternion rot)
    {
        if (targetRotation == null)
             transform.rotation = rot;
        targetRotation = rot;
    }
}
