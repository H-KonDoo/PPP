using DG.Tweening;  //DOTweenを使うときはこのusingを入れる
using UnityEngine;

public class StageSelectMovement : MonoBehaviour
{
	[SerializeField] Vector3 defaultScale = Vector3.one;
	[SerializeField] Vector3 targetScale = Vector3.one;

	public void ExpandScale()
	{
        // カーソル合わせると1秒後に少し大きくなる
		this.transform.DOScale(targetScale,1f).SetEase(Ease.OutBack);
	}
	public void ResetScale()
	{
		// カーソル合わせると1秒後に少し大きくなる
		this.transform.DOScale(defaultScale,1f).SetEase(Ease.OutBack);
	}
}