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
    public string nameofItem;  //This is the message that will be displayed when the player hovers over the object
    public List<DialogueLine> Lines;
    // public int Index = 0;

    // [Header("Interaction Dialogue")]
    // public GameObject dialogueBox;
    // public TextMeshProUGUI text;
    

    // public UnityEvent onInteraction, playerFreeze, playerUnfreeze;

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

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public string Text;
}
