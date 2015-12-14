using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Variables
    private Slider Epx, Heat, Charge;
    private Image heatFill, chargeFill;
    private float lastCharge;
    private bool lastChargeLerping = false;
    #endregion
    #region Setup
    void OnEnable ()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    void Start()
    {
        Epx = transform.GetChild(1).GetComponent<Slider>();
        Heat = transform.GetChild(2).GetComponent<Slider>();
        Charge = transform.GetChild(3).GetComponent<Slider>();

        heatFill = Heat.transform.GetChild(1).GetComponentInChildren<Image>();
        chargeFill = Charge.transform.GetChild(1).GetComponentInChildren<Image>();
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        Epx.value = Dragon.Exp;

        Heat.value = Dragon.Heat;
        if (Dragon.Heat > 80 && !Dragon.Overheat)
        {
            heatFill.color = Color.Lerp(Color.white, Color.red, (Heat.value - 80f) / 20f);
        }
        else if (Dragon.Overheat)
        {
            heatFill.color = Color.magenta;
        }
        else
        {
            heatFill.color = Color.white;
        }

        if (lastCharge > 0 && Dragon.Charge == 0)
        {
            lastChargeLerping = true;
            StartCoroutine(LastChargeLerp(130, lastCharge));
        }
        else if (!lastChargeLerping)
        {
            Charge.value = Dragon.Charge;
            chargeFill.color = Color.Lerp(Color.white, Color.blue, Charge.value / 100f);
        }

        lastCharge = Dragon.Charge;
    }
    
    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Pause)
        {
            Debug.Log("Upgade Screen");
        }

        if (prevGlobalState == State.GlobalState.Pause)
        {
            Debug.Log("Resume Game");
        }
    }

    IEnumerator LastChargeLerp(int speed, float startValue)
    {
        while (startValue > 0 )
        {
            startValue -= Time.deltaTime * speed;
            chargeFill.color = Color.Lerp(Color.white, Color.blue, startValue/100f);
            Charge.value = Mathf.Lerp(0,100, startValue/100f);
            yield return new WaitForEndOfFrame();
        }

        lastChargeLerping = false;
    }
}
