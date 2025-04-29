using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTabs : MonoBehaviour
{
    public GameObject signInPanel;
    public GameObject registerPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowSignIn()
    {
        signInPanel.SetActive(true);
        registerPanel.SetActive(false);

       
    }

    public void ShowRegister()
    {
        signInPanel.SetActive(false);
        registerPanel.SetActive(true);

       
    }
}
