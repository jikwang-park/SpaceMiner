using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode<T> : BehaviorNode<T> where T : MonoBehaviour
{
    protected readonly List<BehaviorNode<T>> childrens = new List<BehaviorNode<T>>();
    protected CompositeNode(T context) : base(context)
    {
    }

    public void AddChild(BehaviorNode<T> node)
    {
        childrens.Add(node);
    }

    public bool RemoveChilde(BehaviorNode<T> node)
    {
        return childrens.Remove(node); 
    }

    public override void Reset()
    {
        base.Reset();
        foreach (var node in childrens)
        {
            node.Reset();
        }
    }
}
