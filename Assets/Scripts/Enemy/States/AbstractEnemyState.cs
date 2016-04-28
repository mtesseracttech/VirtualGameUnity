using UnityEngine;

public abstract class AbstractEnemyState
{
    protected EnemyAgent _agent;

    protected AbstractEnemyState(EnemyAgent agent)
    {
        _agent = agent;
    }

    public abstract void Update();
}