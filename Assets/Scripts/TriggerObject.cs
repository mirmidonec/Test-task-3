using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : Interactable
{
public bool isClickable;

	public override void SecondStart()
	{
		base.SecondStart();
		PlayerInteract.Instance.onGrabObjEquip.AddListener(OnGrabObjEquiped);
		if (!isClickable)
		{
			base.gameObject.layer = 0;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (canInteract)
		{
			GrabObject component = other.gameObject.GetComponent<GrabObject>();
			if (component != null && !PlayerInteract.Instance.isCurrentGrabObject(component))
			{
				OnTriggerOrCollision(other.gameObject.GetComponent<GrabObject>());
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canInteract)
		{
			GrabObject component = other.gameObject.GetComponent<GrabObject>();
			if (component != null && !PlayerInteract.Instance.isCurrentGrabObject(component))
			{
				OnTriggerOrCollision(other.gameObject.GetComponent<GrabObject>());
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log(other.name, other.gameObject);
		if (canInteract)
		{
			GrabObject component = other.gameObject.GetComponent<GrabObject>();
			if (component != null && !PlayerInteract.Instance.isCurrentGrabObject(component))
			{
				OnTriggerExitEvent(other.gameObject.GetComponent<GrabObject>());
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (canInteract)
		{
			GrabObject component = other.gameObject.GetComponent<GrabObject>();
			if (component != null && !PlayerInteract.Instance.isCurrentGrabObject(component))
			{
				OnTriggerOrCollision(other.gameObject.GetComponent<GrabObject>());
			}
		}
	}

	public virtual void OnTriggerOrCollision(GrabObject tempGrabObj)
	{
	}

	public virtual void OnTrigger(GrabObject tempGrabObj)
	{
	}

	public virtual void OnTriggerExitEvent(GrabObject tempGrabObj)
	{
	}

	public virtual void OnCollision(GrabObject tempGrabObj)
	{
	}

	public virtual void OnGrabObjEquiped(GrabObject tempGrabObj)
	{
	}

	public virtual void OnTriggerUpdate()
	{
	}
}
