using GridShared;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Applications.Results;

namespace SD.Web.Components
{
    public class ButtonCellViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(object Item, IGrid Grid, object Object)
        {
            Guid id = ((MovieDto)Item).Id;
            ViewData["gridState"] = Grid.GetState();
            ViewData["returnUrl"] = (string)Object;

            var factory = Task<IViewComponentResult>.Factory;
            return await factory.StartNew(() => View(id));
        }
    }
}
