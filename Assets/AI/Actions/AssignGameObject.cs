using UnityEngine;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AssignGameObject : RAINAction
{

	Expression objectName = new Expression();
	Expression variableName = new Expression();
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		return ActionResult.SUCCESS;
	}

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}