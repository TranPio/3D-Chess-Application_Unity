using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObjectTweenwer))]
[RequireComponent(typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
    private MaterialSetter materialSetter;
    public Board board { protected get; set; }
    public Vector2Int occupiedSquare { get; set; }
    public TeamColor team { get; set; }
    public bool hasMoved { get; private set; }
    public List<Vector2Int> avaliableMoves;
    private IObjectTweenwer tweener;
    public abstract List<Vector2Int> SeclectAvaliableSquare();
    private void Awake()
    {
        avaliableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweenwer>();
        materialSetter = GetComponent<MaterialSetter>();
        hasMoved = false;
    }

    public void SetMaterial(Material material)
    {
        materialSetter.SetSingleMaterial(material);
    }

    public bool IsFromSameTeam(Piece piece)
    {
        return team == piece.team;
    }

    public bool CanMoveTo(Vector2Int coords)
    {
        return avaliableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {

    }

    protected void TryToAddMove(Vector2Int coords)
    {
        avaliableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords,TeamColor team, Board board)
    {
        this.team = team;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePositionFromCoordes(coords);
    }
}
