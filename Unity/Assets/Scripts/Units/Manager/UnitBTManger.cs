using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public enum UnitTypes
{
    Tanker = 1,
    Dealer,
    Healer,
}

public static class UnitBTManager
{
  
    public static BehaviorTree<Unit> GetBehaviorTree(Unit context, UnitTypes types)
    {
        switch (types)
        {
            case UnitTypes.Tanker:
                return InitTankBehaviorTree(context);

            default: 
                return InitTankBehaviorTree(context);
        }
    }

    public static BehaviorTree<Unit> InitTankBehaviorTree(Unit context)
    {
        var tankerbehaviorTree = new BehaviorTree<Unit>(context);
        
        var rootSelectorNode = new SelectorNode<Unit>(context);
        tankerbehaviorTree.SetRoot(rootSelectorNode);

        var attackSqence = new SquenceNode<Unit>(context);
        rootSelectorNode.AddChild(attackSqence);
        attackSqence.AddChild(new IsUnitAliveCondition(context));
        attackSqence.AddChild(new IsUnitCanAttackCondition(context));
        attackSqence.AddChild(new IsUnitUsingSkillCondition(context));
        
        var attackTypeSelector = new SelectorNode<Unit>(context);
        attackSqence.AddChild(attackTypeSelector);
        
        var skillAttackSquence = new SquenceNode<Unit>(context);
        attackTypeSelector.AddChild(skillAttackSquence);
        skillAttackSquence.AddChild(new IsUnitHitCondition(context));
        skillAttackSquence.AddChild(new IsSkillCoolTimeOnCondition(context));
        skillAttackSquence.AddChild(new SkillAttackAction(context));
        attackTypeSelector.AddChild(new NormalAttackAction(context));

        rootSelectorNode.AddChild(new MoveAction(context));

        var idleSquence = new SquenceNode<Unit>(context);
        rootSelectorNode.AddChild(idleSquence);
        idleSquence.AddChild(new IsUnitCanAttackCondition(context));
        idleSquence.AddChild(new IdleAction(context));

        return tankerbehaviorTree;
        
    }
}
