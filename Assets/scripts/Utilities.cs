using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utilities : MonoBehaviour
{
    public string sceneTarget = "Intro";
    public bool timer = false;
    public float waitTime = 3f;

    // Use this for initialization
    void Start()
    {
        if (timer)
        {
            StartCoroutine(ScenesSwitcher());
        }
    }

    public void LoadSceneUI(string value)
    {
        GameControl.instance.LoadScene(value);
    }

    private IEnumerator ScenesSwitcher()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Cambio de escena");
        SceneManager.LoadScene(sceneTarget);
    }
}
