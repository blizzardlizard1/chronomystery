using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "NextScene";

    private void Start()
    {
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(sceneToLoad);
    }
}