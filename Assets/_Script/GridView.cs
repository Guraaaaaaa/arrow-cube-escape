using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;

    [Header("Animation Settings")]
    [SerializeField] private float slideSpeed = 1.6f; // units per second (tốc độ trượt)

    private void OnEnable()
    {
        if (gridManager == null)
        {
            gridManager = GetComponent<GridManager>();
        }

        if (gridManager != null)
        {
            gridManager.OnSlideRequested += PlaySlideAnimation;
            gridManager.OnInvalidMove += PlayInvalidFeedback;
            gridManager.OnArrowRemoved += HandleArrowRemoved;
        }
    }

    private void OnDisable()
    {
        if (gridManager != null)
        {
            gridManager.OnSlideRequested -= PlaySlideAnimation;
            gridManager.OnInvalidMove -= PlayInvalidFeedback;
            gridManager.OnArrowRemoved -= HandleArrowRemoved;
        }
    }

    // Lưu trữ màu gốc để có thể khôi phục sau khi flash
    private Dictionary<ArrrowController, Color> originalColors = new Dictionary<ArrrowController, Color>();

    /// <summary>
    /// Diễn hoạt mũi tên trượt ra khỏi Grid
    /// </summary>
    private void PlaySlideAnimation(ArrrowController arrow, List<Vector3> path, Action onComplete)
    {
        if (path == null || path.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        // 1. Tính tổng khoảng cách (số ô × cellSize)
        float distance = path.Count * gridManager.cellSize;
        
        // 2. Tính thời gian trượt (duration) giới hạn trong khoảng [0.35s, 0.6s] để không quá nhanh hay quá chậm
        float duration = Mathf.Clamp(distance / slideSpeed, 0.35f, 0.6f);

        // 3. Khởi tạo DOTween Sequence để chạy animation
        Sequence sequence = DOTween.Sequence();
        Vector3[] waypoints = path.ToArray();

        // 3a. Trượt ra khỏi biên
        sequence.Append(arrow.transform.DOPath(waypoints, duration, PathType.Linear)
                .SetEase(Ease.OutQuart)); // Bắt đầu trượt nhanh, chậm dần đều ở cuối đường ray

        // 3b. Exit effect: Nhỏ dần và mờ đi (chạy song song)
        sequence.Append(arrow.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
        
        Renderer renderer = arrow.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            sequence.Join(renderer.material.DOFade(0f, 0.25f));
        }

        // Hoàn thành
        sequence.OnComplete(() => 
        {
            onComplete?.Invoke();
        });
    }

    /// <summary> 
    /// Diễn hoạt rung/lắc khi mũi tên bị chặn
    /// </summary>
    private void PlayInvalidFeedback(ArrrowController arrow)
    {
        // Chống spam click gây trôi vị trí (drift)
        if (DOTween.IsTweening(arrow.transform)) return;

        Sequence sequence = DOTween.Sequence();
        
        // Shake ngang
        sequence.Append(arrow.transform.DOShakePosition(0.3f, strength: 0.05f, vibrato: 10, randomness: 0));

        // Flash màu đỏ
        Renderer renderer = arrow.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            if (!originalColors.ContainsKey(arrow))
            {
                originalColors[arrow] = renderer.material.color;
            }
            Color originalColor = originalColors[arrow];

            sequence.Join(renderer.material.DOColor(Color.red, 0.1f));
            sequence.Append(renderer.material.DOColor(originalColor, 0.2f));
        }
    }

    /// <summary>
    /// Xóa object khỏi màn hình khi nhận được lệnh từ GridManager
    /// </summary>
    private void HandleArrowRemoved(ArrrowController arrow)
    {
        // Có thể nâng cấp bằng Object Pooling sau này, hiện tại dùng Destroy để xoá visual
        Destroy(arrow.gameObject);
    }
}
