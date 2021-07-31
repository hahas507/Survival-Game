using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float viewAngle;

    [SerializeField]
    private float viewDistance;

    [SerializeField]
    private LayerMask targetMask;

    private Pig thePig;

    private void Start()
    {
        thePig = GetComponent<Pig>();
    }

    private void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        // OverlapSphere: 주번에 있는 collider을 뽑아내 저장하는 기능
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if (_targetTf.name == "Player")
            {
                Vector3 _dir = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_dir, transform.forward);

                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _dir, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.Log("player is in pig's FOV");
                            Debug.DrawRay(transform.position + transform.up, _dir, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }
            }
        }
    }
}