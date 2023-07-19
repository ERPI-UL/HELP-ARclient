using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Menu : MonoBehaviour
{
    private Transform border_t_l, border_t_r, border_b_l, border_b_r;
    private Transform border_t, border_b, border_l, border_r;
    [SerializeField] protected UnityEvent onShow;

    [Range(0, 40)]
    [SerializeField] protected int width;
    [Range(0, 40)]
    [SerializeField] protected int height;
    [Range(0, 10)]
    [SerializeField] protected int scale;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        resize();
#endif
    }

    public void triggerOnShow()
    {
        onShow.Invoke();
    }

    public void onFocused()
    {
        retreiveObjects();
        border_t.gameObject.SetActive(true);
        border_b.gameObject.SetActive(true);
        border_r.gameObject.SetActive(true);
        border_l.gameObject.SetActive(true);
    }

    public void onBlured()
    {
        retreiveObjects();
        border_t.gameObject.SetActive(false);
        border_b.gameObject.SetActive(false);
        border_r.gameObject.SetActive(false);
        border_l.gameObject.SetActive(false);
    }

    void resize()
    {
        float semiw = width / 2f;
        float semih = height / 2f;
        float scl = scale * 100;

        try
        {
            border_t_l.localPosition = new Vector3(-semiw, 0, semih);
            border_t_r.localPosition = new Vector3(semiw, 0, semih);
            border_b_l.localPosition = new Vector3(-semiw, 0, -semih);
            border_b_r.localPosition = new Vector3(semiw, 0, -semih);
            border_t_l.localScale = new Vector3(scl, scl, scl);
            border_t_r.localScale = new Vector3(scl, scl, scl);
            border_b_l.localScale = new Vector3(scl, scl, scl);
            border_b_r.localScale = new Vector3(scl, scl, scl);
            
            border_t.localPosition = new Vector3(-semiw, 0, semih + scl * 0.004f);
            border_b.localPosition = new Vector3(semiw, 0, -semih - scl * 0.004f);
            border_l.localPosition = new Vector3(-semiw - scl * 0.004f, 0, -semih);
            border_r.localPosition = new Vector3(semiw + scl * 0.004f, 0, semih);
            border_t.localScale = new Vector3(width * 125f, scl, scl);
            border_b.localScale = new Vector3(width * 125f, scl, scl);
            border_l.localScale = new Vector3(height * 125f, scl, scl);
            border_r.localScale = new Vector3(height * 125f, scl, scl);
        }
        catch { retreiveObjects(); }
    }
    void retreiveObjects()
    {
        border_t_l = FindTransform("border_t_l", null);
        border_t_r = FindTransform("border_t_r", null);
        border_b_l = FindTransform("border_b_l", null);
        border_b_r = FindTransform("border_b_r", null);
        border_t = FindTransform("border_t", null);
        border_b = FindTransform("border_b", null);
        border_l = FindTransform("border_l", null);
        border_r = FindTransform("border_r", null);
    }

    Transform FindTransform(string name, Transform root)
    {
        if (root == null) root = transform;
        if (root.name == name) return root;

        for (int i = 0; i < root.childCount; i++)
        {
            Transform res = FindTransform(name, root.GetChild(i));
            if (res != null) return res;
        }
        return null;
    }
}