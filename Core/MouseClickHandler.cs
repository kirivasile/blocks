using System;
using UnityEngine;

namespace Blocks.Core {
    public class MouseClickHandler : MonoBehaviour {
        /*
        Custom class for detecting single and double clicks and mouse dragging
        */

        [SerializeField] float clickThreshold = 0.2f;
        [SerializeField] float dragThreshold = 0.1f;

        float timeSinceLastClick;
        bool singleClick = false;
        bool mouseDown = false;

        bool isDragging = false;
        float timeSinceStartDrag;

        public event Action OnSingleClick;
        public event Action OnDoubleClick;
        public event Action OnDrag;
        public event Action OnStopDrag;

        void Start() {
            timeSinceLastClick = -clickThreshold - 1f;
            timeSinceStartDrag = -1f;
        }

        void Update()
        {
            if (!mouseDown && singleClick && Time.time > timeSinceLastClick + clickThreshold) {
                singleClick = false;
                if (OnSingleClick != null) {
                    OnSingleClick();
                }
                
            }

            if (Input.GetMouseButton(0))
            {
                if (!isDragging)
                {
                    timeSinceStartDrag = Time.time;
                    isDragging = true;
                }

                if (Time.time > timeSinceStartDrag + dragThreshold) {
                    singleClick = false;
                    if (OnDrag != null) {
                        OnDrag();
                    }
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (Time.time > timeSinceLastClick + clickThreshold)
                {
                    singleClick = true;
                }
                else
                {
                    singleClick = false;
                    if (OnDoubleClick != null) {
                        OnDoubleClick();
                    }
                }

                mouseDown = true;
                timeSinceLastClick = Time.time;
            }

            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
                mouseDown = false;

                if (OnStopDrag != null) {
                    OnStopDrag();
                }
            }
        }
    }
}