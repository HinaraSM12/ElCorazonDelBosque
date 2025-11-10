using UnityEngine;
public class DoorDebugHotkeys : MonoBehaviour
{
    public AN_DoorScript door;

    void Update()
    {
        if (!door) return;

        if (Input.GetKeyDown(KeyCode.U)) { door.Unlock(); Debug.Log("Door UNLOCK"); }
        if (Input.GetKeyDown(KeyCode.K)) { door.Lock(); Debug.Log("Door LOCK"); }

        if (Input.GetKeyDown(KeyCode.P))  // cambia lado de apertura
        {
            door.OpenPositive = !door.OpenPositive;
            Debug.Log("OpenPositive = " + door.OpenPositive);
        }
    }
}
