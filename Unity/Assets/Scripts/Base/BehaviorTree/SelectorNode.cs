using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode<T> : CompositeNode<T> where T : MonoBehaviour
{
    private int currentChild;
    public SelectorNode(T context) : base(context)
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
        if(childrens.Count ==0)
        {
            return NodeStatus.Failure;
        }
        while (currentChild <childrens.Count)
        {
            NodeStatus nodeStatus = childrens[currentChild].Execute();
            if(nodeStatus != NodeStatus.Failure)
            {
                return nodeStatus;
            }
            ++currentChild;
        }
        return NodeStatus.Failure;
    }
}
