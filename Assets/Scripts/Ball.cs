using UnityEngine;

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
        if (transform.position.y < -20)
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
                if (groundPlane.Raycast(ray, out float cameraToPlaneDist))
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
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && isSelected)
            {
                // ���� ��
                isSelected = false;

                // rigidBody �߷� ���
                rigid.useGravity = true;

                // normalized�� ���� �ӵ��� ������ �˾Ƴ��� z���� �����ν� ������ ����
                // �ӵ��� �������� ī�޶� x����ӵ��� ī�޶� y����ӵ��� ����ȭ�Ͽ� ������ ��ȯ��
                Vector3 dir = (Camera.main.transform.right * velocity.x + Camera.main.transform.forward * velocity.y).normalized;

                // y �Է°���ŭ z�������� �� ����
                Vector3 power = new Vector3(0, 0, velocity.y).normalized;

                // z ���� ����ȭ�ϸ� 0�� ���̰� ���������� ���̰� ���⿡ ���� �߰�
                // z ���� 1�� �����ϸ� ��¦�� �ص� �ʹ� �ָ��� ��찡 ���ܼ� �����
                // Mathf.Clamp�� ���Ͽ� �ּڰ��� �ִ밪�� �����־ �ڿ������� ����
                dir.z = Mathf.Clamp(1, 0.1f, 1.5f);

                // ���� ȸ������ ī�޶� �����ִ� �������� ����
                rigid.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

                // �� ������
                rigid.AddForce(dir + power, ForceMode.Impulse);

                // fox ������ �ٽ� ����
                if (foxController != null)
                {
                    foxController.ballSelect = false;
                }
            }
        }
    }
}
