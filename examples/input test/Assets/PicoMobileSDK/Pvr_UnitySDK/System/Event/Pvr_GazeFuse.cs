using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pvr_GazeFuse : MonoBehaviour 
{
	public GameObject gazeGameObject;
	private Image image;

	void Start() 
	{
		image = GetComponent<Image>();
	}
	
	void Update() 
	{
		if (gazeGameObject == null || Pvr_GazeInputModule.gazeGameObject == gazeGameObject) 
		{
			FuseAmountChanged(Pvr_GazeInputModule.gazeFraction);
		}
	}

	void FuseAmountChanged(float fuseAmount)
	{
		if (image != null)
		{
			image.fillAmount = fuseAmount;
		}
	}

}
