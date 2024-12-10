using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using System;
using System.Linq;



public class GameManager : NetworkBehaviour
{
	public static GameManager instance;

	public Transform[] cardSlots;
	public Transform[] cardSlots2;

	private Animator camAnim;

	[SerializeField] Unit _player1;
	[SerializeField] Unit _player2;
	Unit _turnPlayer;

	[SerializeField] Button _startButton;

	public enum DuelStates { IDLE, START, PLAYERTURN, ENEMYTURN, END};
	EventFiniteStateMachine<DuelStates> _fsm;

	NetworkRunner runner;

	private void Start()
	{
		instance = this;
		camAnim = Camera.main.GetComponent<Animator>();

		var idle = new EventStates<DuelStates>("Idle");
		var start = new EventStates<DuelStates>("Start");
		var playerTurn = new EventStates<DuelStates>("PlayerTurn");
		var enemyTurn = new EventStates<DuelStates>("EnemyTurn");
		var end = new EventStates<DuelStates>("End");

		EventStateConfigurer.Create(idle)
			.SetTransition(DuelStates.START, start)
			.Done();	
		EventStateConfigurer.Create(start)
			.SetTransition(DuelStates.PLAYERTURN, playerTurn)
			.SetTransition(DuelStates.ENEMYTURN, enemyTurn)
			.Done();
		EventStateConfigurer.Create(playerTurn)
			.SetTransition(DuelStates.ENEMYTURN, enemyTurn)
			.SetTransition(DuelStates.END, end)
			.Done();
		EventStateConfigurer.Create(enemyTurn)
			.SetTransition(DuelStates.PLAYERTURN, playerTurn)
			.SetTransition(DuelStates.END, end)
			.Done();
		EventStateConfigurer.Create(end)
			.Done();

		idle.OnUpdate += () =>
		  {
			  if (runner.ActivePlayers.Count() >= 2) SendInputToFSM(DuelStates.START);
		  };
		start.OnEnter += x =>
		  {
			  RPCBegginingOfDuel();
			  SendInputToFSM(DuelStates.PLAYERTURN);
		  };

		playerTurn.OnEnter += x =>
		  {
			  if(_player1.CheckIfEmpty())
              {
				  _player1.myEyes.Shuffle();
				  _player2.myEyes.Shuffle2();
              }

			  if(!_player1.CheckIfHandFull())
              {
				  _player2.myEyes.DrawCard2(_player1.myEyes.DrawCard());
			  }
			  _player1.hasPlayedACard = false;
			  _turnPlayer = _player1;
		  };
		enemyTurn.OnEnter += x =>
		 {
			 if (_player2.CheckIfEmpty())
			 {
				 _player2.myEyes.Shuffle();
				 _player1.myEyes.Shuffle2();
			 }

			 if (!_player2.CheckIfHandFull())
			 {
				 _player1.myEyes.DrawCard2(_player2.myEyes.DrawCard());
			 }
			 _player2.hasPlayedACard = false;
			 _turnPlayer = _player2;
		 };

		_fsm = new EventFiniteStateMachine<DuelStates>(idle);
	}

	public bool IsTurnPlayer(Unit pRef)
    {
		if (pRef == _turnPlayer) return true;

		return false;
    }

	public bool HasTurnPlayerPlayedCard()
    {
		if (_turnPlayer.hasPlayedACard == false) return false;

		return true;
    }

	void SendInputToFSM(DuelStates inp)
	{
		_fsm.SendInput(inp);
	}

	void AssignOwnersToCards()
    {
		foreach(Card item in _player1.myDeck)
        {
			item.MyOwner = _player1;
        }
		foreach (Card item in _player2.myDeck)
		{
			item.MyOwner = _player2;
		}
	}

	public void AddPlayer(Unit pTAdd)
    {
		RPCAddPlayer(pTAdd);
    }

	[Rpc(RpcSources.All,RpcTargets.All)]
	void RPCAddPlayer(Unit pTAdd)
    {
		if (_player1 == null)
		{
			_player1 = pTAdd;
			//for(int i=0;i<4; i++)
			//{
			//	_player1.myEyes.cardSlots[i] = cardSlots[i];
   //         }

			//for (int i = 0; i < 4; i++)
			//{
			//	_player1.myEyes.cardSlots2[i] = cardSlots2[i];
			//}
			//_player1.myEyes.cardSlots = cardSlots;
			//_player1.myEyes.cardSlots2 = cardSlots2;
			
			return;
		}
		else if (_player2 == null)
		{
			_player2 = pTAdd;
			//for (int i = 0; i < 4; i++)
			//{
			//	_player2.myEyes.cardSlots[i] = cardSlots[i];
			//}

			//for (int i = 0; i < 4; i++)
			//{
			//	_player2.myEyes.cardSlots2[i] = cardSlots2[i];
			//}
			//_player2.myEyes.cardSlots = cardSlots;
			//_player2.myEyes.cardSlots2 = cardSlots2;
			_player1.myEyes.SetDeck2(_player2.myDeck);
			_player2.myEyes.SetDeck2(_player1.myDeck);
			_player1.myEyes.UpdateHealth(_player1.currentHP);
			_player2.myEyes.UpdateHealth(_player2.currentHP);
			_player1.myEyes.UpdateOpponentHealth(_player2.currentHP);
			_player2.myEyes.UpdateOpponentHealth(_player1.currentHP);
			return;
		}
	}

	public void CardDealsDamage(int n, Unit source)
    {
		RPCCardDealsDamage(n, source);
    }

	[Rpc(RpcSources.All, RpcTargets.All)]
	void RPCCardDealsDamage(int n, Unit source)
    {
		if (source == _player1)
		{
			_player1.hasPlayedACard = true;
			if (_player2.TakeDamage(n))
			{
				_player2.myEyes.UpdateHealth(_player2.currentHP);
				_player1.myEyes.UpdateOpponentHealth(_player2.currentHP);
				_player2.myEyes.Loose();
				_player1.myEyes.Victory();
				SendInputToFSM(DuelStates.END);
				return;
			}
			_player2.myEyes.UpdateHealth(_player2.currentHP);
			_player1.myEyes.UpdateOpponentHealth(_player2.currentHP);
			return;
		}
		else if(source==_player2)
        {
			_player2.hasPlayedACard = true;
			if (_player1.TakeDamage(n))
			{	
				_player1.myEyes.UpdateHealth(_player1.currentHP);
				_player2.myEyes.UpdateOpponentHealth(_player1.currentHP);
				_player1.myEyes.Loose();
				_player2.myEyes.Victory();
				SendInputToFSM(DuelStates.END);
				return;
			}
			_player1.myEyes.UpdateHealth(_player1.currentHP);
			_player2.myEyes.UpdateOpponentHealth(_player1.currentHP);
			return;
		}
        
	}

	public void ChangeTurn(Unit source)
    {
		RPCChangeTurn(source);
    }

	[Rpc(RpcSources.All, RpcTargets.All)]
	void RPCChangeTurn(Unit source)
    {
		if(source==_player1)
        {
			SendInputToFSM(DuelStates.ENEMYTURN);
			return;
        }
		else
        {
			SendInputToFSM(DuelStates.PLAYERTURN);
			return;
        }
    }
	[Rpc(RpcSources.All, RpcTargets.All)]
	void RPCBegginingOfDuel()
    {
		StartCoroutine(BeginningOfDuel());
    }

	IEnumerator BeginningOfDuel()
    {
		_player1.myEyes.Shuffle();
		_player2.myEyes.Shuffle();
		yield return new WaitForSeconds(1);

		for (int i = 0; i < 5; i++)
		{
			_player2.myEyes.DrawCard2(_player1.myEyes.DrawCard());
			_player1.myEyes.DrawCard2(_player2.myEyes.DrawCard());
			yield return new WaitForSeconds(0.5f);
		}

		
	}
}
