using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    public Canvas[] canvases; // Array to hold your different canvases

    public void ActivateCanvas(int canvasIndex)
    {
        // Loop through all canvases
        for (int i = 0; i < canvases.Length; i++)
        {
            // Activate the selected canvas and deactivate others
            canvases[i].enabled = (i == canvasIndex);
        }
    }
    void Awake()
    {
        ActivateCanvas(0);
    }

}
