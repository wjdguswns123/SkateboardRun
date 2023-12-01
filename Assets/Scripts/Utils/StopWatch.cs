#if UNITY_EDITOR

using System.Diagnostics;

public static class StopWatch
{
    public static Stopwatch _stopWatch = new Stopwatch();

    public static void Start()
    {
        _stopWatch.Reset();
        _stopWatch.Start();
    }

    public static void Stop()
    {
        _stopWatch.Stop();
        UnityEngine.Debug.Log(string.Format("측정 시간 : {0} ms", _stopWatch.ElapsedMilliseconds));
    }
}

#endif
