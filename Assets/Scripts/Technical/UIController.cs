using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    private Slider Epx, Heat, Charge;

    void Start ()
    {
        Epx = transform.GetChild(1).GetComponent<Slider>();
        Heat = transform.GetChild(2).GetComponent<Slider>();
        Charge = transform.GetChild(3).GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Epx.value = Dragon.Exp;
        Heat.value = Dragon.Heat;
        Charge.value = Dragon.Charge;
	}
}
