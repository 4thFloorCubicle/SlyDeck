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

enum GameState
{
    MainMenu,
    Tutorial,
    Game,
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState state;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter
            .DefaultAdapter
            .CurrentDisplayMode
            .Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter
            .DefaultAdapter
            .CurrentDisplayMode
            .Height;
        _graphics.ApplyChanges();

        state = GameState.MainMenu;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //load assets into AssetManager
        AssetManager.Instance.AddFont("Arial24", Content.Load<SpriteFont>("Arial24"));
        AssetManager.Instance.AddFont("Arial18", Content.Load<SpriteFont>("Arial18"));
        AssetManager.Instance.AddFont("Arial12", Content.Load<SpriteFont>("Arial12"));
        AssetManager.Instance.AddFont("TitleFont", Content.Load<SpriteFont>("TitleFont"));
        AssetManager.Instance.AddFont("SubTitleFont", Content.Load<SpriteFont>("SubTitleFont"));
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
        AssetManager.Instance.AddDeckFilePath("PlayerDeck", "Content\\PlaytestDeckCards.deck");

        Deck deck = DeckManager.Instance.DeckFromFile(
            AssetManager.Instance.GetDeckFilePath("PlayerDeck")
        );
        
        AssetManager.Instance.AddDeck("pDeck", deck);

        Deck eDeck = deck;

        Board gameBoard = new(
            new Vector2(0, 0),
            "Testboard",
            deck,
            "Bob",
            eDeck,
            GraphicsDevice,
            this
        );
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Instance.RefreshInput();

        //check for button presses to switch states
        //main menu -> game
        if (state == GameState.MainMenu && InputManager.Instance.CheckKeyDown(Keys.Enter))
        {
            state = GameState.Game;
        }

        //main menu -> tutorial
        if (state == GameState.MainMenu && InputManager.Instance.CheckKeyDown(Keys.T))
        {
            state = GameState.Tutorial;
        }

        //return to main menu from the tutorial or game screen
        if (
            (state == GameState.Game || state == GameState.Tutorial)
            && InputManager.Instance.CheckKeyDown(Keys.Q)
        )
        {
            state = GameState.MainMenu;
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

        switch (state)
        {
            case GameState.MainMenu:
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("TitleFont"),
                    "SlyDeck",
                    new(
                        (GraphicsDevice.Viewport.Width / 2) - 250,
                        GraphicsDevice.Viewport.Height / 4
                    ),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("SubTitleFont"),
                    "Press Enter to play",
                    new(
                        (GraphicsDevice.Viewport.Width / 2) - 300,
                        GraphicsDevice.Viewport.Height / 2
                    ),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("SubTitleFont"),
                    "Press T to view the Tutorial",
                    new(
                        (GraphicsDevice.Viewport.Width / 2) - 450,
                        (GraphicsDevice.Viewport.Height / 2) + 200
                    ),
                    Color.White
                );
                break;
            case GameState.Tutorial:
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "Press Z, X, or C or click a card to play it",
                    new(100, 100),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "The number on the bottom right of the card is the amount of persuasion playing it will allow you to gain",
                    new(100, 200),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "The bottom left value is your total score, if it is greater than the score of your opponent after playing 5 cards, you win the round!",
                    new(100, 300),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "Each card (besides basic ones) have an ability that can enhance the amount of points you gain.",
                    new(100, 400),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "For example, an effect can double the amount of persuasion gained, or subtract persuasion from the enemy.",
                    new(100, 450),
                    Color.White
                );
                _spriteBatch.DrawString(
                    AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                    "Press Q to return to the main menu. You can also press Q in the game state to return to the main menu at any time",
                    new(100, 550),
                    Color.White
                );
                break;
            case GameState.Game:
                GameObjectManager.Instance.DrawAll(_spriteBatch);
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
