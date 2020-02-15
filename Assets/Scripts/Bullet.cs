using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float m_Speed = 0.8f;
    [SerializeField] private GameObject m_ExplodeMiss;
    [SerializeField] private GameObject m_ExplodeHit;
    [SerializeField] private AudioSource m_BlockSound;
    public Vector3 Player;
    private Vector3 Direction;

    private float m_Time = 0;

    private void Start()
    {
        if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        transform.position = transform.position + Vector3.up;

        transform.rotation = Quaternion.LookRotation(Player - transform.position);
        Direction = (Player - transform.position).normalized;
        m_Time = 0;
    }

    private void Update()
    {
        transform.position = transform.position + Direction * m_Speed * Time.deltaTime;

        if (m_Time >= 10)
        {
            m_BlockSound.Play();
            Destroy(gameObject, m_BlockSound.clip.length);
        }
        m_Time += Time.deltaTime;
        //if (Vector3.Distance(transform.position, Player) < 0.01f)
        //{
        //    Instantiate(m_ExplodeMiss, transform.position, Quaternion.identity);
        //    Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<OurPlayer>();
            player.TakeDamage(1);
            Instantiate(m_ExplodeHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
