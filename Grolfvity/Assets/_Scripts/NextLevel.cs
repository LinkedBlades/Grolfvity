using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [Header("Next level suffix as string")]
    [SerializeField] string nextLevelName;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ball")
        {
            Debug.Log("Loading next level");
        }
    }
}
