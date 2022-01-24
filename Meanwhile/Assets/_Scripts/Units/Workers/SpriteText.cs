using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteText : MonoBehaviour
{
    [SerializeField] private Worker worker;

    private TextMesh textMesh;
    void Start()
    {
        var parent = transform.parent;

        var parentRenderer = parent.GetComponent<Renderer>();
        var renderer = GetComponent<Renderer>();
        renderer.sortingLayerID = parentRenderer.sortingLayerID;
        renderer.sortingOrder = parentRenderer.sortingOrder;

        var spriteTransform = parent.transform;

        textMesh = GetComponent<TextMesh>();
        var pos = spriteTransform.position;
    }
}
