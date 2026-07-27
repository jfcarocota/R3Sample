using UnityEngine;

// Agrega este script al mismo GameObject que PlayerHealth para probar en el Inspector
public class HealthTester : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) player.TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.H)) player.Heal(10);
    }
}
