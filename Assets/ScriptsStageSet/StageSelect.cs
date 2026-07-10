using DG.Tweening;  //DOTweenを使うときはこのusingを入れる
using UnityEngine;

public class StageSelect: MonoBehaviour
{
	public void ExpandScale()
	{
        // カーソル合わせると1秒後に少し大きくなる
		this.transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.OutBack);
	}
	public void ResetScale()
	{
		
	}
}