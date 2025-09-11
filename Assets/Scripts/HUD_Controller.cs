using UnityEngine;
using TMPro;

public class HUD_Controller : MonoBehaviour
{
    public static HUD_Controller instance;
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }
    
    public void HideInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
    
}
