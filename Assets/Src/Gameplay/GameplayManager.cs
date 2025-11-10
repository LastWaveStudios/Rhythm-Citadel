using System;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using Utilities.ObjectPool;
public enum GameplayState
{
    Build,
    Rhythm
}

public class GameplayManager : Utilities.Subsystem<GameplayManager>
{
    #region Callbacks
    public Action<int> onEnemyDeath = delegate { };

    #endregion

    #region Initialization

    private GameplayState _currentstate;

    [SerializeField]
    public Bullets bulletPrefab;

    public PoolManager poolManager = new PoolManager();

    private void Start()
    {
        _currentstate = GameplayState.Build;
        poolManager.RegisterPool<Bullets>(new ObjectPool<Bullets>(bulletPrefab, transform));
        poolManager.Get<Bullets>();
        poolManager.Get<Bullets>();
        poolManager.Get<Bullets>();
    }

    #endregion

    #region Methods
    public void SetBuildState()
    {
        this._currentstate = GameplayState.Build;
    }

    public void SetRhythmState()
    {
        this._currentstate = GameplayState.Rhythm;
    }

    public bool InRhythmState()
    {
        return this._currentstate == GameplayState.Rhythm;
    }

    public bool InBuildState()
    {
        return this._currentstate == GameplayState.Build;
    }

    #endregion

}
