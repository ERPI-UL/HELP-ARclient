using LitJson;
using Microsoft.MixedReality.QR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QRCodeType
{
    Artifact,
    Workplace
}

public class QRScanProvider
{
    static IQRScanProvider provider;
    public static void registerProvider(IQRScanProvider p)
    {
        Debug.Log("Provider registered");
        provider = p;
    }

    public static IQRScanProvider getProvider()
    {
        Debug.Log("Asking for provider");
        return provider;
    }
}
