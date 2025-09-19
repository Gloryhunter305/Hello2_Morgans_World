using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    Interactable currentInteractable;

    GameObject dialogueBox;
    TextMeshProUGUI text;

    // dialogue state handled by PlayerInteract (not by Interactable)
    List<DialogueLine> currentLines;
    int index;
    Coroutine typingCoroutine;
    bool dialogueOpen;

    PlayerMove playerMove;

    void Start()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        if (dialogueBox)
        {
            dialogueBox.SetActive(false);
            text = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        }

        playerMove = FindFirstObjectByType<PlayerMove>();
    }

    void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueOpen)
            {
                // If typing, finish the current line immediately
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    // show full current line
                    if (currentLines != null && index - 1 >= 0 && index - 1 < currentLines.Count)
                        text.text = currentLines[index - 1].Text;
                }
                else
                {
                    // otherwise advance to next line
                    ShowNextLine();
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    // Start the dialogue using the interactable's Lines
                    StartDialogue(currentInteractable.Lines);
                }
            }
        }
    }

    void StartDialogue(List<DialogueLine> lines)
    {
        if (lines == null || lines.Count == 0) return;

        currentLines = lines;
        index = 0;
        dialogueOpen = true;
        FreezePlayer(true);

        if (dialogueBox)
            dialogueBox.SetActive(true);

        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLines == null) return;

        if (index < currentLines.Count)
        {
            typingCoroutine = StartCoroutine(WriteText(currentLines[index].Text));
            index++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator WriteText(string line)
    {
        text.text = "";
        foreach (char letter in line)
        {
            text.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        typingCoroutine = null;
    }

    void EndDialogue()
    {
        if (dialogueBox)
            dialogueBox.SetActive(false);

        dialogueOpen = false;
        currentLines = null;
        index = 0;
        typingCoroutine = null;
        FreezePlayer(false);
    }

    void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            playerMove.SetPlayerInteracting(true);
        }
        else
        {
            playerMove.SetPlayerInteracting(false);
        }
    }

    void CheckInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Interactable newInteractable = hit.collider.GetComponent<Interactable>();
            if (currentInteractable && newInteractable != currentInteractable)
            {
                currentInteractable.DisableOutline();
            }
            if (newInteractable != null)
            {
                SetNewCurrentInteractable(newInteractable);
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        if (currentInteractable != null && currentInteractable != newInteractable)
        {
            currentInteractable.DisableOutline();
        }
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        if (HUD_Controller.instance != null)
            HUD_Controller.instance.ShowInteractionText(currentInteractable.nameofItem);
    }

    void DisableCurrentInteractable()
    {
        if (HUD_Controller.instance != null)
            HUD_Controller.instance.HideInteractionText();
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
