using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            tag = "Untagged";

            GameObject playerPrefab = (GameObject)Resources.Load("prefabs/Dandelion", typeof(GameObject));

            GameObject newPlayer = Instantiate(playerPrefab, transform.position + new Vector3(0, 0.5F, 0), Quaternion.identity);

            Camera.main.GetComponent<CameraFollow>().player = newPlayer.transform;
            GameObject.Find("Backgrounds").GetComponent<BackgroundManager>().player = newPlayer.transform;
            DestroyWhenTooFar[] destroyWhenTooFars = FindObjectsOfType<DestroyWhenTooFar>();
            foreach (DestroyWhenTooFar d in destroyWhenTooFars)
            {
                d.player = newPlayer.transform;
            }
            Follow[] follow = FindObjectsOfType<Follow>();
            foreach (Follow d in follow)
            {
                d.followed = newPlayer.transform;
            }

            GameObject.Find("BugGenerator").GetComponent<ObjectGenerator>().enabled = true;

            EventBus.Instance.onNutritionChange.AddListener(newPlayer.GetComponent<GrowingManager>().UpdateLife);
            
            Destroy(gameObject);
        }
    }

}
