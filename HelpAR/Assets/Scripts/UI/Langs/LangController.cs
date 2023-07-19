using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class LangController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro langLabel;

    void Start()
    {
        displayCurrentLang();
    }

    public void nextLang()
    {
        int index = getCurrentLangIndex();
        if (index < 0) return;

        setLangAtIndex(index + 1);
        displayCurrentLang();
    }

    public void prevLang()
    {
        int index = getCurrentLangIndex();
        if (index < 0) return;

        setLangAtIndex(index - 1);
        displayCurrentLang();
    }

    public int getCurrentLangIndex()
    {
        int index = 0;
        foreach (var key in Lang.Langs.Keys)
        {
            if (key == Lang.Instance.getCode())
                return index;
            index++;
        }
        return -1;
    }

    public void setLangAtIndex(int index)
    {
        string[] keys = Lang.Langs.Keys.ToArray();
        // adding keys.length in case of negative value (ex: -1 for previous when first lang selected)
        int clampedIndex = (index + keys.Length) % keys.Length;

        Lang lang = Lang.Langs.GetValueOrDefault(keys[clampedIndex], null);
        if (lang != null) lang.apply();
    }

    private void displayCurrentLang()
    {
        if (langLabel == null) return;

        string text = Lang.Labels.GetValueOrDefault(Lang.Instance.getCode(), "");
        langLabel.text = text;
    }
}
