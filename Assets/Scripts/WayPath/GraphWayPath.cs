using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraphWayPath : MonoBehaviour {

    private const float gizmosWayPointRadius = 0.04f;
    private const float gizmosWayPathArrowLength = 0.2f;
    private const float gizmosWayPathArrowAngleDegrees = 20f;

    private List<GraphWayPoint> wayPoints;

    private void Start() {
        wayPoints = new();
        for (int wayPointIndex = 0; wayPointIndex < transform.childCount; wayPointIndex++) {
            GameObject child = transform.GetChild(wayPointIndex).gameObject;
            if (!child.TryGetComponent<GraphWayPoint>(out var currentWayPoint)) {
                continue;
            }

            wayPoints.Add(currentWayPoint);
        }
    }

    private void OnDrawGizmos() {
        for (int wayPointIndex = 0; wayPointIndex < transform.childCount; wayPointIndex++) {
            GameObject child = transform.GetChild(wayPointIndex).gameObject;
            if (!child.TryGetComponent<GraphWayPoint>(out var currentWayPoint)) {
                continue;
            }

            Vector3 currentPointPosition = currentWayPoint.transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentPointPosition, gizmosWayPointRadius);

            if (currentWayPoint.getAdjacentWaypoints() == null) {
                continue;
            }

            foreach (var adjacentWayPoint in currentWayPoint.getAdjacentWaypoints()) {
                if (adjacentWayPoint == null) {
                    continue;
                }

                Vector3 adjacentPointPosition = adjacentWayPoint.transform.position;
                
                Vector3 middleLinePoint;
                if (currentPointPosition.y <= adjacentPointPosition.y) {
                    middleLinePoint = new(currentPointPosition.x, adjacentPointPosition.y, currentPointPosition.z);
                } else {
                    middleLinePoint = new(adjacentPointPosition.x, currentPointPosition.y, currentPointPosition.z);
                }

                Gizmos.color = Color.green;
                Gizmos.DrawLine(currentPointPosition, middleLinePoint);
                Gizmos.DrawLine(middleLinePoint, adjacentPointPosition);

                Vector3 arrowPathDirection = (middleLinePoint == adjacentPointPosition ? currentPointPosition : middleLinePoint) - adjacentPointPosition;
                Vector3 arrowSideVector = arrowPathDirection.normalized * gizmosWayPathArrowLength;

                foreach (var arrowSideSign in new float[] { -1f, 1f }) {
                    Vector3 arrowSide = Quaternion.Euler(0f, 0f, gizmosWayPathArrowAngleDegrees * arrowSideSign) * arrowSideVector;
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(adjacentPointPosition, adjacentPointPosition + arrowSide);
                }
            }
        }
    }

    private GraphWayPoint calculateNearestWayPoint(Vector3 position) {
        // TODO collider for each point ??? + optimize
        float minDistance = float.PositiveInfinity;
        GraphWayPoint bestAnswer = null;

        foreach (var currentWayPoint in wayPoints) {
            float currentDistance = (currentWayPoint.transform.position - position).sqrMagnitude;

            if (minDistance > currentDistance) {
                minDistance = currentDistance;
                bestAnswer = currentWayPoint;
            }
        }

        return bestAnswer;
    }

    private Vector3 calculateNearestWayIntersection(Vector3 initialPoint, GraphWayPoint nearestWayPoint, out GraphWayPoint adjacentWayPoint) {
        adjacentWayPoint = nearestWayPoint;

        if (nearestWayPoint.getAdjacentWaypoints().Length == 0) {
            return nearestWayPoint.transform.position;
        }

        float minDistance = float.PositiveInfinity;
        Vector3 bestAnswer = new();

        float initialX = initialPoint.x;
        float initialY = initialPoint.y;
        float nearestX = nearestWayPoint.transform.position.x;

        foreach (var currentAdjacentWayPoint in nearestWayPoint.getAdjacentWaypoints()) {
            float adjacentX = currentAdjacentWayPoint.transform.position.x;

            float nearestXIntersection = Mathf.Clamp(initialX, Mathf.Min(nearestX, adjacentX), Mathf.Max(nearestX, adjacentX));
            float yIntersection = Mathf.Max(nearestWayPoint.transform.position.y, currentAdjacentWayPoint.transform.position.y);
            
            float distance = new Vector2(initialX - nearestXIntersection, initialY - yIntersection).sqrMagnitude;

            if (distance < minDistance) {
                minDistance = distance;
                bestAnswer = new(nearestXIntersection, yIntersection, initialPoint.z);
                adjacentWayPoint = currentAdjacentWayPoint;
            }
        }

        return bestAnswer;
    }

    private long calculateEuclidDistance(GraphWayPoint from, GraphWayPoint to) {
        return (long) (to.transform.position - from.transform.position).sqrMagnitude;
    }

    public List<GraphWayPoint> calculatePath(GraphWayPoint from, GraphWayPoint to) {
        DistanceDescriptor fromDistanceDescriptor = new(from, 0, calculateEuclidDistance(from, to));

        IDictionary<GraphWayPoint, GraphWayPoint> prevoiusPathPoint = new Dictionary<GraphWayPoint, GraphWayPoint>();
        IDictionary<GraphWayPoint, DistanceDescriptor> bestDescriptors = new Dictionary<GraphWayPoint, DistanceDescriptor> {
            [from] = fromDistanceDescriptor
        };

        SortedSet<DistanceDescriptor> advancementCandidates = new() { fromDistanceDescriptor };

        while (advancementCandidates.Count > 0 && !prevoiusPathPoint.ContainsKey(to)) {
            SortedSet<DistanceDescriptor>.Enumerator candidatesEnumerator = advancementCandidates.GetEnumerator();
            candidatesEnumerator.MoveNext();
            DistanceDescriptor currentDescriptor = candidatesEnumerator.Current;
            advancementCandidates.Remove(currentDescriptor);

            if (bestDescriptors.ContainsKey(currentDescriptor.graphWayPoint) && currentDescriptor != bestDescriptors[currentDescriptor.graphWayPoint]) {
                continue;
            }

            foreach (GraphWayPoint adjacentPoint in currentDescriptor.graphWayPoint.getAdjacentWaypoints()) {
                // TODO precalculate euclid distances ?
                long edgeWeight = calculateEuclidDistance(currentDescriptor.graphWayPoint, adjacentPoint);
                long approximateDistanceToFinish = calculateEuclidDistance(adjacentPoint, to);

                DistanceDescriptor adjacentDescriptor = new(adjacentPoint, currentDescriptor.distanceFromStart + edgeWeight, approximateDistanceToFinish);
                bool hasBestDescriptor = bestDescriptors.TryGetValue(adjacentPoint, out var bestDescriptor);

                if (!hasBestDescriptor || adjacentDescriptor.CompareTo(bestDescriptor) < 0) {
                    bestDescriptors[adjacentPoint] = adjacentDescriptor;
                    advancementCandidates.Add(adjacentDescriptor);
                    prevoiusPathPoint[adjacentPoint] = currentDescriptor.graphWayPoint;
                }
            }
        }

        return getPathByOneStepDirections(from, to, prevoiusPathPoint);
    }

    private List<GraphWayPoint> getPathByOneStepDirections(GraphWayPoint from, GraphWayPoint to, IDictionary<GraphWayPoint, GraphWayPoint> prevoiusPathPoint) {
        if (!prevoiusPathPoint.ContainsKey(to)) {
            return new();
        }

        List<GraphWayPoint> result = new() { to };

        while (result[^1] != from) {
            GraphWayPoint lastPoint = result[^1];
            result.Add(prevoiusPathPoint[lastPoint]);
        }

        result.Reverse();
        return result;
    }

    public List<Vector3> calculatePath(Vector3 from, Vector3 to) {
        GraphWayPoint fromWayPoint = calculateNearestWayPoint(from);
        GraphWayPoint toWayPoint = calculateNearestWayPoint(to);
        
        Vector3 fromIntersection = calculateNearestWayIntersection(from, fromWayPoint, out GraphWayPoint adjacentFromWayPoint);
        Vector3 toIntersection = calculateNearestWayIntersection(to, toWayPoint, out GraphWayPoint adjacentToWayPoint);

        HashSet<GraphWayPoint> wayPointsSet = new() { fromWayPoint, adjacentFromWayPoint, toWayPoint, adjacentToWayPoint };
        if (wayPointsSet.Count == 2) {
            return new() { from, fromIntersection, toIntersection, to };
        }

        if (wayPointsSet.Count == 3) {
            GraphWayPoint middleWayPoint;
            if (new HashSet<GraphWayPoint>() { fromWayPoint, adjacentFromWayPoint, toWayPoint }.Count == 3) {
                middleWayPoint = adjacentToWayPoint;
            } else {
                middleWayPoint = toWayPoint;
            }

            return new() { from, fromIntersection, middleWayPoint.transform.position, toIntersection, to };
        }

        List<GraphWayPoint> wayPointPath = calculatePath(fromWayPoint, toWayPoint);
        if (wayPointPath[1] == adjacentFromWayPoint) {
            wayPointPath.RemoveAt(0);
        }
        if (wayPointPath[^2] == adjacentToWayPoint) {
            wayPointPath.RemoveAt(wayPointPath.Count - 1);
        }

        List<Vector3> result = new() { from, fromIntersection };

        foreach (var wayPoint in wayPointPath) {
            result.Add(wayPoint.transform.position);
        }

        result.Add(toIntersection);
        result.Add(to);
        return result;
    }
}
