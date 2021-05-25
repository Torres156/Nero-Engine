using System;
using System.Collections.Generic;
using System.Text;


namespace Nero
{
    using SFML.Graphics;
    using Sprite2 = SFML.Graphics.Sprite;
    using SFMLColor = SFML.Graphics.Color;
    using SFML.System;

    /// <summary>
    /// By: NetExtLib
    /// </summary>
    internal class LargeSprite : Transformable, Drawable
    {
        #region Variables
        private LargeTexture _currenttexture = null;
        private Sprite2[] _spritelist = new Sprite2[0];
        private SFMLColor _spritecolor = Color.White;
        #endregion

        #region Properties

        /// <summary>Gets or sets the color of the sprite.</summary>
        public Color Color
        {
            get
                => (Color)_spritecolor;
            set
            {
                _spritecolor = (SFMLColor)value;
                for (int i = 0; i < _spritelist.Length; i++)
                    _spritelist[i].Color = _spritecolor;
            }
        }

        /// <summary>
        /// Textura
        /// </summary>
        public LargeTexture Texture
        {
            get
                => _currenttexture;
            set
            {
                if (_currenttexture != value)
                {
                    _currenttexture = value;
                    UpdateSpriteList();
                }
            }
        }
        #endregion

        #region Constructors/Destructors

        public LargeSprite()
        {

        }

        public void SetTexture(LargeTexture texture)
        {
            _currenttexture = Texture;
            UpdateSpriteList();
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Texture"></param>
        public LargeSprite(LargeTexture Texture)
        {
            _currenttexture = Texture;
            UpdateSpriteList();
        }
        #endregion

        #region Functions
        private void UpdateSpriteList()
        {
            for (int i = 0; i < _spritelist.Length; i++)
                _spritelist[i] = null;

            _spritelist = new Sprite2[_currenttexture.TextureList.Length];
            for (int i = 0; i < _spritelist.Length; i++)
            {
                _spritelist[i] = new Sprite2(_currenttexture.TextureList[i]);
                _spritelist[i].Position = new Vector2f(_currenttexture.PositionList[i].X, _currenttexture.PositionList[i].Y);
                _spritelist[i].Color = _spritecolor;
            }
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= base.Transform;
            int len = _spritelist.Length;
            for (int i = 0; i < len; i++)
                target.Draw(_spritelist[i], states);
        }
        /// <summary>Not currently implemented.</summary>
        public FloatRect GetLocalBounds()
        {
            throw new Exception("Not currently implemented");
        }
        /// <summary>Not currently implemented.</summary>
        public FloatRect GetGlobalBounds()
        {
            throw new Exception("Not currently implemented");
        }
        #endregion
    }
}
