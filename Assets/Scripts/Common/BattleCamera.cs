using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common
{

    public class BattleCamera : MonoBehaviour
    {
        public Vector2 LeftDownLimit;
        public Vector2 RightUpLimit = new Vector2(16, 16);

        private int zoom = 0;
        public int Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                UpdatePixelCameraZoom();
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set
            {
                transform.position = value;
                if (transform.position.x < LeftDownLimit.x)
                    transform.position = new Vector3(LeftDownLimit.x, transform.position.y, transform.position.z);
                if (transform.position.y < LeftDownLimit.y)
                    transform.position = new Vector3(transform.position.x, LeftDownLimit.y, transform.position.z);
                if (transform.position.x > RightUpLimit.x)
                    transform.position = new Vector3(RightUpLimit.x, transform.position.y, transform.position.z);
                if (transform.position.y > RightUpLimit.y)
                    transform.position = new Vector3(transform.position.x, RightUpLimit.y, transform.position.z);
            }
        }

        UnityEngine.U2D.PixelPerfectCamera pixelPerfectCamera;

        private void Awake()
        {
            pixelPerfectCamera = GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
            UpdatePixelCameraZoom();
        }


        #if UNITY_EDITOR
        public float dragSpeed = 15f;
        private Vector3 cameraOrigin;
        private Vector3 dragOrigin;

        #elif UNITY_ANDROID
        private float dragSpeed = 1f;
        private Vector2 nowPos, prePos;
        private Vector3 movePos;
        #endif

        void Update()
        {
            #if UNITY_EDITOR
            // 마우스를 눌렀을때 위치를 기록한다.
            if (Input.GetMouseButtonDown(0))
            {               
                cameraOrigin = Position;
                dragOrigin = Input.mousePosition;
                return;
            }
            // 마우스를 누르고 있으면 이동한다.
            else if (Input.GetMouseButton(0))
            {
                Vector3 vec = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Position = cameraOrigin - new Vector3(vec.x * dragSpeed, vec.y * dragSpeed, 0);
            }

            #elif UNITY_ANDROID
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    prePos = touch.position - touch.deltaPosition;
                }
                else if(touch.phase == TouchPhase.Moved)
                {
                    nowPos = touch.position - touch.deltaPosition;
                    movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * dragSpeed;
                    transform.Translate(movePos); 
                    prePos = touch.position - touch.deltaPosition;
                }
            }
            #endif

            // 축소
            if (Input.mouseScrollDelta.y < 0 && Zoom > 0)
                Zoom -= 1;
            // 확대
            if (Input.mouseScrollDelta.y > 0 && Zoom < Resolutions.Length - 1 /* && pixelPerfectCamera.refResolutionX > 100*/ )
                Zoom += 1;
        }

        public Vector2Int[] Resolutions =
        {
            // new Vector2Int(240, 134),
            new Vector2Int(320, 180),
            // new Vector2Int(480, 270)
        };

        public void UpdatePixelCameraZoom()
        {
            pixelPerfectCamera.refResolutionX = Resolutions[Zoom].x;
            pixelPerfectCamera.refResolutionY = Resolutions[Zoom].y;

            //// Screen Size를 zoom으로 나누고 홀수일 경우 (-1)짝수로 만들어준다.
            //pixelPerfectCamera.refResolutionX =
            //    (Screen.width / zoom) % 2 == 0 ? Screen.width / zoom : Screen.width / zoom - 1;
            //pixelPerfectCamera.refResolutionY =
            //    (Screen.height / zoom) % 2 == 0 ? Screen.height / zoom : Screen.height / zoom - 1;
        }

    }

}
