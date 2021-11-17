using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WaypointEvaluation
{
    public int highestExposureLevels = 3;
    public int multiplePinchPoints = 3;

    public ListOfWaypointsWithCertainExposure[] highestExposureWaypoints;

    public List<PinchPoint> pinchPoints;

    struct WaypointNeighboor
    {
        public int ID;
        public int previousID;

        public WaypointNeighboor(int iD, int previousID)
        {
            ID = iD;
            this.previousID = previousID;
        }
    };

    public void ClearAllLists()
    {
        pinchPoints.Clear();
    }

    public void FindHighestExposureWaypoints(List<Waypoint> waypoints)
    {
        int[] highestExposures = new int[highestExposureLevels];
        for (int i = 0; i < waypoints.Count; i++)
        {
            int exposuresNumber = waypoints[i].exposure;

            for (int j = 0; j < highestExposures.Length; j++)
            {
                if(exposuresNumber>=highestExposures[j])
                {
                    highestExposures[j] = exposuresNumber;
                    break;
                }
            }
        }
        highestExposureWaypoints = new ListOfWaypointsWithCertainExposure[highestExposureLevels];
        for (int i = 0; i < highestExposureWaypoints.Length; i++)
        {
            highestExposureWaypoints[i].exposure = highestExposures[i];
            highestExposureWaypoints[i].IDs = new List<int>();
            for (int j = 0; j < waypoints.Count; j++)
            {
                if (waypoints[j].exposure == highestExposureWaypoints[i].exposure)
                    highestExposureWaypoints[i].IDs.Add(waypoints[j].ID);
            }
        }
    }

    public void FindAllPinchPoints(Graph graph, List<Waypoint> waypoints)
    {
        pinchPoints = new List<PinchPoint>();
        foreach (var wp in waypoints)
        {
            if (wp.linksToOtherWaypoints.Length == 1)
            {
                if (waypoints[wp.linksToOtherWaypoints[0]].linksToOtherWaypoints.Length == 2)
                    continue;
                int outsideID = wp.linksToOtherWaypoints[0];
                if (waypoints[outsideID].linksToOtherWaypoints.Length > 2)
                {
                    List<int> insideId = new List<int>();
                    insideId.Add(wp.ID);
                    PinchPoint pinchPoint = new PinchPoint(wp.ID, insideId, outsideID);
                    AddPinchPointToListAfterCheckingForDuplicates(pinchPoint);
                }
            }
            else if (wp.linksToOtherWaypoints.Length == 2)
            {
                Waypoint A = waypoints[wp.linksToOtherWaypoints[0]];
                Waypoint B = waypoints[wp.linksToOtherWaypoints[1]];
                List<Waypoint> excluded = new List<Waypoint>();
                excluded.Add(wp);

                int ARegion = graph.GetRegionWaypointCount(A, excluded);
                int BRegion = graph.GetRegionWaypointCount(B, excluded);
                int insideID = ARegion > BRegion ? B.ID : A.ID;
                int outsideID = ARegion > BRegion ? A.ID : B.ID;
                int currentID = wp.ID;

                while(waypoints[outsideID].linksToOtherWaypoints.Length==2)
                {
                    int tempID = outsideID;
                    outsideID = waypoints[outsideID].linksToOtherWaypoints[0] == currentID ? waypoints[outsideID].linksToOtherWaypoints[1] : waypoints[outsideID].linksToOtherWaypoints[0];
                    currentID = tempID;
                    excluded.Add(waypoints[currentID]);
                }

                A = waypoints[insideID];
                B = waypoints[outsideID];

                if (!graph.AStar(A, B, excluded))
                {
                    var pinchPoint = CreatePinchPoint(graph, wp, A, B, excluded, waypoints);
                    if (pinchPoint != null)
                        AddPinchPointToListAfterCheckingForDuplicates(pinchPoint);
                }
            }
        }
        
    }

    private PinchPoint CreatePinchPoint(Graph graph, Waypoint wp, Waypoint A, Waypoint B, List<Waypoint> excluded,List<Waypoint> allWaypoints,bool findEndOfHallway = true)
    {
        int ARegion = graph.GetRegionWaypointCount(A, excluded);
        int BRegion = graph.GetRegionWaypointCount(B, excluded);
        if (ARegion == 0 || BRegion == 0)
            return null;
        int insideID = ARegion > BRegion ? B.ID : A.ID;
        int outsideID = ARegion > BRegion ? A.ID : B.ID;
        int pinchPointID = wp.ID;
        List<int> insideIDs = new List<int>();
        if (findEndOfHallway)
        {
            while (allWaypoints[outsideID].linksToOtherWaypoints.Length == 2)
            {
                int temp = allWaypoints[outsideID].linksToOtherWaypoints.First(t => t != pinchPointID);
                pinchPointID = outsideID;
                outsideID = temp;
            }

            foreach (var node in graph.GetRegionNodes(allWaypoints[insideID], new List<Waypoint> { allWaypoints[pinchPointID] }))
            {
                insideIDs.Add(node.wp.ID);
            }
        }
        else
        {
            insideIDs.Add(insideID);
        }
        PinchPoint pinchPoint = new PinchPoint(pinchPointID, insideIDs, outsideID);
        return pinchPoint;
    }

    void AddPinchPointToListAfterCheckingForDuplicates(PinchPoint N)
    {
        foreach (var pinchPoint in pinchPoints)
        {
            if (N.OutsideID == pinchPoint.OutsideID)
                return;
        }
        pinchPoints.Add(N);
    }
   

    public void FindGoodAmbushLocationsForAllPinchPoints(NSizeBitMatrix lineOfSightMatrix,List<Waypoint> waypoints)
    {
        for (int i = 0; i < pinchPoints.Count; i++)
        {
            BitArray linesOfSightToOutside = lineOfSightMatrix.GetCollumn(pinchPoints[i].OutsideID);
            BitArray linesOfSightOfPinchPoint = lineOfSightMatrix.GetRow(pinchPoints[i].ID);
            BitArray ambushLocations = linesOfSightToOutside.And(linesOfSightOfPinchPoint.Not());
            for (int j = 0; j < ambushLocations.Count; j++)
            {
                if (ambushLocations[j] == true)
                {
                    pinchPoints[i].ambushPointIDs.Add(j);
                }
            }
        }
       
    }




    public PinchPoint IsWaypointAPinchPoint(int waypoint)
    {
        foreach (var pinchPoint in pinchPoints)
        {
            if(waypoint == pinchPoint.ID)
            {
                return pinchPoint;
            }
            foreach (var insidePoint in pinchPoint.InsideIDs)
            {
                if (waypoint == insidePoint)
                    return pinchPoint;
            }
        }

        return null;
    }
}

[System.Serializable]
public struct ListOfWaypointsWithCertainExposure:IComparable<ListOfWaypointsWithCertainExposure>
{
    public List<int> IDs;
    public int exposure;

    public int CompareTo(ListOfWaypointsWithCertainExposure other)
    {
        return exposure.CompareTo(other.exposure);
    }
}

[System.Serializable]
public class PinchPoint
{
    public int ID;
    public List<int> InsideIDs;
    public int OutsideID;
    public List<int> ambushPointIDs;
    
    public PinchPoint(int iD, List<int> insideIDs, int outsideID)
    {
        ID = iD;
        InsideIDs = insideIDs;
        OutsideID = outsideID;
        ambushPointIDs = new List<int>();
    }
}

