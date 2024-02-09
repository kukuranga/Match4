using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        ButtonManager.Instance.SetCharacterController(this);
        _anim = GetComponent<Animator>();
    }

    public void SetAttack(bool val)
    {
        _anim.SetBool("Attack", val);
    }

    public void SetDead()
    {
        _anim.SetBool("Death", true);
    }
}
