using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinerTeam : MonoBehaviour
{
    public static Action<Action<int>> OnSetStaffMiner { get; set; }

    public static Action<int, EMinerLabel> OnSetLabel { get; set; }

    [SerializeField]
    private CinemachineVirtualCamera _followCam;

    private int _staffCount;
    private MinerSpawner _minerSpawner;
    private int _headMinerId;
    private Miner _headMiner;
    private Dictionary<int, Miner> _staffMiners;

    private List<EEnforce> _headStatEnforce;
    private List<EEnforce> _staffStatEnforce;

    public void EnforceStaff(EEnforce enforce)
    {
        float figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));


        if (EEnforce.STAFF_MINER_COUNT != enforce)
        {
            foreach (Miner miner in _staffMiners.Values)
            {
                miner.EnforceStat(enforce, figure);
            }
        }
        else
        {
            _staffCount = (int)figure;
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
        _staffMiners = new Dictionary<int, Miner>();
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
        MinerItem.GetHeadId += GetHeadId;
        MinerItem.OnHeadClick += SetHeadMiner;
        MinerItem.OnStaffClick += SetStaffMiner;
        SelectMinerContent.GetHeadId += GetHeadId;
        SelectMinerContent.GetStaffIdList += GetStaffIdList;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
        MinerItem.GetHeadId -= GetHeadId;
        MinerItem.OnHeadClick -= SetHeadMiner;
        MinerItem.OnStaffClick -= SetStaffMiner;
        SelectMinerContent.GetHeadId -= GetHeadId;
        SelectMinerContent.GetStaffIdList -= GetStaffIdList;
    }

    private void PreGameStart()
    {
        _headMiner = _minerSpawner.SpawnMiner(SaveManager.Save.CurrentHeadId);

        foreach (int id in SaveManager.Save.CurrentStaffIds)
        {
            _staffMiners[id] = _minerSpawner.SpawnMiner(id);
        }

        _followCam.Follow = _headMiner.CameraTr;
    }

    private void SetHeadMiner(MinerItem newHeadItem, MinerItem oldHeadItem)
    {
        switch (newHeadItem.MinerLabel)
        {
            case EMinerLabel.STAFF:
                {
                    // 동료를 반장으로, 반장을 동료로
                    oldHeadItem.SetLabel(EMinerLabel.STAFF);
                    _staffMiners[oldHeadItem.MinerId] = _headMiner;

                    newHeadItem.SetLabel(EMinerLabel.HEAD);
                    _headMiner = _staffMiners[newHeadItem.MinerId];
                    _staffMiners.Remove(newHeadItem.MinerId);

                    float figure;

                    // 해당 동료, 반장 스탯 재계산
                    foreach (EEnforce enforce in _headStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        _headMiner.EnforceStat(enforce, figure);
                    }

                    Miner staff = _staffMiners[oldHeadItem.MinerId];
                    foreach (EEnforce enforce in _staffStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        staff.EnforceStat(enforce, figure);
                    }
                    staff.EnforceStat(EEnforce.HEAD_CRITICAL_PERCENT, 0);
                    staff.EnforceStat(EEnforce.HEAD_CRITICAL_POWER, 0);

                }
                break;
            case EMinerLabel.NONE:
                {
                    oldHeadItem.SetLabel(EMinerLabel.NONE);
                    _minerSpawner.CollectMiner(_headMiner);

                    newHeadItem.SetLabel(EMinerLabel.HEAD);
                    _headMiner = _minerSpawner.SpawnMiner(newHeadItem.MinerId);

                    float figure;

                    // 해당 동료, 반장 스탯 재계산
                    foreach (EEnforce enforce in _headStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        _headMiner.EnforceStat(enforce, figure);
                    }
                }
                break;
            default:
                Debug.Log("정의되지 않은 광부 라벨입니다. : MinerTeam-SetHeadMiner");
                break;
        }
        // 카메라 이동
        _followCam.Follow = _headMiner.CameraTr;
        _headMinerId = newHeadItem.MinerId;
        newHeadItem.InitSelectItem();
    }

    private void SetStaffMiner(MinerItem minerItem)
    {
        switch (minerItem.MinerLabel)
        {
            case EMinerLabel.HEAD:
                OnSetStaffMiner((int minerId) =>
                {
                    // 선택한 
                    // 포지션 바꿀 직원을 정하고
                    // 선택된 직원을 넣기
                    minerItem.SetLabel(EMinerLabel.STAFF);
                    _staffMiners[minerItem.MinerId] = _headMiner;

                    OnSetLabel(minerId, EMinerLabel.HEAD);
                    _headMiner = _staffMiners[minerId];
                    _staffMiners.Remove(minerId);

                    float figure;
                    // 해당 동료, 반장 스탯 재계산
                    foreach (EEnforce enforce in _headStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        _headMiner.EnforceStat(enforce, figure);
                    }

                    Miner staff = _staffMiners[minerItem.MinerId];
                    foreach (EEnforce enforce in _staffStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        staff.EnforceStat(enforce, figure);
                    }
                    staff.EnforceStat(EEnforce.HEAD_CRITICAL_PERCENT, 0);
                    staff.EnforceStat(EEnforce.HEAD_CRITICAL_POWER, 0);

                    _followCam.Follow = _headMiner.CameraTr;
                    _headMinerId = minerId;
                });
                break;
            case EMinerLabel.NONE:
                // 투입 가능한 있원 <= 현재 투입 인원 수 
                if (_staffCount <= _staffMiners.Count)
                {
                    // 선택창 ON
                    OnSetStaffMiner((int minerId) =>
                    {
                        // 뺄 직원을 정하고
                        // 선택된 직원을 넣기
                        OnSetLabel(minerId, EMinerLabel.NONE);
                        _minerSpawner.CollectMiner(_staffMiners[minerId]);
                        _staffMiners.Remove(minerId);

                        minerItem.SetLabel(EMinerLabel.STAFF);
                        AddStaffMiner(minerItem.MinerId);
                    });
                }
                else
                {
                    minerItem.SetLabel(EMinerLabel.STAFF);
                    AddStaffMiner(minerItem.MinerId);
                }
                break;
            default:
                Debug.Log("정의되지 않은 광부 라벨입니다. : MinerTeam-SetStaffMiner");
                break;
        }
        minerItem.InitSelectItem();
    }

    private void AddStaffMiner(int minerId)
    {
        Debug.Log(minerId);
        Miner staff = _staffMiners[minerId] = _minerSpawner.SpawnMiner(minerId);
        float figure;
        foreach (EEnforce enforce in _staffStatEnforce)
        {
            figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
            staff.EnforceStat(enforce, figure);
        }
        staff.EnforceStat(EEnforce.HEAD_CRITICAL_PERCENT, 0);
        staff.EnforceStat(EEnforce.HEAD_CRITICAL_POWER, 0);
    }

    private void ShowMinerInfo(MinerItem minerItem)
    {

    }

    private int GetHeadId()
    {
        return _headMinerId;
    }

    private List<int> GetStaffIdList()
    {
        return _staffMiners.Keys.ToList();
    }
}
