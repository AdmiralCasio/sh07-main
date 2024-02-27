using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    public Canvas[] canvases;

    /// <summary>
    /// Activates the canvas at index <paramref name="canvasIndex"/> of <see cref="canvases"/>.
    /// </summary>
    /// <seealso cref="Canvas"/>
    /// <param name="canvasIndex">the index of the canvas to be activated</param>
    public void ActivateCanvas(int canvasIndex)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].enabled = (i == canvasIndex);
        }
    }
    void Awake()
    {
        ActivateCanvas(1);
        ActivateCanvas(0);
    }

}
