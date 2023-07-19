using LitJson;
using Microsoft.MixedReality.QR;
using UnityEngine;

public class QRInfos
{
    public static QRInfos FromQRObject(QRCode code)
    {
        SpatialGraphCoordinateSystem coords = new SpatialGraphCoordinateSystem();
        coords.Id = code.SpatialGraphNodeId;
        coords.calculate();

        if (Camera.main == null)
        {
            Debug.LogError("Cannot create QRInfo : GameManager.Instance.cam is null");
            return null;
        }
        Vector3 qrShift = Camera.main.transform.TransformPoint(new Vector3(0.04f, -0.04f, 0.00f)) - Camera.main.transform.position;
        Quaternion qrRot = Quaternion.Euler(0f, 0f, 180f);
        QRInfos qr = new QRInfos(code.Data, coords.CurrentPose.position + qrShift, coords.CurrentPose.rotation * qrRot);
        if (!qr.isValid()) return null;
        return qr;
    }

    public string content;
    public JsonData data;
    public Vector3 position;
    public Quaternion rotation;
    public QRCodeType type { get; private set; }
    public int id { get; private set; }
    private bool _is_valid = false;

    public QRInfos(string cont, Vector3 pos, Quaternion rot)
    {
        content = cont;
        position = pos;
        rotation = rot;

        // qrcode content parsing
        if (!cont.Contains(';')) return;
        string[] parts = cont.Split(';');
        if (parts.Length < 2) return;
        if (!parts[0].Contains('=')) return;
        if (!parts[1].Contains('=')) return;
        string[] parts_type = parts[0].Split('=');
        string[] parts_id = parts[1].Split('=');
        if (parts_type.Length < 2) return;
        if (parts_id.Length < 2) return;
        if (parts_type[0] != "type") return;
        if (parts_id[0] != "id") return;
        string type_str = parts_type[1];
        bool isTypeValid = (type_str == "artifact" || type_str == "workplace");
        if (!isTypeValid) return;
        string id_str = parts_id[1];
        bool isIdValid = int.TryParse(id_str, out int id);
        if (!isIdValid) return;
        QRCodeType type = type_str == "artifact" ? QRCodeType.Artifact : QRCodeType.Workplace;

        this.type = type;
        this.id = id;
        _is_valid = true;
    }

    public bool isValid()
    {
        return _is_valid;
    }
}
