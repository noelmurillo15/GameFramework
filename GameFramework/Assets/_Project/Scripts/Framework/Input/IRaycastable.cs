/*
 * IRaycastable -
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

namespace ANM.Framework.Input
{
    public enum CursorType
    {
        None,
        Movement,
        UI,
    }

    public interface IRaycastable
    {
        bool HandleRayCast();
        void OnRaycastExit();
        CursorType GetCursorType();
    }
}
