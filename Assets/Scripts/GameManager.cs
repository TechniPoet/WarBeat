using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public const string UnitTag = "Player";
    public const string StatueTag = "Finish";
    public float tUnitCost;
    public float bUnitCost;
    public static float _TUnitCost;
    public static float _BUnitCost;


    public StatueScript zero;
    public StatueScript one;


	// Use this for initialization
	void Awake ()
    {
        _TUnitCost = tUnitCost;
        _BUnitCost = bUnitCost;
	}
	
	// Update is called once per frame
	void Update () {
	    if (zero.health <= 0 || one.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
