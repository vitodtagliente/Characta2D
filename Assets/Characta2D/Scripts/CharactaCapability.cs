using UnityEngine;
using System;

/*
 * Plugin style system for Characta
 */

namespace Characta2D
{
	[RequireComponent(typeof(Characta2D.CharactaObject))]
    public abstract class CharactaCapability : MonoBehaviour
    {
		Characta2D.CharactaObject _character;
		public Characta2D.CharactaObject character
        {
            get
            {
                if (_character == null)
					_character = GetComponent<Characta2D.CharactaObject>();
                return _character;
            }
        }

		// set to true if the character is controlled by the local player
		// false for the AI or secondary players
		public bool isPlayer { get; private set; }

        private void Awake()
        {
            isPlayer = GetComponent<Characta2D.CharactaPlayerController>() != null;
        }

        public virtual void Activate()
		{
		}

        public virtual void Deactivate()
        {

        }
	}
}
