using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnHome_WINFORM.Games
{
    public class Index
    {
        public Index()
        {
            m_x = 0;
            m_y = 0;
        }

        public Index(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public Index(Index r)
        {
            m_x = r.m_x;
            m_y = r.m_y;
        }

        public int X { get { return m_x; } set { m_x = value; } }
        public int Y { get { return m_y; } set { m_y = value; } }

        int m_x, m_y;
    }

    public class Line
    {
        public Line()
        {
            m_start = new Index();
            m_finish = new Index();
        }

        public Line(Index start, Index finish)
        {
            m_start = new Index(start);
            m_finish = new Index(finish);
        }

        public Index Start { get { return m_start; } set { m_start = new Index(value); } }
        public Index Finish { get { return m_finish; } set { m_finish = new Index(value); } }

        Index m_start;
        Index m_finish;
    }

    public class LogicThreeRow
    {
        public LogicThreeRow(int fieldWidth, int fieldHeight, int typesCount)
        {
            m_score = 0;
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            TypesCount = typesCount;
            m_matrix = new sbyte[FieldHeight, FieldWidth];
        }

        public void FillMatrix()
        {
            Random random = new Random();
            //заполнение матрицы, избегая 3 совпадений подряд
            for (int y = 0; y < FieldHeight; y++)
                for (int x = 0; x < FieldWidth; x++)
                {
                    sbyte value;
                    bool repeat = false;
                    do
                    {
                        value = (sbyte)random.Next(0, TypesCount);
                        repeat = false;
                        if (x >= 2 && (m_matrix[y, x - 2] == m_matrix[y, x - 1] && m_matrix[y, x - 2] == value))
                            repeat = true;
                        else if (y >= 2 && (m_matrix[y - 2, x] == m_matrix[y - 1, x] && m_matrix[y - 2, x] == value))
                            repeat = true;
                    }
                    while (repeat != false);
                    m_matrix[y, x] = value;
                }
        }

        public bool RemoveMatches()
        {
            List<Line> lines = new List<Line>();
            //поиск горизонтальных линий
            sbyte[,] tmpMatrix = (sbyte[,])m_matrix.Clone();
            for (int y = 0; y < FieldHeight; ++y)
                for (int x = 0; x < FieldWidth; ++x)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = x + 1; i < FieldWidth; ++i)
                        if (tmpMatrix[y, i] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = x; i < x + count; ++i)
                            tmpMatrix[y, i] = -1;
                        lines.Add(new Line(new Index(x, y), new Index(x + count - 1, y)));
                    }
                }

            //поиск вертикальных линий
            tmpMatrix = (sbyte[,])m_matrix.Clone();
            for (int y = 0; y < FieldHeight; ++y)
                for (int x = 0; x < FieldWidth; ++x)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = y + 1; i < FieldHeight; ++i)
                        if (tmpMatrix[i, x] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = y; i < y + count; ++i)
                            tmpMatrix[i, x] = -1;
                        lines.Add(new Line(new Index(x, y), new Index(x, y + count - 1)));
                    }
                }

            if (lines.Count == 0)
                return false;

            int baseValue = 10;
            foreach (Line line in lines)
            {
                int count = 0;
                //горизонтальная линия
                if (line.Start.Y == line.Finish.Y)
                    for (int i = line.Start.X; i <= line.Finish.X; ++i)
                    {
                        m_matrix[line.Start.Y, i] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(i, line.Start.Y);
                        count++;
                    }
                //вертикальная линия
                else
                    for (int i = line.Start.Y; i <= line.Finish.Y; ++i)
                    {
                        m_matrix[i, line.Start.X] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(line.Start.X, i);
                        count++;
                    }
                int value = (count - 2) * baseValue;
                m_score += count * value;
            }
            if (MatchesRemoved != null)
                MatchesRemoved();
            return true;
        }

        public bool Fall()
        {
            List<Index> elements = new List<Index>();
            //сдвигаем каждый элемент вниз, если есть место
            for (int y = FieldHeight - 2; y >= 0; --y)
                for (int x = FieldWidth - 1; x >= 0; --x)
                {
                    if (m_matrix[y + 1, x] == -1)
                    {
                        m_matrix[y + 1, x] = m_matrix[y, x];
                        m_matrix[y, x] = -1;
                        elements.Add(new Index(x, y + 1));
                    }
                }
            //добавляем новые элементы сверху
            Random random = new Random();
            for (int x = 0; x < FieldWidth; ++x)
                if (m_matrix[0, x] == -1)
                {
                    m_matrix[0, x] = (sbyte)random.Next(0, TypesCount);
                    elements.Add(new Index(x, 0));
                }
            if (ElementsFalled != null && elements.Count != 0)
                ElementsFalled(elements);
            if (elements.Count == 0)
                return false;
            else
                return true;
        }

        public sbyte GetValue(int x, int y)
        {
            return m_matrix[y, x];
        }

        public void Swap(Index a, Index b)
        {
            sbyte t = m_matrix[a.Y, a.X];
            m_matrix[a.Y, a.X] = m_matrix[b.Y, b.X];
            m_matrix[b.Y, b.X] = t;
        }

        public int GetScore()
        {
            return m_score;
        }
        
        public readonly int FieldWidth;
        public readonly int FieldHeight;
        public readonly int TypesCount;
        
        sbyte[,] m_matrix;
        int m_score;

        public delegate void ElementRemoveHandler(int x, int y);
        public event ElementRemoveHandler ElementRemoved;

        public delegate void MatchesRemoveHandler();
        public event MatchesRemoveHandler MatchesRemoved;

        public delegate void ElementsFallHandler(List<Index> elements);
        public event ElementsFallHandler ElementsFalled;
    }


}
