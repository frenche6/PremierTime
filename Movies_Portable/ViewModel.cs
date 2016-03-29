using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Movies_Portable
{
    public class ViewModel
    {
        public ViewModel()
        {
            Movies = new ObservableCollection<MovieDetails>();
        }

        public ObservableCollection<MovieDetails> Movies { get; set; }

    }
}
