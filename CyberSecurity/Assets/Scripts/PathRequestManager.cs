using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    //A queue that stores path requests 
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    //Adds curent path request into a queue
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newReqeust = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newReqeust);
        instance.TryProcessNext();
    }

    //Processes current path request
    void TryProcessNext()
    {
        //If the script is currently not processing a path and 
        //if there is a request in the queue...
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            //Get first request in the queue
            currentPathRequest = pathRequestQueue.Dequeue();
            //Set 'isProcessingPath' to true
            isProcessingPath = true;
            //Begin pathfinding
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    //Finishes the pathfinding process by returning 'callback' and starts the next request (if there are any)
    public void FinishedProcessing(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    public HashSet<Node> HighlightMovement(Vector3 currentPos)
    {
        return pathfinding.MovementRadius(currentPos);
    }

    //A structure that stores the starting position of the object, the target position of the object
    //and a variable that contains a vector3 list for the path and a bool to determine if the selected
    //path is succesful or not
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}

