using UnityEngine;

public class ModificatorHolder : Destructible
{
    public GameObject Modificator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(Modificator, other.gameObject.transform);
            Destroy(gameObject);
        }
    }
    //--------------------------------------------------------------------------
}
