using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrapObject {

	public enum QualityRating {
		Low,
		Medium,
		High,
	}

	public int Quantity;
	public QualityRating Quality;

	public ScrapObject(ScrapObject.QualityRating quality, int quantity) {

		Quantity = quantity;
		Quality = quality;
	}
}
