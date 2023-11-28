using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MineTable : ScriptableObject
{
	public List<MineEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
