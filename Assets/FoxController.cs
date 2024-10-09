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
        // ��ġ�� ���� ��� �Ѿ
        if(Input.touchCount == 0) return;
        // ��ġ�� 1�� �̻� ���� ���
        if(Input.touchCount > 0)
        {
            if(raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes))
            {
                // �����̵�
                //transform.position = hits[0].pose.position;

                // �̵��ϰ��� �ϴ� ������ ���ϱ�
                Vector3 dir = hits[0].pose.position - transform.position;
                // ���� �پ��ֵ��� y���� 0���� ����
                dir.y = 0;
                // rigidbody�� �̿��� ������(���Ŀ� FixedUpdate�� ��ȯ�ؾ� �� ��)
                rigid.MovePosition(transform.position + dir * Time.deltaTime * speed);
                // ȸ������ ī�޶� �Ĵٺ����� ���� (���Ŀ� �̵��Ҷ��� ���� ������ �Ĵٺ����� bool�� �ڷ�ƾ�� Ȱ���ؾ� �� ��)
                transform.rotation = Quaternion.LookRotation(cam.transform.position - transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
    }
}
