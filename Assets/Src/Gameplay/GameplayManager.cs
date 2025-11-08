using System;
using UnityEngine;

public class GameplayManager : Utilities.Singleton<GameplayManager>
{
    #region Callbacks
    public Action<int> onEnemyDeath = delegate { };

    #endregion

}
