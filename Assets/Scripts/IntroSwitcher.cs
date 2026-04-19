using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private Button button;
    [SerializeField] private bool delay;

    private void Start()
    {   
        if (delay) 
            StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SwitchScene()
    {   
        SceneManager.LoadScene(sceneToLoad);
    }
}