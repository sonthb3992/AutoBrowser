using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nolan.AutoBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region FIELDS

        #endregion

        #region PROPERTIES

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region NAVIGATION PROPERTIES

        #endregion

        #region CONSTRUCTOR

        #endregion

        #region METHODS

        private void Button_WebNavigate_Click(object sender, RoutedEventArgs e)
        {
            Forms.Web_Navigate form = new Forms.Web_Navigate();
            var frmResult = form.ShowDialog();
            if (frmResult.HasValue && frmResult.Value == true)
            {
            }
        }
        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

        
    }
}
