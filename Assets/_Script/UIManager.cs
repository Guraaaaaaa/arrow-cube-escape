using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public GameObject cubeObject; // GameObject cha chứa toàn bộ khối lập phương và grid

    private void OnEnable()
    {
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager != null) gridManager.OnAllCleared += HandleWinEffect;
    }
    
    private void OnDisable()
    {
        if (gridManager != null) gridManager.OnAllCleared -= HandleWinEffect;
    }

    private void HandleWinEffect()
    {
        // t=0.0s : Dừng input hoàn toàn (bằng cách khóa cứng isAnimating trong GridManager)
        gridManager.isAnimating = true;

        Sequence winSeq = DOTween.Sequence();

        if (cubeObject != null)
        {
            // t=0.0s : Cube scale up nhẹ để tạo cảm giác "nổ" chiến thắng
            winSeq.Append(cubeObject.transform.DOScale(1.05f, 0.2f).SetEase(Ease.OutBack));

            // t=0.2s : Tất cả ô grid fade out
            Renderer[] gridRenderers = cubeObject.GetComponentsInChildren<Renderer>();
            foreach (var r in gridRenderers)
            {
                // Chỉ fade các ô lưới (dựa vào tag hoặc tên nếu cần phân biệt với background)
                if (r.gameObject.CompareTag("GridCell") || r.gameObject.name.Contains("Cell") || r.gameObject.name.Contains("Grid"))
                {
                    // Đảm bảo material hỗ trợ transparent để DOFade hoạt động
                    winSeq.Join(r.material.DOFade(0f, 0.3f));
                }
            }
        }

        // t=0.5s : Hiện Win Panel (Giao diện UI)
        winSeq.InsertCallback(0.5f, () => 
        {
            Debug.Log(">>> [UI] HIỂN THỊ BẢNG WIN (WIN PANEL) <<<");
            // Kích hoạt gameObject của panel Win ở đây: winPanel.SetActive(true);
        });

        // t=0.5s -> 1.1s : Star animation (Hiệu ứng sao nổ lên)
        winSeq.InsertCallback(0.7f, () => Debug.Log("⭐ Sao 1 xuất hiện!"));
        winSeq.InsertCallback(0.9f, () => Debug.Log("⭐⭐ Sao 2 xuất hiện!"));
        winSeq.InsertCallback(1.1f, () => Debug.Log("⭐⭐⭐ Sao 3 xuất hiện!"));

        // t=1.1s : Hiện nút "Next Level"
        winSeq.InsertCallback(1.1f, () => 
        {
            Debug.Log(">>> [UI] HIỆN NÚT NEXT LEVEL <<<");
            // Kích hoạt nút bấm ở đây
        });
        
        // Tổng cộng quá trình diễn ra chỉ tốn chưa tới 1.5 giây, giúp người chơi không phải chờ đợi lâu.
    }
}
