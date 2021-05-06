using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GameOnHome_WINFORM
{
    public abstract class AnimationThreeRow
    {
        public AnimationThreeRow(ThreeRow game)
        {
            m_game = game;
            m_time = 250;
            m_steps = 10;
            m_count = 0;
        }

        protected Timer m_timer;
        protected ThreeRow m_game;
        protected int m_time;
        protected int m_steps;
        protected int m_count;

        public delegate void AnimationEndHandler();
    }

    public class SwapAnimation : AnimationThreeRow
    {
        public SwapAnimation(ThreeRow game) : base(game)
        {
            m_steps = 8;
        }

        public void Start(VisualElement a, VisualElement b)
        {
            Point distance = new Point(b.Location.X - a.Location.X, b.Location.Y - a.Location.Y);
            Point speed = new Point(distance.X / m_steps, distance.Y / m_steps);
            int last = m_steps - 1;
            m_count = 0;

            m_timer = new Timer();
            m_timer.Interval = m_time / m_steps;
            m_timer.Tick += delegate
            {
                if (m_count > last)
                {
                    m_timer.Dispose();
                    return;
                }
                if (m_count < last)
                {
                    a.Location = new Point(a.Location.X + speed.X, a.Location.Y + speed.Y);
                    b.Location = new Point(b.Location.X - speed.X, b.Location.Y - speed.Y);
                }
                else
                {
                    a.Location = new Point(a.Location.X + (distance.X - speed.X * last), a.Location.Y + (distance.Y - speed.Y * last));
                    b.Location = new Point(b.Location.X - (distance.X - speed.X * last), b.Location.Y - (distance.Y - speed.Y * last));
                }
                m_count++;
                m_game.Refresh();
            };

            m_timer.Disposed += delegate
            {
                if (AnimationEnd != null)
                    AnimationEnd();
            };

            m_timer.Start();
        }

        public event AnimationEndHandler AnimationEnd = null;
    }

    public class FallAnimation : AnimationThreeRow
    {
        public FallAnimation(ThreeRow game) : base(game)
        {
            m_time = 100;
        }

        public void Start(List<VisualElement> elements)
        {
            int distance = elements.First().Size.Height;
            int speed = distance / m_steps;
            //сдвиг упавших элементов вверх
            for (int j = 0; j < elements.Count; ++j)
            {
                VisualElement element = elements[j];
                element.Location = new Point(element.Location.X, element.Location.Y - distance);
            }
            int last = m_steps - 1;
            m_count = 0;
            m_timer = new Timer();
            m_timer.Interval = m_time / m_steps;
            m_timer.Tick += delegate
            {
                if (m_count > last)
                {
                    m_timer.Dispose();
                    return;
                }
                if (m_count < last)
                {
                    for (int j = 0; j < elements.Count; ++j)
                    {
                        VisualElement element = elements[j];
                        element.Location = new Point(element.Location.X, element.Location.Y + speed);
                    }
                }
                else
                {
                    int delta = distance - speed * last;
                    for (int j = 0; j < elements.Count; ++j)
                    {
                        VisualElement element = elements[j];
                        element.Location = new Point(element.Location.X, element.Location.Y + delta);
                    }
                }
                m_count++;
                m_game.Refresh();
            };

            m_timer.Disposed += delegate
            {
                if (AnimationEnd != null)
                    AnimationEnd();
            };

            m_game.Refresh();

            m_timer.Start();
        }

        public event AnimationEndHandler AnimationEnd = null;
    }
}
