using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPCProjectTemplate.Shared;

public class MovieSearchResult
{
    public MovieSearchResultItem[] Search { get; set; }
    public string totalResults { get; set; }
    public string Response { get; set; }
}
