using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundSignal
{
    private List<ObjectPiece> _groundPieces, _allPieces;
    private float _groundDetectionRayLength;


    public GroundSignal(List<ObjectPiece> pieces, float groundDetectionRayLength)
    {
        _allPieces = pieces;
        _groundDetectionRayLength = groundDetectionRayLength;

        GetGroundPieces(pieces);
    }


    private void GetGroundPieces(List<ObjectPiece> pieces)
    {
        _groundPieces = pieces.Where(t => t == IsGroundPiece(t, DestroyObject.GroundLayers)).ToList();

        foreach (var piece in _groundPieces)
        {
            piece.GroundPiece = true;
        }
    }


    private ObjectPiece IsGroundPiece(ObjectPiece piece, LayerMask groundLayers)
    {
        if (Physics.Raycast(piece.Piece.position, -Vector3.up, out var hit, maxDistance: _groundDetectionRayLength))
        {
            if (groundLayers == (groundLayers | (1 << hit.collider.gameObject.layer)))
            {
                return piece;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }


    public void SendSignals()
    {
        foreach (var piece in _groundPieces)
            piece.SendSignal();
    }


    public void RecieveSignals()
    {
        foreach (var piece in _allPieces)
            piece.RecieveSignal();
    }
}





