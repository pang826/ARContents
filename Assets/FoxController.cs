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
        // 터치가 없을 경우 넘어감
        if(Input.touchCount == 0) return;
        // 터치가 1개 이상 있을 경우
        if(Input.touchCount > 0)
        {
            if(raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes))
            {
                // 순간이동
                //transform.position = hits[0].pose.position;

                // 이동하고자 하는 방향을 구하기
                Vector3 dir = hits[0].pose.position - transform.position;
                // 땅에 붙어있도록 y값은 0으로 설정
                dir.y = 0;
                // rigidbody를 이용한 움직임(이후에 FixedUpdate로 전환해야 할 듯)
                rigid.MovePosition(transform.position + dir * Time.deltaTime * speed);
                // 회전값을 카메라를 쳐다보도록 변경 (이후에 이동할때는 그쪽 방향을 쳐다보도록 bool과 코루틴을 활용해야 할 듯)
                transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
    }
}
