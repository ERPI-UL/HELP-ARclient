using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pagination : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro counter;
    [SerializeField] private int pageNumber;
    [SerializeField] private int pageTotal;

    [SerializeField] private UnityEvent onPreviousClicked;
    [SerializeField] private UnityEvent onNextClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPageNumber(int page)
    {
        pageNumber = page;
        updateCounter();
    }

    public void setPageTotal(int page)
    {
        pageTotal = page;
        updateCounter();
    }

    private void updateCounter()
    {
        counter.text = pageNumber.ToString() + " / " + pageTotal.ToString();
    }

    public void triggerOnNext()
    {
        onNextClicked.Invoke();
    }

    public void triggerOnPrevious()
    {
        onPreviousClicked.Invoke();
    }
}
