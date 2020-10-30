using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Jenky.Content;
using Jenky.States;
using Jenky.Graphics;
using System;

namespace JenkyEditor
{
    public class JenkyGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public ContentHandler contentHandler;

        public Viewport viewport;

        private GameState gameState;

        Action LoadMenu;
        Action<string, string, string, int, int> NewEdit;
        Action<string> LoadEdit;

        public JenkyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Sets the game's current gamestate
        public void SetGameState(GameState _gameState)
        {
            gameState = _gameState;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            contentHandler = new ContentHandler(Content);

            //Setting delegates
            LoadMenu = SwitchToMenu;
            LoadEdit = LoadEditState;
            NewEdit = NewEditState;

            //Setting game screen dimensions
            graphics.PreferredBackBufferWidth = 1880; //1920;
            graphics.PreferredBackBufferHeight = 1000; //1080;
            graphics.IsFullScreen = false;

            graphics.ApplyChanges();

            this.IsMouseVisible = true;

            base.Initialize();
            viewport = GraphicsDevice.Viewport;
            MenuState menuState = new MenuState(GraphicsDevice, contentHandler, NewEdit, LoadEdit, Quit);
            gameState = menuState;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            contentHandler.LoadSpriteFont("font");
            
            contentHandler.LoadSpriteSheet("background");
            contentHandler.LoadSpriteSheet("ui_elements");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Color bgr = new Color(39,43,66);
            GraphicsDevice.Clear(bgr);

            gameState.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void SwitchToMenu()
        {
            SetGameState(new MenuState(GraphicsDevice, contentHandler, NewEdit, LoadEdit, Quit));
        }

        public void NewEditState(string projectPath, string tilePng, string propPng, int tileWidth, int tileHeight)
        {
            SetGameState(new EditState(GraphicsDevice, contentHandler, LoadMenu, tileWidth, tileHeight, projectPath, tilePng, propPng));
        }

        public void LoadEditState(string projectPath)
        {
            SetGameState(new EditState(GraphicsDevice, contentHandler, LoadMenu, projectPath));
            EditState state = gameState as EditState;
            if (!state.ValidFiles)
            {
                SetGameState(new MenuState(GraphicsDevice, contentHandler, NewEdit, LoadEdit, Quit));
            }
        }

        public void Quit()
        {
            this.Exit();
        }
    }
}