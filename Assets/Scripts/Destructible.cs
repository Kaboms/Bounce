using UnityEngine;

public class Destructible : MonoBehaviour
{
    public void Destruct()
    {
        float destructDuration = 0;

        if (TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            particleSystem.Play();
            destructDuration = particleSystem.main.duration;
        }

        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
            meshRenderer.enabled = false;

        if (TryGetComponent<Collider>(out Collider collider))
            collider.enabled = false;

        Destroy(gameObject, destructDuration);
    }
    //--------------------------------------------------------------------------
}
