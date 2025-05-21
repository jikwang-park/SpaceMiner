using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCurtain : MonoBehaviour
{
    private readonly static int hashFadeOut = Animator.StringToHash("SetFadeOut");
    private readonly static int hashFadeIn = Animator.StringToHash("SetFadeIn");

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetFade(bool isFadeOut)
    {
        if (isFadeOut)
        {
            gameObject.SetActive(true);
            animator.SetTrigger(hashFadeOut);
        }
        else
        {
            animator.SetTrigger(hashFadeIn);
        }
    }

    // TODO: ���̵��� �ִϸ��̼� �Ϸ� �� Ŀư ��Ȱ��ȭ �̺�Ʈ
    public void OnFadeInEnd()
    {
        gameObject.SetActive(false);
    }
}
