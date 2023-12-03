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

    private List<EEnforce> _headStatEnforce;
    private List<EEnforce> _staffStatEnforce;

    public void EnforceStaff(EEnforce enforce)
    {
        float figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
        if (EEnforce.STAFF_MINER_COUNT == enforce)
        {
            int count = (int)figure;
            if (_staffMiners.Count < count)
            {
                Miner miner = _minerSpawner.SpawnMiner(1);
                foreach (EEnforce allStat in _staffStatEnforce)
                {
                    miner.EnforceStat(allStat, EnforceManager.GetBase(allStat) + (EnforceManager.GetLevel(allStat) * EnforceManager.GetCoeff(allStat)));
                }
                _staffMiners.Add(miner);
            }
        }
        else
        {
            foreach (Miner miner in _staffMiners)
            {
                miner.EnforceStat(enforce, figure);
            }
        }

    }

    public void EnforceHead(EEnforce enforce)
    {
        float figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
        _headMiner.EnforceStat(enforce, figure);
    }

    private void Awake()
    {
        _minerSpawner = GetComponent<MinerSpawner>();
        _staffMiners = new List<Miner>();
        _headStatEnforce = new List<EEnforce>();
        _staffStatEnforce = new List<EEnforce>();

        for (int i = 0; i < 5; i++)
        {
            _headStatEnforce.Add((EEnforce)i);
        }

        for (int i = 6; i < 9; i++)
        {
            _staffStatEnforce.Add((EEnforce)i);
        }
    }

    private void OnEnable()
    {
        GameApp.OnPreGameStart += PreGameStart;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
    }

    public void PreGameStart()
    {
        _headMiner = _minerSpawner.SpawnMiner(SaveManager.Save.HeadMinerId);
        int staffCount = EnforceManager.GetLevel(EEnforce.STAFF_MINER_COUNT);
        for (int i = 0; i < staffCount; i++)
        {
            _staffMiners.Add(_minerSpawner.SpawnMiner(1));
        }
        _followCam.Follow = _headMiner.CameraTr;
    }
}
