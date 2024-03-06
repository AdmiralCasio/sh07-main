using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class imageTracking : MonoBehaviour
{
    [SerializeField]
    GameObject[] placedPrefab;

    Dictionary<string, GameObject> spawnedPrefab = new();
    ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placedPrefab)
        {
            var newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.SetActive(false);
            newPrefab.name = prefab.name;
            spawnedPrefab.Add(prefab.name, newPrefab);
        }
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += imageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= imageChanged;
    }

    void imageChanged(ARTrackedImagesChangedEventArgs eventArgs)
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
        }
    }

    void updateImage(ARTrackedImage trackedImage)
    {
        var name = trackedImage.referenceImage.name;
        var position = trackedImage.transform.position;

        var prefab = spawnedPrefab[name];
        prefab.transform.position = position;

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            Debug.Log($"{trackedImage.referenceImage.name} : now tracking in scene");
            prefab.SetActive(true);
        }
    }
}
