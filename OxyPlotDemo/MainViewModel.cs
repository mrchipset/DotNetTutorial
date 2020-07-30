using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace OxyPlotDemo
{
    class MainViewModel : INotifyPropertyChanged
    {
        public string Title { get; private set; }
        public IList<DataPoint> BarItems { get; private set; }

        public DelegateCommand ButtonCommand
        {
            get;
            set;
        }
        public MainViewModel()
        {
            this.Title = "OxyPlot Histogram Demo";
            this.BarItems = new List<DataPoint>
            {
                new DataPoint(0, 4),
                new DataPoint(10, 13),
                new DataPoint(20, 15),
                new DataPoint(30, 16),
                new DataPoint(40, 12),
                new DataPoint(50, 12)
            };
            this.ButtonCommand = new DelegateCommand();
            this.ButtonCommand.ExecuteCommand = new Action<object>(AddData);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddData(object obj)
        {
            Random rd = new Random();
            this.BarItems = new List<DataPoint>
            {
                
                new DataPoint(rd.Next() % 100, rd.Next() % 100),
                new DataPoint(rd.Next() % 100, rd.Next() % 100),
                new DataPoint(rd.Next() % 100, rd.Next() % 100),
                new DataPoint(rd.Next() % 100, rd.Next() % 100),
                new DataPoint(rd.Next() % 100, rd.Next() % 100),
                new DataPoint(rd.Next() % 100, rd.Next() % 100)
            };
            NotifyPropertyChanged("BarItems");
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public class DelegateCommand : ICommand
    {
        public Action<object> ExecuteCommand = null;
        public Func<object, bool> CanExecuteCommand = null;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteCommand != null)
            {
                return this.CanExecuteCommand(parameter);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null)
            {
                this.ExecuteCommand(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
