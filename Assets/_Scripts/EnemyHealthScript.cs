using System.Collections;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    [Header("Material Types")]
    [SerializeField] MeshRenderer[] _mRenderer;
    public Material baseMat;
    public Material hitFlashMaterial;

    [Header("Enemy Health Count")]
    public int health = 5;

    [Header("Explosion Effect")]
    public GameObject explosionEffect;

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
