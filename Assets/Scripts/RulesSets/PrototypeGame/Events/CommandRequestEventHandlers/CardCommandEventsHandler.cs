﻿using PrototypeGame.Events.CommandRequestEvents;
using PrototypeGame.Logic.Components.Cards;
using PrototypeGame.Logic.State.Cards;
using System;


namespace PrototypeGame.Events.CommandRequestEventHandlers
{
	internal class CardCommandEventsHandler : IDisposable
	{
		private LogicCardStateManager _logicCardStateManager;
		private CardCommandRequestEvents _cardCommandRequestEvents;

		private SceneCardEvents _sceneCardEvents;

		public CardCommandEventsHandler(LogicCardStateManager logicCardStateManager, CardCommandRequestEvents cardCommandRequestEvents, SceneCardEvents sceneCardEvents) 
		{
			_logicCardStateManager = logicCardStateManager;
			_cardCommandRequestEvents = cardCommandRequestEvents;
			_sceneCardEvents = sceneCardEvents;

			_cardCommandRequestEvents.PlayCardActionRequestEvent += OnPlayCardActionRequest;

			_cardCommandRequestEvents.MoveCardFromHandToDiscardRequestEvent += OnMoveCardFromHandToDiscardRequest;
			_cardCommandRequestEvents.MoveCardFromDiscardToHandRequestEvent += OnMoveCardFromDiscardToHandRequest;

			_cardCommandRequestEvents.ReorganizeCardsInHandRequestEvent += OnReorganizeCardsInHandRequest;
		}

		public void Dispose()
		{
			_cardCommandRequestEvents.PlayCardActionRequestEvent -= OnPlayCardActionRequest;

			_cardCommandRequestEvents.MoveCardFromHandToDiscardRequestEvent -= OnMoveCardFromHandToDiscardRequest;
			_cardCommandRequestEvents.MoveCardFromDiscardToHandRequestEvent -= OnMoveCardFromDiscardToHandRequest;

			_cardCommandRequestEvents.ReorganizeCardsInHandRequestEvent -= OnReorganizeCardsInHandRequest;
		}

		private void OnPlayCardActionRequest(Guid cardId)
		{
			_logicCardStateManager.PlayActionFromCard(cardId);
		}

		private void OnMoveCardFromHandToDiscardRequest(Guid cardId, bool fromUndo)
		{
			ProtoCardData card = _logicCardStateManager.LogicCardState.CardsInHand[cardId];
			_logicCardStateManager.RemoveCardFromHand(card);
			_logicCardStateManager.AddCardToDiscardPile(card);
			_sceneCardEvents.RaiseCardRemovedFromHandEvent(cardId, fromUndo);
		}

		private void OnMoveCardFromDiscardToHandRequest(Guid cardId, bool fromUndo)
		{
			ProtoCardData card = _logicCardStateManager.LogicCardState.CardsInDiscard[cardId];
			_logicCardStateManager.MoveCardFromDiscardPileToHand(card);
			_sceneCardEvents.RaiseCardAddedToHandEvent(card, fromUndo);
		}


		private void OnReorganizeCardsInHandRequest()
		{
			_sceneCardEvents.RaiseCardsInHandReorganizedEvent();
		}

	}
}
