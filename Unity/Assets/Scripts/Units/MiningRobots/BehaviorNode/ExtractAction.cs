using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractAction : ActionNode<MiningRobotController>
{
    public ExtractAction(MiningRobotController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override NodeStatus OnUpdate()
    {
        ItemManager.AddItem(context.PlanetData.ItemID, context.ProductCapacity);
        SoundManager.Instance.PlaySFX("GainMineSFX");
        context.ChangeTarget(true);
        return NodeStatus.Success;
    }
}
