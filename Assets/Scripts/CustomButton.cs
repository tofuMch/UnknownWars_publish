using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler,
    IPointerDownHandler
{
    [SerializeField] private AudioClip pushSound;
    [SerializeField] private AudioClip selectSound;
    private AudioSource audioSource;
    
    private float buttonSize;
    private Vector3 buttonPos;
    private float deltaScale;
    private float deltaPos;

    public void SetButtonSize(float size)
    {
        this.transform.localScale = (buttonSize + size) * Vector3.one;
    }

    public void SetDefaultButtonSize(float size)
    {
        buttonSize = size;
        this.transform.localScale = buttonSize * Vector3.one;
    }
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        buttonPos   = transform.position;
        buttonSize  = transform.localScale.x;
        deltaScale  = 0.05f;
        deltaPos    = 5.0f;
    }
    public System.Action onClickCallback;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
    }

    public System.Action onUpCallback;
    public void OnPointerUp(PointerEventData eventData)
    {
        onUpCallback?.Invoke();
        SetButtonSize(0);
    }

    public System.Action onDownCallback;
    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.PlayOneShot(pushSound);
        onDownCallback?.Invoke();
        SetButtonSize(deltaScale);
    }

    public System.Action onEnterCallback;
    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(selectSound);
        onEnterCallback?.Invoke();
        transform.position += deltaPos * Vector3.up;
    }

    public System.Action onExitCallback;
    public void OnPointerExit(PointerEventData eventData)
    {
        onExitCallback?.Invoke();
        transform.position = buttonPos;
    }
}
