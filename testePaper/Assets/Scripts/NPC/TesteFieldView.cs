using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TesteFieldView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angleVision = 30;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float distanceTarget;

    public List<Transform> targetsTransformView = new List<Transform>();

    public string chaseColor;
    public bool colorDetect = false;

    private void Start()
    {
        StartCoroutine(StartFindTarget(.7f));

        //DEBUG
        //------
        //float radians = angleVision * Mathf.Deg2Rad;
        //Debug.Log("Degrees: " + angleVision + ", Radians: " + radians + ", Sin: " + Mathf.Sin(radians) + ", Cos: " + Mathf.Cos(radians));
    }

    //public void LateUpdate()
    //{
    //    if (chaseColor.Equals(PlayerMove.intance.currentColorName))
    //    {
    //        colorDetect = true;
    //        Move();
    //    }
    //    else
    //    {
    //        colorDetect = false;
    //    }
    //}

    public void Move(float speed)
    {
        try
        {
            Transform alvo = targetsTransformView[0];
            transform.LookAt(alvo);
            if (distanceTarget - 2.5f > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(alvo.position.x, transform.position.y, alvo.position.z), speed * Time.deltaTime);
            }
        }
        catch (Exception e)
        {
            
        }
    }

    IEnumerator StartFindTarget(float delay)
    {

        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTarget();
        }
    }

    void FindTarget()
    {

        targetsTransformView.Clear();

        Collider[] viewTarget = Physics.OverlapSphere(transform.position, radius, targetMask);

        for (int i = 0; i < viewTarget.Length - 1; i++)
        {
            Transform targetTrasnfom = viewTarget[i].transform;
            Vector3 dirTarget = (targetTrasnfom.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirTarget) < angleVision / 2)
            {
                distanceTarget = Vector3.Distance(transform.position, targetTrasnfom.position);

                if (!Physics.Raycast(transform.position, dirTarget, distanceTarget, obstacleMask))
                {
                    targetsTransformView.Add(targetTrasnfom);
                }
            }
        }
    }

    public Vector3 DirView(float angleInDegrees, bool globalAngle)
    {
        if (!globalAngle)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

[CustomEditor(typeof(TesteFieldView))]
public class EditorTeste : Editor
{
    private void OnSceneGUI()
    {
        TesteFieldView tsv = (TesteFieldView)target;

        Handles.color = Color.red;
        //Handles.DrawWireDisc(tsv.transform.position, Vector3.up, tsv.radius);
        Handles.DrawWireArc(tsv.transform.position, Vector3.up, Vector3.forward, 360, tsv.radius);
        Handles.color = Color.black;
        Handles.DrawWireArc(tsv.transform.position, Vector3.up, Vector3.forward, 360, 1.2f);

        Vector3 pointA = tsv.DirView(-tsv.angleVision / 2, false);
        Vector3 pointB = tsv.DirView(tsv.angleVision / 2, false);

        Handles.color = Color.blue;
        Handles.DrawLine(tsv.transform.position, tsv.transform.position + pointA * tsv.radius);
        Handles.DrawLine(tsv.transform.position, tsv.transform.position + pointB * tsv.radius);

        foreach (Transform t in tsv.targetsTransformView)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(tsv.transform.position, t.position);
        }
    }
}
