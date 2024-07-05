using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSGame
{
    public static class TPSGameDataModel
    {
        #region PoolClass
        [System.Serializable]
        public enum PoolObjectType
        {
            SmallEnemy,
            LargeEnemy
        }

        [System.Serializable]
        public class Pool
        {
            public int capacity;
            public GameObject poolGameObject;
            public PoolObjectType PoolObjectType;
        }
        #endregion

        #region Game States
        [System.Serializable]
        public enum GameState
        {
            Playing,
            GameOver
        }
        #endregion

        #region SoundClass
        [System.Serializable]
        public class SoundClip
        {
            public SoundType soundType;
            public AudioClip audioClip;
        }

        [System.Serializable]
        public enum SoundType
        {
            None,
            PlayerWalk,
            PlayerHurt,
            PlayerDie,
            EnemyDie,
            EnemyHit,
            EnemyRoar,
            BackGroundMusic,
            CoinCollect,
            PlayerHit
        }
        #endregion
    }
}