using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquenceNode<T> : CompositeNode<T> where T : MonoBehaviour
{
    protected int currentChild;
    public SquenceNode(T context) : base(context)
    {
    }

    public override void Reset()
    {
        base.Reset();
        currentChild = 0;
    }

    protected override void OnStart()
    {
        base.OnStart();
        currentChild = 0;
    }
    protected override NodeStatus OnUpdate()
    {
        if(currentChild == 0)
        {
            return NodeStatus.Success;
        }
        
        while(currentChild <= childrens.Count)
        {
            NodeStatus status = childrens[currentChild].Execute();
            if (status != NodeStatus.Success)
            {
                return status;
            }

            ++currentChild;
        }

        return NodeStatus.Failure;
    }
}
