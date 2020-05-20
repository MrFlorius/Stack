using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackChip : MonoBehaviour
{
    public enum Hit
    {
        NoHit,
        Hit,
        PerfectHit
    }
    #region Getters
    public Collider Collider
    {
        get
        {
            if (_collider == null) _collider = GetComponent<Collider>();
            return _collider;
        }
        private set => _collider = value;
    }

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
        private set => _rigidbody = value;
    }

    public Renderer Renderer
    {
        get
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            return _renderer;
        }
        set => _renderer = value;
    }
    #endregion

    public Vector3 velocity;
    
    private Collider _collider;
    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
        if (!GameManager.Instance.bounds.Contains(transform.position)) velocity *= -1;

        if (transform.position.y < -10)
        {
            GameManager.Instance.Spawned.Remove(this);
            Destroy(gameObject);
        }
    }

    public void Drop()
    {
        velocity = Vector3.zero;
        Rigidbody.useGravity = true;
        Rigidbody.isKinematic = false;
    }

    public Hit Stop()
    {
        velocity = Vector3.zero;

        var grid = GameManager.Instance.grid;
    
        var bounds = Collider.bounds;
        var boundsOther = GameManager.Instance.LastChip.Collider.bounds;

        var min = bounds.min.SnapToGrid(grid);
        var max = bounds.max.SnapToGrid(grid);

        var minOther = boundsOther.min.SnapToGrid(grid);
        var maxOther = boundsOther.max.SnapToGrid(grid);


        if (boundsOther.GetXZ().Contains(bounds.min) || boundsOther.GetXZ().Contains(bounds.max))
        {
            if (max.x > maxOther.x || max.z > maxOther.z)
            {
                //Если не проверять, то возможрны ситуации при которых масштаб по одной из осей будет равен нулю
                if (!SetByRect(transform, min, new Vector3(maxOther.x, max.y, maxOther.z)))
                {
                    Drop();
                    return Hit.NoHit;
                }

                if (SpawnFallingChop(new Vector3(max.z > maxOther.z ? min.x : max.x,
                    max.y, max.x > maxOther.x ? min.z : max.z), maxOther) is null)
                {
                    return Hit.PerfectHit;
                }
            }
            else
            {
                //Если не проверять, то возможрны ситуации при которых масштаб по одной из осей будет равен нулю
                if (!SetByRect(transform, new Vector3(minOther.x, min.y, minOther.z), max))
                {
                    Drop();
                    return Hit.NoHit;
                }
                
                if (SpawnFallingChop(new Vector3(minOther.x, min.y, minOther.z),
                    new Vector3(max.z < maxOther.z ? max.x : min.x, max.y, max.x < maxOther.x ? max.z : min.z)) is null)
                {
                    return Hit.PerfectHit;
                }
            }
            return Hit.Hit;
        }

        Drop();
        return Hit.NoHit;
    }

    private StackChip SpawnFallingChop(Vector3 min, Vector3 max)
    {
        //проверка на неравенство размеров по осям нулям
        if (!(max - min).All())
            return null;
                
        var c = Instantiate(this);
        SetByRect(c.transform, min, max);
        c.Drop();

        return c;
    }

    private static bool SetByRect(Transform transform, Vector3 min, Vector3 max)
    {
        //проверка на неравенство размеров по осям нулям
        if (!(max - min).All())
            return false;
        
        transform.position = min + (max - min) / 2;
        transform.localScale = max - min;

        return true;
    }
}
