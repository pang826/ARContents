using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARSubsystems;
using Debug = UnityEngine.Debug;

namespace Assets
{
    public class FoxIdle : IFoxState
    {
        [SerializeField] FoxController fox;
        [SerializeField] Ball ball;
        bool findBall = false;
        public FoxIdle(FoxController fox)
        {
            this.fox = fox;
            fox.SetAnimSpeed(0);
            
        }
        public void OnInput()
        {
            if (!findBall && ball == null)
            {
                findBall = true;
                ball = GameObject.FindAnyObjectByType<Ball>();
            }
            if (
                !fox.ballSelect 
                && Input.touchCount > 0 
                && fox.raycastManager.Raycast(Input.GetTouch(0).position, fox.hits, TrackableType.Planes) 
                && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)
               )
            {
                fox.SetState(new FoxWalk(fox));
            }
            if (ball.isGround && ball != null && Vector3.Distance(ball.transform.position, fox.transform.position) > 0.1f)
            {
                fox.SetState(new FoxTraceBall(fox));
            }
            
            
        }

        public void OnMove()
        {
            
            fox.rigid.velocity = Vector3.zero;
            fox.rigid.angularVelocity = Vector3.zero;
            
            if (!fox.isTrace)
            {
                // 터치위치를 쫓아가는 중이 아니라면 카메라를 쳐다보도록 변경
                fox.transform.rotation = Quaternion.LookRotation(fox.cam.transform.position - fox.transform.position);
            }
        }
    }
}
