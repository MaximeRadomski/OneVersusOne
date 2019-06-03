using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButtonContainer : MonoBehaviour
{
	public void SetChildrenGradiantOpacityToVisible()
	{
		for (int i = 0; i < this.transform.childCount; ++i)
		{
			this.transform.GetChild(i).GetComponent<Animator> ().Play ("GradiantOpacityToVisible");
		}
	}
}
