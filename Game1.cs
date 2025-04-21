using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlyDeck.Decks;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.Boards;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

namespace SlyDeck;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1920; // testing width
        //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
        _graphics.PreferredBackBufferHeight =
            //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            1080; // testing height
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //load assets into AssetManager
        AssetManager.Instance.AddFont("Arial24", Content.Load<SpriteFont>("Arial24"));
        AssetManager.Instance.AddFont("Arial12", Content.Load<SpriteFont>("Arial12"));
        AssetManager.Instance.AddTexture("TempCardBack", Content.Load<Texture2D>("TempCardBack"));
        AssetManager.Instance.AddTexture("QueenOfSpades", Content.Load<Texture2D>("QueenOfSpades"));
        AssetManager.Instance.AddTexture("testButton", Content.Load<Texture2D>("testButton"));
        AssetManager.Instance.AddTexture("CardDraft", Content.Load<Texture2D>("CardDraft"));
        AssetManager.Instance.AddTexture("blankSlide", Content.Load<Texture2D>("blankSlide"));
        AssetManager.Instance.AddTexture("Header", Content.Load<Texture2D>("cardImages\\Header"));
        AssetManager.Instance.AddTexture("Footer", Content.Load<Texture2D>("cardImages\\Footer"));
        AssetManager.Instance.AddTexture("Quote", Content.Load<Texture2D>("cardImages\\Quote"));
        AssetManager.Instance.AddTexture("Graph", Content.Load<Texture2D>("cardImages\\Graph"));
        AssetManager.Instance.AddTexture("Closer", Content.Load<Texture2D>("cardImages\\Closer"));
        AssetManager.Instance.AddDeckFilePath("PlayerDeck", "Content\\TestDeckCards.deck");

        Deck deck = DeckManager.Instance.DeckFromFile(
            AssetManager.Instance.GetDeckFilePath("PlayerDeck")
        );

        AssetManager.Instance.AddDeck("pDeck", deck);

        Deck eDeck = deck;

        Board gameBoard = new(new Vector2(0, 0), "Testboard", deck, "Bob", eDeck, GraphicsDevice, this);
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Instance.RefreshInput();

        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
        {
            Exit();
        }

        // check for left click events
        if (InputManager.Instance.SingleMousePress(MouseButton.Left))
        {
            foreach (GameObject gameObject in GameObjectManager.Instance.GetAllGameObjects())
            {
                IClickable clickable = gameObject as IClickable;

                // check if a gameobject is enabled, is a clickable, and if mouse is within the bounds of the clickable
                if (
                    gameObject.Enabled
                    && clickable != null
                    && clickable.Bounds.Contains(Mouse.GetState().Position)
                )
                {
                    clickable.OnLeftClick();
                    break;
                }
            }
        }

        // Question: does it make more sense to loop through it once and conditionally apply logic? or just add loops for each step
        foreach (GameObject gameObject in GameObjectManager.Instance.GetAllGameObjects())
        {
            gameObject.Update(gameTime);
        }



        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.BurlyWood);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack);

        GameObjectManager.Instance.DrawAll(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
