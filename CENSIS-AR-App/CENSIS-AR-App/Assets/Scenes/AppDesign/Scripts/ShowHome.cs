using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHome : MonoBehaviour
{
    public GameObject Background;

    public void OpenPanel()
    {
        Debug.Log("OpenPanel called");
        if (Background != null)
        {
            Background.SetActive(true);
            Debug.Log("Background should be set active");
        }
    }
}
