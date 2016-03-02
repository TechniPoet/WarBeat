using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections;
using CondOptions = ConstFile.ConditionOptions;

public class ValDropdown : MonoBehaviour
{
	public Dropdown varCheck;
	public SpinnerFloat spin;
	public GameObject spinObj;
	public float currVal = 0;
	
	void Awake()
	{
		spinObj.SetActive(varCheck.value == (int)CondOptions.VALUE);
	}
	// Update is called once per frame
	void Update ()
	{
		spinObj.SetActive(varCheck.value == (int)CondOptions.VALUE);
	}
}
