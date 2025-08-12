using System;
using SwinGameSDK;
using CardGames.GameLogic;

namespace CardGames
{
    public class SnapGame
    {
        public static void LoadResources()
        {
            Bitmap cards;

            cards = SwinGame.LoadBitmapNamed ("Cards", "Cards.png");
            SwinGame.BitmapSetCellDetails (cards, 82, 110, 13, 5, 53);      // set the cells in the bitmap to match the cards
			SwinGame.LoadFontNamed ("GameFont", " ChunkFive-Regular.otf", 12);

            cards = SwinGame.LoadBitmapNamed("Cards", "Cards.png");
            SwinGame.BitmapSetCellDetails(cards, 82, 110, 13, 5, 53);

        }

        private static void HandleUserInput(Snap myGame)
        {
            SwinGame.ProcessEvents();


			if (SwinGame.KeyTyped (KeyCode.vk_SPACE))
			{
				myGame.Start();
			}
		}

            if (SwinGame.KeyTyped(KeyCode.vk_SPACE))
            {
                if (!myGame.IsStarted)
                    myGame.Start();
                else
                    myGame.FlipNextCard();
            }


            if (myGame.IsStarted)
            {
                if (SwinGame.KeyTyped(KeyCode.vk_LSHIFT) && 
                    SwinGame.KeyTyped(KeyCode.vk_RSHIFT))
                {
                    // TODO: Add sound effect
                }
                else if (SwinGame.KeyTyped(KeyCode.vk_LSHIFT))
                {
                    myGame.PlayerHit(0);
                }
                else if (SwinGame.KeyTyped(KeyCode.vk_RSHIFT))
                {
                    myGame.PlayerHit(1);
                }
            }
        }

        private static void DrawGame(Snap myGame)
        {
            SwinGame.ClearScreen(Color.White);

            Card top = myGame.TopCard;
            if (top != null)
            {
                SwinGame.DrawText("Top Card: " + top.ToString(), Color.RoyalBlue, 0, 20);
                SwinGame.DrawText("Player 1: " + myGame.Score(0), Color.RoyalBlue, 0, 30);
                SwinGame.DrawText("Player 2: " + myGame.Score(1), Color.RoyalBlue, 0, 40);
                SwinGame.DrawCell(SwinGame.BitmapNamed("Cards"), top.CardIndex, 350, 50);
            }
            else
            {
                SwinGame.DrawText("Press SPACE to start!", Color.RoyalBlue, 0, 20);
            }

            SwinGame.DrawCell(SwinGame.BitmapNamed("Cards"), 52, 160, 50);
            SwinGame.RefreshScreen(60);
        }

        private static void UpdateGame(Snap myGame)
        {
            myGame.Update();
        }

        public static void Main()
        {
            SwinGame.OpenGraphicsWindow("Snap!", 860, 500);
            LoadResources();
            Snap myGame = new Snap();

            while (!SwinGame.WindowCloseRequested())
            {
                HandleUserInput(myGame);
                DrawGame(myGame);
                UpdateGame(myGame);
            }
        }
    }
}