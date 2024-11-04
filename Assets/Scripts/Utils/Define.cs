using UnityEngine;


public class Define
{
    public enum Scene
    {
        Unknown,
        Menu,
        Game
    }

    public enum MouseEvent
    {
        None,

        // 전체적으로 사용하는 부분
        Click,

        // 회로 관련 부분
        Select,
        BodyDrag,
        ConnectBeginDrag,
        ConnectDrag,
        ConnectEndDrag,
    }

    public enum Stage
    {
        
    }

    public enum UIEvent
    {
        None,
        Click,
        
        Make,
        Delete,
        Test,

        Menu,
        NextStage,

    }


}
