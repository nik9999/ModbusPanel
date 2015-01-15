using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

namespace HMI_WPF.Master
{
    [Export]
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }

        [Import]
        ShellViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }        

    }
}
