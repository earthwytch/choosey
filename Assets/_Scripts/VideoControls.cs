using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]

public class VideoControls : MonoBehaviour 
{
    public string videoURL;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        
        // disable Play on Awake for both vide and audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        
        // assign video clip
        videoPlayer.url = videoURL; 
        //videoPlayer.clip = videoClip;

        // setup AudioSource 
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // render video to main texture of parent GameObject
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";

        videoPlayer.prepareCompleted += PlayVideoWhenPrepared;
        videoPlayer.Prepare();
    }

    void Update()
    {
        // space bar to start / pause
        if (Input.GetButtonDown("Jump"))
            PlayPause();
    }
    
    private void PlayPause()
    {
        if (videoPlayer.isPlaying)
                videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    private void PlayVideoWhenPrepared(VideoPlayer theVideoPlayer)
    {

        theVideoPlayer.Play();
    }
}