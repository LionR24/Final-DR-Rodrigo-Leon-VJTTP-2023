using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Random = UnityEngine.Random;
using System;
using System.Linq;


public class UIManager : MonoBehaviour
{
	public List<Card> deck;
	public TextMeshProUGUI deckSizeText;

	public Transform[] cardSlots;
	public bool[] availableCardSlots;

	public List<Card> discardPile;
	public TextMeshProUGUI discardPileSizeText;

	public List<Card> deck2;
	public TextMeshProUGUI deckSizeText2;

	public Transform[] cardSlots2;
	public bool[] availableCardSlots2;

	public List<Card> discardPile2;
	public TextMeshProUGUI discardPileSizeText2;

	List<GameObject> cardsInOpponentsHand;

	private Animator camAnim;

	public TextMeshProUGUI myHealth;
	public TextMeshProUGUI myOpponentHealth;

	[SerializeField] Button _exit;
	[SerializeField] TextMeshProUGUI _victory;
	[SerializeField] TextMeshProUGUI _defeat;
	

	public Card DrawCard()
	{
		if (deck.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			Card randomCard = deck[Random.Range(0, deck.Count)];
			for (int i = 0; i < availableCardSlots.Length; i++)
			{
				if (availableCardSlots[i] == true)
				{
					randomCard.gameObject.SetActive(true);
					randomCard.handIndex = i;
					randomCard.transform.position = cardSlots[i].position;
					randomCard.hasBeenPlayed = false;
					deck.Remove(randomCard);
					availableCardSlots[i] = false;
					return randomCard;
				}
			}
		}

		return default;
	}

	public void DrawCard2(Card randomCard)
	{
		
		if (deck2.Count >= 1)
		{
			camAnim.SetTrigger("shake");

			
			for (int i = 0; i < availableCardSlots2.Length; i++)
			{
				if (availableCardSlots2[i] == true)
				{
					
					GameObject xc = Instantiate(randomCard.myArtRepresentation, cardSlots2[i].position, cardSlots2[i].rotation );
					
					deck2.Remove(randomCard);
					availableCardSlots2[i] = false;
					cardsInOpponentsHand.Add(xc);
					return;
				}
			}
		}
	}

	public void MoveCard2(Card clickedCard)
    {
		Instantiate(clickedCard.hollowCircle, clickedCard.transform.position, clickedCard.transform.rotation);

		GameObject xc=cardsInOpponentsHand.Where(x=>x==clickedCard.myArtRepresentation).First();
		Animator anim = xc.gameObject.GetComponent<Animator>();

		camAnim.SetTrigger("shake");
		anim.SetTrigger("move");

		transform.position += Vector3.down * 2f;

		availableCardSlots2[clickedCard.handIndex] = true;
	}

	public void DiscardCard2(Card discardedCard)
    {
		Instantiate(discardedCard.effect, discardedCard.transform.position, discardedCard.transform.rotation);

		GameObject xc = cardsInOpponentsHand.Where(x => x == discardedCard.myArtRepresentation).First();
		
		cardsInOpponentsHand.Remove(xc);
		Destroy(xc);
	}

	public void Shuffle()
	{
		if (discardPile.Count >= 1)
		{
			foreach (Card card in discardPile)
			{
				deck.Add(card);
			}
			discardPile.Clear();
		}
	}

	public void Shuffle2()
	{
		if (discardPile2.Count >= 1)
		{
			foreach (Card card in discardPile2)
			{
				deck2.Add(card);
			}
			discardPile2.Clear();
		}
	}

	public void AddToDiscard(Card toDiscard)
	{
		discardPile.Add(toDiscard);
	}

	public void UpdateHealth(int n)
    {
		myHealth.text = n.ToString();
    }

	public void UpdateOpponentHealth(int n)
    {
		myOpponentHealth.text = n.ToString();
	}

	public bool CheckHand()
    {
		if (availableCardSlots.Any(x => x == true)) return true;

		return false;
    }

	public void SetDeck2(List<Card> n)
    {
		foreach(Card item in n)
        {
			deck2.Add(item);
        }
    }

	public void OnButtonExit()
    {
		Application.Quit();
    }

	public void Loose()
    {
		_defeat.gameObject.SetActive(true);
		_exit.gameObject.SetActive(true);

    }

	public void Victory()
	{
		_victory.gameObject.SetActive(true);
		_exit.gameObject.SetActive(true);

	}

	private void Update()
	{
		deckSizeText.text = deck.Count.ToString();
		discardPileSizeText.text = discardPile.Count.ToString();
		deckSizeText2.text = deck2.Count.ToString();
		discardPileSizeText2.text = discardPile2.Count.ToString();
	}
}
