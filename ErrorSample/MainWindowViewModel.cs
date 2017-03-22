using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace ErrorSample
{
    class MainWindowViewModel : BindableBase, INotifyDataErrorInfo
    {
        public MainWindowViewModel()
        {
            container = new ErrorsContainer<string>((propertyName) =>
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            });

            OkCommand = new DelegateCommand(() =>
            {
                MessageBox.Show("登録できました");
            }, () => { return !HasErrors; }
            );
        }


        ErrorsContainer<string> container;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        public DelegateCommand OkCommand { get; set; }
        public bool HasErrors
        {
            get
            {
                return container.HasErrors;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return container.GetErrors(propertyName);
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                SetProperty(ref name, value);
                container.ClearErrors(nameof(Name));
                container.SetErrors(nameof(Name), getNameErrors(value));
                OkCommand.RaiseCanExecuteChanged();
            }
        }
        private string name;

        private IEnumerable<string> getNameErrors(string target)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(target))
            {
                result.Add("必須入力です");
            }
            target = target ?? "";

            if (target.Length <= 4)
            {
                result.Add("5文字以上入力してね");
            }

            if (target.ToLower().Contains("test"))
            {
                result.Add("testはダメです。");
            }
            return result;
        }
    }
}
