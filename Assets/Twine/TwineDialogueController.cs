using UnityEngine;

public class TwineDialogueController : MonoBehaviour
{
    public static TwineDialogueController Instance;

    private TwineHTMLParser parser = new();
    public TwinePassage currentPassage;

    public TwineDialogueBank.Entry activeEntry;

    public TwineInteractable activeInteractable;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void LoadTwine(string html)
    {
        parser.LoadTwine(html);
    }

    public void StartDialogue()
    {
        if (parser.startPassageName == null)
        {
            Debug.LogError("No start passage.");
            return;
        }

        ShowPassage(parser.startPassageName);
    }

    public void ShowPassage(string name)
    {
        if (!parser.passages.TryGetValue(name, out var p))
        {
            Debug.LogError("Missing passage: " + name);
            return;
        }

        currentPassage = p;

        if (DialogueUI.Instance == null)
        {
            Debug.LogError("DialogueUI.Instance is null.");
            return;
        }

        DialogueUI.Instance.ShowPassage(p);

        if (p.voiceClip != null && Camera.main != null)
            AudioSource.PlayClipAtPoint(p.voiceClip, Camera.main.transform.position);
    }

    public void Choose(TwineChoice c)
    {
        if (IsEndDialogueChoice(c.targetPassageName))
        {
            if (DialogueUI.Instance != null)
                DialogueUI.Instance.Hide();

            OnDialogueEnded();
            return;
        }

        ShowPassage(c.targetPassageName);
    }

    public void OnDialogueEnded()
    {
        var entry = activeEntry;
        if (entry == null)
            return;

        var inv = PlayerInventory.Instance;

        if (inv == null)
        {
            Debug.LogError("PlayerInventory.Instance is null in OnDialogueEnded.");
            return;
        }

        // Remove items
        if (entry.itemsTakenFromPlayer != null)
        {
            foreach (var item in entry.itemsTakenFromPlayer)
            {
                if (item != null && inv.ContainsItemName(item.itemName))
                {
                    inv.RemoveItem(item);
                    Debug.Log("[Dialogue] Removed item: " + item.itemName);
                }
            }
        }

        // Give items
        if (entry.itemsGivenToPlayer != null)
        {
            foreach (var item in entry.itemsGivenToPlayer)
            {
                if (item != null && inv.AddItem(item))
                    Debug.Log("[Dialogue] Given item: " + item.itemName);
            }
        }

        // Optional persistent destruction of the NPC/object
        if (entry.destroyItself)
        {
            var sync = GetComponent<ObjectSync>();
            if (sync != null)
            {
                sync.DestroyPersistent();
            }
        }

        // Final entry: destroy this scene-local controller so no more dialogue can happen
        if (entry.isFinalEntry)
        {
            if (DialogueUI.Instance != null)
                DialogueUI.Instance.Hide();

            Debug.Log("[Dialogue] Final entry completed. Destroying dialogue controller.");
            Destroy(gameObject);
            return;
        }

        activeEntry = null;
        currentPassage = null;
    }

    private bool IsEndDialogueChoice(string target)
    {
        if (string.IsNullOrWhiteSpace(target))
            return false;

        string normalized = target.Trim().ToLowerInvariant();

        normalized = normalized.Replace("*", "")
                               .Replace(".", "")
                               .Replace("!", "")
                               .Replace("?", "")
                               .Trim();

        return normalized == "end dialogue";
    }
}