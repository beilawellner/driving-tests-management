using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for trySchedule.xaml
    /// </summary>
    public partial class trySchedule : Window
    {
        public trySchedule()
        {
            InitializeComponent();
            List<List<int>> lsts = new List<List<int>>();

            for (int i = 0; i < 5; i++)
            {
                lsts.Add(new List<int>());

                for (int j = 0; j < 5; j++)
                {
                    lsts[i].Add(i * 10 + j);
                }
            }

            lst.ItemsSource = lsts;
        }
    }
}
