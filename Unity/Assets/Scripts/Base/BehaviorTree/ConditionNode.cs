using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    public ConditionNode(T context) : base(context)
    {
    }

    protected abstract override NodeStatus OnUpdate();
}
