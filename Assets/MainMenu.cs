using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public static string PrimaryRebind, SecondaryRebind;

    public void SetPri(Transform self)
    {
        string val = self.GetComponent<InputField>().text;
        PrimaryRebind = val;
    }

    public void SetSec(Transform self)
    {
        string val = self.GetComponent<InputField>().text;
        SecondaryRebind = val;
    }

    public void UISound()
    {
        AudioManager.PlayClip("confirmA", true);
    }

    public void UIConfirm()
    {
        AudioManager.PlayClip("confirmB", true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameRoutine()
    {
        StartCoroutine(StartGame());
    }

    public void ShowHowTo(bool state)
    {
        transform.GetChild(4).gameObject.SetActive(state);
    }

    public void ShowRebind(bool state)
    {
        transform.GetChild(3).gameObject.SetActive(state);
    }

    public void ConfirmRebind()
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
        ShowRebind(false);
    }

    public void HideEndUIScreen(int index)
    {
        transform.parent.GetChild(index).gameObject.SetActive(false);
    }

    public void DisableAllButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            //     transform.GetChild(2).GetChild(i).GetComponent<Image>().canvasRenderer.SetAlpha(1f);
            transform.GetChild(2).GetChild(i).GetComponent<Image>().CrossFadeAlpha(0, 0.5f, false);
        }
    }

    IEnumerator StartGame()
    {
        transform.GetComponent<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        GameManager.StartGame();
    }

}
