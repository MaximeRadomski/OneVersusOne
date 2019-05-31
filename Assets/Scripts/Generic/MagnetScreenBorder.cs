using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScreenBorder : MonoBehaviour
{
	public CameraSide Side;
	public GameObjectType Type;

	private float _mult;

	void Start ()
	{
		_mult = Side == CameraSide.TopBorder ? 1.0f : -1.0f;
		Invoke ("AdjustPosition", 0.0f);
	}

	private void AdjustPosition()
	{
		float WorldSize;
		if (Type == GameObjectType.Text)
			WorldSize = this.GetComponent<RectTransform> ().rect.y * this.transform.lossyScale.y;
		else {
			//WorldSize = this.transform.lossyScale.y / -2f;
			WorldSize = this.GetComponent<SpriteRenderer>().bounds.size.y / -2;
		}
			
		transform.position = new Vector3(transform.position.x, (_mult * Camera.main.orthographicSize) + (_mult * WorldSize), 0.0f);
	}
}

public enum CameraSide
{
	TopBorder,
	BotBorder
}

public enum GameObjectType
{
	NonText,
	Text
}