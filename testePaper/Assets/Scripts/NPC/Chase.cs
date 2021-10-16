using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chase : State
{
    public TesteFieldView tfv;
    public string chaseColor;
    public Patrol patrolState;
    public float speed = 3f;

    private void Start()
    {
        tfv = GetComponentInParent<TesteFieldView>();
    }

    public override State RunCurrentState()
    {
        Debug.Log("Move");
        if (chaseColor.Equals(PlayerMove.intance.currentColorName))
        {
            tfv.Move(3f);
            return this;
        }
        else
        {
            return patrolState;
        }
    }

    //void Move()
    //{
    //    try
    //    {
    //        Transform alvo = tfv.targetsTransformView[0];
    //        transform.LookAt(alvo);
    //        if (tfv.distanceTarget - 3f > 0)
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, new Vector3(alvo.position.x, transform.position.y, alvo.position.z), speed * Time.deltaTime);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogException(e, this);
    //    }
    //}
}
