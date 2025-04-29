using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : MonoBehaviour
{
    public void Loadscenreg()
    {
        SceneManager.LoadScene("register");
    }
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
        SceneManager.LoadScene("restuarants");
    }

    public void LoadsceneC()
    {
        SceneManager.LoadScene("menucasa");
    }

    public void LoadsceneA()
    {
        SceneManager.LoadScene("accomdations");
    }

    public void LoadsceneBook()
    {
        SceneManager.LoadScene("booking");
    }
}
