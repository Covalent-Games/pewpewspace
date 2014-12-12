using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrapPile {

	#region Stored Values
	public GameObject Owner;
	int low;
	int medium;
	int high;
	public int PreviousScrapGained;
	#endregion

	public int QualityLow {
		get {
			return low;
		}
		set {
			//TODO: Move this repeat logic into a single method. (if possible?)
			if (value < 0) {
				Debug.LogError(string.Format(
					"Attempting to remove more scrap ({0}) than is available ({1})!", value, low));
			} else {
				low = value;
			}
		}
	}
	public int QualityMedium {
		get {
			return medium;
		}
		set {
			if (value < 0) {
				Debug.LogError(string.Format(
					"Attempting to remove more scrap ({0}) than is available ({1})!", value, medium));
			} else {
				medium = value;
			}
		}
	}
	public int QualityHigh {
		get {
			return high;
		}
		set {
			if (value < 0) {
				Debug.LogError(string.Format(
					"Attempting to remove more scrap ({0}) than is available ({1})!", value, high));
			} else {
				high = value;
			}
		}
	}

	/// <summary>
	/// Adds the specified amount of scrap to the correct quality hoard.
	/// Can be overloaded with a specific quality and amount.
	/// </summary>
	/// <param name="scrap">A ScrapObject instance</param>
	public void AddScrap(ScrapObject scrap) {

		// Allocate scrap based on quality rating.
		switch (scrap.Quality) { 
			case ScrapObject.QualityRating.Low:
				QualityLow += scrap.Quantity;
				break;
			case ScrapObject.QualityRating.Medium:
				QualityMedium += scrap.Quantity;
				break;
			case ScrapObject.QualityRating.High:
				QualityHigh += scrap.Quantity;
				break;
		}
		PreviousScrapGained = scrap.Quantity;
	}

	/// <summary>
	/// Adds the specified amount of scrap to the correct quality hoard.
	/// Can be overloaded byt using a ScrapObject
	/// </summary>
	/// <param name="rating">The quality of the scrap.</param>
	/// <param name="amount">The amount of scrap to be added.</param>
	public void AddScrap(ScrapObject.QualityRating rating, int amount) {

		// Allocate scrap based on quality rating.
		switch (rating) {
			case ScrapObject.QualityRating.Low:
				QualityLow += amount;
				break;
			case ScrapObject.QualityRating.Medium:
				QualityMedium += amount;
				break;
			case ScrapObject.QualityRating.High:
				QualityHigh += amount;
				break;
		}
		PreviousScrapGained = amount;
	}

	/// <summary>
	/// Attempts to remove the specified scrap from the scrap pile, succeeding
	/// only if the required amount is available.
	/// </summary>
	/// <param name="rating">The quality of the scrap.</param>
	/// <param name="amount">The amount of scrap to be removed.</param>
	/// <returns>True is the scrap was removed, otherwise false.</returns>
	public bool RemoveScrap(ScrapObject.QualityRating rating, int amount) {

		bool success = false;

		//TODO YUCK!?!? Surely there's a cleaner way??? BLARGH
		switch (rating) {
			case ScrapObject.QualityRating.Low:
				if (QualityLow - amount >= 0) {
					QualityLow -= amount;
					success = true;
				}
				break;
			case ScrapObject.QualityRating.Medium:
				if (QualityMedium - amount >= 0) {
					QualityMedium -= amount;
					success = true;
				}
				break;
			case ScrapObject.QualityRating.High:
				if (QualityHigh - amount >= 0) {
					QualityHigh -= amount;
					success = true;
				}
				break;
		}
		return success;
	}
}
