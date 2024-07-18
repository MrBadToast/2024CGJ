using System.Collections;
using UnityEngine;

public class OpponentBehavior : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Animator maskAnimator;

    public OpponentData activeOpponent;
    public Color activeColor = Color.white;

    public void SetNewOpponent(OpponentData opponent,Color color)
    {
        activeOpponent = opponent;
        activeColor = color;
        StopAllCoroutines();
        StartCoroutine(Cor_SetNewOpponent());
    }

    private IEnumerator Cor_SetNewOpponent()
    {
        maskAnimator.Play("OUT");
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sprite = activeOpponent.spr_normal;
        spriteRenderer.color = activeColor;

        maskAnimator.Play("IN");
        yield return new WaitForSeconds(0.5f);
    }

    public void DamageOpponent()
    {
        animator.Play("Hit");
        spriteRenderer.sprite = activeOpponent.getSpr_hit;
        StopCoroutine("Cor_RecoverNormalSprite");
        StartCoroutine("Cor_RecoverNormalSprite");
    }

    public IEnumerator Cor_RecoverNormalSprite()
    {
        yield return new WaitForSeconds(1f);
        spriteRenderer.sprite = activeOpponent.spr_normal;
    }

    public void NeutralizeOpponent()
    {
        animator.Play("Down");
        StopAllCoroutines();
        spriteRenderer.sprite = activeOpponent.spr_neutralized;
    }
}
