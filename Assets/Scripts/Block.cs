using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Block : MonoBehaviour
{
    /// ブロックが壊れたときのイベント
    public Subject<int> OnBroken = new Subject<int>();

    /// ヒットポイント(ブロックが壊れるまでの回数)
    public ReactiveProperty<int> HitPoint { get; private set; } = new ReactiveProperty<int>(1);

    /// ブロックが壊れたときに加算されるスコア
    public int Score { get; private set; } = 10;

    private void Awake()
    {
        // ボールがぶつかったときの処理
        this.OnCollisionEnter2DAsObservable()
            .Subscribe(
                collision =>
                {
                    // ポイントを減らす
                    this.HitPoint.Value--;
                }
            ).AddTo(this);

        // ヒットポイントが0になったら壊れた判定
        this.HitPoint
            .Where(hp => hp <= 0)
            .Subscribe(
                _ =>
                {
                    //this.IsBroken.Value = true;
                    // スコアを流してオブジェクトを破棄
                    this.OnBroken.OnNext(this.Score);
                    this.OnBroken.OnCompleted();
                    Destroy(this.gameObject);
                }
            ).AddTo(this);
    }
}