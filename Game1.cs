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
        _graphics.PreferredBackBufferWidth =
            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
        _graphics.PreferredBackBufferHeight =
            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //load assets into AssetManager
        AssetManager.Instance.AddFont("Arial24", Content.Load<SpriteFont>("Arial24"));
        AssetManager.Instance.AddTexture("QueenOfSpades", Content.Load<Texture2D>("QueenOfSpades"));
        AssetManager.Instance.AddTexture(
            "testButton",
            Content.Load<Texture2D>("testButton")
        );

        Label testLabel = new Label(
            new Vector2(100, 100),
            "Test Label",
            "Hi!",
            AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
        );

        Card testCard = new Card(
            new Vector2(200, 200),
            "Test Card",
            AssetManager.Instance.GetAsset<Texture2D>("QueenOfSpades"),
            "This card has a test effect",
            2,
            CardType.Title
        );
        testCard.LeftClick += testLabel.Toggle;

        Button testButton = new Button(
            new Vector2(300, 100),
            "Test Button",
            "Test Button",
            AssetManager.Instance.GetAsset<Texture2D>("testButton"),
            AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
        );
        testButton.LeftClick += testCard.Toggle;

        TestEffect effect = new TestEffect("Effect used!");
        testCard.AddEffect("Test Effect", effect);
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

        // proc tets effect
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            Card card = (Card)GameObjectManager.Instance.GetGameObject("Test Card");
            card.Play();
        }

        // check for left click events
        if (InputManager.Instance.SingleMousePress(MouseButton.Left))
        {
            foreach (GameObject gameObject in GameObjectManager.Instance.GetAllGameObjects())
            {
                IClickable clickable = gameObject as IClickable;

                // check if a gameobject is a clickable, and if mouse is within the bounds of the clickable
                if (clickable != null && clickable.Bounds.Contains(Mouse.GetState().Position))
                {
                    clickable.OnLeftClick();
                    break;
                }
            }
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
