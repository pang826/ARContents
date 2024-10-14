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
        Debug.Log(curState.ToString());
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

