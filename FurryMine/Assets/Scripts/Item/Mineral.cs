using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mineral : MonoBehaviour
{
    public static Action<Mineral> OnPickMineral { get; set; }

    public int Price { get => _price; }

    private Miner _miner;
    private Vector2 _minerPos { get => _miner.transform.position; }
    private Vector2[] _points = new Vector2[4];
    private Vector2 _endPoint = new Vector2();

    private int _price;
    private float _timerMax;
    private float _timerCur;
    private float _speed = 0.5f;
    private float _startNoise = 6f;
    private float _endNoise = 3f;
    private float _worldScale = 0.5f;

    public void Init(Miner miner, int price)
    {
        _price = price;
        _miner = miner;
        _timerCur = 0;
        _timerMax = Random.Range(0.3f, 0.5f);

        _points[0] = transform.position;
        _points[1] = _points[0] + (_startNoise * ((Random.Range(-_worldScale, _worldScale) * Vector2.right) + (Random.Range(-_worldScale, _worldScale) * Vector2.up)));
        _endPoint = _endNoise * ((Random.Range(-_worldScale, _worldScale) * Vector2.right) + (Random.Range(-_worldScale, _worldScale) * Vector2.up));
    }

    private void Update()
    {
        _timerCur += Time.deltaTime * _speed;
        _points[2] = _minerPos + _endPoint;
        _points[3] = _minerPos;
        transform.position = new Vector2(
            CubicBezierCurve(_points[0].x, _points[1].x, _points[2].x, _points[3].x),
            CubicBezierCurve(_points[0].y, _points[1].y, _points[2].y, _points[3].y)
            );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Consts.MinerTag))
            return;
        collision.GetComponent<Miner>().PlusPrice(this);
        OnPickMineral(this);
    }

    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        // (0~1)의 값에 따라 베지어 곡선 값을 구하기 때문에, 비율에 따른 시간을 구했다.
        float t = _timerCur / _timerMax; // (현재 경과 시간 / 최대 시간)

        // 방정식.
        /*
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
        */

        // 이해한대로 편하게 쓰면.
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }
}
