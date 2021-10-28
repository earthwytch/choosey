using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PagePath : ScriptableObject
{
    // what Page are these options for?
    [SerializeField] 
    private Page _pageIn;
    public Page PageIn { get => _pageIn; }

    // what should we name this path
    [SerializeField] 
    private string _name;
    public string Name { get => _name; }
    
    // what Page should we go to next if all conditions are met?
    [SerializeField] 
    private Page _pageOut;
    public Page PageOut { get => _pageOut; }

    // what text should our user see for this path?
    [SerializeField] 
    private string _description;
    public string Description { get => _description; }

    // TODO: what items and achievements do they need to have before this path becomes available
    // list of items a user must have to make this path available
    //[SerializeField] private List<Item> _itemsRequired = new List<Item>();
    //public ItemsRequired { get => _itemsRequired; }

    // list of achievements a user must have achieved to make this path available
    //[SerializeField] private List<Achievement> _achievementsRequired = new List<Achievement>();
    // public AchievementsRequired { get => _achievementsRequired; }

    // list of items a user must NOT have to make this path available
    // [SerializeField] private List<Item> _itemsProhibited = new List<Item>();
    // public ItemsProhibited { get => _itemsProhibited; }

    // list of achievements a user must have NOT YET achieved to make this path available
    // [SerializeField] private List<Achievement> _achievementsProhibited= new List<Achievement>();
    // public AchievementsProhibited { get => _achievementsProhibited; }


#if UNITY_EDITOR
    // link this path with the correct page when it is created
    public void Initialise(Page pageIn)
    {
        _pageIn = pageIn;
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Rename path")]
    private void RenamePath()
    {
        // set the name of the path to be the same as the name provided
        this.name = _name;

        // save the Path asset
        AssetDatabase.SaveAssets();

        // tell Unity there have been changes that need to be cleaned up
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete path")]
    private void DeletePath()
    {
        // remove this PagePath from the parent Page's list of possible paths
        _pageIn.PagePaths.Remove(this);

        // destroy the path
        Undo.DestroyObjectImmediate(this);

        // save the asset change
        AssetDatabase.SaveAssets();
    }
#endif

}
