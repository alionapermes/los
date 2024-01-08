using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace los.Pages;

[BindProperties]
public class UserEditOrderModel : PageModel
{
    public Int64 ID{get; set;}

    public void OnGet()
    {
    }
}