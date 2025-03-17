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

        //dictionaries

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        //methods

        public void GetAsset<T>(string assetName)
        {
            throw new NotImplementedException();
        }
    }
}
