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
    public bool ballSelect = false; // ���� �����̰� �ִ��� ����
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

    // ���� ���� ��ȯ �޼���
    public void SetState(IFoxState newState)
    {
        curState = newState;
    }

    // �ִϸ��̼� �ӵ� ����
    public void SetAnimSpeed(float speed)
    {
        anim.SetFloat("speed", speed);
    }

    private void Update()
    {
        
        // ���� �����̰� �ִ� ���¸� �Ѿ
        if (ballSelect)
        {
            return;
        }
        curState.OnInput();
    }

    private void FixedUpdate()
    {
        // ���� �����̰� �ִ� ���¸� �Ѿ
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
            // �Ÿ��� �����Ÿ� ���ϰ� �ǰų� ���� ������ ����
            if (Vector3.Distance(hits[0].pose.position, transform.position) <= 0.1f || Input.touchCount == 0)
            {
                isTrace = false;
            }
        }
        yield break;
    }
}




//  // ��ġ�� ���� ��� �Ѿ
//  if (Input.touchCount == 0)
//  {
//      if (!isTrace)
//      {
//          // ��ġ��ġ�� �Ѿư��� ���� �ƴ϶�� ī�޶� �Ĵٺ����� ����
//          transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
//      }
//      return;
//  }
//  
//  // ��ġ�� 1�� �̻� ���� ���
//  if (Input.touchCount > 0 && raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes))
//  {
//      if (!isTrace)
//      {
//          StartCoroutine(Trace());
//      }
//  
//      // �̵��ϰ��� �ϴ� ������ ���ϱ�
//      Vector3 destination = Vector3.MoveTowards(transform.position, hits[0].pose.position, speed * Time.fixedDeltaTime);
//      // rigidbody�� �̿��� ������
//      // ��ġ�� �Ÿ��� ��������� �̵� ����
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