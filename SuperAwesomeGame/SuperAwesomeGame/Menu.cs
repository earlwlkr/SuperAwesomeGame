using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperAwesomeGame.Common;
using SuperAwesomeGame.Controls;

namespace SuperAwesomeGame
{
    public class Menu : GameEntity
    {
        public override EntityState State
        {
            get { return base.State; }
            set
            {
                foreach (var control in Controls)
                {
                    control.State = value;
                }
            }
        }

        public List<Control> Controls { get; private set; }

        public Menu()
        {
            Controls = new List<Control>();
        }

        public void AddMenuItem(Control control)
        {
            Controls.Add(control);

            if (Controls.Count == 1)
            {
                Area.Left = control.Area.Left;
                Area.Top = control.Area.Top;
            }

            // Adjust width and height of the menu to contain all items.
            if (control.Area.Left < Area.Left)
            {
                Area.Width = Area.Left + Area.Width - control.Area.Left;
                Area.Left = control.Area.Left;
            }
            else
            {
                var controlRightBoundary = control.Area.Left + control.Area.Width;
                if (Area.Left + Area.Width < controlRightBoundary)
                {
                    Area.Width = controlRightBoundary - Area.Left;
                }
            }

            if (control.Area.Top < Area.Top)
            {
                Area.Height = Area.Top + Area.Height - control.Area.Top;
                Area.Top = control.Area.Top;
            }
            else
            {
                var controlUpperBoundary = control.Area.Top + control.Area.Height;
                if (Area.Top + Area.Height < controlUpperBoundary)
                {
                    Area.Height = controlUpperBoundary - Area.Top;
                }
            }
        }

        public void Clear()
        {
            Controls.Clear();
            Area = new GameRectangle();
        }

        public float GetSliderValue(string content)
        {
            return Controls.Where(control => control.Content != null && control.Content.ToLower().Contains(content.ToLower())).Select(control =>
            {
                var slider = control as Slider;
                return slider != null ? slider.Value : 0;
            }).FirstOrDefault();
        }

        public bool GetCheckboxValue(string content)
        {
            return Controls.Where(control => control.Content.ToLower().Contains(content.ToLower())).Select(control =>
            {
                var checkbox = control as Checkbox;
                return checkbox != null && checkbox.Value;
            }).FirstOrDefault();
        }

        public override void SlideLeft(float newLeft)
        {
            foreach (var control in Controls)
            {
                control.SlideLeft(newLeft);
            }
        }

        public override void SlideRight(float newLeft)
        {
            foreach (var control in Controls)
            {
                control.SlideRight(newLeft);
            }
        }

        public override void Select(bool toggle)
        {
            base.Select(toggle);

            if (!toggle)
            {
                foreach (var control in Controls)
                {
                    control.Select(false);
                }
                return;
            }

            var state = Mouse.GetState();
            foreach (var control in Controls.Where(control => control.Area.Contains(state.X, state.Y)))
            {
                control.Select(true);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var menuItem in Controls)
            {
                menuItem.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var menuItem in Controls)
            {
                menuItem.Update(gameTime);
            }
        }
    }
}
