using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Coleccionables")]
    public int totalTears = 4;
    private int tearsCollected = 0;

    [Header("Puertas a desbloquear al completar")]
    public List<AN_DoorScript> doorsToOpenWhenComplete = new List<AN_DoorScript>();
    public bool autoFindDoorsByTag = true;

    private float elapsed;
    private bool running;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start()
    {
        running = true;

        if (autoFindDoorsByTag && doorsToOpenWhenComplete.Count == 0)
        {
            var doors = GameObject.FindGameObjectsWithTag("AutoDoor");
            foreach (var go in doors)
            {
                var d = go.GetComponent<AN_DoorScript>();
                if (d != null) doorsToOpenWhenComplete.Add(d);
            }
        }

        UIManager.Instance?.SetTears(tearsCollected, totalTears);
    }

    void Update()
    {
        if (!running) return;
        elapsed += Time.deltaTime;
    }

    public void AddTear()
    {
        tearsCollected = Mathf.Min(tearsCollected + 1, totalTears);
        UIManager.Instance?.SetTears(tearsCollected, totalTears);

        if (tearsCollected >= totalTears)
            UnlockAllRegisteredDoors();
    }

    /* Helpers públicos para no tocar tu lógica */
    public int CurrentTears => tearsCollected;
    public bool HasTears(int needed) => tearsCollected >= needed;
    public bool AllTearsCollected() => tearsCollected >= totalTears;

    public void LevelComplete()
    {
        running = false;
        Debug.Log($"Nivel completado. Tiempo: {FormatTime(elapsed)}");
    }

    private void UnlockAllRegisteredDoors()
    {
        foreach (var door in doorsToOpenWhenComplete)
        {
            if (!door) continue;
            door.Unlock();
            Debug.Log($"Puerta '{door.name}' desbloqueada por completar las lágrimas.");
        }
    }

    private string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 1000f) % 1000f);
        return $"{m:00}:{s:00}.{ms:000}";
    }
}
