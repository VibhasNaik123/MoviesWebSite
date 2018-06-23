using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebSite.Model
{
    public class Gender
    {
        public int _Id { get; set; }
        [Required]
        [DisplayName("Gender")]
        public string _Name { get; set; }
    }
}
