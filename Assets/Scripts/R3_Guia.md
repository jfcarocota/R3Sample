# R3 con Unity — Guía de conceptos

> En lugar de **preguntar** "¿cambió algo?" en cada frame, R3 te **avisa automáticamente** cuando algo cambia.

---

## Instalación

Instala via **NuGetForUnity**: `R3`, `Microsoft.Bcl.TimeProvider`, `Microsoft.Bcl.AsyncInterfaces`.  
Agrega el wrapper de Unity via Package Manager (git URL): `https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity`

---

## Patrón base — siempre en todo MonoBehaviour

Declara un `DisposableBag` como campo, agrega cada suscripción con `.AddTo(ref _subs)`, y en `OnDestroy` llama `_subs.Dispose()`. Sin esto tienes memory leaks.

---

## Lección 01 — ReactiveProperty

Es una variable normal pero con superpoderes: cada vez que cambias su valor, avisa automáticamente a quien esté escuchando. No necesitas llamar nada manualmente — el aviso es automático.

Siempre emite el valor actual al momento de suscribirse (el valor inicial).

---

## Lección 02 — Where

Filtra el stream: el `Subscribe` solo se ejecuta cuando la condición es verdadera. No bloquea el stream original — crea uno nuevo filtrado en paralelo.

Úsalo para reaccionar solo a situaciones específicas, como vida baja o score alto.

---

## Lección 03 — Select

Transforma el valor antes de que llegue al `Subscribe`. Entra un tipo, sale otro. Es igual al `Select` de LINQ.

Se puede encadenar con `Where`: primero filtras, luego transformas. La UI casi siempre necesita un `Select` para convertir números a texto.

---

## Lección 04 — DisposableBag

Contenedor que agrupa todas las suscripciones de un MonoBehaviour. Cuando llamas `Dispose()` en él, cancela todas juntas de golpe.

La diferencia con las lecciones anteriores: los `ReactiveProperty` viven como campos del componente (no dentro de `Start`), así duran mientras el GameObject exista. `OnDestroy` es el lugar correcto para limpiar.

---

## Lección 05 — Subject

Un `Subject` es un emisor manual de eventos. No tiene valor como `ReactiveProperty` — solo emite cuando tú lo decides llamando `OnNext`.

Úsalo para reemplazar `Action` y `UnityEvent`. La ventaja: puedes encadenar todos los operadores de R3 sobre él (`Where`, `Select`, `Take`, etc.).

---

## Lección 06 — Arquitectura reactiva entre componentes

El patrón más valioso de R3: un componente expone sus streams públicamente, y otros se suscriben sin que el primero sepa quién lo escucha.

El `Jugador` no conoce al `Enemigo`. El `Enemigo` no llama métodos del `Jugador` para saber su estado — solo se suscribe a sus streams. Esto elimina el acoplamiento entre sistemas.

---

## Lección 07 — Skip

Ignora los primeros N valores del stream. El uso más común es `Skip(1)` para ignorar el valor inicial de una `ReactiveProperty` cuando solo te importan los cambios posteriores.

`Take(n)` y `Skip(n)` son opuestos: `Take` toma los primeros N y para, `Skip` ignora los primeros N y toma el resto.

---

## Lección 08 — DistinctUntilChanged

Solo emite cuando el valor cambia respecto al anterior. Si asignas el mismo valor dos veces seguidas, el segundo es ignorado.

Muy útil para la UI: evita redibujar elementos cuando llega el mismo número repetido, lo que puede pasar cuando el valor viene de una fuente externa.

---

## Lección 09 — Throttle y Debounce

Dos operadores de tiempo que controlan la frecuencia de eventos.

**Throttle** emite el último valor al final de cada ventana de tiempo fija. No importa cuántos eventos lleguen en ese segundo — solo procesa uno. Ideal para cooldowns y guardar progreso.

**Debounce** espera a que haya una pausa en los eventos, luego emite el último. Si siguen llegando eventos, el temporizador se reinicia. Ideal para búsquedas mientras el usuario escribe.

---

## Lección 10 — Merge

Une dos o más streams en uno solo. En lugar de suscribirte por separado a cada fuente con la misma lógica, `Merge` los combina y tienes un solo `Subscribe` que maneja todos.

Útil para combinar input de teclado, gamepad y touch, o para unir eventos de daño de múltiples fuentes.

---

## Lección 11 — CombineLatest

Combina los **últimos valores** de dos streams y emite cada vez que cualquiera de los dos cambia.

La diferencia con `Merge`: `Merge` une eventos que pasan una vez, `CombineLatest` une estados que persisten. Úsalo para validar condiciones que dependen de múltiples variables — como habilidades que requieren mana Y tener un arma.

---

## Lección 12 — Switch

Cancela el stream anterior cuando llega uno nuevo. Siempre se usa junto con `Select`: `Select` convierte cada valor en un nuevo `Observable`, y `Switch` descarta el anterior y se queda solo con el último.

El problema que resuelve: si lanzas dos búsquedas rápidas, sin `Switch` ambas corren en paralelo y podrías mostrar resultados de la búsqueda vieja. Con `Switch` solo la última importa.

---

## Lección 13 — UI reactiva

La UI se suscribe a los streams de datos y se actualiza sola. Nunca llamas `ActualizarUI()` manualmente — el cambio de valor lo dispara automáticamente.

`OnClickAsObservable()` convierte el evento de un botón en un stream de R3, permitiendo usar todos los operadores sobre clicks: throttle para evitar doble click, take para un solo uso, etc.

---

## Lección 14 — SerializableReactiveProperty

Igual que `ReactiveProperty` pero aparece en el Inspector de Unity. Puedes ver y editar el valor en tiempo real durante Play Mode, y el `Subscribe` se dispara automáticamente al cambiar desde el Inspector.

Úsala para valores que el diseñador o tú necesitan ajustar mientras el juego corre: velocidades, daños, duraciones de efectos.

---

## Lección 15 — Integración con UniTask

Dos conversiones clave:

**Observable → UniTask**: `FirstAsync()` convierte un stream en una operación awaitable. Espera hasta que el stream emita un valor que cumpla la condición y regresa ese valor. Ideal para esperar que ocurra algo específico sin bloquear.

**UniTask → Observable**: `.ToObservable()` mete una operación async dentro de un stream. Combinado con `Switch`, permite cancelar la operación anterior si llega una nueva — el patrón estándar para requests que pueden interrumpirse.

---

## Resumen de operadores

| Operador | Para qué |
|---|---|
| `Where` | Filtrar — solo pasa si la condición es verdadera |
| `Select` | Transformar — convierte el valor en otro tipo |
| `Take(n)` | Solo los primeros N, luego se cancela solo |
| `Skip(n)` | Ignora los primeros N, toma el resto |
| `DistinctUntilChanged` | Ignora si el valor es igual al anterior |
| `Throttle` | Una emisión por ventana de tiempo |
| `Debounce` | Emite después de una pausa en los eventos |
| `Merge` | Une varios streams de eventos en uno |
| `CombineLatest` | Combina los últimos valores de dos streams |
| `Switch` | Cancela el anterior cuando llega uno nuevo |
