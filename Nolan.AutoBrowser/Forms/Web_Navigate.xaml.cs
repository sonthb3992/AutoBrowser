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
using System.Windows.Shapes;

namespace Nolan.AutoBrowser.Forms
{
    /// <summary>
    /// Interaction logic for Web_Navigate.xaml
    /// </summary>
    public partial class Web_Navigate : Window, INotifyPropertyChanged
    {
        #region FIELDS
        private bool _IsUrlValid;

        #endregion

        #region PROPERTIES

        public bool IsUrlValid
        {
            get { return _IsUrlValid; }
            set
            {
                if (value != _IsUrlValid) _IsUrlValid = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region NAVIGATION PROPERTIES

        #endregion

        #region CONSTRUCTOR
        public Web_Navigate()
        {
            InitializeComponent();
        }
        #endregion

        #region METHODS
        private void UrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Uri uriResult;
            this.IsUrlValid = Uri.TryCreate(this.UrlTextBox.Text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        #endregion












        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
