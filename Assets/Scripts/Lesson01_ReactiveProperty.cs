using R3;
using UnityEngine;

public class Lesson01_ReactiveProperty : MonoBehaviour
{
    void Start()
    {
        // 1. Creas una variable reactiva con valor inicial 0
        var contador = new ReactiveProperty<int>(0);

        // 2. Le dices qué hacer cuando cambie
        contador.Subscribe(valor => Debug.Log($"El contador cambió a: {valor}"));

        // 3. Cambias el valor — esto dispara el Subscribe automáticamente
        contador.Value = 1;   // imprime: El contador cambió a: 1
        contador.Value = 2;   // imprime: El contador cambió a: 2
        contador.Value = 5;   // imprime: El contador cambió a: 5

        // 4. Limpias cuando ya no lo necesitas
        contador.Dispose();
    }
}
