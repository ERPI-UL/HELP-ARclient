using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro text;
    [SerializeField] private GameObject menu;
    [SerializeField] private Vector3 shift;
    public static DebugPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += Application_logMessageReceived;
        }
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        string message = "[" + type.ToString().FirstCharacterToUpper() + "] - " + condition.Split('\n')[0];
        if (type == LogType.Exception)
        {
            string[] lines = stackTrace.Split('\n');
            int nbLignes = lines.Length;
            int maxlines = Math.Min(nbLignes, 15);
            for (int i = 0; i < maxlines; i++)
            {
                message += lines[i] + "\n";
            }
        }
        onMessage(message);
    }

// Start is called before the first frame update
void Start()
    {
        transform.localPosition = shift;
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        transform.position = (cam.transform.TransformPoint(shift) - cam.transform.position).normalized;
        transform.position = new Vector3(transform.position.x, cam.transform.position.y - 0.4f, transform.position.z);
        transform.LookAt(cam.transform);
    }

    public void onMessage(string msg)
    {
        string[] arr = (text.text + "\n" + msg).Split('\n');

        text.text = "";
        for (int i = Mathf.Max(0, arr.Length - 30); i < arr.Length; i++)
        {
            text.text += arr[i];
            if (i != arr.Length - 1)
            {
                text.text += "\n";
            }
        }
    }

    public void clear()
    {
        text.text = string.Empty;
    }
}
