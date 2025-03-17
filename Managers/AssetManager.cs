//System Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//MonoGame Imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SlyDeck.Managers
{
    //Authors: Vinny Keeler
    class AssetManager
    {
        //Singleton
        private static AssetManager instance;
        public static AssetManager Instance { get { return instance; } }

        private static AssetManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AssetManager();
            }

            return instance;
        }

        /// <summary>
        /// Constructor for AssetManager
        /// </summary>
        private AssetManager()
        {
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
        }

        //dictionaries

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        //methods

        /// <summary>
        /// Gets and returns an asset
        /// </summary>
        /// <typeparam name="T">The type of the asset requested</typeparam>
        /// <param name="assetName">The key name of the asset requested</param>
        /// <returns>The asset requested</returns>
        /// <exception cref="KeyNotFoundException">//Error is thrown when the asset isn't found or the wrong type of object was requested</exception>
        public T GetAsset<T>(string assetName)
        {
            //If the object requested is a Texture2D
            if (typeof(T) == typeof(Texture2D))
            {
                //checks if the supplied assetname is in the dictionary
                if (textures.ContainsKey(assetName))
                {
                    return (T)(object)textures[assetName];
                }
            }
            //If the object requested is a SpriteFont
            else if (typeof(T) == typeof(SpriteFont))
            {
                //checks if the supplied assetName is in the dictionary
                if (fonts.ContainsKey(assetName))
                {
                    return (T)(object)fonts[assetName];
                }
            }

            //Error is thrown when the asset isn't found or the wrong type of object was requested
            throw new KeyNotFoundException($"Asset '{assetName}' of type {typeof(T).Name} not found");
        }

        /// <summary>
        /// Adds a texture to the textures dictionary
        /// </summary>
        /// <param name="name">The name of the texture</param>
        /// <param name="texture">The texture to be added</param>
        public void AddTexture(string name, Texture2D texture)
        {
            textures[name] = texture;
        }

        /// <summary>
        /// Adds a font to the fonts dictionary
        /// </summary>
        /// <param name="name">The name of the font</param>
        /// <param name="font">The font to be added</param>
        public void AddFont(string name, SpriteFont font)
        {
            fonts[name] = font;
        }
    }
}
