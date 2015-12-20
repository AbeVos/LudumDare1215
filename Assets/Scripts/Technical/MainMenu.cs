using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    #region Public functions
    public void ToggleOverlay(int index)
    {
        if (HasError) { return; }
        if (transform.GetChild(index).gameObject.activeInHierarchy)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(index).gameObject.SetActive(true);
        }

    }

    public void ToggleMenu(int index)
    {
        if (HasError) { return; }
        if (transform.GetChild(0).GetChild(index).gameObject.activeInHierarchy)
        {
            transform.GetChild(0).GetChild(index).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).GetChild(index).gameObject.SetActive(true);
        }

    }

    public void FadeOutButtonsAllInContainer(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            container.GetChild(i).GetComponent<Graphic>().canvasRenderer.SetAlpha(1f);
            container.GetChild(i).GetComponent<Graphic>().CrossFadeAlpha(0, 0.5f, false);
        }
    }

    public void StartGame()
    {
        if (!HasError)
        {
            StartCoroutine(StartGameRoutine());
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayUISound()
    {
        AudioManager.PlayClip("confirmA", true);
    }

    public void PlayUIConfirm()
    {
        AudioManager.PlayClip("confirmB", true);
    }

    public void SetHasError(bool enabled)
    {
        HasError = enabled;
    }
    #endregion

    #region build-in function
    void OnEnable()
    {
        UIController.mainMenu = this;
    }
    #endregion

    #region Key rebind
    public static string PrimaryRebind, SecondaryRebind;
    public static bool HasError = false;

    public void SetPrimaryRebound(Transform self)
    {
        string value = self.GetComponent<InputField>().text;
        PrimaryRebind = value;
    }

    public void SetSecondaryRebound(Transform self)
    {
        string value = self.GetComponent<InputField>().text;
        SecondaryRebind = value;
    }

    public void ConfirmRebind(GameObject ErrorPopup)
    {
        if (PrimaryRebind != null && SecondaryRebind != null)
        {
            InputManager.keysRemaped = true;
            HasError = false;

            try
            {
                PrimaryRebind = "" + PrimaryRebind[0];
                SecondaryRebind = "" + SecondaryRebind[0];
                PrimaryRebind = PrimaryRebind.ToUpper();
                SecondaryRebind = SecondaryRebind.ToUpper();
            }
            catch (System.IndexOutOfRangeException)
            {
                HasError = true;
                TogglePopUp(ErrorPopup);
            }
            catch (System.ArgumentException)
            {
                HasError = true;
                TogglePopUp(ErrorPopup);
            }

            InputManager.PrimaryButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), PrimaryRebind);
            InputManager.SecondaryButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), SecondaryRebind);
        }
        else
        {
            HasError = true;
            TogglePopUp(ErrorPopup);
        }
    }
    #endregion

    public void TogglePopUp(GameObject popUp)
    {
        if (popUp.activeInHierarchy)
        {
            popUp.SetActive(false);
        }
        else
        {
            popUp.SetActive(true);
        }
    }

    private IEnumerator StartGameRoutine()
    {
        transform.GetChild(0).GetComponent<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.35f);
        ToggleOverlay(0);
        //gameObject.SetActive(false);
        GameManager.StartGame();
    }

    public IEnumerator ShowWinOverlay()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetComponent<Graphic>().color = new Color(0.09f, 0.16f, 0.09f);
        transform.GetChild(2).GetComponent<Graphic>().canvasRenderer.SetAlpha(0f);
        transform.GetChild(2).GetComponent<Graphic>().CrossFadeAlpha(1f, 1f, false);
        transform.GetChild(2).GetChild(0).GetComponentInChildren<Graphic>().CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(1f);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public IEnumerator ShowLoseOverlay()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).GetComponent<Graphic>().color = new Color(0.16f, 0.09f, 0.09f);
        transform.GetChild(3).GetComponent<Graphic>().canvasRenderer.SetAlpha(0f);
        transform.GetChild(3).GetComponent<Graphic>().CrossFadeAlpha(1f, 1f, false);
        transform.GetChild(3).GetChild(0).GetComponentInChildren<Graphic>().CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(1f);
        transform.GetChild(3).gameObject.SetActive(true);
    }
}

