using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleWallBehavior : MonoBehaviour
{
	public Sprite SpriteOn, SpriteOff;

	private int _order;

	void Start()
	{
		Disable ();
		_order = 1;
	}

	public void Enable()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteOn;
		this.gameObject.GetComponent<PolygonCollider2D> ().enabled = true;
	}

	public void Disable()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteOff;
		this.gameObject.GetComponent<PolygonCollider2D> ().enabled = false;
	}

	public void Reset()
	{
		Disable ();
		GameObject.Find ("MiddleWall02").GetComponent<MiddleWallBehavior> ().Disable ();
		GameObject.Find ("MiddleWall03").GetComponent<MiddleWallBehavior> ().Disable ();
		//var newWallId = Random.Range (1, 4);
		_order++;
		if (_order > 3)
			_order = 1;
		if (_order == 1)
			Enable ();
		else
			GameObject.Find ("MiddleWall" + _order.ToString("D2")).GetComponent<MiddleWallBehavior> ().Enable ();
	}
}
