using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaymentGateWay.Domain.Utilities;

public class SelectListItemComparer : IEqualityComparer<SelectListItem>
{
    public bool Equals(SelectListItem x, SelectListItem y)
    {
        return x.Text == y.Text;
    }

    public int GetHashCode(SelectListItem item)
    {
        int hashText = item.Text == null ? 0 : item.Text.GetHashCode();
        return hashText;
    }
}
