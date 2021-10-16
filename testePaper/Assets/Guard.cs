using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float speed = 5f;
    public float waitTime = .3f;
    public float turnSpeed = .2f;
    bool coroutineON;
    public Transform pathHolder;
    Vector3[] wayPointsControl;


    private void Start()
    {
        Vector3[] wayPoints = new Vector3[pathHolder.childCount];
        wayPointsControl = wayPoints;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = pathHolder.GetChild(i).position;
            wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
        }

        StartCoroutine(FollowPath(wayPoints, true));
    }

    public void PatrolControl(bool patrol)
    {
        if (patrol && !coroutineON)
        {
            StartCoroutine(FollowPath(wayPointsControl, true));
        }
        else if(coroutineON)
        {
            StopCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath(Vector3[] wayPoints, bool patrol)
    {
        coroutineON = true;
        transform.position = wayPoints[0];

        int targetWayPopintsIndex = 1;
        Vector3 targetWayPoint = wayPoints[targetWayPopintsIndex];
        transform.LookAt(targetWayPoint);

        while (patrol)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);
            if (transform.position == targetWayPoint)
            {
                targetWayPopintsIndex = (targetWayPopintsIndex + 1) % wayPoints.Length;
                targetWayPoint = wayPoints[targetWayPopintsIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnFace(targetWayPoint));
            }
            yield return null;
        }
        coroutineON = false;
    }

    IEnumerator TurnFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .2f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
