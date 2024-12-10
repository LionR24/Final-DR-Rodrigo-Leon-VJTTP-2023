using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Unit : NetworkBehaviour
{
	public int maxHP;
	public int currentHP;

	public UIManager myEyes;

	public List<Card> myDeck;
	[HideInInspector] public bool hasDrawnThisTurn;
	[HideInInspector] public bool hasPlayedACard;

	public bool TakeDamage(int dmg)
	{
		currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public bool CheckIfHandFull()
    {
		if (myEyes.CheckHand())return false;

		return true;
    }

	public bool CheckIfEmpty()
    {
		if (myEyes.deck.Count < 1) return true;

		return false;
    }

}
