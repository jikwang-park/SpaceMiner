using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            case UnitTypes.Dealer:
                return InitDelarBehaviorTree(context);
            case UnitTypes.Healer:
                return InitHealerBehaviorTree(context);
            default:
                return null;
        }
    }

    public static BehaviorTree<Unit> SetBehaviorTree(Unit context, UnitTypes type)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                return InitTankBehaviorTree(context);

            case UnitTypes.Dealer:
                return InitDelarBehaviorTree(context);
            case UnitTypes.Healer:
                return InitHealerBehaviorTree(context);
            default:
                return InitTankBehaviorTree(context);
        }
    }

    public static BehaviorTree<Unit> InitTankBehaviorTree(Unit context)
    {
        var tankBehaviortree = new BehaviorTree<Unit>(context);

        var rootSelectorNode = new SelectorNode<Unit>(context);
        
        

        var attackSequence = new SquenceNode<Unit>(context);
        attackSequence.AddChild(new IsUnitAliveCondition(context));
        attackSequence.AddChild(new IsUnitOnAttackRangeCondition(context));

        var attackSelectorNode = new SelectorNode<Unit>(context);

        var skillAttackSequence = new SquenceNode<Unit>(context);
        skillAttackSequence.AddChild(new IsAutoSkillModeCondition(context));
        skillAttackSequence.AddChild(new IsTankerCanUseSkillCondition(context));
        skillAttackSequence.AddChild(new SkillAttackAction(context));

        var normalAttackSequence = new SquenceNode<Unit>(context);
        normalAttackSequence.AddChild(new IsUnitCanNormalAttackCondition(context));
        normalAttackSequence.AddChild(new NormalAttackAction(context));
        //
        var moveSelector = new SelectorNode<Unit>(context);

        var frontUnitMoveSequence = new SquenceNode<Unit>(context);
        frontUnitMoveSequence.AddChild(new IsUnitExceedMonsterPosCondition(context));
        frontUnitMoveSequence.AddChild(new IsUnitFrontLineCondition(context));
        frontUnitMoveSequence.AddChild(new IsUnitInSafeDistanceCondition(context));
        frontUnitMoveSequence.AddChild(new MoveAction(context));

     
        var idleSequence = new SquenceNode<Unit>(context);
        idleSequence.AddChild(new IsUnitCanIdleCondition(context));
        idleSequence.AddChild(new IdleAction(context));

        tankBehaviortree.SetRoot(rootSelectorNode);

        rootSelectorNode.AddChild(attackSequence);

        attackSequence.AddChild(attackSelectorNode);
        attackSelectorNode.AddChild(skillAttackSequence);
        attackSelectorNode.AddChild(normalAttackSequence);

        rootSelectorNode.AddChild(moveSelector);
        moveSelector.AddChild(frontUnitMoveSequence);


        rootSelectorNode.AddChild(idleSequence);

        return tankBehaviortree;
    }

    public static BehaviorTree<Unit> InitDelarBehaviorTree(Unit context)
    {
        var tankBehaviortree = new BehaviorTree<Unit>(context);

        var rootSelectorNode = new SelectorNode<Unit>(context);



        var attackSequence = new SquenceNode<Unit>(context);
        attackSequence.AddChild(new IsUnitAliveCondition(context));
        attackSequence.AddChild(new IsUnitOnAttackRangeCondition(context));

        var attackSelectorNode = new SelectorNode<Unit>(context);

        var skillAttackSequence = new SquenceNode<Unit>(context);
        skillAttackSequence.AddChild(new IsAutoSkillModeCondition(context));
        skillAttackSequence.AddChild(new IsDealerCanUseSkill(context));
        skillAttackSequence.AddChild(new SkillAttackAction(context));

        var normalAttackSequence = new SquenceNode<Unit>(context);
        normalAttackSequence.AddChild(new IsUnitCanNormalAttackCondition(context));
        normalAttackSequence.AddChild(new NormalAttackAction(context));
        //
        var moveSelector = new SelectorNode<Unit>(context);



        var notFrontUnitMoveSequence = new SquenceNode<Unit>(context);

        notFrontUnitMoveSequence.AddChild(new IsUnitExceedMonsterPosCondition(context));

        notFrontUnitMoveSequence.AddChild(new IsUnitInSafeDistanceCondition(context));
        notFrontUnitMoveSequence.AddChild(new MoveAction(context));

        var idleSequence = new SquenceNode<Unit>(context);
        idleSequence.AddChild(new IsUnitCanIdleCondition(context));
        idleSequence.AddChild(new IdleAction(context));

        tankBehaviortree.SetRoot(rootSelectorNode);

        rootSelectorNode.AddChild(attackSequence);

        attackSequence.AddChild(attackSelectorNode);
        attackSelectorNode.AddChild(skillAttackSequence);
        attackSelectorNode.AddChild(normalAttackSequence);

        rootSelectorNode.AddChild(moveSelector);
        moveSelector.AddChild(notFrontUnitMoveSequence);


        rootSelectorNode.AddChild(idleSequence);

        return tankBehaviortree;

    }
    public static BehaviorTree<Unit> InitHealerBehaviorTree(Unit context)
    {
        var tankBehaviortree = new BehaviorTree<Unit>(context);

        var rootSelectorNode = new SelectorNode<Unit>(context);



        var attackSequence = new SquenceNode<Unit>(context);
        attackSequence.AddChild(new IsUnitAliveCondition(context));
        attackSequence.AddChild(new IsUnitOnAttackRangeCondition(context));

        var attackSelectorNode = new SelectorNode<Unit>(context);

        var skillAttackSequence = new SquenceNode<Unit>(context);
        skillAttackSequence.AddChild(new IsAutoSkillModeCondition(context));
        skillAttackSequence.AddChild(new IsHealerCanUseSkillCondition(context));
        skillAttackSequence.AddChild(new SkillAttackAction(context));

        var normalAttackSequence = new SquenceNode<Unit>(context);
        normalAttackSequence.AddChild(new IsUnitCanNormalAttackCondition(context));
        normalAttackSequence.AddChild(new NormalAttackAction(context));
        //
        var moveSelector = new SelectorNode<Unit>(context);


        var notFrontUnitMoveSequence = new SquenceNode<Unit>(context);
        notFrontUnitMoveSequence.AddChild(new IsUnitExceedMonsterPosCondition(context));

        notFrontUnitMoveSequence.AddChild(new IsUnitInSafeDistanceCondition(context));
        notFrontUnitMoveSequence.AddChild(new MoveAction(context));

        var idleSequence = new SquenceNode<Unit>(context);
        idleSequence.AddChild(new IsUnitCanIdleCondition(context));
        idleSequence.AddChild(new IdleAction(context));

        tankBehaviortree.SetRoot(rootSelectorNode);

        rootSelectorNode.AddChild(attackSequence);

        attackSequence.AddChild(attackSelectorNode);
        attackSelectorNode.AddChild(skillAttackSequence);
        attackSelectorNode.AddChild(normalAttackSequence);

        rootSelectorNode.AddChild(moveSelector);
        moveSelector.AddChild(notFrontUnitMoveSequence);


        rootSelectorNode.AddChild(idleSequence);

        return tankBehaviortree;
    }

}
