using UnityEngine;
using UnityEngine.SceneManagement;

public class OurPlayer : MonoBehaviour
{
    private int Health = 3;
    [SerializeField] private AudioSource m_HurtSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TakeDamage(1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        m_HurtSound.Play();

        if (Health <= 0)
        {
            UIWinScreen.SetDefeatedWarlock();
        }
    }
}
