using System.Collections;
using UnityEngine;
using Valve.VR;

public class Wand : MonoBehaviour
{
#pragma warning disable 0649
    public MeshRenderer MeshRenderer;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_DeathExplosions;
    [SerializeField] private GameObject m_MagicEffect; // pf_vfx-ult_demo_psys_oneshot_megaflare2
    [SerializeField] private GameObject m_MagicBullet;
    [SerializeField] private float m_ThrowTreshold = 6.3f;
    [SerializeField] private float m_Range = 6.3f;
    [SerializeField] private AnimationCurve m_MoveCurve;
    [SerializeField] private AudioSource m_FireSound;
    [SerializeField] private AudioSource m_ExplosionSound;

    private float m_PeekSpeed = 0;
    private bool m_IsThrowing = false;
    private const float kCoolDown = 8f;
    private float m_CooldownTimer = 0;

    private static float sHealth;

    [HideInInspector] public bool Danger = false;

    private bool m_LiterallyDying = false;

    private void Start()
    {
        sHealth = 120f;
        m_CooldownTimer = kCoolDown;
    }

#pragma warning disable IDE0060 // Remove unused parameter
    public void OnTransformUpdated(SteamVR_Behaviour_Pose pose, SteamVR_Input_Sources _sources)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        if (m_LiterallyDying)
        {
            return;
        }

        float speed = pose.GetVelocity().magnitude;
        var pos = pose.origin.position;
        var rot = pose.origin.rotation;

        if (Input.GetKeyDown(KeyCode.G))
        {
            Fire(transform.position, Quaternion.identity);
        }

        float opacity = m_MoveCurve.Evaluate(speed);
        MeshRenderer.material.SetColor("_Color", new Color(1, 1, 1, opacity));

        if ((opacity >= 0.05f) && Danger)
        {
            sHealth -= 8 * opacity * Time.deltaTime;

            if (sHealth <= 0)
            {
                m_LiterallyDying = true;
                Instantiate(m_DeathExplosions, pos, rot).SetActive(true);
                m_ExplosionSound.Play();
                MeshRenderer.enabled = false;
                Destroy(gameObject, 3);
                UIWinScreen.SetDefeatedWizard();
            }
        }

        if (m_CooldownTimer <= 0)
        {
            if (!m_IsThrowing && (speed > m_ThrowTreshold))
            {
                m_IsThrowing = true;
            }

            if (m_IsThrowing)
            {
                if (m_PeekSpeed < speed)
                {
                    m_PeekSpeed = speed;
                }
                else
                {
                    m_PeekSpeed = 0;
                    m_IsThrowing = false;

                    Fire(pos, rot);
                }
            }
        }
        else if (opacity > 0.1f)
        {
            m_CooldownTimer -= speed * Time.deltaTime;
        }
    }

    private void Fire(Vector3 pos, Quaternion rot)
    {
        m_CooldownTimer = kCoolDown;
        m_FireSound.Play();
        StartCoroutine(FireDelay(pos, rot));
    }

    private IEnumerator FireDelay(Vector3 pos, Quaternion rot)
    {
        yield return new WaitForSeconds(1.352750f);

        Vector3 spawnPos = (pos - m_Player.transform.position).normalized * m_Range;

        var explosionParticle = Instantiate(m_MagicEffect, spawnPos, rot);
        explosionParticle.SetActive(true);

        var bulletObj = Instantiate(m_MagicBullet, spawnPos, rot);
        var bullet = bulletObj.GetComponent<Bullet>();
        bullet.Player = m_Player.transform.position;
    }
}
