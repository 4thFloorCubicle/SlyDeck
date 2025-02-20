using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SlyDeck.GameObjects.UI;
using SlyDeck.GameObjects;
using SlyDeck.Managers;
using SlyDeck.GameObjects.Card;

namespace SlyDeck;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        fonts["Arial24"] = Content.Load<SpriteFont>("Arial24");

        Label testLabel = new Label(new Vector2(100, 100), "Test Label", "Hi!", fonts["Arial24"]);
        GameObjectManager.Instance.AddGUIElement(testLabel);

        Card testCard = new Card(new Vector2(200, 200), "Test Card", "This card has a test effect", 2, 4, 2, CardType.Header);
        GameObjectManager.Instance.Ad
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // TODO: Add your update logic here

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
