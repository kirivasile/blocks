using UnityEngine;

namespace Blocks.Quad {
    public class QuadLine : MonoBehaviour 
    {
        /*
        Quad component responsible for quad connection and drawing lines
        */

        QuadLine connectedQuad;
        LineRenderer line;

        [SerializeField] Material lineMaterial;

        void Start() {
            QuadMovement move = GetComponent<QuadMovement>();
            if (move != null) {
                move.OnMove += OnMove;
            }
        }

        void OnMove() {
            if (connectedQuad != null) {
                if (line != null) {
                    MoveLine(0, transform.position);
                }
                else {
                    connectedQuad.MoveLine(1, transform.position);
                }
            }
        }

        public void MoveLine(int priority, Vector3 position) {
            if (line != null) {
                line.SetPosition(priority, position);
            }
        }

        public bool ConnectWithQuad(int priority, QuadLine other) {
            if (HasConnection()) return false;

            this.connectedQuad = other;

            if (priority == 0) {
                line.SetPosition(1, other.transform.position);
            }

            return true;
        }

        public bool AttachLineToMouse(Vector3 position) {
            if (HasConnection()) return false;

            DrawLine(position);

            return true;
        }

        bool HasConnection() {
            return connectedQuad != null;
        }

        void DrawLine(Vector3 endPosition) {
            line = gameObject.AddComponent<LineRenderer>();
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            line.positionCount = 2;
            line.sortingOrder = 1;
            line.material = lineMaterial;

            line.SetPosition(0, transform.position);
            line.SetPosition(1, endPosition);
        }

        public void DeleteLine() {
            this.connectedQuad = null;
            if (line != null) {
                Destroy(line);
            }
        }

        void OnDestroy() {
            if (connectedQuad != null) {
                connectedQuad.DeleteLine();
            }

            if (line != null) {
                DeleteLine();
            }
        }
    }
}