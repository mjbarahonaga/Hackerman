using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AnimSaveGame : MonoBehaviour
{
    public Animator Animator;
    public TextMeshProUGUI TextMesh;
    private int _idParameter;
    private void Start()
    {
        _idParameter = Animator.StringToHash("OnSave");
        GameManager.OnSaveGame += StartAnim;
    }

    private void OnDestroy()
    {
        GameManager.OnSaveGame -= StartAnim;
    }

    public void StartAnim(string text)
    {
        TextMesh.text = text;
        Animator.SetTrigger(_idParameter);
    }
}
