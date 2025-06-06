using UnityEngine;

public class hitBehaviour : MonoBehaviour
{
    public GameBehaviour gameManager;

    private void Start()
    {
        GameObject temp = GameObject.Find("GameManager");
        if (temp != null)
        {
            gameManager = temp.GetComponent<GameBehaviour>();
            Debug.Log("Game Manager найден.");
        }
        else
        {
            Debug.Log("Game Manager не найден.");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            gameManager.Items += 1;
        }
    }
}

