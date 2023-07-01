using UnityEditor;
using UnityEngine;

public class CircularWayPath : MonoBehaviour {

    private const float gizmosWayPointRadius = 0.04f;

    private void OnDrawGizmos() {
        if (Selection.Contains(gameObject)) {
            return;
        }

        for (int childIndex = 0; childIndex < transform.childCount; childIndex++) {
            GameObject child = transform.GetChild(childIndex).gameObject;
            if (Selection.Contains(child)) {
                OnDrawGizmosSelected();
                return;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        for (int wayPointIndex = 0; wayPointIndex < transform.childCount; wayPointIndex++) {
            int nextWaypointIndex = (wayPointIndex + 1) % wayPointsCount;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(getWayPoint(wayPointIndex), getWayPoint(nextWaypointIndex));
            Gizmos.DrawSphere(getWayPoint(wayPointIndex), gizmosWayPointRadius);
        }
    }

    public int wayPointsCount {
        get {
            return transform.childCount;
        }
    }

    public Vector2 getWayPoint(int index) {
        return ToddMath.vector3ToVector2(transform.GetChild(index).position);
    }
}
