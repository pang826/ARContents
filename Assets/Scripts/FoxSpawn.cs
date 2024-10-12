using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoxSpawn : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField] GameObject fox;
    [SerializeField] Vector2 screenCenter;
    bool isSpawn;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        isSpawn = false;
    }
    private void Update()
    {
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        if (!isSpawn && raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            fox.SetActive(true);
            isSpawn = true;
            hits = null;
        }
    }
}
