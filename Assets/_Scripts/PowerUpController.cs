using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    PlayerController player;
    PlayerShootScript shooter;
    [SerializeField] float powerUpDuration = 10f;
    [SerializeField] float moveSpeedBoost = 6f;
    [SerializeField] float shotSpeedBoost = 0.05f;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShootScript>();
    }
    //Power Up Coroutines
    private IEnumerator SpeedPowerUp()
    {
        float ogSpeed = player.moveSpeed;
        player.moveSpeed = moveSpeedBoost;
        yield return new WaitForSeconds(powerUpDuration);
        player.moveSpeed = ogSpeed;
    }

    private IEnumerator BigBulletPowerUp()
    {
        int ogShotID = shooter.shotID;
        shooter.shotID = 2; //Big Bullet ID is 2
        yield return new WaitForSeconds(powerUpDuration);
        shooter.shotID = ogShotID;
    }

    private IEnumerator ShotSpeedPowerUp()
    {
        float ogShootSpeed = shooter.shootSpeed;
        shooter.shootSpeed = shotSpeedBoost;
        yield return new WaitForSeconds(powerUpDuration);
        shooter.shootSpeed *= ogShootSpeed;
    }

    //Power Up Functions (Buttons can't use Coroutines directly apparently
    public void IncreaseSpeed()
    {
        StartCoroutine(SpeedPowerUp());
    }
    public void BigBullets()
    {
        StartCoroutine(BigBulletPowerUp());
    }
    public void FireRateUp()
    {
        StartCoroutine(ShotSpeedPowerUp());
    }
    public void SpreadCountPowerUp()
    {
        if (shooter.shotID != 3)
        {
            shooter.shotID = 3;
        }
        shooter.spreadCount++;
    }
}
