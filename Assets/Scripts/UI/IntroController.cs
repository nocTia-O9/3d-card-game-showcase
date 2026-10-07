using System;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public ObjectEventSO loadMenuEvent;
    PlayableDirector director;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        //动画播放停止时执行事件
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Update()
    {
        //按下空格键时如果动画正在播放，直接停止
         if (Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Playing)
         {
             director.Stop();
         }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        loadMenuEvent.RaiseEvent(null, this);
    }
}
