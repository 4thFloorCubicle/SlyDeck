using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SlyDeck.GameObjects.UI;
using SlyDeck.GameObjects;
using SlyDeck.Managers;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;

namespace SlyDeck;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D queenOfSpades; // test texture
    private Texture2D buttonTestTexture;

    private Dictionary<string, SpriteFont> fonts;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        fonts = new Dictionary<string, SpriteFont>();

        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        fonts["Arial24"] = Content.Load<SpriteFont>("Arial24");

        queenOfSpades = Content.Load<Texture2D>("QueenOfSpades");
        buttonTestTexture = Content.Load<Texture2D>("TestButton");

        Label testLabel = new Label(new Vector2(100, 100), "Test Label", "Hi!", fonts["Arial24"]);
        GameObjectManager.Instance.AddGameObject(testLabel);

        Card testCard = new Card(new Vector2(200, 200), "Test Card", queenOfSpades, "This card has a test effect", 2, CardType.Title);
        GameObjectManager.Instance.AddGameObject(testCard);

        Button testButton = new Button(new Vector2(300, 100), "Test Button", "Test Button", buttonTestTexture, fonts["Arial24"]);
        GameObjectManager.Instance.AddGameObject(testButton);
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
