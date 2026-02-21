using UnityEngine;

public class ObjectSync : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private string[] linkedObjectIDs;

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

        foreach (var id in linkedObjectIDs)
            SceneStateManager.Instance.MarkDestroyed(id);
            
        Destroy(gameObject);
    }
}