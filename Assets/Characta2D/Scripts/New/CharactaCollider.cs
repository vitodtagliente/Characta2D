using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C2D
{
    [AddComponentMenu("C2D/Collider")]
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharactaCollider : MonoBehaviour
    {
        /* this is based on a box collider, which can be configured (in the editor) to fit the sprite */
        BoxCollider2D _collider;
        public new BoxCollider2D collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider2D>();
                return _collider;
            }
        }
    }
}
