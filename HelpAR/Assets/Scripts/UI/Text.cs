using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Text : MonoBehaviour
{
    [SerializeField] private LANG label;
    [SerializeField] private string text;
    [SerializeField] private float fontSize = 3;
    private bool mode_label = true;
    private TMPro.TextMeshPro textLabel;

    // Start is called before the first frame update
    void Start()
    {
        textLabel = GetComponent<TMPro.TextMeshPro>();
        textLabel.fontSize = fontSize;

        mode_label = string.IsNullOrEmpty(text);
        if (mode_label)
            Lang.Instance.registerTranslation(() =>
            {
                textLabel.text = Lang.Instance.getString(label);
            });
        else textLabel.text = text;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        mode_label = string.IsNullOrEmpty(text);
        if (textLabel == null) textLabel = GetComponent<TMPro.TextMeshPro>();
        textLabel.text = mode_label ? Lang.Instance.getString(label) : text;
        textLabel.fontSize = fontSize;
#endif
    }

    public void setLabel(LANG label)
    {
        if (textLabel == null) textLabel = GetComponent<TMPro.TextMeshPro>();
        textLabel.text = Lang.Instance.getString(label);
        this.label = label;
        mode_label = true;
    }

    public void setLabel(string text)
    {
        if (textLabel == null) textLabel = GetComponent<TMPro.TextMeshPro>();
        textLabel.text = text;
        this.text = text;
        mode_label = false;
    }
}
