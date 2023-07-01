using System;

[Serializable]
public class DistanceDescriptor : IComparable<DistanceDescriptor> {

    public DistanceDescriptor(GraphWayPoint graphWayPoint, long distanceFromStart, long approximateDistanceToFinish) {
        this.graphWayPoint = graphWayPoint;
        this.distanceFromStart = distanceFromStart;
        this.approximateDistanceToFinish = approximateDistanceToFinish;
    }

    public GraphWayPoint graphWayPoint { get; }

    public long distanceFromStart { get; }

    public long approximateDistanceToFinish { get; }

    public int CompareTo(DistanceDescriptor other) {
        if (graphWayPoint == other.graphWayPoint) {
            return 0;
        }

        int firstComparisonResult = (distanceFromStart + approximateDistanceToFinish).CompareTo(other.distanceFromStart + other.approximateDistanceToFinish);
        if (firstComparisonResult != 0) {
            return firstComparisonResult;
        }

        return approximateDistanceToFinish.CompareTo(other.approximateDistanceToFinish);
    }
}
