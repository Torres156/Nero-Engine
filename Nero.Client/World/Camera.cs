using Nero.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.World
{
    class Camera
    {
        static View view;

        /// <summary>
        /// Posição
        /// </summary>
        public static Vector2 Position
        {
            get => (Vector2)view.Center;
            set
            {
                var v = Vector2.Max(value, Game.Size / 2);
                v = Vector2.Min(v, (Vector2)(Map.MapInstance.Current.Size + Int2.One) * 32 - Game.Size / 2);
                view.Center = v.Floor();
            }
        }

        /// <summary>
        /// Inicio da área da camera
        /// </summary>
        /// <returns></returns>
        public static Int2 Start()
            => Int2.Max((Int2)(view.Center - Game.Size / 2) / 32 - Int2.One, Int2.Zero);

        /// <summary>
        /// Final da área da camera
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Int2 End(Map.MapInstance m)
            => Int2.Min((Int2)(view.Center + Game.Size) / 32 + Int2.One, m.Size);

        /// <summary>
        /// Inicia a camera
        /// </summary>
        public static void Initialize()
        {
            view = new View(Vector2.Zero, Game.Size);
        }

        /// <summary>
        /// Seta o zoom
        /// </summary>
        /// <param name="value"></param>
        public static void SetZoom(float value)
        {
            view.Zoom(value);
        }

        /// <summary>
        /// Reseta o zoom
        /// </summary>
        public static void ResetZoom()
        {
            var center = view.Center;
            view?.Dispose();
            view = new View(center, Game.Size);
        }

        /// <summary>
        /// Inicia o modo camera
        /// </summary>
        public static void Begin()
        {
            Game.Window.SetView(view);
        }

        /// <summary>
        /// Finaliza o modo camera
        /// </summary>
        public static void End()
        {
            Game.Window.SetView(Game.DefaultView);
        }

        /// <summary>
        /// Move a camera
        /// </summary>
        /// <param name="value"></param>
        public static void Move(Vector2 value)
        {
            view.Move(value.Floor());
            view.Center = ((Vector2)view.Center).Round();
        }

        /// <summary>
        /// Mouse na camera
        /// </summary>
        /// <returns></returns>
        public static Int2 GetMousePosition()
            => (Int2)Game.Window.MapPixelToCoords(Game.MousePosition, view);

        
        public static void Resize()
        {
            var ratio = Game.Size.y / Game.Size.x;
            view.Size = new Vector2(Game.Size.x , Game.Size.x * ratio).Round();
            
        }
    }
}
