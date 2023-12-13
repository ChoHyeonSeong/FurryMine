using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinerTeam : MonoBehaviour
{
    public static Action<Action<int>> OnSetStaffMiner { get; set; }
    public static Action<Action<int>> OnSetMinerEquip { get; set; }

    public static Action<int, EMinerLabel> OnSetLabel { get; set; }
    public static Action<int, bool> OnSetWear { get; set; }

    public int HeadMinerId { get => _headMinerId; }
    public Dictionary<int, int> CurrentMinerEquip { get => _minerEquips; }

    [SerializeField]
    private CinemachineVirtualCamera _followCam;

    private bool _isUpEscape = true;
    private int _staffCount;
    private MinerSpawner _minerSpawner;
    private int _headMinerId;
    private Miner _headMiner;
    private Dictionary<int, Miner> _staffMiners;
    private Dictionary<int, int> _minerEquips;  // key == equipId, value == minerId
    private Dictionary<int, Equip> _equips;

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

    public void GoToOtherMine()
    {
        List<Miner> allMIner = new List<Miner>
        {
            _headMiner
        };
        foreach (Miner miner in _staffMiners.Values)
            allMIner.Add(miner);

        foreach (Miner miner in allMIner)
        {
            // 모든 광부의 target을 Null로 만들기
            miner.EnterMine();
        }
    }

    public List<int> GetCurrentStaffIdList()
    {
        List<int> crtStaffIdList = new List<int>();
        foreach (int id in _staffMiners.Keys)
        {
            crtStaffIdList.Add(id);
        }
        return crtStaffIdList;
    }

    public Miner GetMiner(int minerId)
    {
        if (minerId == _headMinerId)
            return _headMiner;
        else if (_staffMiners.ContainsKey(minerId))
            return _staffMiners[minerId];
        else
            return null;
    }

    private void Awake()
    {
        _minerSpawner = GetComponent<MinerSpawner>();
        _staffMiners = new Dictionary<int, Miner>();
        _headStatEnforce = new List<EEnforce>();
        _staffStatEnforce = new List<EEnforce>();
        _equips = new Dictionary<int, Equip>();

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
        MinerItem.OnClickHead += SetHeadMiner;
        MinerItem.OnClickStaff += SetStaffMiner;
        EquipItem.OnClickWear += SetMinerEquip;
        SelectMinerContent.GetHeadId += GetHeadId;
        SelectMinerContent.GetStaffIdList += GetStaffIdList;
        EscapeButton.OnClickEscape += EscapeHeadMiner;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
        MinerItem.GetHeadId -= GetHeadId;
        MinerItem.OnClickHead -= SetHeadMiner;
        MinerItem.OnClickStaff -= SetStaffMiner;
        EquipItem.OnClickWear -= SetMinerEquip;
        SelectMinerContent.GetHeadId -= GetHeadId;
        SelectMinerContent.GetStaffIdList -= GetStaffIdList;
        EscapeButton.OnClickEscape -= EscapeHeadMiner;
    }

    private void PreGameStart()
    {
        // 광부 소환 및 기본 스탯 세팅
        _headMiner = _minerSpawner.SpawnMiner(SaveManager.Save.CurrentHeadId);

        foreach (int id in SaveManager.Save.CurrentStaffIds)
        {
            _staffMiners[id] = _minerSpawner.SpawnMiner(id);
        }
        _followCam.Follow = _headMiner.CameraTr;


        // 장비 소환 및 착용
        _minerEquips = SaveManager.Save.CurrentMinerEquip;
        foreach (int equipId in SaveManager.Save.EquipIds)
        {
            Equip equip = _equips[equipId] = new Equip(TableManager.EquipTable[equipId]);
            if (_minerEquips.ContainsKey(equipId))
            {
                int minerId = _minerEquips[equipId];
                if (minerId != _headMinerId)
                    _staffMiners[minerId].PutOnEquip(equip);
                else
                    _headMiner.PutOnEquip(equip);
            }
        }
    }

    private void SetHeadMiner(MinerItem newHeadItem, MinerItem oldHeadItem)
    {
        switch (newHeadItem.MinerLabel)
        {
            case EMinerLabel.STAFF:
                {
                    // 동료를 반장으로, 반장을 동료로
                    oldHeadItem.SetLabel(EMinerLabel.STAFF);
                    newHeadItem.SetLabel(EMinerLabel.HEAD);

                    SwapHeadStaff(oldHeadItem.MinerId, newHeadItem.MinerId);
                }
                break;
            case EMinerLabel.NONE:
                {
                    // 착용장비가 있다면 장비해제
                    if (_headMiner.EquipId != -1)
                    {
                        _minerEquips.Remove(_headMiner.EquipId);
                        OnSetWear(_headMiner.EquipId, false);
                        _headMiner.TakeOffEquip();
                    }

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
                    OnSetLabel(minerId, EMinerLabel.HEAD);

                    SwapHeadStaff(minerItem.MinerId, minerId);

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
                        // 착용장비가 있다면 장비해제
                        if (_staffMiners[minerId].EquipId != -1)
                        {
                            _minerEquips.Remove(minerId);
                            OnSetWear(_staffMiners[minerId].EquipId, false);
                            _staffMiners[minerId].TakeOffEquip();
                        }
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

    private void SetMinerEquip(EquipItem equipItem)
    {
        // 경우의 수 ::
        // 착용 중이지 않은 장비를 투입된 인원 중에 착용
        // 착용 중이지 않은 장비를 이미 착용 장비가 있는 인원에게 착용
        // 착용 중인 장비를 투입된 인원 중에 착용
        // 착용 중인 장비를 착용중인 인원에 착용

        OnSetMinerEquip((minerId) =>
        {
            // 이 장비를 이미 착용하고 있는 광부가 있을 경우 
            if (_minerEquips.ContainsKey(equipItem.EquipId))
            {
                // 장비를 착용한 광부와 선택한 광부가 같을 경우
                if (_minerEquips[equipItem.EquipId] == minerId)
                    return;

                // 해당 장비를 착용하고 있는 사람의 장비를 해제
                int putOnMinerId = _minerEquips[equipItem.EquipId];

                // 스탯 재계산
                if (putOnMinerId != _headMinerId)
                {
                    _staffMiners[putOnMinerId].TakeOffEquip();
                    RecalculateStaffStat(_staffMiners[putOnMinerId]);
                }
                else
                {
                    _headMiner.TakeOffEquip();
                    RecalculateHeadStat();
                }
            }
            else
            {
                equipItem.SetWear(true);
            }

            Miner miner = minerId == _headMinerId ? _headMiner : _staffMiners[minerId];
            // 다른 장비를 착용하고 있을 경우
            if (miner.EquipId != -1)
            {
                // 장비 착용 현황에서 삭제
                _minerEquips.Remove(miner.EquipId);
                // 착용 라벨 삭제
                OnSetWear(miner.EquipId, false);
                // 장비를 해제
                miner.TakeOffEquip();
            }
            miner.PutOnEquip(_equips[equipItem.EquipId]);
            _minerEquips[equipItem.EquipId] = minerId;
            // 스탯 재계산
            if (minerId == _headMinerId)
                RecalculateHeadStat();
            else
                RecalculateStaffStat(miner);
        });
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

    private void SwapHeadStaff(int headId, int staffId)
    {
        Miner staff = _staffMiners[headId] = _headMiner;

        _headMiner = _staffMiners[staffId];
        _staffMiners.Remove(staffId);

        // 해당 동료, 반장 스탯 재계산
        RecalculateHeadStat();
        RecalculateStaffStat(staff);
    }

    private void RecalculateHeadStat()
    {
        float figure;
        // 해당 동료, 반장 스탯 재계산
        foreach (EEnforce enforce in _headStatEnforce)
        {
            figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
            _headMiner.EnforceStat(enforce, figure);
        }
    }

    private void RecalculateStaffStat(Miner staff)
    {
        float figure;
        foreach (EEnforce enforce in _staffStatEnforce)
        {
            figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
            staff.EnforceStat(enforce, figure);
        }
        staff.EnforceStat(EEnforce.HEAD_CRITICAL_PERCENT, 0);
        staff.EnforceStat(EEnforce.HEAD_CRITICAL_POWER, 0);
    }

    private int GetHeadId()
    {
        return _headMinerId;
    }

    private List<int> GetStaffIdList()
    {
        return _staffMiners.Keys.ToList();
    }

    private void EscapeHeadMiner()
    {
        Vector3 escapePos = transform.position;
        if (_isUpEscape)
        {
            _isUpEscape = false;
        }
        else
        {
            escapePos.y *= -1;
            _isUpEscape = true;
        }
        _headMiner.transform.position = escapePos;
    }
}
