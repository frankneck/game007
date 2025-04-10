using UnityEngine;
using UnityEngine.UI;

public class PlayerHealt : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI() // вызывается каждый кадр
    {
        Debug.Log(health);
        float fillForward = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        // будет работать до тех пор, пока не будет подходить под условие
        if (fillBack > hFraction) // если бэк полоска показывает больше, чем факт хп => игрок потерял хп
        {
            frontHealthBar.fillAmount = hFraction; // форвард полоска уменьшается до факт хп
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime; // value can be more than 1
            float percentComplet = Mathf.Min(lerpTimer / chipSpeed, 1f); // but here we fix it
            percentComplet *= percentComplet; // for cool anim
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplet);
        }
        else if (fillForward < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplet = Mathf.Min(lerpTimer / chipSpeed, 1.0f);
            percentComplet *= percentComplet;
            frontHealthBar.fillAmount = Mathf.Lerp(fillForward, backHealthBar.fillAmount, percentComplet);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
