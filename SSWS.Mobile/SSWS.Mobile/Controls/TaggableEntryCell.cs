using Xamarin.Forms;

namespace SSWS.Mobile.Controls
{
    public class TaggableEntryCell<T> : EntryCell, ITaggableCell<T>
    {
        public T Tag { get; set; }
    }
}
