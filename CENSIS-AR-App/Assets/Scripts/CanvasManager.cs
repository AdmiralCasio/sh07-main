using Mapbox.Map;
using Mapbox.Unity.Map;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    public Canvas[] canvases; // Array to hold your different canvases
    public Camera mapCamera;
    public AbstractMap map;

    public void ActivateCanvas(int canvasIndex)
    {
        // Loop through all canvases
        for (int i = 0; i < canvases.Length; i++)
        {
            // Activate the selected canvas and deactivate others
            canvases[i].enabled = (i == canvasIndex);
        }

        int[] mapCanvases = new int[] { 0, 1, 3 };

        mapCamera.gameObject.SetActive(mapCanvases.Contains(canvasIndex));
        map.gameObject.SetActive(mapCanvases.Contains(canvasIndex));

        if (canvasIndex == 0)
        {
            mapCamera.rect = new Rect(mapCamera.rect.x, mapCamera.rect.y, mapCamera.rect.width, 0.4f);
        }
        else
        {
            mapCamera.rect = new Rect(mapCamera.rect.x, mapCamera.rect.y, mapCamera.rect.width, 1);
        }
    }
    void Awake()
    {
        ActivateCanvas(1);
        ActivateCanvas(0);
    }

}
