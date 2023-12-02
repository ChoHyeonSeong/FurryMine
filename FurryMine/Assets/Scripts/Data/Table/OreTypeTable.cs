using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class OreTypeTable : ScriptableObject
{
	public List<OreTypeEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
