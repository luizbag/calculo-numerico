using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalculoNumerico
{
    public partial class LagrangeView : Form
    {
        public LagrangeView()
        {
            InitializeComponent();
        }
        Bitmap b;
        private void pcbImagem_MouseClick(object sender, MouseEventArgs e)
        {
            b = (Bitmap)Bitmap.FromFile("Lagrange400.bmp");
            b = (Bitmap)b.Clone();
            if (e.Button == MouseButtons.Left)
            {
                if (!VerificarRepeticao(e.X - 200, -e.Y + 200))
                {
                    InserirDados(e.X - 200, -e.Y + 200);
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        for (int i = 0; i < grid.Rows.Count; i++)
                        {
                            int x = Convert.ToInt32(grid.Rows[i].Cells[0].Value);
                            int y = Convert.ToInt32(grid.Rows[i].Cells[1].Value);

                            g.FillEllipse(new SolidBrush(Color.Red), x + 198, -y + 198, 5, 5);
                        }
                    }
                    pcbImagem.Image = b;
                    pcbImagem.Refresh();
                }
            }
            else
            {
                grid.Rows.Clear();
                pcbImagem.Image = b;
                pcbImagem.Refresh();
            }
        }

        private void InserirDados(int x, int y)
        {
            grid.Rows.Add(new object[] { x, y });

            lblPontos.Text = grid.Rows.Count + " pontos";
            if (grid.Rows.Count > 1)
            {
                Interpolar();
            }
        }

        private bool VerificarRepeticao(int x, int y)
        {
            int tam = grid.Rows.Count;
            for (int i = 0; i < tam; i++)
            {
                double xi = Convert.ToInt32(grid.Rows[i].Cells[0].Value);
                double yi = Convert.ToInt32(grid.Rows[i].Cells[1].Value);

                if (x == xi && y == yi)
                    return true;
            }

            return false;
        }

        private double Lagrange(double x)
        {
            int tam = grid.Rows.Count;
            double Li;
            double y = 0;
            y = 0;
            for (int i = 0; i < tam; i++)
            {
                Li = 1;

                double xi = Convert.ToInt32(grid.Rows[i].Cells[0].Value);
                double yi = Convert.ToInt32(grid.Rows[i].Cells[1].Value);

                for (int j = 0; j < tam; j++)
                {
                    int xj = Convert.ToInt32(grid.Rows[j].Cells[0].Value);

                    if (i != j)
                    {
                        Li *= (x - xj) / (xi - xj);
                    }
                }

                y += Li * yi;
            }

            return y;
        }

        private void Interpolar()
        {
            for (double x = -199; x < 200; x = x + 0.3)
            {
                double y0 = -Lagrange(x - 0.3) + 200;
                double y1 = -Lagrange(x) + 200;

                double x0 = (x - 0.3) + 200;
                double x1 = x + 200;
                if (!double.IsNaN(x1 + x0 + y0 + y1))
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        try
                        {
                            if (y0 < 0)
                                y0 = -1;
                            if (y0 > 400)
                                y0 = 401;
                            if (y1 > 400)
                                y1 = 401;
                            if (y1 < 0)
                                y1 = -1;


                            g.DrawLine(Pens.Blue, (int)x0, (int)y0, (int)x1, (int)y1);
                        }
                        catch { }
                    }

                if (chkOnline.Checked)
                {
                    pcbImagem.Image = b;
                    pcbImagem.Refresh();
                    Application.DoEvents();
                }
            }

            if (!chkOnline.Checked)
            {
                pcbImagem.Image = b;
                pcbImagem.Refresh();
            }
        }
    }
}
