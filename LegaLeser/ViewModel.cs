using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TCD;
using Windows.UI.Xaml;

namespace LegaLeser
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private string _InputText = "Hallo, hier kann ein ganz normaler Text eingefügt werden.";
        public string InputText { get { return _InputText; } set { _InputText = value; OnPropertyChanged(); } }

        public Visibility InputVisibility { get { return (State == State.Clean) ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility OutputVisibility { get { return (State == State.Reading) ? Visibility.Visible : Visibility.Collapsed; } }

        private string _OutputText;
        public string OutputText { get { return _OutputText; } set { _OutputText = value; OnPropertyChanged(); } }
        
        private List<string> _Words = new List<string>();
        public List<string> Words { get { return _Words; } set { _Words = value; OnPropertyChanged(); } }

        private State _State = State.Clean;
        public State State
        {
            get { return _State; }
            set
            {
                _State = value;
                StartCommand.RaiseCanExecuteChanged();
                CleanCommand.RaiseCanExecuteChanged();
                NextWordCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(InputVisibility));
                OnPropertyChanged(nameof(OutputVisibility));
                OnPropertyChanged();
            }
        }

        private RelayCommand _StartCommand;
        public RelayCommand StartCommand { get { return _StartCommand; } set { _StartCommand = value; OnPropertyChanged(); } }

        private RelayCommand _CleanCommand;
        public RelayCommand CleanCommand { get { return _CleanCommand; } set { _CleanCommand = value; OnPropertyChanged(); } }

        private RelayCommand _NextWordCommand;
        public RelayCommand NextWordCommand { get { return _NextWordCommand; } set { _NextWordCommand = value; OnPropertyChanged(); } }


        public ViewModel()
        {
            StartCommand = new RelayCommand(delegate
            {
                string text = InputText;
                //Words = text.Split(' ').ToList();
                Words = text.Select(c => c.ToString()).ToList();
                OutputText = string.Empty;
                InputText = string.Empty;
                State = State.Reading;
                NextWordCommand.Execute(null);
            }, () => State == State.Clean);
            NextWordCommand = new RelayCommand(delegate
            {
                if (Words.Count > 0)
                {
                    OutputText += Words[0] + "";
                    Words.RemoveAt(0);
                }
            }, () => State == State.Reading && Words.Count > 0);
            CleanCommand = new RelayCommand(delegate
            {
                //InputText = string.Join(" ", Words);
                InputText = string.Join("", Words);
                OutputText = string.Empty;
                State = State.Clean;
            }, () => State == State.Reading);
        }
    }
    public enum State
    {
        Clean, Reading
    }
}
