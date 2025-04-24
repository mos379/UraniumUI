namespace UraniumUI.Material.Controls;

public class TreeViewSelectionChangedEventArgs : EventArgs
{
    public IReadOnlyList<object> OldSelection { get; }
    public IReadOnlyList<object> NewSelection { get; }

    public TreeViewSelectionChangedEventArgs(IEnumerable<object> oldSelection, IEnumerable<object> newSelection)
    {
        OldSelection = new List<object>(oldSelection).AsReadOnly();
        NewSelection = new List<object>(newSelection).AsReadOnly();
    }
}
