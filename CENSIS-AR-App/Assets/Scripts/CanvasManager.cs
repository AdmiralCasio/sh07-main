using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Map;
using System.Linq;

public class CanvasManager : MonoBehaviour
{
    public Canvas[] canvases; // Array to hold your different canvases
    public AbstractMap map;
    public Camera mapCamera;
    public void ActivateCanvas(int canvasIndex)
    {
        // Loop through all canvases
        for (int i = 0; i < canvases.Length; i++)
        {
            // Activate the selected canvas and deactivate others
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
