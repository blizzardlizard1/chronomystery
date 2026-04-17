using UnityEngine;

public class TwineDialogueController : MonoBehaviour
{
    public static TwineDialogueController Instance;

    private TwineHTMLParser parser = new();
    public TwinePassage currentPassage;

    public TwineDialogueBank.Entry activeEntry;

    private void Awake()
    {
        Instance = this;
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

        DialogueUI.Instance.ShowPassage(p);

        if (p.voiceClip != null)
            AudioSource.PlayClipAtPoint(p.voiceClip, Camera.main.transform.position);
    }

    public void Choose(TwineChoice c)
    {
        if (c.targetPassageName == "*End dialogue*")
        {
            DialogueUI.Instance.Hide();
            OnDialogueEnded();
            return;
        }

        ShowPassage(c.targetPassageName);
    }

    public void OnDialogueEnded()
    {
        var entry = activeEntry;
        if (entry == null) return;

        var inv = PlayerInventory.Instance;

        // Remove items
        if (entry.itemsTakenFromPlayer != null)
        {
            foreach (var item in entry.itemsTakenFromPlayer)
            {
                if (inv.HasItemName(item.itemName))
                {
                    inv.RemoveItem(item);
                    Debug.Log("Removed: " + item.itemName);
                }
            }
        }

        // Give items
        if (entry.itemsGivenToPlayer != null)
        {
            foreach (var item in entry.itemsGivenToPlayer)
            {
                if (inv.AddItem(item))
                    Debug.Log("Given: " + item.itemName);
            }
        }
        if (entry.destroyItself) {
            var obj = gameObject.GetComponent<ObjectSync>();
            if (obj) {
                obj.DestroyPersistent();
            }
        }

    }
}