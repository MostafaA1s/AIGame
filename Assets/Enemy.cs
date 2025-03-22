using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyType;
    public bool canBeAttacked = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeAttacked) return;
        if (collision.CompareTag(enemyType))
        {
            canBeAttacked = false;
            Destroy(collision.gameObject);
            EmeyManager.instance.IncScore();

            gameObject.SetActive(false);

        }
        else
        {
            EmeyManager.instance.DecScore();

        }
    }

    public void ResetAttack()
    {
        canBeAttacked = true;
    }
}
