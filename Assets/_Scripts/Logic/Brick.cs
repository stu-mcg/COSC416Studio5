using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private Coroutine destroyRoutine = null;

    private void OnCollisionEnter(Collision other)
    {
        if (destroyRoutine != null) return;
        if (!other.gameObject.CompareTag("Ball")) return;

        // camera shake
        CameraShake.Shake(0.5f, 0.5f);

        destroyRoutine = StartCoroutine(DestroyWithDelay());
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // two physics frames to ensure proper collision
        GameManager.Instance.OnBrickDestroyed(transform.position);
        Destroy(gameObject);
    }
}
