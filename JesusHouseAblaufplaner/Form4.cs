using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Statistic
{
    public partial class Form4 : Form
    {

        private String[][] table;
        public Dictionary<String, TimeSpan> stats = new Dictionary<String, TimeSpan>();

        public Form4(String[][] table, int rows, Dictionary<String, TimeSpan> stats)
        {
            InitializeComponent();

            this.table = new String[rows][];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                table[i] = new String[4];
            }
            
            this.table = table;
            this.stats = stats;

            chartPlan.Series["Benötigte Zeit"].Points.AddXY(table[0][1], stats[table[0][1]].TotalMinutes);
            chartPlan.Series["Geplante Zeit"].Points.AddXY("Test", 40);
            chartPlan.Series["Benötigte Zeit"].Points.AddXY("Test2", 30);
            chartPlan.Series["Geplante Zeit"].Points.AddXY("Test2", 20);
        }
    }
}
