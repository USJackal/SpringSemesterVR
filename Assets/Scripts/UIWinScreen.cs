using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWinScreen : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private UnityEngine.UI.Text m_Text;
    [SerializeField] private AudioSource m_WizardsWin;
    [SerializeField] private AudioSource m_WarlockWin;

    private static UIWinScreen sInstance;

    private static int sDefeatedWizards = 0;

    private static bool sHasWon = false;

    private void Start()
    {
        sInstance = this;
        sDefeatedWizards = 0;
        sHasWon = false;
    }

    public static void SetDefeatedWizard()
    {
        if (sHasWon)
        {
            return;
        }

        sDefeatedWizards++;
        if (sDefeatedWizards == 2)
        {
            sInstance.m_Text.text = "WARLOCK WINS";
            sInstance.m_WarlockWin.Play();
            sHasWon = true;

            sInstance.StartCoroutine(RestartGame());
        }
    }

    public static void SetDefeatedWarlock()
    {
        if (sHasWon)
        {
            return;
        }

        sHasWon = true;
        sInstance.m_Text.text = "WIZARDS WIN";
        sInstance.m_WizardsWin.Play();

        sInstance.StartCoroutine(RestartGame());
    }

    private static IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(14);
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
