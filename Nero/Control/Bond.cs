using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.System;
    using SFML.Window;
    public abstract class Bond : Control
    {
        #region Properties
        protected List<Control> controls;
        protected List<Form> forms;
        protected Control priority;
        protected FormDragged formDragged;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        public Bond() : base()
        {
            controls = new List<Control>();
            forms = new List<Form>();
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public Bond(Bond bond) : base(bond)
        {
            controls = new List<Control>();
            forms = new List<Form>();
        }

        /// <summary>
        /// Encontra um controle
        /// </summary>
        /// <param name="name"></param>
        public T FindControl<T>(string name) where T : Control
        {
            var control = (T)controls.Where(i => i.Name.ToLower() == name.ToLower()).FirstOrDefault();
            if (control == null)
                control = forms.Where(i => i.Name.ToLower() == name.ToLower()).FirstOrDefault() as T;

            return control;
        }

        /// <summary>
        /// Encontra um controle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindControl<T>() where T : Control
            => FindControl<T>(typeof(T).Name);

        /// <summary>
        /// Desenha os controles vinculados
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            if (controls.Count > 0)
            {
                var count = controls.Count;
                for (int i = 0; i < count; i++)
                    if (controls[i] != null && controls[i] != priority && controls[i].Visible)
                        target.Draw(controls[i]);
            }

            if (forms.Count > 0)
            {
                var count = forms.Count;
                for (int i = 0; i < count; i++)
                    if (forms[i] != null && forms[i].Visible)
                        target.Draw(forms[i]);
            }

            priority?.Draw(target, states);

        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (Hover())
            {
                if (formDragged.form != null) return true;

                if (priority != null && priority.MousePressed(e)) return true;

                if (forms.Count > 0)
                    for (int i = forms.Count - 1; i >= 0; i--)
                        if (forms[i] != null && forms[i].Visible && forms[i].MousePressed(e)) return true;

                if (controls.Count > 0)
                    for (int i = controls.Count - 1; i >= 0; i--)
                        if (controls[i] != null && controls[i] != priority && controls[i].Visible && controls[i].MousePressed(e)) return true;
            }

            return result;
        }

        /// <summary>
        /// Mouse quando solta
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseReleased(MouseButtonEvent e)
        {
            var hover = base.MouseReleased(e);
            if (Hover())
            {
                if (formDragged.form != null)
                {
                    formDragged.form.Form_Dragged();
                    formDragged.form = null;
                    return true;
                }

                if (priority != null && priority.Visible && priority.MouseReleased(e)) return true;

                if (forms.Count > 0)
                    for (int i = forms.Count - 1; i >= 0; i--)
                        if (forms[i] != null && forms[i].Visible && forms[i].MouseReleased(e)) return true;

                if (controls.Count > 0)
                    for(int i = controls.Count - 1; i >= 0; i--)
                        if (controls[i] != null && controls[i] != priority && controls[i].Visible && controls[i].MouseReleased(e)) return true;

                TextBox.Focus = null;
            }

            return hover;
        }

        /// <summary>
        /// Movimento do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseMoved(Vector2 e)
        {
            var hover = base.MouseMoved(e);
            if (Hover())
            {
                if (formDragged.form != null)
                {
                    var newp = new Vector2(e.x, e.y) - formDragged.MousePosition;
                    SetPositionFormWithAnchor(formDragged, newp);
                    return true;
                }

                if (priority != null && priority.Visible && priority.MouseMoved(e)) return true;

                if (forms.Count > 0)
                    for (int i = forms.Count - 1; i >= 0; i--)
                        if (forms[i] != null && forms[i].Visible && forms[i].MouseMoved(e)) return true;

                if (controls.Count > 0)
                    for (int i = controls.Count - 1; i >= 0; i--)
                        if (controls[i] != null && controls[i] != priority && controls[i].Visible && controls[i].MouseMoved(e)) return true;

            }

            return hover;
        }

        /// <summary>
        /// Movimento do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseScrolled(MouseWheelScrollEventArgs e)
        {
            var hover = base.MouseScrolled(e);
            if (Hover())
            {
                if (formDragged.form != null) return true;

                if (priority != null && priority.Visible && priority.MouseScrolled(e)) return true;

                if (forms.Count > 0)
                    for (int i = forms.Count - 1; i >= 0; i--)
                        if (forms[i] != null && forms[i].Visible && forms[i].MouseScrolled(e)) return true;

                if (controls.Count > 0)
                    for (int i = controls.Count - 1; i >= 0; i--)
                        if (controls[i] != null && controls[i] != priority && controls[i].Visible && controls[i].MouseScrolled(e)) return true;

            }

            return hover;
        }

        /// <summary>
        /// Adiciona um controle ao vinculo
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(Control control)
        {
            if (control.GetType().Name == "Form" || control.GetType().BaseType.Name == "Form")
                forms.Add((Form)control);
            else
                controls.Add(control);
        }

        /// <summary>
        /// Seta um controle como prioridade
        /// </summary>
        /// <param name="control"></param>
        public void SetControlPriority(Control control)
        {
            priority = control;
        }

        /// <summary>
        /// Remove o Foco
        /// </summary>
        /// <param name="control"></param>
        public void RemoveFocusForm(Form control)
        {
            if (forms.Count == 0) return;
            if (control == forms[0]) return;

            var lst = new List<Form>();
            lst.Add(control);
            foreach (var i in forms)
                if (i != control)
                    lst.Add(i); // Ultimo será o novo Foco

            forms = lst;
        }

        /// <summary>
        /// Define o foco de Janela
        /// </summary>
        /// <param name="control"></param>
        public void SetFocusForm(Form control)
        {
            if (forms.Count == 0) return;
            if (control == forms[forms.Count - 1]) return;

            var lst = new List<Form>();
            foreach (var i in forms)
                if (i != control) lst.Add(i);

            lst.Add(control);
            forms = lst;
        }

        /// <summary>
        /// Muda a posição conforme ancoragem
        /// </summary>
        /// <param name="formDragged"></param>
        /// <param name="value"></param>
        protected void SetPositionFormWithAnchor(FormDragged formDragged, Vector2 value)
        {
            var form = formDragged.form;
            switch (form.Anchor)
            {
                case Anchors.TopLeft: form.Position = value; break;
                case Anchors.TopCenter: form.Position = value - new Vector2((Size.x - form.Size.x) / 2, 0); break;
                case Anchors.TopRight: form.Position = new Vector2(Size.x - form.Size.x - value.x, value.y); break;
                case Anchors.Left: form.Position = new Vector2(value.x, value.y - (Size.y - form.Size.y) / 2); break;
                case Anchors.Right: form.Position = new Vector2(Size.x - form.Size.x - value.x, value.y - (Size.y - form.Size.y) / 2); break;
                case Anchors.BottomLeft: form.Position = new Vector2(value.x, Size.y - form.Size.y - value.y); break;
                case Anchors.BottomCenter: form.Position = new Vector2(value.x - (Size.x - form.Size.x) / 2, Size.y - form.Size.y - value.y); break;
                case Anchors.BottomRight: form.Position = new Vector2(Size.x - form.Size.x - value.x, Size.y - form.Size.y - value.y); break;
                case Anchors.Center: form.Position = value - (Size - form.Size) / 2; break;
            }
        }

        /// <summary>
        /// Movimenta o formulario
        /// </summary>
        /// <param name="form"></param>
        /// <param name="mousePosition"></param>
        public void SetDragForm(Form form, Vector2 mousePosition)
        {
            if (formDragged.form != null) return;
            formDragged = new FormDragged(form, mousePosition);
        }

        public override void Resize()
        {
            if (controls.Count > 0)
                foreach (var i in controls)
                    i.Resize();

            if (forms.Count > 0)
                foreach (var i in forms)
                    i.Resize();
        }

        #endregion
    }
}
