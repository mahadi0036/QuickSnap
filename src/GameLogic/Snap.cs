using System;
using SwinGameSDK;

#if DEBUG
using NUnit.Framework;
#endif 

namespace CardGames.GameLogic
{
    public class Snap
    {
        private readonly Card[] _topCards = new Card[2];
        private readonly Deck _deck;
        private readonly Timer _gameTimer;
        private int _flipTime = 1000;
        private int[] _score = new int[2];
        private bool _started = false;

        public Snap()
        {
            _deck = new Deck();
            _gameTimer = SwinGame.CreateTimer();
        }

        public Card TopCard
        {
            get { return _topCards[1]; }
        }

        public bool CardsRemain
        {
            get { return _deck.CardsRemaining > 0; }
        }

        public int FlipTime
        {
            get { return _flipTime; }
            set { _flipTime = value; }
        }

        public bool IsStarted
        {
            get { return _started; }
        }

        public void Start()
        {
            if (!IsStarted)
            {
                _started = true;
                _deck.Shuffle();
                FlipNextCard();
                _gameTimer.Start();
            }
        }

        public void FlipNextCard()
        {
            if (_deck.CardsRemaining > 0)
            {
                _topCards[0] = _topCards[1];
                _topCards[1] = _deck.Draw();
                _topCards[1].TurnOver();
            }
        }

        public void Update()
        {
            if (IsStarted && _gameTimer.Ticks > _flipTime)
            {
                _gameTimer.Reset();
                FlipNextCard();
            }
        }

        public int Score(int idx)
        {
            if (idx >= 0 && idx < _score.Length)
                return _score[idx];
            return 0;
        }

        public void PlayerHit(int player)
        {
            if (player >= 0 && player < _score.Length && IsStarted)
            {
                if (_topCards[0] != null && _topCards[0].Rank == _topCards[1].Rank)
                {
                    _score[player]++;
                }
                else
                {
                    _score[player] = Math.Max(0, _score[player] - 1);
                }
                _started = false;
                _gameTimer.Stop();
            }
        }

        #region Snap Game Unit Tests
        #if DEBUG
        public class SnapTests
        {
            [Test]
            public void TestSnapCreation()
            {
                Snap s = new Snap();
                Assert.IsTrue(s.CardsRemain);
                Assert.IsNull(s.TopCard);
            }

            [Test]
            public void TestFlipNextCard()
            {
                Snap s = new Snap();
                Assert.IsTrue(s.CardsRemain);
                Assert.IsNull(s.TopCard);
                s.FlipNextCard();
                Assert.IsNull(s._topCards[0]);
                Assert.IsNotNull(s._topCards[1]);
            }
        }
        #endif
        #endregion
    }
}