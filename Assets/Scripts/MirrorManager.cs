using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    public static MirrorManager Instance { get; private set; }
    public static string TargetPortalID { get; set; }

    [Header("Transition")]
    [SerializeField] private FadeTransition fadeTransition;

    public ISceneTransition Transition => fadeTransition;
    public bool IsTransitioning { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}