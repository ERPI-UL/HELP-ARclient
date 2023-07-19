using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityLoading : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro title;
    [SerializeField] private TMPro.TextMeshPro label;
    [SerializeField] private GameObject loading;

    public void setTitle(string text)
    {
        title.text = text;
    }

    public void setLabel(string text)
    {
        label.text = text;
    }

    public void setLoading(bool isLoading)
    {
        loading.SetActive(isLoading);
    }
}
