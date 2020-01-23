using UnityEngine;
using UnityEditor;

namespace Blocks.Core {
    
}
public class Mouse {
    /*
    Library class for mouse and raycasting
    */
    
    static float distanceToScreen;

    static Mouse() {
        distanceToScreen = Camera.main.WorldToScreenPoint(Vector3.zero).z;
    }

    static public bool GetMousePosition(out Vector3 position)
    {
        if (!CheckPositionInsideScreen(Input.mousePosition)) {
            position = Vector3.zero;
            return false;
        }

        position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
        position.z = 0f;

        return true;
    }

    static public bool CheckPositionInsideScreen(Vector3 position) {
#if UNITY_EDITOR
        if (
            position.x <= 0 ||
            position.y <= 0 ||
            position.x >= Handles.GetMainGameViewSize().x - 1 ||
            position.y >= Handles.GetMainGameViewSize().y - 1
        ) {
            return false;
        }
#else
        if (
            position.x <= 0 || 
            position.y <= 0 || 
            position.x >= Screen.width - 1 || 
            position.y >= Screen.height - 1
        ) { 
            return false;
        }
#endif
        return true;
    }

    static public GameObject GetQuadOnScreenPosition(Vector3 screenPosition) {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(screenPosition));

        foreach (RaycastHit hit in hits) {
            if (hit.transform.tag == "Quad") {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}