using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SAViE.Models
{
    public class Topic : DbContext, INotifyPropertyChanged
    {
        [Key]
        public int ID { get; set; }

        private string name = "New topic";
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string? text;
        public string? Text
        {
            get => text;
            set => Set(ref text, value);
        }

        private int noteId;
        public int NoteId
        {
            get => noteId;
            set => Set(ref noteId, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
