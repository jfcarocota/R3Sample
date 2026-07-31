using R3;
using UnityEngine;

public class Lesson02_Where : MonoBehaviour
{
    void Start()
    {
        var vida = new ReactiveProperty<int>(100);

        // Sin filtro: avisa en CADA cambio
        vida.Subscribe(v => Debug.Log($"Vida: {v}"));

        // Con Where: avisa SOLO cuando se cumple la condición
        vida.Where(v => v <= 25)
            .Subscribe(v => Debug.Log($"PELIGRO! Vida baja: {v}"));

        // Simulamos daño
        vida.Value = 80;
        vida.Value = 50;
        vida.Value = 20;  // este sí activa el Where
        vida.Value = 10;  // este también

        vida.Dispose();
    }
}
