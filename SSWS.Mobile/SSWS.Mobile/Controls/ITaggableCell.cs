using Xamarin.Forms;

namespace SSWS.Mobile.Controls
{
    public interface ITaggableCell<T>
    {
        T Tag { get; set; }
    }
}
