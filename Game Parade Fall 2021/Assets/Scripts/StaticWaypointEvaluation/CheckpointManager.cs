using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    Checkpoint[] _checkpoints;

    public Checkpoint CurrentCheckpoint { get; private set; }

    void Start()
    {
        _checkpoints = GetComponentsInChildren<Checkpoint>();
        foreach (var checkpoint in _checkpoints)
        {
            checkpoint.onCheckpointTrigger += SetCurrentCheckpoint;
        }
        CurrentCheckpoint = _checkpoints[0];
        GameManager.Instance.onBirdCaught += HandleBirdCaught;
    }

    private void HandleBirdCaught()
    {
        GameManager.Instance.RespawnBird(CurrentCheckpoint);
    }

    private void OnDestroy()
    {
        foreach (var checkpoint in _checkpoints)
        {
            checkpoint.onCheckpointTrigger -= SetCurrentCheckpoint;
        }
    }

    void SetCurrentCheckpoint(Checkpoint checkpoint) { CurrentCheckpoint = checkpoint; }
}
