using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoxController : MonoBehaviour
{
    private IFoxState curState;

    public ARRaycastManager raycastManager;
    public List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public Rigidbody rigid;
    public float speed;
    Animator anim;
    public Camera cam;
    public bool isTrace;
    public bool ballSelect = false; // 공이 움직이고 있는지 여부
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        speed = 0.2f;
        anim = GetComponent<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isTrace = false;

        gameObject.SetActive(false);
    }
    private void Start()
    {
        curState = new FoxIdle(this);
    }

    // 현재 상태 전환 메서드
    public void SetState(IFoxState newState)
    {
        curState = newState;
    }

    // 애니메이션 속도 조절
    public void SetAnimSpeed(float speed)
    {
        anim.SetFloat("speed", speed);
    }

    private void Update()
    {
        
        // 공이 움직이고 있는 상태면 넘어감
        if (ballSelect)
        {
            return;
        }
        curState.OnInput();
    }

    private void FixedUpdate()
    {
        // 공이 움직이고 있는 상태면 넘어감
        if (ballSelect)
        {
            return;
        }
        curState.OnMove();
    }
        
    
    
    public IEnumerator Trace()
    {
        isTrace = true;
        while (isTrace)
        {
            yield return null;
            // 거리가 일정거리 이하가 되거나 손을 놓으면 종료
            if (Vector3.Distance(hits[0].pose.position, transform.position) <= 0.1f || Input.touchCount == 0)
            {
                isTrace = false;
            }
        }
        yield break;
    }
}




//  // 터치가 없을 경우 넘어감
//  if (Input.touchCount == 0)
//  {
//      if (!isTrace)
//      {
//          // 터치위치를 쫓아가는 중이 아니라면 카메라를 쳐다보도록 변경
//          transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
//      }
//      return;
//  }
//  
//  // 터치가 1개 이상 있을 경우
//  if (Input.touchCount > 0 && raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes))
//  {
//      if (!isTrace)
//      {
//          StartCoroutine(Trace());
//      }
//  
//      // 이동하고자 하는 방향을 구하기
//      Vector3 destination = Vector3.MoveTowards(transform.position, hits[0].pose.position, speed * Time.fixedDeltaTime);
//      // rigidbody를 이용한 움직임
//      // 터치한 거리와 가까워지면 이동 종료
//      if (Vector3.Distance(hits[0].pose.position, transform.position) >= 0.2f)
//      {
//          rigid.MovePosition(destination);
//      }
//  
//      if (hits[0].pose.position - transform.position != Vector3.zero)
//      {
//          transform.rotation = Quaternion.LookRotation((hits[0].pose.position - transform.position).normalized);
//      }
//  }