using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Opcional")]
    public AudioClip pickupSfx;

    private bool _taken;

    void OnTriggerEnter(Collider other)
    {
        if (_taken) return;

        if (other.CompareTag("Player"))
        {
            _taken = true;
            GameManager.instance.AddTear();

            if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
            gameObject.SetActive(false); // para poder reiniciar la escena sin recrearlo
        }
    }
}
