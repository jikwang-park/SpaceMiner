using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum NodeStatus
{
    Success,
    Failure,
    Running,
}

public abstract class BehaviorNode<T> where T : MonoBehaviour
{
    protected readonly T context;
    protected bool isStated = false;

    protected BehaviorNode(T context)
    {
        this.context = context;
    }

    public virtual void Reset()
    {
        isStated = false;
    }

    protected virtual void OnStart()
    {

    }

    protected abstract NodeStatus OnUpdate();

    protected virtual void OnEnd()
    {

    }

    public NodeStatus Execute()
    {
        if(!isStated)
        {
            isStated = true;
            OnStart();
        }
        NodeStatus status = OnUpdate();
        if (status != NodeStatus.Running)
        {
            OnEnd();
            isStated= false;
        }
        return status;
    }
}
