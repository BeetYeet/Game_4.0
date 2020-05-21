using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controll
{
    public class CharacterAnimation : MonoBehaviour
    {
        public Animator anim;
        public CharacterController controller;

        private void Update()
        {
            anim.SetBool("Attacking", controller.isAttacking);
            anim.SetInteger("MoveState", (int)controller.moveState);
        }
    }

    public enum SpeedState { Idle, Walking, Running }
}