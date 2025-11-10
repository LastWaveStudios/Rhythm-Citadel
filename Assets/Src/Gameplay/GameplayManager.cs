using System;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
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

    private void Start()
    {
        _currentstate = GameplayState.Build;
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
