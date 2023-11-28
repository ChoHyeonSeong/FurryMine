using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class PriceTable : ScriptableObject
{
	public List<PriceEntity> Table; // Replace 'EntityType' to an actual type that is serializable.
}
