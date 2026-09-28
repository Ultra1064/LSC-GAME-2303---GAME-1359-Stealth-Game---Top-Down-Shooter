using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    SoundManager audio;
    [Header("Material Types")]
    [SerializeField] MeshRenderer[] _mRenderer;
    public Material baseMat;
    public Material hitFlashMaterial;

    [Header("Enemy Health Count")]
    public int health = 10;

    [Header("Explosion Effect")]
    public GameObject explosionEffect;

    private void Awake()
    {
        audio = FindAnyObjectByType<SoundManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            Destroy(other.gameObject);
            StartCoroutine(TakeDamage());
        }
        else if (other.gameObject.tag == "PlayerBigBullet")
        {
            StartCoroutine(TakeDamage());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindAnyObjectByType<GameManager>().GameOver();
            audio.PlaySound2D("GameOver");
        }
    }

    IEnumerator TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.025f);
            HitStopManager.instance.DoHitStop(0.3f, 0.5f);
            Destroy(gameObject);
            GameManager.instance.activeEnemyCount--;
            GameManager.instance.EnemyKillCount();
            audio.PlaySound3D("EnemyDie", transform.position);
        }
        else
        {
            foreach (MeshRenderer m in _mRenderer)
            {
                m.material = hitFlashMaterial;
            }
            yield return new WaitForSeconds(0.1f);
            foreach (MeshRenderer m in _mRenderer)
            {
                m.material = baseMat;
            }
        } 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(MeshRenderer m in _mRenderer)
        {
            m.material = baseMat;
        }
    }
}
