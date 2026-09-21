using System.Collections;
using UnityEngine;

public class PlayerShootScript : MonoBehaviour
{
    TS_Inputs _inputs;
    PlayerController _ctrl;

    [Header("Spawn Setup")]
    public Transform bulletSpawnPoint;


    [Header("Bullet Types")]
    public Rigidbody baseBullet;
    public Rigidbody bigBullet;
    public float shotForce = 700;
    public int shotID = 1;
    public int spreadCount = 3;
    public float totalSpreadAngle = 30;

    [Header("Set Shoot Timing")]
    public float shootSpeed = 0.2f;
    public bool canShoot;

    void Awake()
    {
        _inputs = new TS_Inputs();
        _ctrl = GetComponent<PlayerController>();
        canShoot = true;
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }
    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        if(_inputs.Player.Shoot.IsPressed() && canShoot)
        {
            StartCoroutine(PlayerShoot());
        }
    }

    IEnumerator PlayerShoot()
    {
        if (_ctrl.isGamepad) //This makes the rumble only happen when using controller
            RumbleManager.Instance.RumblePulse(0.2f, 0.2f, 0.1f);
        canShoot = false;
        Rigidbody _shot;
        switch (shotID)
        {
            case 1: //Regular bullets
                _shot = Instantiate(baseBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * shotForce, ForceMode.Impulse);
                yield return new WaitForSeconds(shootSpeed);
                break;
            case 2: //Big piercing slow shooting bullets
                _shot = Instantiate(bigBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
                _shot.AddForce(bulletSpawnPoint.forward * (shotForce * 0.75f), ForceMode.Impulse);
                yield return new WaitForSeconds(shootSpeed * 4);
                break;
            case 3: //Spread
                float angleStep = totalSpreadAngle / (spreadCount - 1);
                float startAngle = -totalSpreadAngle / 2f;
                for (int x = 0; x < spreadCount; x++)
                {
                    float angle = startAngle + angleStep * x;
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                    Vector3 direction = rotation * transform.forward;
                    _shot = Instantiate(baseBullet, bulletSpawnPoint.position, Quaternion.LookRotation(direction)) as Rigidbody;
                    _shot.AddForce(direction  * shotForce, ForceMode.Impulse);
                }
                yield return new WaitForSeconds(shootSpeed);
                break;
        }
        canShoot = true;
    }
}
