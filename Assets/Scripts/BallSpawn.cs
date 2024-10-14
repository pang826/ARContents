using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject ball;
    bool isSpawn;
    private void Update()
    {
        ball = GameObject.FindAnyObjectByType<Ball>().gameObject;
    }

    public void SpawnBall()
    {
        if (ballPrefab != null)
        {
            if (GameObject.FindAnyObjectByType<Ball>() == null)
            {
                // �̹� ���������� ��� ���� X
                Instantiate(ballPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 0.1f, Camera.main.transform.position.z + 0.2f), Camera.main.transform.rotation);
            }
        }
    }

    public void DeleteBall()
    {
        Destroy(ball);
    }
}
