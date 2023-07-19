using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class QRMark : MonoBehaviour
{
    GameObject[] corners = new GameObject[4];
    [Range(0.01f, 0.2f)]
    [SerializeField] float maxSize = 0.1f;
    [Range(0.01f, 0.2f)]
    [SerializeField] float minSize = 0.08f;
    [SerializeField] float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        retreiveCorners();
    }

    // Update is called once per frame
    void Update()
    {
        if (corners[0] == null) retreiveCorners();
        
        float ratio = (Mathf.Cos(Time.realtimeSinceStartup * speed) + 1f) / 2f;
        float size = minSize + (maxSize - minSize) * ratio;
        float semisize = size / 2f;

        corners[0].transform.localPosition = new Vector3(-semisize, semisize, 0);
        corners[1].transform.localPosition = new Vector3(semisize, semisize, 0);
        corners[2].transform.localPosition = new Vector3(-semisize, -semisize, 0);
        corners[3].transform.localPosition = new Vector3(semisize, -semisize, 0);
    }

    void retreiveCorners()
    {
        if (transform.childCount < 4) return;
        for (int i = 0; i < 4; i++)
        {
            corners[i] = transform.GetChild(i).gameObject;
        }
    }
}
