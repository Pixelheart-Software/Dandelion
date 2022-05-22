using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMineralsDetector : MonoBehaviour
{
    private NutritionResourceController resource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puddle") || collision.CompareTag("Nutrients"))  
        {
            resource = collision.GetComponent<NutritionResourceController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (resource != null && collision.gameObject == resource.gameObject)
        {
            resource = null;
        }
    }

    public NutritionResourceController GetResource()
    {
        return resource;
    }
}
