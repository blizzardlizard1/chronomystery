using UnityEngine;

public class TwineInteractable : MonoBehaviour
{
    public TwineDialogueBank dialogueBank;

    public void StartDialogue()
    {
        TwineDialogueBank.Entry fallback = null;

        foreach (var entry in dialogueBank.dialogues)
        {
            if (string.IsNullOrEmpty(entry.requiredTag))
            {
                fallback = entry;
                continue;
            }

            var obj = FindValidTaggedObject(entry.requiredTag);

            if (obj == null)
                continue;

            // inventory requirement
            bool hasAll = true;

            if (entry.itemsRequiredBeforeDialogue != null)
            {
                foreach (var item in entry.itemsRequiredBeforeDialogue)
                {
                    if (!PlayerInventory.Instance.HasItem(item))
                    {
                        hasAll = false;
                        break;
                    }
                }
            }

            if (!hasAll)
                continue;

            Load(entry);
            return;
        }

        if (fallback != null)
        {
            Load(fallback);
        }
    }

    private void Load(TwineDialogueBank.Entry entry)
    {
        TwineDialogueController.Instance.activeEntry = entry;
        TwineDialogueController.Instance.LoadTwine(entry.htmlFile.text);
        TwineDialogueController.Instance.StartDialogue();
    }

    private GameObject FindValidTaggedObject(string tag)
    {
        var objs = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in objs)
        {
            var sync = obj.GetComponent<ObjectSync>();

            if (sync == null)
                return obj;

            var mgr = SceneStateManager.Instance;

            if (mgr.IsDestroyed(sync.ObjectID))
                continue;

            bool hidden = sync.StartsHidden && !mgr.IsSpawned(sync.ObjectID);
            if (hidden)
                continue;

            foreach (var r in obj.GetComponentsInChildren<Renderer>())
                if (r.enabled)
                    return obj;
        }

        return null;
    }
}