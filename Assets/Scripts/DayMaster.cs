using TMPro;
using UnityEngine;

public class DayMaster : MonoBehaviour
{
    public int currentDay = 0;
    public GameObject finalGameObject;
    public TextMeshProUGUI dayText;

    public void AdvanceDay()
    {
        currentDay++;
        Debug.Log("Day advanced to: " + currentDay);
    }

    private void Update()
    {
        dayText.text = "Day: " + (currentDay + 1);

        if (currentDay >= 6 && finalGameObject != null)
        {
            finalGameObject.SetActive(true);
        }
    }
}
