using System;
using System.Collections.Generic;
using System.Text;
using Nero.Control;

namespace Nero.Client.Scenes
{
    using static Renderer;
    class GameplayScene : SceneBase
    {
        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            Game.BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Descarrega os recursos
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Desenha a cena
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
        }
    }
}
