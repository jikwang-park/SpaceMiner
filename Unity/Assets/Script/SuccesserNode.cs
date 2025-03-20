using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesserNode<T> : DecoratorNode<T> where T : MonoBehaviour
{
    public SuccesserNode(T context) : base(context)
    {
    }

    protected override NodeStatus ProcessChild()
    {
        NodeStatus status = child.Execute();

        status = NodeStatus.Success;
        return status;
    }
}
