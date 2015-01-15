using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModbusHelper.WPF.Controls
{
    [TemplatePart(Type = typeof(TextBlock), Name = "PART_VIEW_VALUE")]
    [TemplatePart(Type = typeof(TextBox), Name = "PART_EDIT_VALUE")]
    [TemplatePart(Type = typeof(Path), Name = "PART_SEPARATOR")]
    public class NumericInputBox : Control
    {
        private string oldValue;

        private bool isEditable;

        private TextBlock viewValue;
        private TextBox editValue;
        private Path separator;



        public string Value
        {

            get { return (string)GetValue(ValueProperty); }

            set { SetValue(ValueProperty, value); }

        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(NumericInputBox), new FrameworkPropertyMetadata("0123456789", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public String Caption
        {
            get { return (string)base.GetValue(CaptionProperty); }
            set { base.SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(NumericInputBox), new FrameworkPropertyMetadata(string.Empty));

        public double CaptionWidth
        {

            get { return (double)GetValue(CaptionWidthProperty); }

            set { SetValue(CaptionWidthProperty, value); }

        }

        public static readonly DependencyProperty CaptionWidthProperty = DependencyProperty.Register("CaptionWidth", typeof(double), typeof(NumericInputBox), new FrameworkPropertyMetadata((double)50));

        public double SeparatorWidth
        {

            get { return (double)GetValue(SeparatorWidthProperty); }

            set { SetValue(SeparatorWidthProperty, value); }

        }

        public static readonly DependencyProperty SeparatorWidthProperty = DependencyProperty.Register("SeparatorWidth", typeof(double), typeof(NumericInputBox), new FrameworkPropertyMetadata((double)10));



        public bool Quality
        {
            get { return (bool)GetValue(QualityProperty); }
            set { SetValue(QualityProperty, value); }
        }

        public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("Quality", typeof(bool), typeof(NumericInputBox), new UIPropertyMetadata(true, new PropertyChangedCallback(qualityChangedCallBack)));

        static void qualityChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            NumericInputBox numericInputBox = (NumericInputBox)property;
            if ((bool)args.NewValue)
            {
                numericInputBox.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF237BE4"));
                numericInputBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF99DEFF"));
            }
            else
            {
                numericInputBox.BorderBrush = System.Windows.Media.Brushes.Red;
                numericInputBox.Background = System.Windows.Media.Brushes.Yellow;
            }
        }

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(NumericInputBox), new UIPropertyMetadata(false, new PropertyChangedCallback(isEdiatbleChangedCallBack)));

        private static void isEdiatbleChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
           NumericInputBox numericInputBox = (NumericInputBox)property;
           numericInputBox.isEditable =  ((bool)args.NewValue);
           if (numericInputBox.separator != null)
           {
               if (numericInputBox.isEditable)
                   numericInputBox.separator.Data = Geometry.Parse("M 0,2 L 0,0");
               else
                   numericInputBox.separator.Data = Geometry.Parse("M 0,2 L 1,1 L 0,0");
           }
        }

        static NumericInputBox()
        {
            WidthProperty.OverrideMetadata(typeof(NumericInputBox), new FrameworkPropertyMetadata((double)100));
            HeightProperty.OverrideMetadata(typeof(NumericInputBox), new FrameworkPropertyMetadata((double)35));
            BorderBrushProperty.OverrideMetadata(typeof(NumericInputBox), new FrameworkPropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF237BE4"))));
            BackgroundProperty.OverrideMetadata(typeof(NumericInputBox), new FrameworkPropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF99DEFF"))));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericInputBox), new FrameworkPropertyMetadata(typeof(NumericInputBox)));
        }





        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();

            this.viewValue = (TextBlock)this.GetTemplateChild("PART_VIEW_VALUE");
            this.editValue = (TextBox)this.GetTemplateChild("PART_EDIT_VALUE");
            this.separator = (Path)this.GetTemplateChild("PART_SEPARATOR");
            
            if (this.isEditable)
                this.separator.Data = Geometry.Parse("M 0,2 L 0,0");
            else
                this.separator.Data = Geometry.Parse("M 0,2 L 1,1 L 0,0");
            
            this.editValue.LostFocus += this.OnTextBoxLostFocus;
            this.editValue.KeyDown += this.OnTextBoxKeyDown;
            this.editValue.PreviewTextInput += this.filterTextInput;
        }





        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {

            base.OnMouseDoubleClick(e);
            if (isEditable)
            {
                this.viewValue.Visibility = Visibility.Hidden;
                this.editValue.Visibility = Visibility.Visible;

                oldValue = this.viewValue.Text;

                this.editValue.Text = this.viewValue.Text;
                this.editValue.Focus();
                this.editValue.SelectAll();


                Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
                Mouse.Capture(this);
            }
        }


        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (this.editValue.Visibility == Visibility.Visible)
            {
                this.viewValue.Text = String.IsNullOrEmpty(this.editValue.Text) ? "0" : this.editValue.Text;
                finishEditing();
            }
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.viewValue.Text = String.IsNullOrEmpty(this.editValue.Text) ? "0" : this.editValue.Text;
                finishEditing();
            }
            if (e.Key == Key.Escape)
            {
                finishEditing();
            }
        }

        private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
        {
            this.viewValue.Text = String.IsNullOrEmpty(this.editValue.Text) ? "0" : this.editValue.Text;
            finishEditing();
        }

        void finishEditing()
        {
            this.viewValue.Visibility = Visibility.Visible;
            this.editValue.Visibility = Visibility.Collapsed;
            this.editValue.FocusVisualStyle = null;

            Mouse.Capture(null);
            Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);

            if (oldValue != this.viewValue.Text)
            {
                OnValueChanged(this.viewValue.Text);
            }

        }

        void filterTextInput(object sender, TextCompositionEventArgs e)
        {
            double result;
            bool sign = ((TextBox)sender).Text.IndexOf("-") == 0 && e.Text.Equals("-") && ((TextBox)sender).Text.Length > 0;
            bool dot = ((TextBox)sender).Text.IndexOf(".") < 0 && e.Text.Equals(".") && ((TextBox)sender).Text.Length > 0;
            if (!(double.TryParse(e.Text, out result) || (dot || sign)))
            {
                e.Handled = true;
            }
        }





        #region ValueChanged Routed Event
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(ValueChangedEventHandler), typeof(NumericInputBox));

        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        protected void OnValueChanged(string newValue)
        {
            ValueChangedEventArgs e = new ValueChangedEventArgs(NumericInputBox.ValueChangedEvent, newValue);
            RaiseEvent(e);
        }
        #endregion

    }

    #region ValueChangedEventArgs
    public class ValueChangedEventArgs : RoutedEventArgs
    {
        public ValueChangedEventArgs(RoutedEvent routedEvent, string value)
            : base(routedEvent)
        {
            this.Value = value;
        }

        public string Value
        {
            get;
            private set;
        }
    }
    #endregion

}
