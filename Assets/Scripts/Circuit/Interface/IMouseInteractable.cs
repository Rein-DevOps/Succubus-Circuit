using UnityEngine;

public interface IMouseInteractable
{
    void OnClick(Vector2 mousePosition);
    void OnPress(Vector2 mousePosition);
    void OnRelease(Vector2 mousePosition);
}