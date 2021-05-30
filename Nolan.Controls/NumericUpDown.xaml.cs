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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nolan.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        #region DEPENDENCY PROPERTIES


        /// <summary>
        /// Get or set the Maximum value of the control.
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(int.MaxValue));


        /// <summary>
        /// Get or set the Minimum value of the control
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));



        /// <summary>
        /// Get or set the current value of the control
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));



        /// <summary>
        /// Get or set the color that the border of this control will be if invalid text is entered.
        /// </summary>
        public Brush InvalidBorderBrush
        {
            get { return (Brush)GetValue(InvalidBorderBrushProperty); }
            set { SetValue(InvalidBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InvalidBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvalidBorderBrushProperty =
            DependencyProperty.Register("InvalidBorderBrush", typeof(Brush), typeof(NumericUpDown), new PropertyMetadata(new SolidColorBrush(Colors.Red)));


        private Brush originalBorderBrush;
        #endregion



        public NumericUpDown()
        {
            InitializeComponent();
        }

        private void NumericUpDown_Loaded(object sender, RoutedEventArgs e)
        {
            this.originalBorderBrush = this.BorderBrush;
        }

        //This method change the border of this control to InvalidBorderBrush whenever an invalid value is entered.
        private void TxtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null) return;

            if (this.originalBorderBrush == null)
                this.originalBorderBrush = this.BorderBrush.Clone();

            int _numValue;
            bool _isInt = int.TryParse(txtNum.Text, out _numValue);

            //If the text entered is not an integer or is an integer which is large than the Maximum or less than the Minimum
            // then change the border to InvalidBorderBrush.
            if (!_isInt || _numValue < this.Minimum || _numValue > this.Maximum)
            {
                this.BorderBrush = this.InvalidBorderBrush;
                return;
            }

            //If the text entered is valid then restore the border brush
            this.BorderBrush = this.originalBorderBrush;
        }

        private void CmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value > this.Minimum)
            {
                this.Value--;
            }
        }

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            if (this.Value < this.Maximum)
            {
                this.Value++;
            }
        }


        /// <summary>
        /// This method discard the invalid input and display the current Value in the textbox.
        /// </summary>
        private void txtNum_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //If the input text is null, empty or whitespaces
            if (string.IsNullOrWhiteSpace(this.txtNum.Text))
            {
                this.txtNum.Text = this.Value.ToString();
                this.BorderBrush = this.originalBorderBrush;
                return;
            }

            int _numValue;
            bool _isInt = int.TryParse(txtNum.Text, out _numValue);

            //If the text entered is not an integer or is an integer which is large than the Maximum or less than the Minimum
            // then change it to the current value.
            if (!_isInt || _numValue < this.Minimum || _numValue > this.Maximum)
            {
                this.txtNum.Text = this.Value.ToString();
                this.BorderBrush = this.originalBorderBrush;
                return;
            }
        }
    }
}
