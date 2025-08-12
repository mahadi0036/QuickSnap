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


		/// <summary>
		/// Create a new game of Snap!
		/// </summary>
		public Snap ()
		{
			_deck = new Deck ();
		    _gameTimer = SwinGame.CreateTimer ();
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


		/// <summary>
		/// Start the Snap game playing!
		/// </summary>
		public void Start()
		{
			if ( ! IsStarted )			// only start if not already started!
			{
				_started = true;
				
				_deck.Shuffle ();		// Return the cards and shuffle

				FlipNextCard ();
				_gameTimer.Start(); 		// Flip the first card...
						// Flip the first card...
			}
		}
			
		public void FlipNextCard()
		{
			if (_deck.CardsRemaining > 0)			// have cards...
			{
				_topCards [0] = _topCards [1];		// move top to card 2
				_topCards [1] = _deck.Draw ();		// get a new top card
				_topCards[1].TurnOver();			// reveal card
			}
		}

		/// <summary>
		/// Update the game. This should be called in the Game loop to enable
		/// the game to update its internal state.
		/// </summary>
		public void Update()
		{
			if (_gameTimer.Ticks > _flipTime)  // যদি সময় বেশি হয়ে যায়
             {
                _gameTimer.Reset();
                FlipNextCard();
             }//TODO: implement update to automatically slip cards!
		}

		/// <summary>
		/// Gets the player's score.
		/// </summary>
		/// <value>The score.</value>
		public int Score(int idx)
		{
			if ( idx >= 0 && idx < _score.Length )
				return _score[idx]; 
			else
				return 0;
		}

		/// <summary>
		/// The player hit the top of the cards "snap"! :)
		/// Check if the top two cards' ranks match.
		/// </summary>
		public void PlayerHit (int player)
		{
			//TODO: consider deducting score for miss hits???
			if ( player >= 0 && player < _score.Length &&  	// its a valid player
				 IsStarted && 								// and the game is started
				 _topCards [0] != null && _topCards [0].Rank == _topCards [1].Rank) // and its a match
			{
				_score[player]++;
				//TODO: consider playing a sound here...
			}

			// stop the game...
			_started = false;
			_gameTimer.Stop();
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
				Assert.IsNull (s.TopCard);
			}

			[Test]
			public void TestFlipNextCard()
			{
				Snap s = new Snap();

				Assert.IsTrue(s.CardsRemain);
				Assert.IsNull (s.TopCard);

				s.FlipNextCard ();

				Assert.IsNull (s._topCards [0]);
				Assert.IsNotNull (s._topCards [1]);
			}
		}

		#endif 
		#endregion
	}
}

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
    }
}