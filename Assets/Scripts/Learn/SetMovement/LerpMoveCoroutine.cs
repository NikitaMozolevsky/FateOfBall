

using System.Collections;
using UnityEngine;

public class LerpMoveCoroutine : MonoBehaviour
{
    
    public Transform mTarget;
    private Vector3 mStartingPos;
    private Vector3 mTargetPos;

    public float desiredDuration;
    public AnimationCurve curve;

    private void Start()
    {
        mStartingPos = transform.position;
        mTargetPos = mTarget.position;

        // Запускаем куратину при старте
        StartCoroutine(MoveTowardsTarget());
    }

    private IEnumerator MoveTowardsTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            transform.position = Vector3.Lerp(mStartingPos, mTargetPos, curve.Evaluate(percentageComplete));
            yield return null;
        }

        // По достижении цели выводим сообщение и, при необходимости, выполняем другие действия
        Debug.Log("Target Reached!");

        // Опционально можно выполнять дополнительные действия после достижения цели

        // Завершаем корутину
        yield break;
    }
    
}