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
        // 화면을 터치했을 경우에만 반응
        if (Input.touchCount > 0)
        {
            // 첫번째 터치
            Touch touch = Input.GetTouch(0);
            // 터치를 시작했을 때
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 위치의 레이
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        // fox 움직임 정지
                        foxController.ballSelect = true;
                        isSelected = true;
                        rigid.useGravity = false;
                        pastPos = touch.position;
                    }
                }
            }
            // 터치하면서 드래그중일때
            else if (touch.phase == TouchPhase.Moved && isSelected)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                // 화면과 평행한 Plane 생성
                Plane groundPlane = new Plane(Vector3.forward, transform.position);
                // plane.Raycast는 ray의 origin과 plane이 닿았을 경우 true 값을 반환하고 out을 통하여 거리를 부여함
                if (groundPlane.Raycast(ray, out float cameraToPlaneDist))
                {
                    // ray.GetPoint(float) 는 ray로 부터 float만큼 떨어진 지점을 반환
                    // 공의 위치를 이동
                    transform.position = ray.GetPoint(cameraToPlaneDist);

                    // 속도 = 거리 / 시간
                    velocity = (touch.position - pastPos) / Time.deltaTime;
                    // 다음 update 주기의 pastPos를 위한 터치포인트 저장
                    pastPos = touch.position;
                }
            }
            // 화면에서 터치를 그만두었을 때
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled && isSelected)
            {
                // 선택 끝
                isSelected = false;

                // rigidBody 중력 사용
                rigid.useGravity = true;

                // normalized를 통해 속도의 방향을 알아내고 z값을 줌으로써 앞으로 향함
                // 속도가 가해지는 카메라 x방향속도과 카메라 y방향속도를 정규화하여 방향을 반환함
                Vector3 dir = (Camera.main.transform.right * velocity.x + Camera.main.transform.forward * velocity.y).normalized;

                // y 입력값만큼 z방향으로 값 증감
                Vector3 power = new Vector3(0, 0, velocity.y).normalized;

                // z 값을 정규화하면 0과 차이가 없을정도로 차이가 나기에 따로 추가
                // z 값을 1로 고정하면 살짝만 해도 너무 멀리갈 경우가 생겨서 어색함
                // Mathf.Clamp를 통하여 최솟값과 최대값을 정해주어서 자연스럽게 변경
                dir.z = Mathf.Clamp(1, 0.1f, 1.5f);

                // 공의 회전값을 카메라가 보고있는 방향으로 설정
                rigid.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

                // 공 던지기
                rigid.AddForce(dir + power, ForceMode.Impulse);

                // fox 움직임 다시 시작
                if (foxController != null)
                {
                    foxController.ballSelect = false;
                }
            }
        }
    }
}
