using UnityEngine;
using UnityEngine.SceneManagement;

public class SmoothCameraFollow : MonoBehaviour
{
    public static SmoothCameraFollow Instance { get; private set; }

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject.transform.root.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject.transform.root.gameObject);

        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }
    
    public void SetTarget(Transform player)
    {
        if (player)
        {
            target = player;
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        // Find the player if we don't have one yet
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                return; // Player hasn't spawned yet, skip this frame
        }

        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}