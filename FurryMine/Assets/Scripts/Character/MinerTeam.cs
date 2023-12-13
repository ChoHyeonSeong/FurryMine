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
            // ��� ������ target�� Null�� �����
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
        // ���� ��ȯ �� �⺻ ���� ����
        _headMiner = _minerSpawner.SpawnMiner(SaveManager.Save.CurrentHeadId);

        foreach (int id in SaveManager.Save.CurrentStaffIds)
        {
            _staffMiners[id] = _minerSpawner.SpawnMiner(id);
        }
        _followCam.Follow = _headMiner.CameraTr;


        // ��� ��ȯ �� ����
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
                    // ���Ḧ ��������, ������ �����
                    oldHeadItem.SetLabel(EMinerLabel.STAFF);
                    newHeadItem.SetLabel(EMinerLabel.HEAD);

                    SwapHeadStaff(oldHeadItem.MinerId, newHeadItem.MinerId);
                }
                break;
            case EMinerLabel.NONE:
                {
                    // ������� �ִٸ� �������
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

                    // �ش� ����, ���� ���� ����
                    foreach (EEnforce enforce in _headStatEnforce)
                    {
                        figure = EnforceManager.GetBase(enforce) + (EnforceManager.GetLevel(enforce) * EnforceManager.GetCoeff(enforce));
                        _headMiner.EnforceStat(enforce, figure);
                    }
                }
                break;
            default:
                Debug.Log("���ǵ��� ���� ���� ���Դϴ�. : MinerTeam-SetHeadMiner");
                break;
        }
        // ī�޶� �̵�
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
                    // ������ 
                    // ������ �ٲ� ������ ���ϰ�
                    // ���õ� ������ �ֱ�
                    minerItem.SetLabel(EMinerLabel.STAFF);
                    OnSetLabel(minerId, EMinerLabel.HEAD);

                    SwapHeadStaff(minerItem.MinerId, minerId);

                    _followCam.Follow = _headMiner.CameraTr;
                    _headMinerId = minerId;
                });
                break;
            case EMinerLabel.NONE:
                // ���� ������ �ֿ� <= ���� ���� �ο� �� 
                if (_staffCount <= _staffMiners.Count)
                {
                    // ����â ON
                    OnSetStaffMiner((int minerId) =>
                    {
                        // ������� �ִٸ� �������
                        if (_staffMiners[minerId].EquipId != -1)
                        {
                            _minerEquips.Remove(minerId);
                            OnSetWear(_staffMiners[minerId].EquipId, false);
                            _staffMiners[minerId].TakeOffEquip();
                        }
                        // �� ������ ���ϰ�
                        // ���õ� ������ �ֱ�
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
                Debug.Log("���ǵ��� ���� ���� ���Դϴ�. : MinerTeam-SetStaffMiner");
                break;
        }
        minerItem.InitSelectItem();
    }

    private void SetMinerEquip(EquipItem equipItem)
    {
        // ����� �� ::
        // ���� ������ ���� ��� ���Ե� �ο� �߿� ����
        // ���� ������ ���� ��� �̹� ���� ��� �ִ� �ο����� ����
        // ���� ���� ��� ���Ե� �ο� �߿� ����
        // ���� ���� ��� �������� �ο��� ����

        OnSetMinerEquip((minerId) =>
        {
            // �� ��� �̹� �����ϰ� �ִ� ���ΰ� ���� ��� 
            if (_minerEquips.ContainsKey(equipItem.EquipId))
            {
                // ��� ������ ���ο� ������ ���ΰ� ���� ���
                if (_minerEquips[equipItem.EquipId] == minerId)
                    return;

                // �ش� ��� �����ϰ� �ִ� ����� ��� ����
                int putOnMinerId = _minerEquips[equipItem.EquipId];

                // ���� ����
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
            // �ٸ� ��� �����ϰ� ���� ���
            if (miner.EquipId != -1)
            {
                // ��� ���� ��Ȳ���� ����
                _minerEquips.Remove(miner.EquipId);
                // ���� �� ����
                OnSetWear(miner.EquipId, false);
                // ��� ����
                miner.TakeOffEquip();
            }
            miner.PutOnEquip(_equips[equipItem.EquipId]);
            _minerEquips[equipItem.EquipId] = minerId;
            // ���� ����
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

        // �ش� ����, ���� ���� ����
        RecalculateHeadStat();
        RecalculateStaffStat(staff);
    }

    private void RecalculateHeadStat()
    {
        float figure;
        // �ش� ����, ���� ���� ����
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
