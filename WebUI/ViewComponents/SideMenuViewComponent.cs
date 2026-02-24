using Microsoft.AspNetCore.Mvc;
using WebUI.Models.UI;

namespace WebUI.ViewComponents;


public class SideMenuViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var menuItems = new List<MenuItem>()
        {
            new MenuItem
            {
                Title = "Dashboard",
                Icon = "<i class=\"ki-duotone ki-element-11 fs-2\"><span class=\"path1\"></span><span class=\"path2\"></span><span class=\"path3\"></span><span class=\"path4\"></span></i>",
                Path = "/Home/Index"
            },
            new MenuItem
            {
                Title = "Blog",
                Icon = "<i class=\"fa-regular fa-folder-open\"></i>",
                GroupName = "Pages",
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem
                    {
                        Title = "Managment",
                        Icon = "<i class=\"fa-regular fa-file-lines\"></i>",
                        Path = "/Blog/Index"
                    }
                }
            },
            new MenuItem
            {
                Title = "Category",
                Icon = "<i class=\"fa-regular fa-folder-open\"></i>",
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem
                    {
                        Title = "Managment",
                        Icon = "<i class=\"fa-regular fa-file-lines\"></i>",
                        Path = "/Category/Index"
                    }
                }
            },
            new MenuItem
            {
                Title = "BlogComment",
                Icon = "<i class=\"fa-regular fa-folder-open\"></i>",
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem
                    {
                        Title = "Managment",
                        Icon = "<i class=\"fa-regular fa-file-lines\"></i>",
                        Path = "/BlogComment/Index"
                    }
                }
            },
            new MenuItem
            {
                Title = "User",
                Icon = "<i class=\"fa-regular fa-folder-open\"></i>",
                GroupName = "User Pages",
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem
                    {
                        Title = "Managment",
                        Icon = "<i class=\"fa-regular fa-file-lines\"></i>",
                        Path = "/User/Index"
                    }
                }
            },
        };

        string currentPath = (HttpContext.Request.Path.Value ?? string.Empty).TrimEnd('/'); ;

        foreach (var menu in menuItems)
        {
            HandleActiveMenu(menu, currentPath);
        }
        return View(menuItems);
    }


    private bool HandleActiveMenu(MenuItem item, string currentPath)
    {
        bool isActive = !string.IsNullOrWhiteSpace(item.Path) && (currentPath.Equals(item.Path, StringComparison.OrdinalIgnoreCase) || currentPath.StartsWith(item.Path + "/", StringComparison.OrdinalIgnoreCase));
        bool hasActiveChild = false;

        if (item.SubMenuItems != null)
        {
            foreach (var child in item.SubMenuItems)
            {
                if (HandleActiveMenu(child, currentPath))
                    hasActiveChild = true;
            }
        }

        item.IsActive = isActive;
        item.HasActiveChild = hasActiveChild;

        return isActive || hasActiveChild;
    }
}
