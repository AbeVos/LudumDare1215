using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DisableAllButtons();
                StartCoroutine(StartGame());
            }
            );
        transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DisableAllButtons();
                StartCoroutine(StartGame());
            }
            );
        transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(
            () =>
            {
                DisableAllButtons();
                StartCoroutine(StartGame());
            }
            );
        transform.GetChild(2).GetChild(3).GetComponent<Button>().onClick.AddListener(
            () =>
            {
                Application.Quit();
            }
            );
    }

    private void DisableAllButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(2).GetChild(i).GetComponent<Button>().interactable = false;
            transform.GetChild(2).GetChild(i).GetComponent<Image>().CrossFadeAlpha(0, 5f, false);
        }
    }

    IEnumerator StartGame()
    { 
        yield return new WaitForSeconds(0.5f);
        GameManager.StartGame();
    }

    void Update()
    {

    }
}
