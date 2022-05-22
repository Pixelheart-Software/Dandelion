using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingManager : MonoBehaviour
{
    public GameObject leafPrefab;
    public GameObject stemPrefab;


    private List<GameObject> leaves = new List<GameObject>();
    private GameObject stem;
    private GameObject flower;
    private GameObject blossomedBud;

    private Rigidbody2D myRigidBody;

    private float seedTimer = 0;

    private List<Transform> seeds = new List<Transform>();

    void Start()
    {
        foreach (Transform tr in transform)
        {
            if (tr.CompareTag("Leaf"))
            {
                leaves.Add(tr.gameObject);
            }
        }

        myRigidBody = GetComponent<Rigidbody2D>();

        GrowLeaves(0.20F);
    }

    private void Update()
    {
        if (blossomedBud != null)
        {
            UpdateTimer();
            FadeInSeeds();
        }
    }

    public void UpdateLife(float currentLife)
    {
        currentLife = Mathf.Clamp(currentLife, 0F, 1F);

        if (blossomedBud != null)
        {
        }
        else if (flower != null)
        {
            if (currentLife < 0.75F)
            {
                RemoveStem();
                StopSeedTimer();
            }
            else if (currentLife < 1F)
            {
                GrowFlower(currentLife);
            }
            else
            {
                CreateBlossomedBud();
                StartSeedTimer();
                FadeInSeeds();
            }
        }
        else if (stem != null)
        {
            if (currentLife < 0.5F)
            {
                RemoveStem();
            }
            else if (currentLife < 0.75F)
            {
                GrowStem(currentLife);
            }
            else
            {
                CreateFlower();
                GrowFlower(currentLife);
            }
        }
        else if (leaves.Count == 2)
        {
            if (currentLife < 0.25)
            {
                RemoveLeaf();
            }
            else if (currentLife < 0.5F)
            {
                GrowLeaves(currentLife);
            }
            else
            {
                CreateStem();
                GrowStem(currentLife);
            }
        }
        else if (leaves.Count == 1)
        {
            if (currentLife <= 0F)
            {
                GameOver();
            }
            else if (currentLife < 0.25)
            {
                GrowLeaves(currentLife);
            }
            else
            {
                GrowLeaves(currentLife);
                AddLeaf(currentLife - 0.25F);
            }
        }
    }

    private void CreateBlossomedBud()
    {
        foreach (Transform tr in stem.transform)
        {
            if (tr.name == "blossomed_bud")
            {
                blossomedBud = tr.gameObject;
            }
        }
    }

    private void StopSeedTimer()
    {
        seedTimer = 0;
    }

    private void FadeInSeeds()
    {
        LerpAlpha(flower, 0, (10F - seedTimer) / 10F);

        foreach (Transform tr in blossomedBud.transform)
        {
            LerpAlpha(tr.gameObject, 1F, (10F - seedTimer) / 10F);
        }
    }

    private void LerpAlpha(GameObject obj, float alpha, float ratio)
    {
        alpha = Mathf.Clamp01(alpha);
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color tmp = spriteRenderer.color;
        tmp.a = Mathf.Lerp(tmp.a, alpha, ratio);

        spriteRenderer.color = tmp;
    }

    private void StartSeedTimer()
    {
        seedTimer = 10;
    }

    private void UpdateTimer()
    {
        seedTimer -= Time.deltaTime;
        EventBus.Instance.onSeedTimerChange.Invoke((10F - seedTimer) / 10);

        if (seedTimer <= 0)
        {
            ThrowSeeds();
        }
    }

    private void ThrowSeeds()
    {
        stem.GetComponent<Collider2D>().enabled = false;

        foreach (Transform tr in blossomedBud.transform)
        {
            GameObject gm = tr.gameObject;

            if (gm.name != "core")
            {
                tr.parent = null;
                Rigidbody2D rb = gm.GetComponent<Rigidbody2D>();
                rb.simulated = true;
                rb.WakeUp();
                rb.AddForce(new Vector2(0.01F, 0.01F), ForceMode2D.Impulse);
                rb.bodyType = RigidbodyType2D.Dynamic;

                seeds.Add(tr);
            }
        }

        Destroy(gameObject.GetComponent<MovementController>());
        Camera.main.GetComponent<CameraFollow>().player = null;
        gameObject.tag = "Untagged";
        StartCoroutine("WaitForBestSeed");
    }

    IEnumerator WaitForBestSeed()
    {
        yield return new WaitForSeconds(2);

        Transform farthest = seeds[0];

        foreach (Transform seed in seeds)
        {
            if (seed.position.x > farthest.position.x)
            {
                farthest = seed;
            }
        }

        farthest.tag = "Player";
        Camera.main.GetComponent<CameraFollow>().player = farthest;
        GameObject.Find("Backgrounds").GetComponent<BackgroundManager>().player = farthest;
        DestroyWhenTooFar[] destroyWhenTooFars = FindObjectsOfType<DestroyWhenTooFar>();
        foreach (DestroyWhenTooFar d in destroyWhenTooFars)
        {
            d.player = farthest;
        }

        Follow[] follow = FindObjectsOfType<Follow>();
        foreach (Follow d in follow)
        {
            d.followed = farthest;
        }

        EventBus.Instance.onNutritionChange.RemoveListener(GetComponent<GrowingManager>().UpdateLife);

        farthest.gameObject.AddComponent<SeedPlanter>();

        foreach (Transform seed in seeds)
        {
            if (seed != farthest)
            {
                seed.gameObject.AddComponent<SeedFadeOut>();
            }
        }

        GameObject.Find("BugGenerator").GetComponent<ObjectGenerator>().enabled = false;

        var bugs = GameObject.FindObjectsOfType<BugController>();
        foreach (var bug in bugs)
        {
            Destroy(bug.gameObject);
        }

        Destroy(gameObject);
    }


    private void GrowFlower(float current)
    {
        float scale = current;
        flower.transform.localScale = Vector2.one * scale;
    }

    private void CreateFlower()
    {
        foreach (Transform tr in stem.transform)
        {
            if (tr.name == "flower")
            {
                flower = tr.gameObject;
            }
        }
    }

    private void RemoveStem()
    {
        stem.transform.parent = null;

        stem = null;
        flower = null;
        blossomedBud = null;
    }

    private void GrowStem(float current)
    {
        stem.transform.localScale = Vector2.one * current;
    }

    private void CreateStem()
    {
        stem = Instantiate(stemPrefab, gameObject.transform);

        HingeJoint2D leafJoint = stem.GetComponent<HingeJoint2D>();

        leafJoint.connectedBody = myRigidBody;
    }

    private void RemoveLeaf()
    {
        GameObject leafToRemove = leaves[0];
        foreach (GameObject leaf in leaves)
        {
            if (leaf.transform.localScale.y < 1)
            {
                leafToRemove = leaf;
            }
        }

        leaves.Remove(leafToRemove);
        leafToRemove.transform.parent = null;
        Destroy(leafToRemove.GetComponent<HingeJoint2D>());
    }

    private void GrowLeaves(float current)
    {
        float scale = Mathf.Clamp01((leaves.Count == 2 ? (current - 0.25F) : current) / 0.25F);


        foreach (GameObject leaf in leaves)
        {
            if (scale < Mathf.Abs(leaf.transform.localScale.x))
            {
                continue;
            }

            if (leaf.transform.localScale.x < 1)
            {
                leaf.transform.localScale = new Vector2(scale * Mathf.Sign(leaf.transform.localScale.x), scale);
            }
        }
    }

    private void AddLeaf(float current)
    {
        GameObject leaf = Instantiate(leafPrefab, gameObject.transform);
        leaves.Add(leaf);

        HingeJoint2D leafJoint = leaf.GetComponent<HingeJoint2D>();

        if (leaves.Count > 1)
        {
            leaf.transform.localScale = new Vector3(-1, 1, 1);
            leaf.transform.position = new Vector2(leaf.transform.position.x + 0.02F, leaf.transform.position.y);
        }

        leaf.transform.localScale = leaf.transform.localScale * current;

        leafJoint.connectedBody = myRigidBody;
    }

    private void GameOver()
    {
        Debug.LogError("TODO: Game Over");
    }
}