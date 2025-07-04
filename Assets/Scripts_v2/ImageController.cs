using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField] private Image image;

    /// <summary>
    /// ��ܹϤ��ó]�w Sprite
    /// </summary>
    public void Show(Sprite sprite)
    {
        if (image == null) return;

        image.sprite = sprite;
        image.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ùϤ�
    /// </summary>
    public void Hide()
    {
        if (image == null) return;

        image.gameObject.SetActive(false);
    }
}
