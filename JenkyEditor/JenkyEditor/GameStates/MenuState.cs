using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Objects;
using Jenky.Graphics;
using Jenky.States;
using Jenky.IO;
using Jenky.Content;
using System.IO;

namespace JenkyEditor
{
    public class MenuState : GameState
    {
        #region vars

        private MenuWindow menu;
        private NewDialog newDialog;

        private InputHandler input;

        private Action<string, string, string, int, int> NewEdit;
        private Action<string> LoadEdit;
        private Action Exit;

        private DialogManager dialog;
        private string documentsPath;
        private Background background;

        private int uiScale;
        private int backgroundScale;

        #endregion

        #region init

        public MenuState(GraphicsDevice _graphicsDevice, ContentHandler _contentHandler, Action<string, string, string, int, int> _NewEdit, Action<string> _LoadEdit, Action _Exit) : base(_graphicsDevice, _contentHandler)
        {
            LoadEdit = _LoadEdit;
            NewEdit = _NewEdit;
            Exit = _Exit;

            dialog = new DialogManager();
            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            uiScale = 2;
            backgroundScale = 4;

            int menuWidth = 120;
            int menuHeight = 140;

            int newWidth = 314;
            int newHeight = 118;

            int menuX = graphicsDevice.Viewport.Width / 2 - (menuWidth * uiScale) / 2;
            int menuY = graphicsDevice.Viewport.Height / 2 - (menuHeight * uiScale) / 2;

            int newX = graphicsDevice.Viewport.Width / 2 - (newWidth * uiScale) / 2;
            int newY = graphicsDevice.Viewport.Height / 2 - (newHeight * uiScale) / 2;

            input = new InputHandler();

            Action[] buttonEvents = new Action[5] { NewPress, LoadPress, ExitPress , NewConfirmPress, CancelPress};

            SpriteFont font = contentHandler.GetSpriteFont("font");
            Texture2D uiTexture = contentHandler.GetSpriteSheet("ui_elements");
            Texture2D lineTexture = new Texture2D(graphicsDevice, 1, 1);
            lineTexture.SetData(new[] { Color.White });

            newDialog = new NewDialog(newX, newY, newWidth, newHeight, uiScale, buttonEvents[3], buttonEvents[4], uiTexture, lineTexture, font, input, dialog);
            menu = new MenuWindow(menuX, menuY, menuWidth, menuHeight, uiScale, buttonEvents, uiTexture, lineTexture, font, input);
            background = new Background(64, 64, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, backgroundScale);
            camera = new Camera(graphicsDevice.Viewport, input);
        }

        #endregion

        #region methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Start drawing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: GetCameraTransform()); // ARGUEMENTS 1 AND 2 ARE DEFAULTS

            background.Draw(spriteBatch, contentHandler.GetSpriteSheet("background"));

            // Stop drawing
            spriteBatch.End();

            // Start drawing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp); 
            
            if (newDialog.Active)
            {
                newDialog.Draw(spriteBatch);
            }
            else
            {
                menu.Draw(spriteBatch);
            }

            // Stop drawing
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();
            background.Update(gameTime);

            if (newDialog.Active)
            {
                newDialog.Update(gameTime);
            }
            else
            {
                menu.Update(gameTime);
            }

        }

        private bool CheckInputs(Tuple<string, int, int> inputText)
        {
            return inputText.Item1 != string.Empty && inputText.Item2 > 0 && inputText.Item3 > 0;
        }
        
        #endregion

        #region events

        public void NewPress()
        {
            newDialog.Activate();
        }

        public void LoadPress()
        {
            string projectPath = Path.Combine(documentsPath, @"JenkyEditor\");
            projectPath = dialog.GetProjectPath(projectPath);

            if (projectPath != string.Empty)
            {
                LoadEdit(projectPath);
            }

        }

        public void ExitPress()
        {
            Exit();
        }

        public void NewConfirmPress()
        {
            Tuple<string, int, int> inputText = newDialog.GetNewMap();
            
            string projectPath = Path.Combine(documentsPath, @"JenkyEditor\" + inputText.Item1);

            if(!Directory.Exists(projectPath) && CheckInputs(inputText))
            {
                Directory.CreateDirectory(projectPath);
                NewEdit(projectPath, newDialog.TilePng, newDialog.PropPng, inputText.Item2, inputText.Item3);
            }
        }

        public void CancelPress()
        {
            newDialog.DeActivate();
        }

        #endregion
    }
}
