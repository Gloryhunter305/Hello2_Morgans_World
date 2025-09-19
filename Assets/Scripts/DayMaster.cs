using UnityEngine;

public class DayMaster : MonoBehaviour
{
    public int currentDay = 0;

    public void AdvanceDay()
    {
        currentDay++;
        Debug.Log("Day advanced to: " + currentDay);
    }
}
