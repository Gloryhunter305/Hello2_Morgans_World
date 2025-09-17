using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 3f;
    Interactable currentInteractable;

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
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
                Debug.Log("Not looking at interactable");
            }
        }
        else
        {
            DisableCurrentInteractable();   //not in range of anything
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
        HUD_Controller.instance.ShowInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUD_Controller.instance.HideInteractionText();
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
