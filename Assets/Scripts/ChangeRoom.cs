using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeRoom : MonoBehaviour
{   
    public bool open = true;
    public GameObject doorOpen;
    public GameObject doorClose;
    public ItemData unlockItem;

    [SerializeField] private string destinationScene;
    [SerializeField] private string roomID;
    [SerializeField] private Transform spawnPoint;

    public string RoomID => roomID;
    public Transform SpawnPoint => spawnPoint;

    private ISceneTransition Transition => MirrorManager.Instance?.Transition;

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Player") && !MirrorManager.Instance.IsTransitioning)
    //     {
    //         MirrorManager.TargetPortalID = roomID;
    //         MirrorManager.Instance.StartCoroutine(ChangeRoomRoutine());
    //     }
    // }

    void Start() {
        doorOpen.SetActive(false);
        doorClose.SetActive(false);

        UpdateDoor();
    }

    public void TryEntering() {
        if (open) {
            MirrorManager.TargetPortalID = roomID;
            MirrorManager.Instance.StartCoroutine(ChangeRoomRoutine());
        }
        else {
            var inv = PlayerController.Instance.GetComponent<PlayerInventory>();
            if (unlockItem && inv.HasItem(unlockItem)) {
                OpenDoor();
                DialogueDisplay.Instance.Show(unlockItem.name + " unlocked the door!");
            }
            else {
                DialogueDisplay.Instance.Show("The door is locked...");
            }
        }
    }

    private IEnumerator ChangeRoomRoutine()
    {
        MirrorManager.Instance.IsTransitioning = true;

        if (Transition != null)
            yield return Transition.OnExitScene();

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(destinationScene);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
        {
            Transition?.OnLoadProgress(loadOp.progress / 0.9f);
            yield return null;
        }

        if (Transition != null)
            yield return Transition.OnSceneReady();

        loadOp.allowSceneActivation = true;
        yield return null;

        if (Transition != null)
            yield return Transition.OnEnterScene();

        MirrorManager.Instance.IsTransitioning = false;
    }

    void OpenDoor() {
        open = true;
        UpdateDoor();
    }

    void UpdateDoor() {
        if (open)
        {
            doorOpen.SetActive(true);
        }
        else
        {
            doorClose.SetActive(true);
        }
    }
}