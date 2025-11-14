using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCapsule : TriggerObject
{
    public CoffeeStation coffeeStation;

    public Transform capsuleParent;

    public override void OnTriggerOrCollision(GrabObject tempGrabObj)
    {
        base.OnTriggerOrCollision(tempGrabObj);
        if (tempGrabObj.grabObjectID == 6 && !coffeeStation.alreadyHasCoffeeCapsule())
        {
            coffeeStation.SetCoffeeCapsule();
            AudioController.Instance.SpawnCombineSoundAtPos(base.transform.position);
            Object.Destroy(tempGrabObj.gameObject);
        }
    }
}
