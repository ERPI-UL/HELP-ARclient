using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorHighlightManager : MonoBehaviour
{
    [SerializeField] private ArtifactScanner scanner;
    private IQRScanProvider QRScanner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnAnchorHighlights(int artifactId)
    {
        if (QRScanner == null) QRScanner = QRScanProvider.getProvider();
        QRScanner.beginUnknownQRScan(QRCodeType.Artifact, infos =>
        {

        }, (infos, progress) =>
        {

        });
    }
}
