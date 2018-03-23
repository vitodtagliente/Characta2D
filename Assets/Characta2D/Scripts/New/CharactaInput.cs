using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C2D
{
    [RequireComponent(typeof(C2D.CharactaPhysics))]
    public class CharactaInput : MonoBehaviour
    {
        /* this is based on a box collider, which can be configured (in the editor) to fit the sprite */
        CharactaPhysics _physics;
        public new CharactaPhysics physics
        {
            get
            {
                if (_physics == null)
                    _physics = GetComponent<CharactaPhysics>();
                return _physics;
            }
        }


        // The movement speed
        public float speed = 7f;
        public string horizontalInputAxis = "Horizontal";
        public string verticalInputAxis = "Vertical";

        // Get the use input
        public void Update()
        {
            physics.inputMovement.x = Input.GetAxis(horizontalInputAxis);
            physics.inputMovement.y = Input.GetAxis(verticalInputAxis);
            // update the velocity according to the user input
            physics.velocity.x = physics.inputMovement.x * speed;
        }
    }

}