using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EditorCollider : MonoBehaviour
{
    private void Awake()
    {
        bool isInEditor = false;
#if UNITY_EDITOR
        isInEditor = true;
#endif
        if (!isInEditor) Destroy(gameObject);
    }
}
