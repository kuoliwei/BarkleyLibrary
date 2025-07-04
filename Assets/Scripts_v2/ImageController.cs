using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField] private Image image;

    /// <summary>
    /// 顯示圖片並設定 Sprite
    /// </summary>
    public void Show(Sprite sprite)
    {
        if (image == null) return;

        image.sprite = sprite;
        image.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隱藏圖片
    /// </summary>
    public void Hide()
    {
        if (image == null) return;

        image.gameObject.SetActive(false);
    }
}
