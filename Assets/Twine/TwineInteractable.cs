using UnityEngine;

public class TwineInteractable : MonoBehaviour
{
    public TwineDialogueBank dialogueBank;

    public void StartDialogue()
    {
        if (dialogueBank == null)
        {
            Debug.LogError($"[{name}] No dialogue bank assigned.");
            return;
        }

        Debug.Log($"[{name}] Starting dialogue check...");

        TwineDialogueBank.Entry fallback = null;

        // 1. Check all tag-based entries
        foreach (var entry in dialogueBank.dialogues)
        {
            string tag = entry.requiredTag;

            if (string.IsNullOrEmpty(tag))
            {
                fallback = entry;
                continue;
            }

            var found = FindValidTaggedObject(tag);
            Debug.Log($"[{name}] Requires '{tag}' → found: {found}");

            if (found != null)
            {
                LoadEntry(entry);
                return;
            }
        }

        // 2. Use fallback
        if (fallback != null)
        {
            Debug.Log($"[{name}] Using fallback dialogue '{fallback.htmlFile.name}'.");
            LoadEntry(fallback);
            return;
        }

        Debug.LogWarning($"[{name}] No dialogues matched.");
    }

    private void LoadEntry(TwineDialogueBank.Entry entry)
    {
        TwineDialogueController.Instance.LoadTwine(entry.htmlFile.text);
        TwineDialogueController.Instance.StartDialogue();
    }


    private GameObject FindValidTaggedObject(string tag)
    {
        var objs = GameObject.FindGameObjectsWithTag(tag);
        var mgr = SceneStateManager.Instance;

        foreach (var obj in objs)
        {
            var sync = obj.GetComponent<ObjectSync>();

            // If object has no ObjectSync, treat it as normal
            if (sync == null)
                return obj;

            // Skip if marked destroyed
            if (mgr.IsDestroyed(sync.ObjectID))
                continue;

            // If startsHidden but has not been spawned yet → ignore
            bool isHidden = sync.StartsHidden && !mgr.IsSpawned(sync.ObjectID);
            if (isHidden)
                continue;

            // Check if at least one renderer is visible
            bool rendererVisible = false;
            foreach (var r in obj.GetComponentsInChildren<Renderer>())
            {
                if (r.enabled)
                {
                    rendererVisible = true;
                    break;
                }
            }

            if (!rendererVisible)
                continue;

            return obj;
        }

        return null;
    }
}