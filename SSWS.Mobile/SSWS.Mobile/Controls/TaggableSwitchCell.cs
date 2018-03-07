using Xamarin.Forms;

namespace SSWS.Mobile.Controls
{
    public class TaggableSwitchCell<T> : SwitchCell, ITaggableCell<T>
    {
        public T Tag { get; set; }
    }
}
