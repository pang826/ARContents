using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class FoxTraceBall : IFoxState
    {
        [SerializeField] FoxController fox;
        [SerializeField] GameObject ball;
        [SerializeField] Animator anim;
        bool findBall = false;
        public FoxTraceBall(FoxController fox)
        {
            this.fox = fox;
            fox.SetAnimSpeed(2);
        }
        public void OnInput()
        {
            if (!findBall && ball == null)
            {
                findBall = true;
                ball = GameObject.FindAnyObjectByType<Ball>().gameObject;
            }
            if (!ball.GetComponent<Ball>().isGround || Vector3.Distance(fox.transform.position, ball.transform.position) < 0.1f || ball == null)
            {
                findBall = false;
                fox.SetState(new FoxIdle(fox));
            }
        }
        public void OnMove()
        {
            if (findBall)
            {
                Vector3 destination = Vector3.MoveTowards(fox.transform.position, ball.transform.position, 0.4f * Time.deltaTime);
                fox.rigid.MovePosition(destination);
            }
        }
    }
}
