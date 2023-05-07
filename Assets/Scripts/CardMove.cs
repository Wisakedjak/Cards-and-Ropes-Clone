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
    [SerializeField] private int cardLevel;
    private int _tearTry=5;
    void Start()
    {
        
    }

    public void Trigger(int ropeLevel,GameObject rope,BoxCollider boxCollider)
    {
        _trigger(ropeLevel,rope,boxCollider);
    }

    private void _trigger(int ropeLevel,GameObject rope,BoxCollider boxCollider)
    {
        
        if (cardLevel>ropeLevel)
        {
            boxCollider.enabled = false;
            rope.GetComponent<ObiRope>().tearingEnabled = true;
            GameManager.instance.GainMoney(ropeLevel+1);
            _tearTry--;
        }
        else
        {
            _isMoving = false;
            _moveTween.Kill();
            _tearTry--;
            boxCollider.gameObject.GetComponent<RopeColliderScript>().ropeLevel--;
            if (_tearTry<=0)
            {
                transform.DOMoveZ(transform.position.z + 2.5f, .5f).OnComplete(()=>_destroyObject());
            }
            else
            {
                transform.DOMoveZ(transform.position.z + 2.5f, .5f).OnComplete(()=>transform.DOMoveZ(transform.position.z - 5f, .25f).OnComplete(()=>_move()));
            }
            
        }
        
    }

    void _destroyObject()
    {
        GameManager.instance.DestroyCardAndCheckGameOver(gameObject);
        
    }

    void _rotate()
    {
        _rotateTween=transform.DOLocalRotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetRelative(true)
            .SetEase(Ease.Linear).SetLoops(-1);
    }

    void _move()
    {
        _moveTween=transform.DOMoveZ(100f, 10f);
    }

    public void ThrowCard()
    {
        GetComponent<ObiCollider>().enabled = true;
        _move();
        _rotate();
    }

    private void OnEnable()
    {
        //_isMoving = true;
       
        //solver.OnCollision += solver_OnCollision;
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
