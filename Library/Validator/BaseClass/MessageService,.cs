using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Library.Validator.Mail;
using Library.Validator;
using Library.Interface.Service;

namespace Library.Validator.BaseClass
{
    public abstract class Message : IMessageService, INotifyPropertyChanged
    {

        private string _messages = string.Empty;

        public string Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    OnPropertyChanged(nameof(Messages));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Process();

    }
}
