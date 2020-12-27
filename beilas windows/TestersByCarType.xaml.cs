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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for TestersByCarType.xaml
    /// </summary>
    public partial class TestersByCarType : Window
    {
        private BL.IBL bl;
        public TestersByCarType()
        {
            InitializeComponent();
            bl = IBL_imp.Instance;
            this.testercarComboBox.ItemsSource = Enum.GetValues(typeof(BE.CarType));
        }
    }
}
