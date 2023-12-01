using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerTeam : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _followCam;

    private Miner _headMiner;

    private void OnEnable()
    {
        GameApp.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameApp.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        _headMiner = GameManager.Player;
        _followCam.Follow = _headMiner.CameraTr;
    }
}
