using System.Collections.Generic;
using UnityEngine;

public class PiecesInitializer
{
    public List<ObjectPiece> _pieces;


    public PiecesInitializer(Transform objectToDestroy, float timeOffsetBetweenSignals)
    {
        _pieces = new();

        for (int i = 1; i < objectToDestroy.childCount; i++)
        {
            var pieceObject = objectToDestroy.GetChild(i);
            PreparePiece(pieceObject.gameObject);

            var piece = new ObjectPiece(pieceObject, timeOffsetBetweenSignals);
            _pieces.Add(piece);
        }

        PiecesPostInitializationProcessing();
    }


    private void PiecesPostInitializationProcessing()
    {
        foreach (var piece in _pieces)
        {
            piece.GetNeighborPieces(_pieces);
        }
    }


    private void PreparePiece(GameObject piece)
    {
        piece.layer = LayerMask.NameToLayer("Destructable");
        piece.AddComponent<MeshCollider>().convex = true;
    }


    public List<ObjectPiece> GetPieces()
    {
        return _pieces;
    }
}
