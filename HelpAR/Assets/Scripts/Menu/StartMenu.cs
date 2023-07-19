using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject loading;
    [SerializeField] GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartActivity()
    {
        SceneManager.LoadSceneAsync(1);
        loading.SetActive(true);
        button.SetActive(false);
    }
}
