using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Ball : MonoBehaviour
{
    bool isSelected;
    FoxController foxController;
    Rigidbody rigid;
    Vector2 pastPos;
    Vector2 velocity;
    private void Awake()
    {
        isSelected = false;
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        foxController = FindAnyObjectByType<FoxController>();
        rigid.useGravity = false;
    }
    private void Update()
    {
        if(transform.position.y < - 20)
        {
            Destroy(gameObject);
        }
        Debug.Log(transform.position.y);
        // ȭ���� ��ġ���� ��쿡�� ����
        if (Input.touchCount > 0)
        {
            // ù��° ��ġ
            Touch touch = Input.GetTouch(0);
            // ��ġ�� �������� ��
            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ�� ��ġ�� ����
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        // fox ������ ����
                        foxController.ballSelect = true;
                        isSelected = true;
                        rigid.useGravity = false;
                        pastPos = touch.position;
                    }
                }
            }
            // ��ġ�ϸ鼭 �巡�����϶�
            else if (touch.phase == TouchPhase.Moved && isSelected)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                // ȭ��� ������ Plane ����
                Plane groundPlane = new Plane(Vector3.forward, transform.position);
                // plane.Raycast�� ray�� origin�� plane�� ����� ��� true ���� ��ȯ�ϰ� out�� ���Ͽ� �Ÿ��� �ο���
                if(groundPlane.Raycast(ray, out float cameraToPlaneDist))
                {
                    // ray.GetPoint(float) �� ray�� ���� float��ŭ ������ ������ ��ȯ
                    // ���� ��ġ�� �̵�
                    transform.position = ray.GetPoint(cameraToPlaneDist);

                    // �ӵ� = �Ÿ� / �ð�
                    velocity = (touch.position - pastPos) / Time.deltaTime;
                    // ���� update �ֱ��� pastPos�� ���� ��ġ����Ʈ ����
                    pastPos = touch.position;
                }
            }
            // ȭ�鿡�� ��ġ�� �׸��ξ��� ��
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && isSelected)
            {
                isSelected = false;
                
                rigid.useGravity = true;
                // normalized�� ���� �ӵ��� ������ �˾Ƴ��� z���� �����ν� ������ ����
                // �ӵ��� �������� x����ӵ��� y����ӵ�, z ���Ⱚ�� ����ȭ�Ͽ� ������ ��ȯ��
                Vector3 dir = new Vector3(velocity.x, velocity.y, 1f).normalized;
                // (���� �պκ� + �ӵ��� ����) * �ӵ������� y����(�󸶳� ���� �巡�� �߳Ŀ� ���� ������ �޸� �ϰ� �;���)
                rigid.AddForce((transform.forward + dir) * dir.y, ForceMode.Impulse);
                // fox ������ �ٽ� ����
                if (foxController != null)
                {
                    foxController.ballSelect = false;
                }
            }
        }
    }
}
