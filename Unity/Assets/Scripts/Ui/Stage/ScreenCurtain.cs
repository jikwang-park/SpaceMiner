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

    // TODO: 페이드인 애니메이션 완료 시 커튼 비활성화 이벤트
    public void OnFadeInEnd()
    {
        gameObject.SetActive(false);
    }
}
