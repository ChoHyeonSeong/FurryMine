using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinerTeam : MonoBehaviour
{
    public static Action<int> OnSelectMiner { get; set; }
    public static Action<Action<int>> OnSetMinerEquip { get; set; }

    public static Action<int, EMinerLabel> OnSetLabel { get; set; }
    public static Action<int, bool> OnSetWear { get; set; }
    public static Action<int> OnInitSelectMiner { get; set; }

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
        _minerEquips = new Dictionary<int, int>();
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
        MinerItem.OnClickHead += SetHeadMiner;
        MinerItem.OnClickStaff += SetStaffMiner;
        SelectMinerPanel.OnSetStaffMiner += SetStaffMiner;
        SelectMinerPanel.OnSetMinerEquip += SetMinerEquip;
        EscapeButton.OnClickEscape += EscapeHeadMiner;
    }

    private void OnDisable()
    {
        GameApp.OnPreGameStart -= PreGameStart;
        MinerItem.OnClickHead -= SetHeadMiner;
        MinerItem.OnClickStaff -= SetStaffMiner;
        SelectMinerPanel.OnSetStaffMiner -= SetStaffMiner;
        SelectMinerPanel.OnSetMinerEquip -= SetMinerEquip;
        EscapeButton.OnClickEscape -= EscapeHeadMiner;
    }

    private void PreGameStart()
    {
        // 광부 소환 및 기본 스탯 세팅
        _headMiner = _minerSpawner.SpawnMiner(SaveManager.Save.CurrentHeadId);
        _headMinerId = SaveManager.Save.CurrentHeadId;

        foreach (int id in SaveManager.Save.CurrentStaffIds)
        {
            _staffMiners[id] = _minerSpawner.SpawnMiner(id);
        }
        _followCam.Follow = _headMiner.CameraTr;


        // 장비 소환 및 착용
        foreach(MinerEquip minerEquip in SaveManager.Save.CurrentMinerEquip)
        {
            _minerEquips[minerEquip.EquipId] = minerEquip.MinerId;
        }

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

    private void SetHeadMiner(int minerId)
    {
        // minerId == 반장이 될 광부
        // 동료 광부 중에 있는지 확인
        Debug.Log($"SetHeadMiner - minerId == {minerId}");
        Debug.Log($"SetHeadMiner - headMinerId == {_headMinerId}");
        OnSetLabel(minerId, EMinerLabel.HEAD);
        if (_staffMiners.ContainsKey(minerId))
        {
            OnSetLabel(_headMinerId, EMinerLabel.STAFF);
            SwapHeadStaff(_headMinerId, minerId);
        }
        else
        {
            OnSetLabel(_headMinerId, EMinerLabel.NONE);

            if (_headMiner.EquipId != -1)
            {
                _minerEquips.Remove(_headMiner.EquipId);
                OnSetWear(_headMiner.EquipId, false);
                _headMiner.TakeOffEquip();
            }

            _minerSpawner.CollectMiner(_headMiner);
            _headMiner = _minerSpawner.SpawnMiner(minerId);

            RecalculateHeadStat();
        }
        // 카메라 이동
        _followCam.Follow = _headMiner.CameraTr;
        _headMinerId = minerId;
        OnInitSelectMiner(minerId);
    }

    private void SetStaffMiner(int minerId)
    {
        if(minerId == _headMinerId)
        {
            OnSelectMiner(minerId);
        }
        else
        {
            // 투입 가능한 있원 > 현재 투입 인원 수 
            if (_staffCount > _staffMiners.Count)
            {
                // 그냥 투입
                OnSetLabel(minerId, EMinerLabel.STAFF);
                AddStaffMiner(minerId);
            }
            else
            {
                OnSelectMiner(minerId);
            }
        }
        OnInitSelectMiner(minerId);
    }

    private void SetStaffMiner(int staffMinerId, int selectMinerId)
    {
        OnSetLabel(staffMinerId, EMinerLabel.STAFF);
        if (staffMinerId == _headMinerId)
        {
            OnSetLabel(selectMinerId, EMinerLabel.HEAD);
            SwapHeadStaff(staffMinerId, selectMinerId);
            _followCam.Follow = _headMiner.CameraTr;
            _headMinerId = selectMinerId;
        }
        else
        {
            if (_staffMiners[selectMinerId].EquipId != -1)
            {
                _minerEquips.Remove(_staffMiners[selectMinerId].EquipId);
                OnSetWear(_staffMiners[selectMinerId].EquipId, false);
                _staffMiners[selectMinerId].TakeOffEquip();
            }
            OnSetLabel(selectMinerId, EMinerLabel.NONE);
            _minerSpawner.CollectMiner(_staffMiners[selectMinerId]);
            _staffMiners.Remove(selectMinerId);
            AddStaffMiner(staffMinerId);
        }
    }

    private void SetMinerEquip(int equipId, int selectMinerId)
    {
        // 경우의 수 ::
        // 착용 중이지 않은 장비를 투입된 인원 중에 착용
        // 착용 중이지 않은 장비를 이미 착용 장비가 있는 인원에게 착용
        // 착용 중인 장비를 투입된 인원 중에 착용
        // 착용 중인 장비를 착용중인 인원에 착용

        // 이 장비를 이미 착용하고 있는 광부가 있고, 그 광부가 선택한 광부가 아닐 경우
        if (_minerEquips.ContainsKey(equipId))
        {
            if (_minerEquips[equipId] != selectMinerId)
            {
                // 해당 장비를 착용하고 있는 사람의 장비를 해제
                int putOnMinerId = _minerEquips[equipId];

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
                return;
        }
        else
        {
            OnSetWear(equipId, true);
        }

        Miner miner = selectMinerId == _headMinerId ? _headMiner : _staffMiners[selectMinerId];
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
        miner.PutOnEquip(_equips[equipId]);
        _minerEquips[equipId] = selectMinerId;
        // 스탯 재계산
        if (selectMinerId == _headMinerId)
            RecalculateHeadStat();
        else
            RecalculateStaffStat(miner);
    }

    private void AddStaffMiner(int minerId)
    {
        Miner staff = _staffMiners[minerId] = _minerSpawner.SpawnMiner(minerId);
        RecalculateStaffStat(staff);
    }

    private void SwapHeadStaff(int headId, int staffId)
    {
        Miner staff = _staffMiners[headId] = _headMiner;

        _headMiner = _staffMiners[staffId];
        _headMinerId = staffId;

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