using Microsoft.AspNetCore.Mvc;

public class ThemeController : Controller
{
    public IActionResult Toggle()
    {
        var current = HttpContext.Session.GetString("THEME");
        if (current == "dark")
            HttpContext.Session.SetString("THEME", "light");
        else
            HttpContext.Session.SetString("THEME", "dark");

        // Geri geldiği sayfaya yönlendir
        return Redirect(Request.Headers["Referer"].ToString());
    }
}
