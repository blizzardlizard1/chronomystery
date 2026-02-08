using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    [SerializeField] private GameObject promptObject;
    [SerializeField] private TMPro.TextMeshProUGUI promptText;

    private void Awake()
    {
        Instance = this;
        promptObject.SetActive(false);
    }

    public void Show(string text)
    {
        promptText.text = text;
        promptObject.SetActive(true);
    }

    public void Hide()
    {
        promptObject.SetActive(false);
    }
}