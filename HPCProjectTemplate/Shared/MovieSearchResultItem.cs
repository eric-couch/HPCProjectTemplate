using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace HPCProjectTemplate.Shared;

public class MovieSearchResultItem
{
    public string Title { get; set; }
    public string Year { get; set; }
    [Display(Name = "IMDB ID")]
    public string imdbID { get; set; }
    public string Type { get; set; }
    public string Poster { get; set; }
    
}
