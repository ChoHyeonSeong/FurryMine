using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerTeam : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _followCam;

    private Miner _headMiner;
    private MinerSpawner _minerSpawner;
    private List<Miner> _staffMiners;

    public void EnforceTeam(EEnforce enforce, float enforceFigure)
    {
        switch (enforce)
        {
            case EEnforce.STAFF_MINER_COUNT:
                int count = (int)enforceFigure;
                for (int i = _staffMiners.Count; i < count; i++)
                {
                    _staffMiners.Add(_minerSpawner.SpawnMiner(1));
                }
                break;
        }
    }

    private void Awake()
    {
        _minerSpawner = GetComponent<MinerSpawner>();
        _staffMiners = new List<Miner>();
    }

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
