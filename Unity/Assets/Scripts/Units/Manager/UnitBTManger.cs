using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public enum UnitTypes
{
    Tanker,
    Dealer,
    //Healer,
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
            default: 
                return InitTankBehaviorTree(context);
        }
    }

    public static BehaviorTree<Unit> InitTankBehaviorTree(Unit context)
    {
        //var tankBehaviortree = new BehaviorTree<Unit>(context);

        //var rootSelcetorNode = new SelectorNode<Unit>(context);

        //var attackSequence = new SquenceNode<Unit>(context);

        //attackSequence.AddChild(new IsUnitAliveCondition(context));
        //attackSequence.AddChild(new IsUnitOnRangeCondition(context));
        //attackSequence.AddChild(new IsUnitCanAttackCondition(context));
        //attackSequence.AddChild(new NormalAttackAction(context));

        //var moveSequence = new SquenceNode<Unit>(context);

        //moveSequence.AddChild(new IsUnitNotOnRangeCondition(context));
        //moveSequence.AddChild(new MoveAction(context));

        //rootSelcetorNode.AddChild(attackSequence);
        //rootSelcetorNode.AddChild(moveSequence);

        //tankBehaviortree.SetRoot(rootSelcetorNode);

        //return tankBehaviortree;

        var tankBehaviortree = new BehaviorTree<Unit>(context);

        var rootSelcetorNode = new SelectorNode<Unit>(context);
        tankBehaviortree.SetRoot(rootSelcetorNode);

        var attackSequence = new SquenceNode<Unit>(context);
        rootSelcetorNode.AddChild(attackSequence);

        attackSequence.AddChild(new IsUnitAliveCondition(context));
        attackSequence.AddChild(new IsUnitOnRangeCondition(context));
        attackSequence.AddChild(new IsUnitCanAttackCondition(context));
        attackSequence.AddChild(new NormalAttackAction(context));

        var moveSequence = new SquenceNode<Unit>(context);
        rootSelcetorNode.AddChild(moveSequence);

        moveSequence.AddChild(new IsUnitNotOnRangeCondition(context));
        moveSequence.AddChild(new MoveAction(context));

        rootSelcetorNode.AddChild(new IdleAction(context));

        return tankBehaviortree;






        //var tankerbehaviorTree = new BehaviorTree<Unit>(context);

        //var rootSelectorNode = new SelectorNode<Unit>(context);
        //tankerbehaviorTree.SetRoot(rootSelectorNode);

        //var attackSqence = new SquenceNode<Unit>(context);
        //rootSelectorNode.AddChild(attackSqence);
        //attackSqence.AddChild(new IsUnitAliveCondition(context));
        //attackSqence.AddChild(new IsUnitOnRangeCondition(context));
        //attackSqence.AddChild(new IsUnitUsingSkillCondition(context));

        //var attackTypeSelector = new SelectorNode<Unit>(context);
        //attackSqence.AddChild(attackTypeSelector);

        //var skillAttackSquence = new SquenceNode<Unit>(context);
        //attackTypeSelector.AddChild(skillAttackSquence);
        //skillAttackSquence.AddChild(new IsUnitHitCondition(context));
        //skillAttackSquence.AddChild(new IsSkillCoolTimeOnCondition(context));
        //skillAttackSquence.AddChild(new SkillAttackAction(context));
        //attackTypeSelector.AddChild(new NormalAttackAction(context));


        //rootSelectorNode.AddChild(new MoveAction(context));


        //var idleSquence = new SquenceNode<Unit>(context);
        //rootSelectorNode.AddChild(idleSquence);
        //idleSquence.AddChild(new IsUnitOnRangeCondition(context));
        //idleSquence.AddChild(new IdleAction(context));

        //return tankerbehaviorTree;

    }

    public static BehaviorTree<Unit> InitDelarBehaviorTree(Unit context)
    {
        var dealerBehaviorTree = new BehaviorTree<Unit>(context);

        var rootSelcetorNode = new SelectorNode<Unit>(context);

        var attackSequence = new SquenceNode<Unit>(context);

        attackSequence.AddChild(new IsUnitAliveCondition(context));
        attackSequence.AddChild(new IsUnitOnRangeCondition(context));
        attackSequence.AddChild(new IsUnitCanAttackCondition(context));
        attackSequence.AddChild(new NormalAttackAction(context));

        var moveSequence = new SquenceNode<Unit>(context);

        moveSequence.AddChild(new IsUnitNotOnRangeCondition(context));
        moveSequence.AddChild(new MoveAction(context));

        rootSelcetorNode.AddChild(attackSequence);
        rootSelcetorNode.AddChild(moveSequence);
        rootSelcetorNode.AddChild(new IdleAction(context));
        dealerBehaviorTree.SetRoot(rootSelcetorNode);

        return dealerBehaviorTree;
    }

}
