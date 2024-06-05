using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Sector : MonoBehaviour
{
    const float MaximumNormalVectorSlope = 0.5f;

    public bool Bad;
    public Mesh GoodSectorMesh;
    public Mesh BadSectorMesh;
    public Material GoodSectorColor;
    public Material BadSectorColor;

    private void Awake()
    {
        ChooseSectorMesh();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.TryGetComponent(out Player player)) return;
        if (!VerticalPlane(collision)) return;
        if (Bad)
        {
            player.Die();
        }
        else
            player.Bounce();
    }

    private bool VerticalPlane(Collision collision)
    {
        Vector3 normal = -collision.contacts[0].normal.normalized;
        float dot = Vector3.Dot(normal, Vector3.up);
        if (dot >= MaximumNormalVectorSlope)
            return true;
        return false;
    }

    private void OnValidate()
    {
        ChooseSectorMesh();
    }

    private void ChooseSectorMesh()
    {
        MeshFilter sectorMash = GetComponent<MeshFilter>();
        Renderer sectorRenderer = GetComponent<Renderer>();
        
        if(GoodSectorMesh == null || BadSectorMesh == null) return;
        
        if (Bad)
        {
            sectorMash.mesh = BadSectorMesh;
            sectorRenderer.sharedMaterial = BadSectorColor;
        }
        else
        {
            sectorMash.mesh = GoodSectorMesh;
            sectorRenderer.sharedMaterial = GoodSectorColor; 
        }
    }
}