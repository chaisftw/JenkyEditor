using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace Jenky.Content
{
    public class ContentHandler
    {
        #region vars

        private ContentManager content;

        private IDictionary<string, Texture2D> spriteSheets;
        private IDictionary<string, SoundEffect> soundFiles;
        private IDictionary<string, SpriteFont> spriteFonts;
        private IDictionary<string, Effect> effects;

        #endregion

        #region init

        public ContentHandler(ContentManager _content)
        {
            content = _content;
            spriteSheets = new Dictionary<string, Texture2D>();
            soundFiles = new Dictionary<string, SoundEffect>();
            spriteFonts = new Dictionary<string, SpriteFont>();
            effects = new Dictionary<string, Effect>();
        }

        #endregion

        #region pipeline

        //Load a sprite sheet and add it to the dictionary
        public void LoadSpriteSheet(string contentPath)
        {
            spriteSheets.Add(contentPath, content.Load<Texture2D>(contentPath));
        }

        //Load a sound file and add it to the dictionary
        public void LoadSoundFile(string contentPath)
        {
            soundFiles.Add(contentPath, content.Load<SoundEffect>(contentPath));
        }

        //Load a sprite font and add it to the dictionary
        public void LoadSpriteFont(string contentPath)
        {
            spriteFonts.Add(contentPath, content.Load<SpriteFont>(contentPath));
        }

        //Load an effect and add it to the dictionary
        public void LoadEffect(string contentPath)
        {
            effects.Add(contentPath, content.Load<Effect>(contentPath));
        }

        //Get a sprite sheet
        public Texture2D GetSpriteSheet(string contentPath)
        {
            return spriteSheets[contentPath];
        }

        //Get a sound effect
        public SoundEffect GetSoundFile(string contentPath)
        {
            return soundFiles[contentPath];
        }

        //Get a sprite font
        public SpriteFont GetSpriteFont(string contentPath)
        {
            return spriteFonts[contentPath];
        }

        //Get an effect
        public Effect GetEffect(string contentPath)
        {
            return effects[contentPath];
        }

        #endregion pipeline

        #region stream

        public Texture2D GetImageFile(GraphicsDevice graphicsDevice, string filePath)
        {
            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open);
                Texture2D texture = Texture2D.FromStream(graphicsDevice, stream);
                stream.Close();
                return texture;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        #endregion stream
    }
}
