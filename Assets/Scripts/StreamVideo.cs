using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour {

    public RawImage videoImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(1);
            break;
        }
        videoImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

}
