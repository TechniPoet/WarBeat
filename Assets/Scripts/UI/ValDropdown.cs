using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections;

public class ValDropdown : MonoBehaviour
{
	public Dropdown varCheck;
	public SpinnerFloat spin;
	public GameObject spinObj;
	
	void Awake()
	{
		spinObj.SetActive(varCheck.value == 2);
	}
	// Update is called once per frame
	void Update ()
	{
		spinObj.SetActive(varCheck.value == 2);
	}
}
