using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    [Header("Animations")]
    private Animator animator;
    private Image image;

    [Header("Variables")]
    public int healthThreshold;
    [SerializeField] private int playerInt;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Player Number", playerInt);
        image = GetComponent<Image>();
    }

    public void PopHeart()
    {
        animator.SetTrigger("Pop");
    }

    public void DestroyHeart()
    {
        image.enabled = false;
    }
}
