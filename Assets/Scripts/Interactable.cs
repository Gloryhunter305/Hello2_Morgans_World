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

    // [Header("Fall / Respawn (optional)")]
    // public Transform fallStartPoint;       // where to place the player before the freefall (set in Inspector)
    // public Transform respawnPoint;         // where the player should be restored after the fall
    // public CanvasGroup screenFade;         // optional fade UI (set in Inspector)
    // public float preFallDelay = 2f;        // wait this many seconds after teleport, then start fall
    // public float fallbackDepthOffset = 10f; // used when fallStartPoint is not assigned

    // // Call this to teleport the player to the fall start, wait then start the fall+respawn sequence
    // public void TriggerTeleportAndFall(PlayerMove player)
    // {
    //     if (player == null)
    //         player = FindFirstObjectByType<PlayerMove>();
    //     if (player == null)
    //     {
    //         Debug.LogWarning("TriggerTeleportAndFall: no PlayerMove found");
    //         return;
    //     }

    //     // teleport player to configured fall start, or move them down by fallbackDepthOffset
    //     if (fallStartPoint != null)
    //         player.transform.position = fallStartPoint.position;
    //     else
    //         player.transform.position = player.transform.position + Vector3.down * fallbackDepthOffset;

    //     // optional: reset velocity immediately if a Rigidbody exists so freefall starts cleanly
    //     var rb = player.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.linearVelocity = Vector3.zero;
    //         rb.useGravity = true;
    //     }

    //     // start the wait + fall coroutine
    //     StartCoroutine(TeleportThenFallCoroutine(player));
    // }

    // private IEnumerator TeleportThenFallCoroutine(PlayerMove player)
    // {
    //     yield return new WaitForSeconds(preFallDelay);

    //     // Kick off the existing fall & respawn routine (this coroutine disables the player, applies downward force and teleports to respawnPoint)
    //     DoFallAndRespawn(player, respawnPoint, screenFade);
    // }

    // // Call this to perform the "fall out of world and respawn" effect
    // public void DoFallAndRespawn(PlayerMove player, Transform respawnPoint, CanvasGroup screenFade)
    // {
    //     StartCoroutine(FallAndRespawnCoroutine(player, respawnPoint, screenFade));
    // }

    // private IEnumerator FallAndRespawnCoroutine(PlayerMove player, Transform respawnPoint, CanvasGroup screenFade)
    // {
    //     // 1. Make player fall (disable controls, apply downward force or move down)
    //     player.enabled = false;
    //     Rigidbody rb = player.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.linearVelocity = Vector3.zero;
    //         rb.useGravity = true;
    //         rb.AddForce(Vector3.down * 10f, ForceMode.VelocityChange);
    //     }

    //     // 2. Fade screen to black
    //     float fadeTime = 1f;
    //     for (float t = 0; t < fadeTime; t += Time.deltaTime)
    //     {
    //         screenFade.alpha = Mathf.Lerp(0, 1, t / fadeTime);
    //         yield return null;
    //     }
    //     screenFade.alpha = 1;

    //     // 3. Wait a moment
    //     yield return new WaitForSeconds(0.5f);

    //     // 4. Teleport player to respawn point
    //     player.transform.position = respawnPoint.position;
    //     player.transform.rotation = respawnPoint.rotation;
    //     if (rb != null)
    //     {
    //         rb.linearVelocity = Vector3.zero;
    //         rb.useGravity = false;
    //     }

    //     // 5. Fade screen back in
    //     for (float t = 0; t < fadeTime; t += Time.deltaTime)
    //     {
    //         screenFade.alpha = Mathf.Lerp(1, 0, t / fadeTime);
    //         yield return null;
    //     }
    //     screenFade.alpha = 0;

    //     // 6. Re-enable player controls
    //     player.enabled = true;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        // dialogueBox = GameObject.Find("DialogueBox");
        // dialogueBox.SetActive(false);
        // text = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        DisableOutline();
    }

    // public void Interact()
    // {
    //     onInteraction.Invoke();  
    // }

    // public void ImprintDialogue()
    // {
    //     if (Index < Lines.Count)
    //     {
    //         playerFreeze.Invoke(); //Freeze player
    //         dialogueBox.SetActive(true);
    //         StartCoroutine(WriteText(Lines[Index].Text));
    //         // text.text = Lines[Index].Text;
    //         Index++;
    //     }
    //     else
    //     {
    //         text.text = "";
    //         Index = 0;
    //         dialogueBox.SetActive(false);
    //         playerUnfreeze.Invoke();     //Unfreeze player 
    //     }
    // }

    // private IEnumerator WriteText(string line)
    // {
    //     text.text = "";
    //     foreach (char letter in line)
    //     {
    //         text.text += letter;
    //         yield return new WaitForSecondsRealtime(0.05f);
    //     }
    // }

    // Call this to perform the configured day change on the global DayMaster
    public void TriggerDayChange()
    {
        DayMaster dm = FindFirstObjectByType<DayMaster>();
        if (dm == null) return;

        if (setDayToSpecific)
        {
            dm.currentDay = Mathf.Max(0, setDayIndex);
            Debug.Log($"DayMaster: set day to {dm.currentDay} via {name}");
        }
        else
        {
            if (dm.currentDay == 6) return; // Prevent going past day 6
            for (int i = 0; i < Mathf.Max(0, advanceBy); i++)
                dm.AdvanceDay();
            Debug.Log($"DayMaster: advanced {advanceBy} day(s) via {name}. New day: {dm.currentDay}");
        }
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
