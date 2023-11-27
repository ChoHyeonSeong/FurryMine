using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class MineData : ScriptableObject
{
	public List<MineEntity> Data; // Replace 'EntityType' to an actual type that is serializable.
}
