using System;
using UnityEngine;

namespace Blocks.Quad {
    public class QuadMovement : MonoBehaviour
    {
        /*
        Quad component responsible only for movement
        */

        public event Action OnMove;

        public void Move(Vector3 position) {
            if (CheckPosition(position)) {
                transform.position = new Vector3(position.x, position.y, 0f);

                if (OnMove != null) {
                    OnMove();
                }
            }
        }

        bool CheckPosition(Vector3 position)
        {
            float dx = transform.localScale.x / 2f;
            float dy = transform.localScale.y / 2f;
            Vector3[] offsets = {
                new Vector3(-dx, dy, 0f),
                new Vector3(dx, dy, 0f),
                new Vector3(dx, -dy, 0f),
                new Vector3(-dx, -dy, 0f)
            };

            foreach (Vector3 offset in offsets)
            {
                Vector3 offsetPosition = position + offset;
                Vector3 screenOffsetPosition = Camera.main.WorldToScreenPoint(offsetPosition);
                GameObject res = Mouse.GetQuadOnScreenPosition(screenOffsetPosition);
                if (res != gameObject && res != null) {
                    return false;
                }
            }

            return true;
        }
    }
}
