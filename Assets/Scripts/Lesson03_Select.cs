using R3;
using UnityEngine;

public class Lesson03_Select : MonoBehaviour
{
    void Start()
    {
        var vida = new ReactiveProperty<int>(100);

        // Select convierte int -> string antes de llegar al Subscribe
        vida.Select(v => $"HP: {v}/100")
            .Subscribe(texto => Debug.Log(texto));

        // Puedes combinar Where + Select
        vida.Where(v => v <= 0)
            .Select(_ => "Game Over")
            .Subscribe(mensaje => Debug.Log(mensaje));

        vida.Value = 50;   // imprime: HP: 50/100
        vida.Value = 10;   // imprime: HP: 10/100
        vida.Value = 0;    // imprime: HP: 0/100  y  Game Over

        vida.Dispose();
    }
}
