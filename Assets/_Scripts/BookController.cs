using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]

public class BookController : MonoBehaviour 
{

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    [SerializeField] 
    private string _bookTitle;
    public string BookTitle { get => _bookTitle; }

    [SerializeField] 
    private Page _firstPage;
    public Page FirstPage { get => _firstPage; }

    [SerializeField]
    private GameObject _pathOptionPrefab;

    [SerializeField]
    private GameObject _pathOptionContainer;

    private Page _currentPage;
    public Page CurrentPage { get => _currentPage; set => _currentPage = value; }

    private GameObject _instructions;
    
    [SerializeField] 
    private List<Page> _pages;
    public List<Page> Pages { get => _pages; }

    private bool _showingPaths = false;
    private List<GameObject> _availablePathOptions = new List<GameObject>();

    private GameObject _titleBanner;

    void Start()
    {
        Debug.Log("Starting book");

        // initialise variables
        _instructions = GameObject.Find("Instructions");
        _titleBanner = GameObject.Find("BookTitle");
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        // show book title
        if ( _titleBanner ) 
        {
            Debug.Log("Changing banner text");
            Text titleText = _titleBanner.GetComponent<Text>();
            titleText.text = _bookTitle;
        }
    
        // disable Play on Awake for both video and audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        // setup AudioSource 
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // render video to main texture of parent GameObject
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";

        // set up events
        videoPlayer.prepareCompleted += PlayVideoWhenPrepared; // video prepared 
        videoPlayer.loopPointReached += SetCurrentPaths; // video finished

        Debug.Log("Finding the first page to display");
        _currentPage = _firstPage; // TIDY: pass this as a variable to the function


        // change alpha of pathoptions to 0 to make it disappear
        // TODO: need to make function
        Color c = _pathOptionContainer.GetComponent<Image>().color;
        c.a = 0f;
        _pathOptionContainer.GetComponent<Image>().color = c;
      

        PrepareVideo(_currentPage, videoPlayer);

    }

    private void changeVisibility(Component component) {

        Debug.Log("Changing visibility");

    }

    private void SetCurrentPaths(VideoPlayer theVideoPlayer)
    {
        // Get the paths available for the current video that has just finished
        Debug.Log("Displaying all the relevant paths for this page");

        //clear out any existing paths
        _availablePathOptions.Clear();

        // show option container by increasing the alpha
        Color ci = _pathOptionContainer.GetComponent<Image>().color;
        ci.a = .4f;
        _pathOptionContainer.GetComponent<Image>().color = ci;
        //Color ct = 


        // cycle through all the PagePaths and create a radio option
        for( int i = 0; i < _currentPage.PagePaths.Count; i++ ){
            Debug.Log(_currentPage.PagePaths[i].Description);

            // instantiate the radio path option
            GameObject pathOption = Instantiate(_pathOptionPrefab, _pathOptionContainer.transform);

            // get height of path option
            RectTransform rt = pathOption.GetComponent<RectTransform>();
            Debug.Log(rt.rect.height);
            Vector2 currentPosition = rt.anchoredPosition;
            currentPosition.y = -((i + 1) * rt.rect.height);
            rt.anchoredPosition = currentPosition;


            // get radio path option child label text and update it
            Transform pathOptionLabel = pathOption.transform.Find("Label");
            Text pathOptionLabelText = pathOptionLabel.GetComponent<Text>();
            pathOptionLabelText.text = _currentPage.PagePaths[i].Description;

            // set toggle group
            //

            //may need to update tag to get selection to work
            //pathOption.tag =

            // store it in case we need it later - may not need this
            _availablePathOptions.Add(pathOption);


        }

        // let the Update function know it is time to accept input
        _showingPaths = true;
    }


    private void PrepareVideo(Page theCurrentPage, VideoPlayer theVideoPlayer)
    {

        Debug.Log("Preparing the video to play");
        Debug.Log("Current video is...");
        Debug.Log(theCurrentPage.VideoURL);
        // change alpha of option container to show video player
        //
        
        theVideoPlayer.url = theCurrentPage.VideoURL;
        theVideoPlayer.Prepare();
    }

    void Update()
    {
        if(_showingPaths)
        {
            // Debug.Log("Waiting for keyboard selection");
        }


        // space bar to start / pause
        //if (Input.GetButtonDown("Jump"))
        //    PlayPause();
    }

    private void PlayVideoWhenPrepared(VideoPlayer theVideoPlayer)
    {
        theVideoPlayer.Play();
    }
}