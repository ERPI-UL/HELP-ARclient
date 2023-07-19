using UnityEngine;

[ExecuteInEditMode]
public class TextButton : Button
{
    [SerializeField] private LANG label;
    [SerializeField] private string text;
    [SerializeField] private int fontSize = 16;
    private bool mode_label = true;
    private TMPro.TextMeshPro textLabel;

    // Start is called before the first frame update
    void Start()
    {
        start();
        textLabel = FindTransform("TextLabel").GetComponent<TMPro.TextMeshPro>();
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
        update();
#if UNITY_EDITOR
        mode_label = string.IsNullOrEmpty(text);
        if (textLabel == null) textLabel = FindTransform("TextLabel").GetComponent<TMPro.TextMeshPro>();
        textLabel.rectTransform.sizeDelta = new Vector2(width * 40, height * 20);
        textLabel.text = mode_label ? Lang.Instance.getString(label) : text;
        textLabel.fontSize = fontSize;
#endif
    }

    public void setLabel(LANG label)
    {
        if (textLabel == null) textLabel = FindTransform("TextLabel").GetComponent<TMPro.TextMeshPro>();
        textLabel.text = Lang.Instance.getString(label);
        this.label = label;
        mode_label = true;
    }

    public void setLabel(string text)
    {
        if (textLabel == null) textLabel = FindTransform("TextLabel").GetComponent<TMPro.TextMeshPro>();
        textLabel.text = text;
        this.text = text;
        mode_label = false;
    }
}
