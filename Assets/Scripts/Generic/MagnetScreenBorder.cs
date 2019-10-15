using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScreenBorder : MonoBehaviour
{
	public CameraVerticalSide VerticalSide;
    public CameraHorizontalSide HorizontalSide;
    public GameObjectType Type;

	private float _verticalMult;
    private float _horizontalMult;

    void Start ()
	{
        if (VerticalSide != CameraVerticalSide.None)
        {
            _verticalMult = VerticalSide == CameraVerticalSide.TopBorder ? 1.0f : -1.0f;
            Invoke("AdjustVerticalPosition", 0.0f);
        }
        if (HorizontalSide != CameraHorizontalSide.None)
        {
            _horizontalMult = HorizontalSide == CameraHorizontalSide.RightBorder ? 1.0f : -1.0f;
            Invoke("AdjustHorizontalPosition", 0.0f);
        }
    }

	private void AdjustVerticalPosition()
	{
		float WorldYSize;
		if (Type == GameObjectType.Text)
			WorldYSize = this.GetComponent<RectTransform> ().rect.y * this.transform.lossyScale.y;
		else {
			//WorldSize = this.transform.lossyScale.y / -2f;
			WorldYSize = this.GetComponent<SpriteRenderer>().bounds.size.y / -2.0f;
		}
			
		transform.position = new Vector3(transform.position.x, (_verticalMult * Camera.main.orthographicSize) + (_verticalMult * WorldYSize), 0.0f);
	}

    private void AdjustHorizontalPosition()
    {
        float WorldXSize;
        if (Type == GameObjectType.Text)
            WorldXSize = this.GetComponent<RectTransform>().rect.x * this.transform.lossyScale.x;
        else
        {
            //WorldSize = this.transform.lossyScale.x / -2f;
            WorldXSize = this.GetComponent<SpriteRenderer>().bounds.size.x / -2.0f;
        }

        transform.position = new Vector3((_horizontalMult * Camera.main.orthographicSize * Camera.main.aspect) + (_horizontalMult * (WorldXSize - 0.05f)), transform.position.y, 0.0f);
    }
}

public enum CameraVerticalSide
{
    None,
	TopBorder,
	BotBorder
}

public enum CameraHorizontalSide
{
    None,
    LeftBorder,
    RightBorder
}

public enum GameObjectType
{
	NonText,
	Text
}