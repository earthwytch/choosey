using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Page", menuName = "Chooser/Page", order = 1)]
public class Page : ScriptableObject
{
    
    // what video url should we display on this page
    [SerializeField] 
    private string _videoURL;
    public string VideoURL { get => _videoURL; }

    // is this the end of the story? There should only be 1 start page.
    [SerializeField] 
    private bool _isFirst;
    public bool IsFirst { get => _isFirst; }

    // is this the end of the story? There could be multiple end pages.
    [SerializeField] 
    private bool _isEnd;
    public bool IsEnd { get => _isEnd; }

    // what paths can we take from this page? TODO: add getters
    [SerializeField] 
    private List<PagePath> _pagePaths = new List<PagePath>();
    public List<PagePath> PagePaths { get => _pagePaths; set => _pagePaths = value; }

#if UNITY_EDITOR
    // add option to create paths for this page using the editor
    [ContextMenu("New path")]
    private void MakeNewPath()
    {
        // get a new instance of a PagePath
        PagePath newPagePath = ScriptableObject.CreateInstance<PagePath>();

        // add a temporary name for the PagePath
        newPagePath.name = "Path";

        // call PagePath's initialise function, passing in the current Page
        newPagePath.Initialise(this);

        // add this new PagePath to the list of paths for this page
        _pagePaths.Add(newPagePath);

        // add the new PagePath under the current Page Assets
        AssetDatabase.AddObjectToAsset(newPagePath, this);
        AssetDatabase.SaveAssets();

        // tell Unity there have been changes that need to be cleaned up
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(newPagePath);

    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Delete all")]
    private void DeleteAll()
    {
        // cycle through all the PagePaths for this Page, counting backwards down to 0
        for (int i = _pagePaths.Count; i-- > 0;)
        {
            // copy the PagePath to a temp holder variable
            PagePath tmp = _pagePaths[i];

            // remove it from the Page's list of available PagePaths
            _pagePaths.Remove(tmp);

            // delete that sucker, then rinse and repeat
            Undo.DestroyObjectImmediate(tmp);
        }

        // save the changes
        AssetDatabase.SaveAssets();
    }
#endif

}
