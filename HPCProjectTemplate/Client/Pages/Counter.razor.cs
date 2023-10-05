using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HPCProjectTemplate.Client.Pages;

public partial class Counter
{
    private int currentCount = 0;
    
    [Parameter]
    public int IncrementAmount { get; set; } = 1;
    public string message { get; set; }

    private void IncrementCount(MouseEventArgs args)
    {
        currentCount += IncrementAmount;
        message = $"Mouse coordinates: {args.ClientX} {args.ClientY}";
    }
}
