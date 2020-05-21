using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controll
{
    public abstract class CharacterController : MonoBehaviour
    {
        public bool isAttacking;
        public SpeedState moveState;
    }
}