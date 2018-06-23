using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Model
{
    public class Movie
    {
        [NotMapped]
        public Decimal _Id { get; set; }
        [Required]
        [DisplayName("Movie Name")]
        public string _Name { get; set; }

        [Required]
        [DisplayName("Year of Release")]
        public int _YearOfRelease { get; set; }

        [Required]
        [DisplayName("Plot of the Movie")]
        public string _Plot { get; set; }

        public string _ImageLocation { get; set; }

        [Required]
        [DisplayName("Select Producer")]
        public Decimal _SelectedProducer { get; set; }

        [Required]
        [DisplayName("Select Actors")]
        public List<Decimal> _SelectedActors { get; set; } = new List<Decimal>();

        [DisplayName("Movie Poster")]
        public Image _Image { get; set; } = new Image();

        [DisplayName("Actors")]
        public Dictionary<Decimal, string> _Actors { get; set; } = new Dictionary<Decimal, string>();

        [DisplayName("Producer")]
        public string _ProducerName { get; set; }
    }
}
