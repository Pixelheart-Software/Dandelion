using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGenerator : MonoBehaviour
{
    public Transform minX, maxX;
    public int maxCount;
    public float minDelay = 0;
    public GameObject seedPrefab;
    public Canvas parent;
    public float maxScale = 2;

    private int currentCount;

    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(GenerateSeed());
    }

    IEnumerator GenerateSeed()
    {
        yield return new WaitForSeconds(minDelay + Random.Range(0, minDelay));

        CreateObject();

        StartCoroutine(GenerateSeed());
    }

    private void CreateObject()
    {
        if (currentCount >= maxCount)
        {
            return;
        }
        GameObject seed = Instantiate(seedPrefab, RandomPos(), Quaternion.identity, parent.transform);
        seed.transform.localScale = Vector3.one * Random.Range(1f, maxScale);
        seed.GetComponent<DestroyWhenTooFar>().player = gameObject.transform;
        currentCount++;
    }

    private Vector2 RandomPos()
    {
        return new Vector2(Random.Range(minX.position.x, maxX.position.x), transform.position.y);
    }

    public void ObjectDestroyed(GameObject obj)
    {
        if (obj.CompareTag("Seed"))
        {
            currentCount--;
        }
    }
}
