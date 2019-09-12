using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    public void reloadscene()
    {
        SceneManager.LoadScene(1);
    }

    public void titlescene()
    {
        SceneManager.LoadScene(0);
    }
}
