using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Sprite RapidImg, ChargeImg;

    private Slider Health, Exp, Heat, Charge;
    private Image healthFill, epxFill, heatFill, chargeFill;
    private GameObject EventSystem;
    private float[] ExpIntervals;
    private float lastCharge;
    private bool lastChargeLerping = false;
    private bool isFading;
    #endregion

    #region Setup
    void OnEnable()
    {
        State.OnGlobalStateChanged += State_OnGlobalStateChanged;
    }

    void Start()
    {
        Health = transform.GetChild(1).GetComponent<Slider>();
        Exp = transform.GetChild(2).GetComponent<Slider>();
        Heat = transform.GetChild(3).GetComponent<Slider>();
        Charge = transform.GetChild(4).GetComponent<Slider>();

        healthFill = Health.transform.GetChild(1).GetComponentInChildren<Image>();
        epxFill = Exp.transform.GetChild(1).GetComponentInChildren<Image>();
        heatFill = Heat.transform.GetChild(1).GetComponentInChildren<Image>();
        chargeFill = Charge.transform.GetChild(1).GetComponentInChildren<Image>();

        ExpIntervals = UpgradeManger.GetExpIntervals();
        Exp.maxValue = ExpIntervals[Dragon.Rank];
    }
    #endregion

    void Update()
    {
        if (State.Current == State.GlobalState.Game)
        {
            // Health ////////////////////////////////////////////
            Health.value = Dragon.Health;

            // Exp ////////////////////////////////////////////
            Exp.value = Dragon.Exp;
            if (Dragon.Exp >= ExpIntervals[Dragon.Rank])
            {
                GameManager.LevelUp();
            }

            // Heat ////////////////////////////////////////////
            Heat.value = Dragon.Heat;
            if (Dragon.Heat > 70 && !Dragon.Overheat)
            {
                heatFill.color = Color.Lerp(Color.white, Color.red, (Heat.value - 70f) / 20f);
            }
            else if (Dragon.Overheat)
            {
                heatFill.color = Color.magenta;
            }
            else
            {
                heatFill.color = Color.white;
            }

            // Charge ////////////////////////////////////////////
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
    }

    private void ShowUpgrades()
    {
        StartCoroutine(FadeInOverlay());
    }

    private void HideUpgrades()
    {
        transform.GetChild(5).gameObject.SetActive(false);
        State.SetState(State.GlobalState.Game);
    }

    private void State_OnGlobalStateChanged(State.GlobalState prevGlobalState, State.GlobalState newGlobalState)
    {
        if (newGlobalState == State.GlobalState.Start)
        {
            StartCoroutine(ShowStartMenu());
        }

        if (newGlobalState == State.GlobalState.Pause)
        {
            Cursor.visible = true;
            ShowUpgrades();
        }

        if (prevGlobalState == State.GlobalState.Pause)
        {
            Cursor.visible = false;
            HideUpgrades();
        }

        if (newGlobalState == State.GlobalState.Initialize)
        {
            Cursor.visible = false;

            //  Fade GUI in.
            for (int i = 0; i < 5; i++)
            {
                if (i >= 1 && i < 5)
                {
                    transform.GetChild(i).GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0);
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Graphic>().CrossFadeAlpha(1f, GameManager.introTime, false);

                    transform.GetChild(i).GetChild(0).GetComponent<Graphic>().CrossFadeAlpha(1f, GameManager.introTime, false);
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Graphic>().CrossFadeAlpha(1f, GameManager.introTime, false);
                }
                else
                {
                    transform.GetChild(i).GetComponent<CanvasRenderer>().SetAlpha(0);
                    transform.GetChild(i).GetComponent<Graphic>().CrossFadeAlpha(1f, GameManager.introTime, false);
                }
            }
        }

        if (newGlobalState == State.GlobalState.Win)
        {
            StartCoroutine(ShowWinScreen());
        }
    }

    IEnumerator LastChargeLerp(int speed, float startValue)
    {
        while (startValue > 0)
        {
            startValue -= Time.deltaTime * speed;
            chargeFill.color = Color.Lerp(Color.white, Color.blue, startValue / 100f);
            Charge.value = Mathf.Lerp(0, 100, startValue / 100f);
            yield return new WaitForEndOfFrame();
        }

        lastChargeLerping = false;
    }

    public IEnumerator FadeInOverlay()
    {
        transform.GetChild(5).gameObject.SetActive(true);

        transform.GetChild(5).GetComponent<Graphic>().canvasRenderer.SetAlpha(0);
        transform.GetChild(5).GetComponent<Graphic>().CrossFadeAlpha(0.8f, 1f, false);

        transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Graphic>().canvasRenderer.SetAlpha(0);
        transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Graphic>().CrossFadeAlpha(0.8f, 1f, false);

        transform.GetChild(5).GetChild(0).GetChild(1).GetComponent<Graphic>().canvasRenderer.SetAlpha(0);
        transform.GetChild(5).GetChild(0).GetChild(1).GetComponent<Graphic>().CrossFadeAlpha(0.8f, 1f, false);
        
        var ups = UpgradeManger.GetUpdate(Dragon.Rank);

        Dragon.Rank++;
        Exp.maxValue = ExpIntervals[Dragon.Rank];
        Dragon.Exp = 0;

        for (int i = 0; i < 2; i++)
        {
            var up = ups[i];

            if (up.WeaponType == 2)
            {
                transform.GetChild(5).GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>().sprite = ChargeImg;
            }
            else
            {
                transform.GetChild(5).GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>().sprite = RapidImg;
            }

            transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(
                () => Dragon.UpgradeWeapon(up));

            transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(
                () => HideUpgrades());


            transform.GetChild(5).GetChild(0).GetChild(i).GetChild(1).GetComponent<Text>().text = ups[i].Name;
            transform.GetChild(5).GetChild(0).GetChild(i).GetChild(2).GetComponent<Text>().text = ups[i].ToolTip;
        }

        yield return new WaitForSeconds(1f);
        transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Button>().interactable = true;
        transform.GetChild(5).GetChild(0).GetChild(1).GetComponent<Button>().interactable = true;
    }

    IEnumerator ShowStartMenu()
    {
        yield return new WaitForSeconds(1.5f);
        transform.Find("Milly (7)").GetChild(0).GetComponentInChildren<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        transform.Find("Milly (7)").GetComponent<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        transform.Find("Milly (7)").gameObject.SetActive(false);
    }

    IEnumerator ShowWinScreen()
    {
        transform.Find("YouWin (9)").gameObject.SetActive(true);
        transform.Find("YouWin (9)").GetComponent<Graphic>().CrossFadeAlpha(1f, 1f, false);
        transform.Find("YouWin (9)").GetChild(0).GetComponentInChildren<Graphic>().CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(0.5f);
    }
}
