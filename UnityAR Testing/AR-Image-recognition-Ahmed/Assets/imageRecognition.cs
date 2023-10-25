using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class imageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placedPrefab;

    private Dictionary<string, GameObject> spawnedPrefab = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placedPrefab)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefab.Add(prefab.name, newPrefab);
        }
    }
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += imageChanged;
    }
    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= imageChanged;
    }
    private void imageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log($"{trackedImage.referenceImage.name} added to scene");
            updateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            Debug.Log($"{trackedImage.referenceImage.name} updated in scene");
            updateImage(trackedImage);
            trackedImage.destroyOnRemoval = true;
            if (trackedImage.trackingState == TrackingState.Limited)
            {
                Debug.Log($"{trackedImage.referenceImage.name}");
                spawnedPrefab[trackedImage.referenceImage.name].SetActive(false);
            }
            
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log($"{trackedImage.referenceImage.name} removed from scene");
            spawnedPrefab[trackedImage.referenceImage.name].SetActive(false);
            Destroy(spawnedPrefab[trackedImage.referenceImage.name]);
        }
    }
    private void updateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefab[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject go in spawnedPrefab.Values)
        {
            if (go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}
