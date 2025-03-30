using UraniumUI.Extensions;

namespace UraniumUI.Material.Controls;
public class CollectionTreeViewHierarchicalSelectBehavior : Behavior<CheckBox>
{
    protected override void OnAttachedTo(CheckBox bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.CheckChanged += CheckBox_CheckChanged;
    }
    protected override void OnDetachingFrom(CheckBox bindable)
    {
        base.OnDetachingFrom(bindable);

        bindable.CheckChanged -= CheckBox_CheckChanged;
    }

    private void CheckBox_CheckChanged(object sender, EventArgs e)
    {
        var checkBox = sender as CheckBox;

        var holder = checkBox.FindInParents<CollectionTreeViewNodeHolderView>();

        lock (holder)
        {
            if (holder.TreeView.IsBusy)
            {
                return;
            }

            holder.TreeView.IsBusy = true;

            ApplyHierarchicalSelection(checkBox);

            CheckStateItself(holder);

            holder.TreeView.IsBusy = false;
        }
    }

    protected virtual void ApplyHierarchicalSelection(CheckBox checkBox)
    {
        var holder = checkBox.FindInParents<CollectionTreeViewNodeHolderView>() ?? throw new InvalidOperationException("CheckBox isn't in a TreeView ItemTemplate");

        // 👇 Ensure the node is expanded so children are created
        if (!holder.IsExpanded && !holder.IsLeaf)
        {
            holder.IsExpanded = true;
        }

            var allChildren = holder.TreeView
            .GetChildViewsOf(holder);
        //.FindManyInChildrenHierarchy<CollectionTreeViewNodeHolderView>().ToList();
        //.Where(x => x.ParentHolderView == holder);

        foreach (var child in allChildren)
            {
                var childCheckBox = FindCheckBox(child);
                if (childCheckBox.IsChecked != checkBox.IsChecked)
                {
                    childCheckBox.IsChecked = checkBox.IsChecked;
                    ApplyHierarchicalSelection(childCheckBox);
                }
            }
    }

    protected virtual void CheckStateItself(CollectionTreeViewNodeHolderView holder, bool forcedSemiSelected = false)
    {
        if (holder == null || holder.Parent is null)
        {
            return;
        }

        var mainCheckBox = FindCheckBox(holder);

        if (forcedSemiSelected)
        {
            mainCheckBox.IconGeometry = InputKit.Shared.Controls.PredefinedShapes.Line;
            if (!mainCheckBox.IsChecked)
            {
                mainCheckBox.IsChecked = true;
            }
            return;
        }
        else
        {
            mainCheckBox.IconGeometry = InputKit.Shared.Controls.PredefinedShapes.Check;
        }

        var children = holder.TreeView
             .GetChildViewsOf(holder)
        //.FindManyInChildrenHierarchy<CollectionTreeViewNodeHolderView>()
        //.Where(x => x.ParentHolderView == holder)
        .ToList();

        if (children.Count > 0)
        {
            var firstCheck = FindCheckBox(children[0])?.IsChecked ?? false;

            foreach (var child in children)
            {
                var check = FindCheckBox(child);
                if (check.IsChecked != firstCheck)
                {
                    mainCheckBox.IconGeometry = InputKit.Shared.Controls.PredefinedShapes.Line;
                    if (!mainCheckBox.IsChecked)
                    {
                        mainCheckBox.IsChecked = true;
                    }
                    CheckStateItself(holder.ParentHolderView, true);
                    return;
                }
            }

            if (mainCheckBox.IsChecked != firstCheck)
            {
                mainCheckBox.IsChecked = firstCheck;
            }
        }

        CheckStateItself(holder.ParentHolderView);
    }

    private static CheckBox FindCheckBox(CollectionTreeViewNodeHolderView child)
    {
        if (child.NodeView is CheckBox checkBox)
        {
            return checkBox;
        }

        return child.FindInChildrenHierarchy<CheckBox>();

        throw new InvalidOperationException("CheckBox isn't in a TreeView ItemTemplate");
    }
}

