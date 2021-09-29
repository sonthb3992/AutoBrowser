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

namespace Nolan.AutoClicker.Views
{
    /// <summary>
    /// Interaction logic for AddMouseCommandDialog.xaml
    /// </summary>
    public partial class AddMouseCommandDialog : Window
    {
        public AddMouseCommandDialog()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.AddMouseCommandDialogViewModel();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
