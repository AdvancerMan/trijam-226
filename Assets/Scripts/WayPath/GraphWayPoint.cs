using UnityEngine;

public class GraphWayPoint : MonoBehaviour {

    [SerializeField]
    private GraphWayPoint[] adjacentWayPoints;

    [SerializeField]
    private float secondsToSwitch = 1;

    private int nextWayPointIndex = 0;
    private float secondsSinceLastSwitch = 0;

    private void Update() {
        secondsSinceLastSwitch += Time.deltaTime;

        if (secondsSinceLastSwitch >= secondsToSwitch) {
            nextWayPointIndex += 1;
            secondsSinceLastSwitch = 0;

            if (nextWayPointIndex >= adjacentWayPoints.Length) {
                nextWayPointIndex = 0;
            }
        }
    }

    public GraphWayPoint getNextWayPoint() {
        if (adjacentWayPoints.Length == 0) {
            return null;
        }

        return adjacentWayPoints[nextWayPointIndex];
    }

    public GraphWayPoint[] getAdjacentWaypoints() {
        return adjacentWayPoints;
    }

    public override string ToString() {
        return gameObject.name;
    }
}
