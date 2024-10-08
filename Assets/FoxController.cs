using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoxController : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField] Rigidbody rigid;
    [SerializeField] Vector2 center;
    [SerializeField] float speed;
    [SerializeField] Camera cam;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
        speed = 3f;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.touchCount == 0) return;

        if(Input.touchCount > 0)
        {
            if(raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes))
            {
                //transform.position = hits[0].pose.position;
                Vector3 dir = hits[0].pose.position - transform.position;
                dir.y = 0;
                rigid.MovePosition(transform.position + dir * Time.deltaTime * speed);
                transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
    }
}
