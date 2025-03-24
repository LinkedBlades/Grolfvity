using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolesManager : MonoBehaviour
{
    public int holesInLevel;

    public void GetHolesInLevel()
    {
        holesInLevel = GameObject.FindGameObjectsWithTag("Hole").Length;
        Debug.Log("Holes remaining: " + holesInLevel);
    }
    
    public bool HolesRemaining()
    {
        if(holesInLevel > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateHolesRemaining()
    {
        holesInLevel--;
    }

}
