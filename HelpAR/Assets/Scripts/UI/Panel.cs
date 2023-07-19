using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    public UnityEvent onShow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void triggerOnShow()
    {
        onShow.Invoke();
    }
}
