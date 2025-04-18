using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : MonoBehaviour
{

    public void LoadsceneM()
    {
        SceneManager.LoadScene("menutaj");
    }

    public void LoadsceneH()
    {
        SceneManager.LoadScene("homepage");
    }

    public void LoadsceneR()
    {
        SceneManager.LoadScene("restaurants");
    }
}
