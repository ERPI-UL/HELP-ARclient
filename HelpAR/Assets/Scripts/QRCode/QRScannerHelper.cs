#if UNITY_EDITOR

using AimXRToolkit.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class QRScannerHelper : Editor
{
    Vector3 qrPos, qrRot;
    QRCodeType type;
    int id;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // draw qr code controls zone
        GUILayout.Space(10);
        GUILayout.BeginVertical("QR Scanner options", "window");
        GUILayout.BeginHorizontal();
        GUILayout.Label("QR Position");
        qrPos.x = float.Parse(GUILayout.TextField(qrPos.x.ToString(), GUILayout.Width(50)));
        qrPos.y = float.Parse(GUILayout.TextField(qrPos.y.ToString(), GUILayout.Width(50)));
        qrPos.z = float.Parse(GUILayout.TextField(qrPos.z.ToString(), GUILayout.Width(50)));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("QR Rotation");
        qrRot.x = float.Parse(GUILayout.TextField(qrRot.x.ToString(), GUILayout.Width(50)));
        qrRot.y = float.Parse(GUILayout.TextField(qrRot.y.ToString(), GUILayout.Width(50)));
        qrRot.z = float.Parse(GUILayout.TextField(qrRot.z.ToString(), GUILayout.Width(50)));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("QR Type");
        type = (QRCodeType)EditorGUILayout.EnumPopup(type, GUILayout.Width(100));
        GUILayout.Label("QR Id");
        id = int.Parse(GUILayout.TextField(id.ToString()));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Trigger QR event"))
        {
            Debug.Log("Triggering fake " + type + " QR Code detection for id " + id);
            QRCodesManager.Instance.triggerFakeDetection(new QRInfos(
                "type=" + ((type == QRCodeType.Artifact) ? "artifact" : "workplace") + ";id=" + id,
                qrPos,
                Quaternion.Euler(qrRot)
            ));
            return;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}

#endif