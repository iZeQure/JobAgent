using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class PageMetaModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static PageMetaModel instance = null;
        private static readonly object pageMetaModelLock = new object();
        private string title = "Forside";

        private PageMetaModel() { }

        public static PageMetaModel Instance
        {
            get
            {
                lock (pageMetaModelLock)
                {
                    if (instance == null) instance = new PageMetaModel();

                    return instance;
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Title
        {
            get
            {
                if (title == string.Empty) return "Forside";

                return title;
            }
            set
            {
                NotifyPropertyChanged(title);
                title = value;
            }
        }
    }
}
