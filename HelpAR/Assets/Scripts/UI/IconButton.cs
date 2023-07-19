using UnityEngine;

[ExecuteInEditMode]
public class IconButton : Button
{
    [SerializeField] private Sprite icon;
    [Range(0f, 1f)]
    [SerializeField] private float iconSize;
    private SpriteRenderer sprRend;

    // Start is called before the first frame update
    void Start()
    {
        start();
        sprRend = FindTransform("Icon").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        update();
#if UNITY_EDITOR
        if (sprRend == null)
        {
            sprRend = FindTransform("Icon").GetComponent<SpriteRenderer>();
            if (sprRend == null) return;
        }
        sprRend.sprite = icon;
        float size = Mathf.Min(width, height) * iconSize * 0.2f;
        sprRend.transform.localScale = new Vector3(size, size, size);
#endif
    }
}
