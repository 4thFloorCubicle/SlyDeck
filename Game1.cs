using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlyDeck.GameObjects;
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
        AssetManager.Instance.AddTexture("QueenOfSpades", Content.Load<Texture2D>("QueenOfSpades"));
        AssetManager.Instance.AddTexture("testButton", Content.Load<Texture2D>("testButton"));
        AssetManager.Instance.AddTexture("CardDraft", Content.Load<Texture2D>("CardDraft"));
        AssetManager.Instance.AddTexture("blankSlide", Content.Load<Texture2D>("blankSlide"));
        AssetManager.Instance.AddTexture("Header", Content.Load<Texture2D>("cardImages\\Header"));
        AssetManager.Instance.AddTexture("Footer", Content.Load<Texture2D>("cardImages\\Footer"));
        AssetManager.Instance.AddTexture("Quote", Content.Load<Texture2D>("cardImages\\Quote"));
        AssetManager.Instance.AddTexture("Graph", Content.Load<Texture2D>("cardImages\\Graph"));
        AssetManager.Instance.AddTexture("Closer", Content.Load<Texture2D>("cardImages\\Closer"));
        AssetManager.Instance.AddDeckFilePath("TestDeck", "Content\\TestDeckCards.deck");

        Label testLabel = new Label(
            new Vector2(100, 100),
            "Test Label",
            "Hi!",
            AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
        );

        Card testCard = new Card(
            new Vector2(200, 200),
            "Blank Slide",
            AssetManager.Instance.GetAsset<Texture2D>("CardDraft"),
            "This card has a test effect",
            2,
            CardType.Header,
            AssetManager.Instance.GetAsset<Texture2D>("blankSlide")
        );

        TestEffect testEffect = new TestEffect();
        AdditivePowerEffect add2 = new AdditivePowerEffect(2, PowerType.EffectPower);
        AttacherEffect add2Attacher = new AttacherEffect(add2, TargetMode.Self);

        testCard.AddEffect(testEffect);
        testCard.AddEffect(add2Attacher);

        DeckManager.Instance.DeckFromFile(AssetManager.Instance.GetDeckFilePath("TestDeck"));
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

        if (InputManager.Instance.SingleKeyPress(Keys.Space))
        {
            Card card = (Card)GameObjectManager.Instance.GetGameObject("Blank Slide");
            card.Toggle();
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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        GameObjectManager.Instance.DrawAll(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
