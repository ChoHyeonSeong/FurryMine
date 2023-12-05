using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class EquipTable : ScriptableObject
{
	public List<EquipEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
