using AimXRToolkit.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AimXRToolkit.Models;

public class ArtifactScanner : MonoBehaviour
{
    [SerializeField] private UnityEvent onScanStart;
    [SerializeField] private UnityEvent<int> onScanChange;
    [SerializeField] private UnityEvent onScanEnd;
    [SerializeField] private WorkPlaceManager workplaceManager;

    private List<ArtifactInstance> instances = new List<ArtifactInstance>();

    public void scanArtifacts()
    {
        Workplace wp = workplaceManager.GetWorkplace();
        wp.GetArtifacts().ForEach(instance => { instances.Add(instance); });
        onScanStart.Invoke();
        scanNextArtifact();
    }

    private void scanNextArtifact()
    {
        if (instances.Count <= 0)
        {
            onScanEnd.Invoke();
            return;
        }
        
        int instanceId = instances[0].id;
        int artifactId = instances[0].artifactId;
        instances.RemoveAt(0);

        onScanChange.Invoke(artifactId);

        IQRScanProvider qrScanner = QRScanProvider.getProvider();
        qrScanner.beginKnownQRScan(QRCodeType.Artifact, artifactId, (infos) =>
        {
            Dictionary<int, ArtifactManager> artifacts = workplaceManager.GetArtifacts();
            float distanceToAnchor = -1;
            Debug.Log("Finding candidate for artifact " + artifactId + " among " + artifacts.Count + " instances ...");
            foreach (int instanceId in artifacts.Keys)
            {
                Debug.Log("> Trying with instance " + instanceId);
                bool success = artifacts.TryGetValue(instanceId, out ArtifactManager manager);
                if (!success) return; // what ?
                if (manager.GetArtifact().GetId() != artifactId)
                {
                    Debug.Log("< Not the right artifact (" + manager.GetArtifact().GetId() + " != " + artifactId + ")");
                    continue;
                }

                Vector3 artifactPosition = Utils.toUnityCoord(manager.GetArtifact().GetAnchor().GetPosition());
                float distance = Vector3.Distance(artifactPosition, infos.position);
                if (distanceToAnchor < 0 || distance < distanceToAnchor)
                {
                    distanceToAnchor = distance;
                    Debug.Log("= Found candidate to artifact " + artifactId + " : " + instanceId);
                    workplaceManager.setArtifactInstance(artifactId, instanceId);
                    Transform artifactAnchor = manager.transform.parent;
                    artifactAnchor.position = infos.position;
                    artifactAnchor.rotation = infos.rotation;
                    scanNextArtifact();
                    break;
                } else
                {
                    Debug.Log("< Too far from scanned anchor (" + distance + "m)");
                }
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
