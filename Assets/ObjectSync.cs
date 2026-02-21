using UnityEngine;

public class ObjectSync : MonoBehaviour
{
    [SerializeField] private string objectID;

    void Start()
    {
        if (SceneStateManager.Instance.IsDestroyed(objectID))
        {
            Destroy(gameObject);
        }
    }

    public void DestroyPersistent()
    {
        SceneStateManager.Instance.MarkDestroyed(objectID);
        Destroy(gameObject);
    }
}