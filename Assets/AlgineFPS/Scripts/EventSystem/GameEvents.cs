using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine
{
    public class GameEvents
    {
        private static GameEvents _current;

        public static GameEvents Current {
            get
            {
                if (_current == null)
                {
                    _current = new GameEvents();
                    return _current;
                }
                else
                {
                    return _current;
                }
            }
            private set { }
        }
        public Action OnPlayerDeath;
        public Action OnPlayerResPawn;

        public void PlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }
        public void PlayerResPawn()
        {
            OnPlayerResPawn?.Invoke();
        }
    }
}
