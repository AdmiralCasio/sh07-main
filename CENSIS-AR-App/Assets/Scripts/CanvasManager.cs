using Mapbox.Map;
using Mapbox.Unity.Map;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    
    public Canvas[] canvases;
    public AbstractMap map;
    public Camera mapCamera;

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

        int[] activeCanvases = { 0, 1, 3 };
        mapCamera.gameObject.SetActive(activeCanvases.Contains(canvasIndex));
        map.gameObject.SetActive(activeCanvases.Contains(canvasIndex));

        if (canvasIndex == 0)
        {
            mapCamera.rect = new Rect(mapCamera.rect.x, mapCamera.rect.y,  mapCamera.rect.width, 0.4f);
            map.UpdateMap();
        }

        else
        {
            mapCamera.rect = new Rect(mapCamera.rect.x, mapCamera.rect.y, mapCamera.rect.width, 1);
            map.UpdateMap();
        }
    }
    void Start()
    {
        ActivateCanvas(1);
        ActivateCanvas(0);
    }

}
