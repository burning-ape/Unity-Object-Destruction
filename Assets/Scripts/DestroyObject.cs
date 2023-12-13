using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private float
        _velocityToDestruct = 0.15f,
        _timeOffsetBetweenSignals = 0.03f,
        _groundDetectionRayLength = 1.0f;


    [SerializeField]
    private LayerMask
        _groundLayers,
        _destructeeLayer;

    private List<ObjectPiece> _pieces;

    private PiecesInitializer _pieceInitializer;
    private GroundSignal _groundSignal;

    public static LayerMask GroundLayers { get; private set; }
    public static LayerMask DestructeeLayer { get; private set; }


    private void Awake()
    {
        PrepareObject();

        GroundLayers = _groundLayers;
        DestructeeLayer = _destructeeLayer;

        _pieceInitializer = new(transform, _timeOffsetBetweenSignals);
        _groundSignal = new(_pieceInitializer.GetPieces(), _groundDetectionRayLength);

        _pieces = _pieceInitializer.GetPieces();
    }


    private void PrepareObject()
    {
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {
        _groundSignal.RecieveSignals();
        _groundSignal.SendSignals();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (DestructeeLayer.Contains(other.gameObject.layer)
        && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > _velocityToDestruct)
        {
            var piece = _pieces.Where(t => t.Col == other.contacts[0].thisCollider).FirstOrDefault();
            piece.AddPhysicsToThePiece();
        }
    }
}
