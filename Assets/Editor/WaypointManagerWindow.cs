using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow {

    [MenuItem("Tools/WayPoint Editor")]
    //this method will let us efficiently manage our waypoint system
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform must be selected. Please assign root transofrm", MessageType.Warning); ;
        } else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();

    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        //selected gameobject is not empty & contains waypoint component
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            //then create waypoint before
            if(GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }
            //create waypoint after 
            if(GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            //remove previous waypoint
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }

    void CreateWaypoint()
    {
        GameObject waypointObjeect = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObjeect.transform.SetParent(waypointRoot, false);

        WayPoint wayPoint = waypointObjeect.GetComponent<WayPoint>();
        if (waypointRoot.childCount > 1)
        {
            wayPoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<WayPoint>();
            wayPoint.previousWaypoint.nextWayPoint = wayPoint;
            //Place the waypoint at the last position
            wayPoint.transform.position = wayPoint.previousWaypoint.transform.position;
            wayPoint.transform.forward = wayPoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = wayPoint.gameObject;

    }

    void CreateWaypointBefore()
    {
        //create new waypoints
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        //get new object waypoint of waypoint components
        WayPoint newWayPoint = waypointObject.GetComponent<WayPoint>();
        //generate waypoints for selected points for our objects with waypoint component
        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();
        //waypointObjects of position is now the new slected waypoint of our position
        waypointObject.transform.position = selectedWaypoint.transform.position;
        //move forward that position
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        //check if selected waypoint has a previous waypoint assigned
       if(selectedWaypoint.previousWaypoint != null)
        {
            //set that to our previous waypoints 
            newWayPoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            //set that to our next waypoint
            selectedWaypoint.previousWaypoint.nextWayPoint = newWayPoint;
        }

        //new waypoint is now our selected waypoint
        newWayPoint.nextWayPoint = selectedWaypoint;

        //selected waypoint of our previous point is now being passed the newWayPoint
        selectedWaypoint.previousWaypoint = newWayPoint;

        //change the GameObject’s place in this hierarchy.useful 
        //if you are intentionally ordering the children of a GameObject such as when using Layout Group components
        //newway point sets new position of gameobjects within index of set & get sibling
        newWayPoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        //select new way point for active objects
        Selection.activeGameObject = newWayPoint.gameObject;

    }

    void CreateWaypointAfter()
    {
        //create new waypoints
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        //get new object waypoint of waypoint components
        WayPoint newWayPoint = waypointObject.GetComponent<WayPoint>();
        //generate waypoints for selected points for our objects with waypoint component
        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();
        //waypointObjects of position is now the new slected waypoint of our position
        waypointObject.transform.position = selectedWaypoint.transform.position;
        //move forward that position
        waypointObject.transform.forward = selectedWaypoint.transform.forward;
        //previous point of new waypoint is now our selected point
        newWayPoint.previousWaypoint = selectedWaypoint;

        //check if selected waypoint has a next waypoint assigned
        if (selectedWaypoint.nextWayPoint != null)
        {
            //set that the selected waypoint of our next waypoint from our previous selected waypoint to new point
            selectedWaypoint.nextWayPoint.previousWaypoint = newWayPoint;
            //set our new waypoint to next way point of selected waypoint
            newWayPoint.nextWayPoint = selectedWaypoint.nextWayPoint;
        }

        selectedWaypoint.nextWayPoint = newWayPoint;

        newWayPoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWayPoint.gameObject;

    }

    void RemoveWaypoint()
    {
        //select the waypoints 
        WayPoint selectecWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        //if next selected waypoint is not empty
        if(selectecWaypoint.nextWayPoint != null)
        {
            //selected waypoint of next and previous point is removed
            selectecWaypoint.nextWayPoint.previousWaypoint = selectecWaypoint.previousWaypoint;
        }

        if (selectecWaypoint.previousWaypoint != null)
        {
            //selected waypoint of next and previous point is now the next point
            selectecWaypoint.previousWaypoint.nextWayPoint = selectecWaypoint.nextWayPoint;
            //make selection of new waypoint active
            Selection.activeGameObject = selectecWaypoint.previousWaypoint.gameObject;
        }
        DestroyImmediate(selectecWaypoint.gameObject);
    }
}
