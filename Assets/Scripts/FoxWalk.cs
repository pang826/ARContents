using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets
{
    public class FoxWalk : IFoxState
    {
        FoxController fox;
        public FoxWalk(FoxController fox)
        {
            this.fox = fox;
            fox.SetAnimSpeed(1);
        }

        public void OnInput()
        {
            if (Input.touchCount == 0)
            {
                fox.SetState(new FoxIdle(fox));
            }
            if (!fox.isTrace)
            {
                fox.StartCoroutine(fox.Trace());
            }
        }

        public void OnMove()
        {
            fox.rigid.velocity = Vector3.zero;
            fox.rigid.angularVelocity = Vector3.zero;
            // 이동하고자 하는 방향을 구하기
            Vector3 destination = Vector3.MoveTowards(fox.transform.position, fox.hits[0].pose.position, fox.speed * Time.fixedDeltaTime);
            // rigidbody를 이용한 움직임
            // 터치한 거리와 가까워지면 이동 종료
            if (Vector3.Distance(fox.hits[0].pose.position, fox.transform.position) >= 0.1f)
            {
                fox.rigid.MovePosition(destination);
            }
            else
            {
                fox.SetState(new FoxIdle(fox));
            }

            if (fox.hits[0].pose.position - fox.transform.position != Vector3.zero && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                fox.transform.rotation = Quaternion.LookRotation((fox.hits[0].pose.position - fox.transform.position).normalized);
            }
        }
    }
}
