using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQRScanProvider
{
    public void beginUnknownQRScan(QRCodeType type, System.Action<QRInfos> onResult, System.Action<QRInfos, float> onProgress = null);
    public void beginKnownQRScan(QRCodeType type, int id, System.Action<QRInfos> onResult, System.Action<QRInfos, float> onProgress = null);
    public void cancelScan();
}
