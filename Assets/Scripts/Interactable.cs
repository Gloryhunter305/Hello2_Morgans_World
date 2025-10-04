using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Variables")]
    Outline outline;
    public string nameofItem;
    public List<DialogueLine> Lines;

    // New: control whether this interactable changes the day
    [Header("Day Change (optional)")]
    public bool advanceDayOnInteract = false;       // enable day change for this interactable
    public bool advanceAfterDialogue = false;       // do the change after dialogue ends (true) or immediately on interact (false)
    public int advanceBy = 1;                       // how many days to advance (used if not setting specific day)
    public bool setDayToSpecific = false;           // set absolute day index instead of advancing
    public int setDayIndex = 0;                     // absolute day index to set when setDayToSpecific == true


    public UnityEvent fadingIn, fadingOut; // Event to trigger fading effect

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        // dialogueBox = GameObject.Find("DialogueBox");
        // dialogueBox.SetActive(false);
        // text = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        DisableOutline();
    }

    // Call this to perform the configured day change on the global DayMaster
    public void TriggerDayChange()
    {
        DayMaster dm = FindFirstObjectByType<DayMaster>();
        VignetteScript vs = FindFirstObjectByType<VignetteScript>();
        if (vs == null) return;
        if (dm == null) return;

        if (setDayToSpecific)
        {
            dm.currentDay = Mathf.Max(0, setDayIndex);
            Debug.Log($"DayMaster: set day to {dm.currentDay} via {name}");
        }
        else
        {
            if (dm.currentDay == 6) return; // Prevent going past day 6
            // start coroutine to advance days with delays
            StartCoroutine(AdvanceDaysCoroutine(dm));
        }
    }

    // Coroutine that advances days, invokes fade, teleports player, and waits between iterations
    private IEnumerator AdvanceDaysCoroutine(DayMaster dm)
    {
        int times = Mathf.Max(0, advanceBy);
        for (int i = 0; i < times; i++)
        {
            var player = FindFirstObjectByType<PlayerMove>();
            player.SetPlayerInteracting(true); // Prevent player movement during transition
            dm.AdvanceDay();
            fadingIn?.Invoke();   // Start fading effect

            yield return new WaitForSeconds(1f); // Wait for fade to complete (adjust as needed)

            // Teleport player close to bed location
            if (player != null)
                player.transform.position = new Vector3(1.81f, 0.3f, -18.1f);

            fadingOut?.Invoke();  // Fade back in
            yield return new WaitForSecondsRealtime(1f);
            player.SetPlayerInteracting(false); // Re-enable player movement
        }

        Debug.Log($"DayMaster: advanced {times} day(s) via {name}. New day: {dm.currentDay}");
    }

    public void DisableOutline()
    {
        if (outline != null) outline.enabled = false;
    }

    public void EnableOutline()
    {
        if (outline != null) outline.enabled = true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public string Text;
    public string Day;  //The day this dialogue line is associated with

}
