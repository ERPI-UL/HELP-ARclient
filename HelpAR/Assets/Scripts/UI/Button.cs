using System;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    private Transform border_t_l, border_t_r, border_b_l, border_b_r;
    private Transform border_t, border_r, border_b, border_l;
    private Transform back_t_l, back_t_r, back_b_l, back_b_r;
    private Transform back_t, back_r, back_b, back_l;
    private Transform back_middle;
    private Transform back, borders, content;
    private BoxCollider boxCollider;

    [SerializeField] AudioClip onPressSound;
    [SerializeField] AudioClip onReleaseSound;
    [SerializeField] private AudioSource audioSource;

    private float pressAmount = 0f;
    private float targetPressAmount = 0f;
    private bool clicked = false;

    public UnityEvent onPress;
    public UnityEvent onRelease;
    [Range(0.01f, 0.3f)]
    [SerializeField] protected float width = 0.2f;
    [Range(0.01f, 0.3f)]
    [SerializeField] protected float height = 0.1f;
    [Range(0.01f, 0.3f)]
    [SerializeField] protected float depth = 0.02f;

    void Awake()
    {
        pressAmount = 0f;
        targetPressAmount = 0f;
        updateButtonParts();
    }

    // Start is called before the first frame update
    protected void start()
    {
        retreiveObjects();
        onResized();
    }

    // Update is called once per frame
    protected void update()
    {
#if UNITY_EDITOR
        if (boxCollider == null) retreiveObjects();
        if (boxCollider != null)
            onResized();
#endif
        pressAmount += (targetPressAmount - pressAmount) * Time.deltaTime * 20;
        updateButtonParts();
    }

    private void OnTriggerEnter(Collider col)
    {
        Vector3 localPosition = transform.InverseTransformPoint(col.ClosestPoint(transform.position));
        onStartColliding(localPosition);
    }

    private void OnTriggerStay(Collider col)
    {
        Vector3 localPosition = transform.InverseTransformPoint(col.ClosestPoint(transform.position));
        onCollision(localPosition);
    }

    private void OnTriggerExit(Collider col)
    {
        onEndColliding();
    }

    private void updateButtonParts()
    {
        float semid = depth / 2f;
        if (back == null) retreiveObjects();
        back.localPosition = new Vector3(0, 0, 0);
        borders.localPosition = new Vector3(0, 0, -semid + pressAmount * depth / 2f);
        content.localPosition = new Vector3(0, 0, -semid + pressAmount * depth / 2f);
    }

    protected void onResized()
    {
        float semiw = width / 2f;
        float semih = height / 2f;
        float radius = 0.004f;

        try
        {
            border_t_l.localPosition = new Vector3(-semiw, semih, 0);
            border_t_r.localPosition = new Vector3(semiw, semih, 0);
            border_b_l.localPosition = new Vector3(-semiw, -semih, 0);
            border_b_r.localPosition = new Vector3(semiw, -semih, 0);

            back_t_l.localPosition = new Vector3(-semiw, semih, 0);
            back_t_r.localPosition = new Vector3(semiw, semih, 0);
            back_b_l.localPosition = new Vector3(-semiw, -semih, 0);
            back_b_r.localPosition = new Vector3(semiw, -semih, 0);

            border_t.localPosition = new Vector3(-semiw, semih + radius, 0);
            border_r.localPosition = new Vector3(semiw + radius, semih, 0);
            border_b.localPosition = new Vector3(semiw, -semih - radius, 0);
            border_l.localPosition = new Vector3(-semiw - radius, -semih, 0);

            back_t.localPosition = new Vector3(-semiw, semih, 0);
            back_r.localPosition = new Vector3(semiw, semih, 0);
            back_b.localPosition = new Vector3(semiw, -semih, 0);
            back_l.localPosition = new Vector3(-semiw, -semih, 0);

            float borderSize = 0.008f;
            float borderY = height / borderSize;
            float borderX = width / borderSize;
            float backSize = 0.004f;
            float backY = height / backSize;
            float backX = width / backSize;

            border_t.localScale = new Vector3(borderX, 1, 1);
            border_b.localScale = new Vector3(borderX, 1, 1);
            border_l.localScale = new Vector3(borderY, 1, 1);
            border_r.localScale = new Vector3(borderY, 1, 1);

            back_t.localScale = new Vector3(backX, 1, 1);
            back_b.localScale = new Vector3(backX, 1, 1);
            back_l.localScale = new Vector3(backY, 1, 1);
            back_r.localScale = new Vector3(backY, 1, 1);

            back_middle.localPosition = new Vector3(-semiw, -semih, 0);
            back_middle.localScale = new Vector3(backX, backY, 1);

            boxCollider.size = new Vector3(width, height, depth) + new Vector3(0.01f, 0.01f, 0.01f);

            updateButtonParts();
        } catch
        {
            retreiveObjects();
        }
    }

    protected void onStartColliding(Vector3 pos)
    {
        targetPressAmount = 0f;
        Debug.Log("Button(" + gameObject.name + ") - onStartColliding");
    }

    protected void onCollision(Vector3 pos)
    {
        float newPressAmount = Math.Min((pos.z + depth / 2f) / depth, 0.5f) * 2f;
        Debug.Log("Button(" + gameObject.name + ") - onCollision : " + newPressAmount);

        if (newPressAmount > targetPressAmount + 0.8f) return;

        targetPressAmount = newPressAmount;
        if (targetPressAmount > 0.5f)
        {
            targetPressAmount = 0.9f;
            if (!clicked)
            {
                audioSource.clip = onPressSound;
                audioSource.time = 0f;
                audioSource.Play();
                onPress.Invoke();
                clicked = true;
            }
        }
        updateButtonParts();
    }

    protected void onEndColliding()
    {
        Debug.Log("Button(" + gameObject.name + ") - onEndColliding");
        targetPressAmount = 0f;
        if (clicked)
        {
            audioSource.clip = onReleaseSound;
            audioSource.time = 0f;
            audioSource.Play();
            Debug.Log("onRelease : " + onRelease.GetPersistentEventCount() + " events called");
            onRelease.Invoke();
            clicked = false;
        }
        pressAmount = 0f;
        targetPressAmount = 0f;
        updateButtonParts();
    }

    void retreiveObjects()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();

        border_t_l = FindTransform("border_t_l", null);
        border_t_r = FindTransform("border_t_r", null);
        border_b_l = FindTransform("border_b_l", null);
        border_b_r = FindTransform("border_b_r", null);
        back_t_l = FindTransform("back_t_l", null);
        back_t_r = FindTransform("back_t_r", null);
        back_b_l = FindTransform("back_b_l", null);
        back_b_r = FindTransform("back_b_r", null);
        border_t = FindTransform("border_t", null);
        border_r = FindTransform("border_r", null);
        border_b = FindTransform("border_b", null);
        border_l = FindTransform("border_l", null);
        back_t = FindTransform("back_t", null);
        back_r = FindTransform("back_r", null);
        back_b = FindTransform("back_b", null);
        back_l = FindTransform("back_l", null);
        back_middle = FindTransform("back_middle", null);

        back = FindTransform("back", null);
        borders = FindTransform("borders", null);
        content = FindTransform("content", null);
    }

    protected Transform FindTransform(string name, Transform root = null)
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
