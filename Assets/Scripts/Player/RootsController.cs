using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RootsController : MonoBehaviour
{
    private bool rooted = false;

    public float diggedRootShowSpeed = 0.5f;

    private List<GameObject> rootsWalking = new List<GameObject>();

    private SpriteRenderer diggedRootRenderer;
    private RootMineralsDetector diggedRootMineralsDetector;

    private Vector2 corePosition;
    private Vector2 unrootedPosition;

    private PlayerLifeController lifeController;

    private void Awake()
    {
        unrootedPosition = transform.position;

        foreach (Transform tr in transform)
        {
            if (tr.CompareTag("Roots"))
            {
                rootsWalking.Add(tr.gameObject);
            }

            if (tr.CompareTag("RootDigged")) 
            {
                diggedRootRenderer = tr.GetComponent<SpriteRenderer>();
                diggedRootMineralsDetector = tr.GetComponent<RootMineralsDetector>();
            }

            if (tr.gameObject.name == "CorePosition")
            {
                corePosition = tr.position;
            }
        }

        lifeController = GetComponent<PlayerLifeController>();

        RootIn(false);
    }

    private void Update()
    {
        ShowGroundRoots(!rooted);

        ShowDiggedRoot(rooted);
    }

    private void MoveFeet(bool rooted)
    {
        if (rooted)
        {
            unrootedPosition = transform.position;
            transform.position = new Vector2(transform.position.x, corePosition.y);
        } else
        {
            transform.position = new Vector2(transform.position.x, unrootedPosition.y);
        }

    }

    public void RootIn(bool rooted)
    {
        this.rooted = rooted;
        MoveFeet(rooted);
    }

    private void ShowDiggedRoot(bool show)
    {
        Color tmp = diggedRootRenderer.color;
        float newAlpha = show ? 1F : 0F;
        if (show)
        {
            tmp.a = Mathf.Lerp(tmp.a, newAlpha, diggedRootShowSpeed * Time.deltaTime);
        } else
        {
            tmp.a = 0f;
        }
        
        diggedRootRenderer.color = tmp;
    }

    private void ShowGroundRoots(bool show)
    {
        foreach (GameObject root in rootsWalking)
        {
            root.SetActive(show);
        }
    }

    internal bool IsRooted()
    {
        return rooted;
    }

    internal void GetNutrition()
    {
        if (!rooted)
        {
            return;
        }

        NutritionResourceController stuffUnderground = diggedRootMineralsDetector.GetResource();

        if (stuffUnderground == null)
        {
            return;
        }

        Nutrition nutrition = stuffUnderground.GetNutrition();

        if (nutrition.type == NutritionType.Minerals)
        {
            lifeController.AddMinerals(nutrition.amount);
        }
        else if (nutrition.type == NutritionType.Water)
        {
            lifeController.AddWater(nutrition.amount);
        }
    }
}

