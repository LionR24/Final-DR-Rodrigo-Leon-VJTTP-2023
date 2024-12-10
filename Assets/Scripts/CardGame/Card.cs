using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
	public bool hasBeenPlayed;
	public int handIndex;

	GameManager gm;

	private Animator anim;
	private Animator camAnim;

	public GameObject effect;
	public GameObject hollowCircle;

	
	[SerializeField] TextMeshProUGUI _attack;
	//[SerializeField] TextMeshProUGUI _defense;
	[SerializeField] int _attackValue;

	[SerializeField] Unit _myOwner;
	public Unit MyOwner { get { return _myOwner; } set { _myOwner = value; } }
	
	
	public GameObject myArtRepresentation;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();
		camAnim = Camera.main.GetComponent<Animator>();
	}
	private void OnMouseDown()
	{
		if (!_myOwner.HasInputAuthority) return;

		if(gm.IsTurnPlayer(_myOwner))
        {
			if(!gm.HasTurnPlayerPlayedCard())
            {
				if (!hasBeenPlayed)
				{
					Instantiate(hollowCircle, transform.position, Quaternion.identity);

					camAnim.SetTrigger("shake");
					anim.SetTrigger("move");

					transform.position += Vector3.up * 2f;

					hasBeenPlayed = true;
					_myOwner.myEyes.availableCardSlots[handIndex] = true;
					Invoke("MoveToDiscardPile", 2f);

					gm.CardDealsDamage(_attackValue, MyOwner);
				}
			}
        }
	}

	void MoveToDiscardPile()
	{
		Instantiate(effect, transform.position, Quaternion.identity);
		//gm.discardPile.Add(this);
		_myOwner.myEyes.AddToDiscard(this);
		gameObject.SetActive(false);
	}

}
