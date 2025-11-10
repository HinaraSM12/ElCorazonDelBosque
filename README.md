# ElCorazonDelBosque
Slice jugable en tercera persona que valida el loop central del GDD: explorar → recolectar → abrir puertas → meta. En un laberinto ambientado con assets, luces y música, el jugador reúne 4 Lágrimas. Al conseguir 1, se desbloquea una puerta tutorial; con 4, la puerta final. Esta demo sirve como blocking del prólogo y antesala del juego.


# Gus & Palomino: El Corazón del Bosque – Demo (Laberinto de las Lágrimas)

> **Plataformas 3D cooperativo (demo single-player compatible).** Recolecta lágrimas, abre puertas por progreso (1 y 4), cruza la meta, gana y cierra el juego. Incluye HUD, mensajes en UI y audio (ambiente, puerta, pick-ups).

---

## ✨ Características clave

* **Mecánica base:** exploración + recolección de 4 lágrimas.
* **Puertas por umbral:** puerta tutorial (1) y puerta final (4) con `DoorUnlockByTears`.
* **Meta con UI:** “¡GANASTE!” y salida automática del juego (`GoalWinUI`).
* **HUD integrado:** contador “Lágrimas: X/4” + mensajes contextuales.
* **Sonido:** ambiente en loop, crujidos/desbloqueo/golpe de puerta y SFX al recoger.
* **Set dressing listo:** reemplazo de primitivas por assets y luces básicas.

---

## 🎮 Controles

* **Mover:** WASD
* **Mirar:** Mouse
* **Saltar:** Space

---

## 🧩 Loop de juego (demo)

1. Recolecta la 1.ª lágrima → se desbloquea la **puerta tutorial**.
2. Continúa hasta **4/4** → se desbloquea la **puerta final**.
3. Cruza la **meta** → mensaje en UI → cierre del juego.

---

## 🗂️ Estructura de scripts

```
Assets/Scripts/
  Gameplay/
    GameManager.cs                 // conteo de lágrimas, helpers y fin de nivel
    Collectible.cs                 // trigger de lágrima + SFX
    DoorUnlockByTears.cs           // umbral por puerta (1 y 4)
    AN_DoorScript.cs               // bisagra/física/locks
    GoalWinUI.cs                   // “¡GANASTE!” + cierre seguro
    DoorMessageByTears.cs          // (opcional) mensaje contextual por puerta
  Audio/
    AudioManager.cs                // SFX 3D + loops, crossfade
    AmbientAudio.cs                // arranque de ambiente
    DoorCreakSFX.cs                // creak/lock/slam según movimiento
  UI/
    UIManager.cs                   // HUD lágrimas + mensajes
StarterAssets/
  ThirdPersonController/...        // con guardas si CC está desactivado
```

---

## ⚙️ Configuración rápida (Escena)

1. **GameManager**

   * Arrastra el prefab o añade el script.
   * `totalTears = 4`.

2. **Lágrimas**

   * Prefab con **Collider (Is Trigger)** + `Collectible.cs`.
   * Asigna `pickupSfx` (opcional).

3. **Puertas**

   * GO con **Rigidbody (no kinematic)**, **HingeJoint** (Axis `0,1,0`, Use Limits ON), `AN_DoorScript`.
   * Añade `DoorUnlockByTears`:

     * Puerta tutorial → `requiredTears = 1`, `startLockedMode = Locked`.
     * Puerta final → `requiredTears = 4`, `startLockedMode = Locked`.
   * (Opcional) `DoorCreakSFX` con `unlockSfx`, `creakLoop`, `slamSfx`.

4. **Meta**

   * BoxCollider (Is Trigger) + `GoalWinUI.cs`.
   * `winText = "¡GANASTE!"`, `secondsBeforeQuit = 2.0`, `freezePlayerUntilQuit = true`.

5. **Audio**

   * Crea `AudioManager` en la escena (persistente).
   * (Opcional) `AmbientAudio` en un GO con `ambientLoop`.

---

## 🛠️ Requisitos y versiones

* **Unity:** 2021.3+ (o tu versión del repo).
* **Packages:** Starter Assets – Third Person Controller, TextMeshPro.

---

## ▶️ Cómo ejecutar

* **Editor:** Abre la escena principal y presiona **Play**.
* **Build:** `File → Build Settings → Build (Windows/Mac/Linux)` y ejecuta el binario.

---

## 🔧 Personalización rápida

* **Umbral de puertas:** en cada instancia de `DoorUnlockByTears.requiredTears`.
* **HUD/Mensajes:** `UIManager.SetTears(...)` y `UIManager.ShowMessage(...)`.
* **SFX:** asigna tus clips en `Collectible`, `DoorCreakSFX` y `AmbientAudio`.
* **Cierre del juego:** `GoalWinUI.secondsBeforeQuit` para ajustar el delay.

---

## 🧪 Checklist de la demo

* [x] 4 coleccionables con SFX y HUD.
* [x] Puerta tutorial (1 lágrima).
* [x] Puerta final (4 lágrimas).
* [x] Meta con UI y cierre limpio.
* [x] Ambiente y sonidos base.

---

## 🖼️ Medios (coloca tus enlaces)

* **Video gameplay (≥1 min):** *link público*
* **Build / carpeta pública:** *link público*
* **Capturas:**

  * `Docs/level_overview.png`
  * `Docs/door_tutorial.png`
  * `Docs/final_door_goal.png`

---

## 🧭 Visión (GDD resumida)

Demo inspirada en **“Gus & Palomino: El Corazón del Bosque”**: plataformas 3D cooperativo con exploración, puzzles ambientales y un mundo reactivo. En la demo “Laberinto de las Lágrimas” se valida el bucle de **explorar → resolver → desbloquear**.

---

## 🐞 Problemas conocidos

* Si duplicas una puerta, confirma `requiredTears` en la **instancia**.
* Evitar el warning `CharacterController.Move on inactive`: ya mitigado con guardas y desactivación en `GoalWinUI`.

---

## 📜 Licencia / Créditos

* Starter Assets – Third Person Controller (Unity).
* SFX/Música: [*fuente*](https://mp3cut.net/es/).
* Código y diseño: Hinara Pastora Sánchez Mata.
* Licencia del repo: *MIT / CC-BY /*.

