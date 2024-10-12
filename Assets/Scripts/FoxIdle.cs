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
        
        public FoxIdle(FoxController fox)
        {
            this.fox = fox;
            fox.SetAnimSpeed(0);
        }
        public void OnInput()
        {
            if (Input.touchCount > 0 && fox.raycastManager.Raycast(Input.GetTouch(0).position, fox.hits, TrackableType.Planes) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                fox.SetState(new FoxWalk(fox));
            }
            if (!fox.isTrace)
            {
                // 터치위치를 쫓아가는 중이 아니라면 카메라를 쳐다보도록 변경
                fox.transform.rotation = Quaternion.LookRotation(fox.cam.transform.position - fox.transform.position);
            }
        }

        public void OnMove()
        {
            // idle 일땐 움직임 x
        }
    }
}
