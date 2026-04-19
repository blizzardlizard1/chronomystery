using UnityEngine;

public class TwineDialogueController : MonoBehaviour
{
    public static TwineDialogueController Instance;

    private TwineHTMLParser parser = new();
    public TwinePassage currentPassage;

    public TwineDialogueBank.Entry activeEntry;

    public TwineInteractable activeInteractable;
    public GameObject gameManager;
    public GameObject inventory;
    
    private AudioSource audioSource;

    public TwineAudioMap currentAudioMap;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (!gameManager) {
            gameManager = SceneStateManager.Instance.gameObject;
        }

        if (!inventory) {
            inventory = GameObject.FindGameObjectWithTag("Inventory");
        }
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
        inventory.SetActive(false);
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

        PlayVoiceForPassage(p);
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
        inventory.SetActive(true);
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
            if (activeInteractable.TryGetComponent<ObjectSync>(out var sync))
            {
                sync.DestroyPersistent();
            }
        }

        // Optional changing tag for sequential entries
        if (!string.IsNullOrEmpty(entry.changeTag) && gameManager) {
            gameManager.tag = entry.changeTag;
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
        currentAudioMap = null;
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

    private void PlayVoiceForPassage(TwinePassage passage)
    {
        if (audioSource == null)
            return;

        audioSource.Stop();

        if (currentAudioMap == null || passage == null)
            return;

        AudioClip clip = currentAudioMap.GetClip(passage.pid);
        if (clip == null)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}