using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeShadow : MonoBehaviour
{
    public Vector2 shadowPosition;
    public Material shadowColor;
    private GameObject fakeShadow;
    private Vector3 targetPosition;
    private bool isDelay = true;
    void Awake()
    {
        StartCoroutine("DelayOnStart");

    }
    private void LateUpdate()
    {
        if (transform.hasChanged&&!isDelay)
            UpdateFakeShadow();
    }
    public void UpdateFakeShadow()
    {
        fakeShadow.transform.position = transform.position + (Vector3)shadowPosition;
    }
    IEnumerator DelayOnStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        fakeShadow = new GameObject("Shadow", typeof(SpriteRenderer));
        SpriteRenderer fakeShadowSprite = fakeShadow.GetComponent<SpriteRenderer>();
        fakeShadowSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        fakeShadowSprite.color = shadowColor.color;
        fakeShadowSprite.sortingOrder = -1;
        fakeShadow.transform.parent = transform;
        fakeShadow.transform.rotation = transform.rotation;
        UpdateFakeShadow();
        isDelay = false;
    }
}
