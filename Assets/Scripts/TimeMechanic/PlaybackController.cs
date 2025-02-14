//using System.Collections.Generic;
//using UnityEngine;

//public class PlaybackController
//{
//    private static PlaybackController _instance;

//    private PlaybackController() { }

//    public static PlaybackController Instance => _instance ??= new PlaybackController();

//    public bool isPlayback = false;
//    private int _playbackFrame = 0;
//    private int _playbackSpeed = 0;
//    private List<TransformRecorder> _recorders = new List<TransformRecorder>();

//    public int Frame => _playbackFrame;
//    public int PlaybackSpeed => _playbackSpeed;

//    public void RegisterRecorder(TransformRecorder recorder)
//    {
//        if (!_recorders.Contains(recorder))
//        {
//            _recorders.Add(recorder);
//        }
//    }

//    public void UnregisterRecorder(TransformRecorder recorder)
//    {
//        _recorders.Remove(recorder);
//    }

//    public void Pause()
//    {
//        isPlayback = false;
//    }

//    public void Resume()
//    {
//        isPlayback = true;
//    }

//    private void RestoreFrame()
//    {
//        foreach (var recorder in _recorders)
//        {
//            recorder.RestoreFrame(_playbackFrame);
//        }
//    }

//    public void Update()
//    {
//        if (isPlayback)
//        {
//            _playbackFrame = Mathf.Max(0, _playbackFrame + _playbackSpeed);
//            RestoreFrame();
//        }
//        else
//        {
//            _playbackFrame++;
//        }
//    }
//}
