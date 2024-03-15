using System.Linq;
using Mapbox.Unity.Map;
using UnityEngine;

namespace CENSIS.Runtime
{
    /// <summary>
    /// Activates the canvas at index <paramref name="canvasIndex"/> of <see cref="canvases"/>.
    /// </summary>
    /// <seealso cref="Canvas"/>
    /// <param name="canvasIndex">the index of the canvas to be activated</param>

    public class CanvasManager : MonoBehaviour
    {
        public Canvas[] canvases; 
        public AbstractMap map;
        public Camera mapCamera;
        private void ActivateCanvas(int canvasIndex)
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].enabled = (i == canvasIndex);
            }

            int[] activeCanvases = { 0, 1, 3 };
            mapCamera.gameObject.SetActive(activeCanvases.Contains(canvasIndex));
            map.gameObject.SetActive(activeCanvases.Contains(canvasIndex));
            
            var rect = mapCamera.rect;
            if (canvasIndex == 0)
            {

               
                mapCamera.rect = new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    0.4f
                );
                map.UpdateMap();
            }
            else
            {
                mapCamera.rect = new Rect(
                    rect.x,
                    rect.y,
                    rect.width,
                    1
                );
                map.UpdateMap();
            }
        }

        void Start()
        {
            ActivateCanvas(1);
            ActivateCanvas(0);
        }
    }
}
