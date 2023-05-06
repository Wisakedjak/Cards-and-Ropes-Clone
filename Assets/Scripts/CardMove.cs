using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Obi;
using DG.Tweening;

public class CardMove : MonoBehaviour
{
    public ObiSolver solver;
    private bool _isMoving;
    private Tween _moveTween, _rotateTween;
    void Start()
    {
        
    }

    public void Trigger()
    {
        _trigger();
    }

    void _trigger()
    {
        _isMoving = false;
        _moveTween.Kill();
        transform.DOMoveZ(transform.position.z + 2.5f, 1f).OnComplete(()=>transform.DOMoveZ(transform.position.z - 5f, .5f).OnComplete(()=>_move()));
    }

    void _rotate()
    {
        _rotateTween=transform.DOLocalRotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetRelative(true)
            .SetEase(Ease.Linear).SetLoops(-1);
    }

    void _move()
    {
        _moveTween=transform.DOMoveZ(30f, 15f);
    }

    private void OnEnable()
    {
        //_isMoving = true;
        _move();
        _rotate();
        solver.OnCollision += solver_OnCollision;
    }

    void StartMove()
    {
        transform.Translate(Vector3.forward*Time.deltaTime*10f);
    }
    int count;
    void solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        
        
        // just iterate over all contacts in the current frame:
        foreach (Oni.Contact contact in e.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance <= 0)
            {
              
                ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                if (col != null)
                {
                   
                    break;
                }
            }
        }
        
    }
    

    void Update()
    {
        if (_isMoving)
        {
            StartMove();
        }
        
    }
}