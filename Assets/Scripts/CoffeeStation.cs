using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeStation : TriggerObject
{
public GameObject greenLight;

	private bool hasCoffeeCapsule;

	public bool hasPower = true;

	public Cup currentCraftingObj;

	public Animator coffeePourObj;

	public Transform cupPos;

	public GameObject fakeCapsuleObj;

	public Transform capsuleStartPos;

	public Transform capsuleEndPos;

	public ParticleSystem steamParticle;

	private Coroutine tempCor;

	private bool coffeePoured;

	public AudioObject coffeSound;

	public override void OnTriggerOrCollision(GrabObject tempGrabObj)
	{
		base.OnTriggerOrCollision(tempGrabObj);
		if (currentCraftingObj != null && currentCraftingObj == tempGrabObj)
		{
			return;
		}
		if (tempGrabObj.grabObjectID == 4 && currentCraftingObj == null && tempGrabObj.GetComponent<Cup>() != null && tempGrabObj.name != "Coffee")
		{
			currentCraftingObj = tempGrabObj.GetComponent<Cup>();
			currentCraftingObj.GetComponent<Rigidbody>().isKinematic = true;
			currentCraftingObj.Equip(cupPos, resetRot: true);
			currentCraftingObj.ResetScale();
			currentCraftingObj.canInteract = false;
			tempGrabObj.SetMeshesLayer(6);
			if (alreadyHasCoffeeCapsule())
			{
				SetCanMakeCoffee();
			}
		}
		else if (currentCraftingObj != null && tempGrabObj.grabObjectID == 5 && hasCoffeeCapsule && coffeePoured)
		{
			tempGrabObj.SetOnPlace(currentCraftingObj.lidPosition, currentCraftingObj, setParentOfGrabObj: false);
			currentCraftingObj.grabObjectID = 7;
			currentCraftingObj.canInteract = true;
			currentCraftingObj.GetChildMeshes();
			currentCraftingObj.CoffeeCompleted();
			greenLight.SetActive(value: false);
			SetCoffeeCapsule(b: false);
			coffeePoured = false;
		}
	}

	public override void PressVirtual()
	{
		base.PressVirtual();
		Debug.Log("Make coffee");
	}

	private void SetCanMakeCoffee()
	{
		if (hasPower)
		{
			tempCor = StartCoroutine(pouringCoffee());
		}
	}

	public void SetPower(bool b)
	{
		hasPower = b;
		if (b && currentCraftingObj != null && alreadyHasCoffeeCapsule())
		{
			SetCanMakeCoffee();
		}
	}

	private IEnumerator pouringCoffee()
	{
		yield return new WaitForSeconds(0.3f);
		float lerpValue = 0f;
		float time = 1f;
		Vector3 startPos = capsuleStartPos.position;
		Vector3 endPos = capsuleEndPos.position;
		while (lerpValue <= time)
		{
			fakeCapsuleObj.transform.position = Vector3.Lerp(startPos, endPos, lerpValue / time);
			lerpValue += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		greenLight.SetActive(value: true);
		fakeCapsuleObj.transform.position = endPos;
		fakeCapsuleObj.gameObject.SetActive(value: false);
		coffeePourObj.gameObject.SetActive(value: true);
		coffeePourObj.Play("CoffeeAnimPour", -1, 0f);
		steamParticle.Play(withChildren: true);
		coffeSound.PlayWithPitch(1f);
		time = 8f;
		lerpValue = 0f;
		while (lerpValue <= time)
		{
			currentCraftingObj.SetCoffeeValue(lerpValue / time * 100f);
			lerpValue += Time.deltaTime;
			yield return null;
		}
		steamParticle.Stop();
		currentCraftingObj.SetCoffeeValue(100f);
		coffeePourObj.Play("CoffeePourEnd", -1, 0f);
		yield return new WaitForSeconds(1f);
		coffeePourObj.gameObject.SetActive(value: false);
		greenLight.SetActive(value: false);
		tempCor = null;
		coffeePoured = true;
	}

	public override void OnGrabObjEquiped(GrabObject tempGrabObj)
	{
		base.OnGrabObjEquiped(tempGrabObj);
		if (currentCraftingObj == tempGrabObj)
		{
			currentCraftingObj.ResetScale();
			currentCraftingObj = null;
		}
	}

	public void SetCoffeeCapsule(bool b = true)
	{
		if (b && currentCraftingObj != null)
		{
			SetCanMakeCoffee();
		}
		if (b)
		{
			fakeCapsuleObj.transform.position = capsuleStartPos.transform.position;
			fakeCapsuleObj.SetActive(value: true);
		}
		hasCoffeeCapsule = b;
	}

	public bool alreadyHasCoffeeCapsule()
	{
		return hasCoffeeCapsule;
	}
}
