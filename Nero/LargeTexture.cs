using System;
using System.Collections.Generic;
using System.IO;

namespace Nero
{
    using TextureSFML = SFML.Graphics.Texture;
    using SFMLColor = SFML.Graphics.Color;
    using SFML.Graphics;
    using SFML.System;

    /// <summary>
    /// By: NetExtLib
    /// </summary>
    public class LargeTexture
    {
        #region Variables
        private TextureSFML[] _texturelist = null;
        private Vector2u[] _positionlist = null;
        private bool _issmoothed = false;
        private Vector2u _totalsize = new Vector2u(0, 0);
        #endregion

        #region Properties
        /// <summary>
        /// Ativa o algoritimo redimensionamento suavel
        /// </summary>
        public bool Smooth
        {
            get
              => _issmoothed;

            set
            {
                _issmoothed = value;
                for (int i = 0; i < _texturelist.Length; i++)
                    _texturelist[i].Smooth = value;
            }
        }

        /// <summary>
        /// Tamanho total
        /// </summary>
        public Vector2u Size
            => _totalsize;

        /// <summary>
        /// Lista de texturas
        /// </summary>
        public TextureSFML[] TextureList
            => _texturelist;

        /// <summary>
        /// Lista de posicionamento
        /// </summary>
        public Vector2u[] PositionList
            => _positionlist;
        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Filename"></param>
        public LargeTexture(string Filename)
        {
            Image img = new Image(Filename);
            Create(img);
            img.Dispose();
            img = null;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Image"></param>
        public LargeTexture(Image Image)
        {
            Create(Image);
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Stream"></param>
        public LargeTexture(Stream Stream)
        {
            Image img = new Image(Stream);
            Create(img);
            img.Dispose();
            img = null;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public LargeTexture(uint Width, uint Height)
        {
            Image img = new Image(Width, Height);
            Create(img);
            img.Dispose();
            img = null;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Pixels"></param>
        public LargeTexture(uint Width, uint Height, byte[] Pixels)
        {
            Image img = new Image(Width, Height, Pixels);
            Create(img);
            img.Dispose();
            img = null;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Pixels"></param>
        public LargeTexture(Color[,] Pixels)
        {
            Image img = new Image(Pixels);
            Create(img);
            img.Dispose();
            img = null;
        }

        private void Create(Image CurrentImage)
        {
            List<TextureSFML> newtextlist = new List<TextureSFML>();
            List<Vector2u> newposlist = new List<Vector2u>();
            uint maxsize = TextureSFML.MaximumSize;
            for (uint y = 0; y < CurrentImage.Size.Y; y += maxsize)
            {
                for (uint x = 0; x < CurrentImage.Size.X; x += maxsize)
                {
                    TextureSFML newtexture = new TextureSFML(CurrentImage, new IntRect((int)x, (int)y, (int)Math.Min(maxsize, CurrentImage.Size.X - x), (int)Math.Min(maxsize, CurrentImage.Size.Y - y)));
                    newtexture.Repeated = false;
                    newtexture.Smooth = _issmoothed;
                    newtextlist.Add(newtexture);
                    newposlist.Add(new Vector2u(x, y));
                }
            }
            _texturelist = newtextlist.ToArray();
            _positionlist = newposlist.ToArray();
            _totalsize = CurrentImage.Size;
        }
        #endregion

        #region Functions

        /// <summary>
        /// Atualiza os pixels da textura
        /// </summary>
        /// <param name="Pixels"></param>
        public void Update(byte[] Pixels)
        {
            if (_texturelist.Length > 1)
            {
                for (int i = 0; i < _texturelist.Length; i++)
                {
                    byte[] subpixels = new byte[_texturelist[i].Size.X * _texturelist[i].Size.Y * 4];
                    for (int y = 0; y < _texturelist[i].Size.Y; y++)
                        Array.Copy(Pixels, (_positionlist[i].X * 4) + (((y + _positionlist[i].Y) * _totalsize.X * 4)), subpixels, _texturelist[i].Size.X * y * 4, _texturelist[i].Size.X * 4);

                    _texturelist[i].Update(subpixels, _texturelist[i].Size.X, _texturelist[i].Size.Y, 0, 0);
                }
            }
            else if (_texturelist.Length == 1)
                _texturelist[0].Update(Pixels, _totalsize.X, _totalsize.Y, 0, 0);
        }

        public void Destroy()
        {
            if (_texturelist.Length > 0)
                foreach (var i in _texturelist)
                    i.Dispose();
        }

        #endregion
    }
}
