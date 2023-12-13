using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ObjectPiece
{
    private Transform _piece;
    private Collider _col;
    private bool _isDestroyed;
    private List<ObjectPiece> _neighborPieces;
    private float _timeOffsetBetweenSignals;

    public bool GroundPiece;

    public Transform Piece { get { return _piece; } }
    public Collider Col { get { return _col; } }


    public ObjectPiece(Transform piece, float timeOffsetBetweenSignals)
    {
        _piece = piece;
        _timeOffsetBetweenSignals = timeOffsetBetweenSignals;
        _col = _piece.GetComponent<Collider>();
    }


    public void GetNeighborPieces(List<ObjectPiece> pieces)
    {
        _neighborPieces = new();

        var bounds = _piece.GetComponent<MeshCollider>().bounds;
        bounds.extents *= 1.00001f;

        foreach (var otherPiece in pieces)
        {
            if (bounds.Intersects(otherPiece.Col.bounds))
            {
                _neighborPieces.Add(otherPiece);
            }
        }
    }


    public void AddPhysicsToThePiece()
    {
        if (_isDestroyed) return;
        _isDestroyed = true;

        // Add physics to the peice
        _piece.gameObject.layer = DestroyObject.DestructeeLayer.GetLayerNumberFromMask();
        _piece.gameObject.AddComponent<Rigidbody>();
        _piece.transform.SetParent(null);
    }


    private float _lastTime = 0.0f;
    public void SendSignal()
    {
        if (_isDestroyed) return;

        List<ObjectPiece> usedPieces = new();

        _lastTime = Time.time;

        foreach (var piece in _neighborPieces)
            piece.AcceptSignal(_lastTime, usedPieces);
    }


    public void AcceptSignal(float signalTime, List<ObjectPiece> usedPieces)
    {
        if (usedPieces.Contains(this) || _isDestroyed || GroundPiece) return;

        usedPieces.Add(this);

        _lastTime = signalTime;

        foreach (var piece in _neighborPieces)
        {
            piece.AcceptSignal(_lastTime, usedPieces);
        }
    }


    public void RecieveSignal()
    {
        if (_isDestroyed || GroundPiece) return;

        if (_lastTime + _timeOffsetBetweenSignals < Time.time)
        {
            AddPhysicsToThePiece();
        }
    }
}
