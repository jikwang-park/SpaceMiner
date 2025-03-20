using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree<T> where T : MonoBehaviour
{
    private readonly T context;
    private BehaviorNode<T> rootNode;

    public BehaviorTree(T context)
    {
        this.context = context;
    }

    public void SetRoot(BehaviorNode<T> node)
    {
        this.rootNode = node;
    }

    public NodeStatus Update()
    {
        if(rootNode == null)
        {
            return NodeStatus.Failure;
        }

        return rootNode.Execute();
    }

    public void Reset()
    {
        if(rootNode == null)
        {
            rootNode.Reset();
        }
    }

 
}
