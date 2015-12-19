using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    #region Public functions
    public void ToggleOverlay(int index)
    {
        if (transform.GetChild(index).gameObject.activeInHierarchy)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(index).gameObject.SetActive(true);
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
        StartCoroutine(StartGameRoutine());
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
    #endregion

    #region Key rebind
    public static string PrimaryRebind, SecondaryRebind;
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

    public void ConfirmRebind(int rebindScIndex)
    {
        if (PrimaryRebind != null && SecondaryRebind != null)
        {
            InputManager.keysRemaped = true;
            PrimaryRebind = "" + PrimaryRebind[0];
            SecondaryRebind = "" + SecondaryRebind[0];
            PrimaryRebind = PrimaryRebind.ToUpper();
            SecondaryRebind = SecondaryRebind.ToUpper();

            InputManager.PrimaryButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), PrimaryRebind);
            InputManager.SecondaryButton = (KeyCode)System.Enum.Parse(typeof(KeyCode), SecondaryRebind);
        }

        ToggleOverlay(rebindScIndex);
    }
    #endregion

    private IEnumerator StartGameRoutine()
    {
        transform.GetComponent<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        GameManager.StartGame();
    }
}

