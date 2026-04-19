using UnityEngine;

public class TwineInteractable : MonoBehaviour
{
    public TwineDialogueBank dialogueBank;


    public void StartDialogue()
    {
        if (dialogueBank == null)
        {
            Debug.LogError($"[{name}] TwineInteractable: dialogueBank is not assigned.");
            return;
        }

        if (dialogueBank.dialogues == null || dialogueBank.dialogues.Length == 0)
        {
            Debug.LogError($"[{name}] TwineInteractable: dialogueBank has no dialogue entries.");
            return;
        }

        if (PlayerInventory.Instance == null)
        {
            Debug.LogError($"[{name}] TwineInteractable: PlayerInventory.Instance is null.");
            return;
        }

        TwineDialogueBank.Entry fallback = null;

        foreach (var entry in dialogueBank.dialogues)
        {
            if (entry == null)
                continue;

            bool hasTagRequirement = !string.IsNullOrEmpty(entry.requiredTag);
            bool hasInventoryRequirement = entry.itemsRequiredBeforeDialogue != null &&
                                           entry.itemsRequiredBeforeDialogue.Length > 0;

            // true fallback = no requirements at all
            if (!hasTagRequirement && !hasInventoryRequirement)
            {
                fallback = entry;
                continue;
            }

            if (hasTagRequirement)
            {
                var obj = FindValidTaggedObject(entry.requiredTag);
                if (obj == null)
                    continue;
            }

            if (hasInventoryRequirement)
            {
                bool hasAll = true;
                string missingItem = "";

                foreach (var required in entry.itemsRequiredBeforeDialogue)
                {
                    if (required == null)
                    {
                        Debug.LogWarning($"[{name}] TwineInteractable: one required item entry is null in '{entry.htmlFile?.name}'.");
                        continue;
                    }

                    if (!PlayerInventory.Instance.ContainsItemName(required.itemName))
                    {
                        hasAll = false;
                        missingItem = required.itemName;
                        break;
                    }
                }

                if (!hasAll)
                {
                    Debug.Log($"[Dialogue] Missing inventory item: {missingItem}");
                    continue;
                }
            }

            Load(entry);
            return;
        }

        if (fallback != null)
        {
            Load(fallback);
            return;
        }

        Debug.LogWarning($"[{name}] TwineInteractable: no matching dialogue and no fallback.");
    }

    private void Load(TwineDialogueBank.Entry entry)
    {
        if (TwineDialogueController.Instance == null)
        {
            Debug.LogWarning($"[{name}] No dialogue controller available.");
            return;
        }

        TwineDialogueController.Instance.activeEntry = entry;
        TwineDialogueController.Instance.currentAudioMap = entry.audioMap;
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