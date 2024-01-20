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
    public class Notes : DbContext, INotifyPropertyChanged
    {
        [Key]
        public int ID { get; set; }

        private string name = "New note";
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

       /* private List<Topic> topics;
        public List<Topic> Topics 
        {
            get => topics;
            set=> Set(ref topics, value);
        } */

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
