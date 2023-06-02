using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Character character { get; set; }
    private bool isButtonPressed = false;

    private void Awake()
    {
        character = FindAnyObjectByType<Character>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonPressed = false;
        character.Jump();
        //character.SetDefaultJumpVector();
        //character.GeneratingJumpVector();
    }

    void Update()
    {
        if (isButtonPressed)
        {
            character.GeneratingJumpVector();
        }
    }
}
