using System.Collections.Generic;
using UnityEngine;
using Blocks.Quad;
using Blocks.Core;

namespace Blocks.Manage {
    [RequireComponent(typeof(MouseClickHandler))]
    public class QuadManager : MonoBehaviour
    {
        /*
        Main manager class for creating, deleting and moving quads
        */

        [SerializeField] GameObject quadPrefab;
        MouseClickHandler mouseHandler;

        List<GameObject> quads;

        Vector3 lastMousePosition = Vector3.zero;
        bool mouseInsideScreen = true;

        QuadLine lastConnectedQuad;
        bool lineAttachedToMouse = false;

        QuadMovement draggedQuad;

        void Start()
        {
            quads = new List<GameObject>();
            mouseHandler = GetComponent<MouseClickHandler>();

            mouseHandler.OnSingleClick += OnSingleClick;
            mouseHandler.OnDoubleClick += OnDoubleClick;
            mouseHandler.OnDrag += OnDrag;
            mouseHandler.OnStopDrag += OnStopDrag;
        }

        void Update() {
            UpdateMousePosition();

            if (lineAttachedToMouse) {
                lastConnectedQuad.MoveLine(1, lastMousePosition);
            }
        }

        void OnDrag() {
            if (lineAttachedToMouse) return;

            if (draggedQuad == null) {
                GameObject quadOnMouse = Mouse.GetQuadOnScreenPosition(Input.mousePosition);
                if (quadOnMouse != null) {
                    draggedQuad = quadOnMouse.GetComponent<QuadMovement>();
                }
            }

            if (draggedQuad != null) {
                draggedQuad.Move(lastMousePosition);
            }
        }

        void OnStopDrag() {
            draggedQuad = null;
        }

        void UpdateMousePosition() {
            Vector3 newMousePosition;
            mouseInsideScreen = Mouse.GetMousePosition(out newMousePosition);
            if (mouseInsideScreen) {
                lastMousePosition = newMousePosition;
            }
        }

        void OnSingleClick() {

            GameObject quadOnMouse = Mouse.GetQuadOnScreenPosition(Input.mousePosition);
            if (quadOnMouse != null) {
                MakeLineBetweenQuads(quadOnMouse.GetComponent<QuadLine>());
                return;
            }

            if (lineAttachedToMouse) {
                DeleteSingleLine();
                return;
            }

            if (!CheckQuadInstantiation()) return;

            CreateQuad();
        }

        void CreateQuad() {
            GameObject newQuad = Instantiate<GameObject>(quadPrefab, lastMousePosition, Quaternion.identity, transform);
            newQuad.GetComponent<MeshRenderer>().material.SetColor(
                "_Color",
                new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                )
            );
            quads.Add(newQuad);
        }

        void OnDoubleClick() {
            lineAttachedToMouse = false;
            DestroyQuadOnMouse();
        }

        bool CheckQuadInstantiation() {
            float dx = quadPrefab.transform.localScale.x / 2f;
            float dy = quadPrefab.transform.localScale.y / 2f;
            Vector3[] offsets = {
                new Vector3(-dx, dy, 0f),
                new Vector3(dx, dy, 0f),
                new Vector3(dx, -dy, 0f),
                new Vector3(-dx, -dy, 0f)
            };

            if (!mouseInsideScreen) return false;

            foreach (Vector3 offset in offsets) {
                Vector3 offsetPosition = lastMousePosition + offset;
                Vector3 screenOffsetPosition = Camera.main.WorldToScreenPoint(offsetPosition);
                if (Mouse.GetQuadOnScreenPosition(screenOffsetPosition) != null) {
                    return false;
                }
            }

            return true;
        }

        void DestroyQuadOnMouse() {
            GameObject quadOnMouse = Mouse.GetQuadOnScreenPosition(Input.mousePosition);
            if (quadOnMouse != null) {
                Destroy(quadOnMouse);
            }
        }

        void MakeLineBetweenQuads(QuadLine quad) {
            if (lastConnectedQuad == quad) {
                return;
            }

            if (lastConnectedQuad == null) {
                MakeSingleLine(quad);      
            } else {
                MakeConnectedLine(quad);
            }
        }

        void MakeSingleLine(QuadLine quad) {
            bool result = quad.AttachLineToMouse(lastMousePosition);

            if (result) {
                lastConnectedQuad = quad;
                lineAttachedToMouse = true;
            }
        }

        void DeleteSingleLine() {
            lastConnectedQuad.DeleteLine();
            lastConnectedQuad = null;
            lineAttachedToMouse = false;
        }

        void MakeConnectedLine(QuadLine quad) {
            bool result = quad.ConnectWithQuad(1, lastConnectedQuad);

            if (result) {
                lineAttachedToMouse = false;
                lastConnectedQuad.ConnectWithQuad(0, quad);
                lastConnectedQuad = null;
            }
        }

        
    }
}