using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WayPointEditor 
{
    //tell unity to draw gizmo regardless if gameobject is selected or not and make gizmopickable
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    //draws waypoint gizmo for visual debugging or setup aids in the Scene view.
    public static void OnDrawSceneGizmo(WayPoint waypoint, GizmoType gizmoType)
    {
        //if gizmo gets selected it will render a color
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            //else if gizmo is not selected color will be something else
            Gizmos.color = Color.yellow * 0.5f;
        }

        //waypoint gizmo line will be rendered as a spehre
        Gizmos.DrawSphere(waypoint.transform.position, -1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        if(waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
            Vector3 offsetTo = waypoint.previousWaypoint.transform.right * waypoint.previousWaypoint.width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.previousWaypoint.transform.position + offsetTo);
        }

        if (waypoint.nextWayPoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.width / 2f;
            Vector3 offsetTo = waypoint.nextWayPoint.transform.right * -waypoint.nextWayPoint.width / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.nextWayPoint.transform.position + offsetTo);


        }


    }
}
