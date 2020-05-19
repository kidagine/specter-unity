using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private readonly int _expWorth = 3;


    public void TakeDamage(int damageAmount, GameObject damagerObject)
    {
		Debug.Log("HIT");
        GameManager.Instance.Player.TakeExp(_expWorth);
        Destroy(gameObject);
    }
}
