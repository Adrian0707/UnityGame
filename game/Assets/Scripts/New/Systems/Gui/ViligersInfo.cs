using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViligersInfo : MonoBehaviour
{
    public Fireplace fireplace;
    void Start()
    {
      // fireplace = GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>();
       transform.Find("Text").GetComponent<TextMeshProUGUI>().text = 
       fireplace.fireplaceStats.maxViligers.Value.ToString()+"/"+ GameObject.FindGameObjectsWithTag("Viliger").Length;
    }

    public void Change()
    {
        if (fireplace = GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>())
        {
            if (transform.Find("Text") != null)
            {
                transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                   fireplace.fireplaceStats.maxViligers.Value.ToString() + "/" + GameObject.FindGameObjectsWithTag("Viliger").Length;
            }
        }
    }
}
