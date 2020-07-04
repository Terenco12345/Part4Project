using UnityEngine;
using System.Collections;
using Mirror;

public static class BoardHandlerReaderWriter
{
    public static void WriteBoard(this NetworkWriter writer, BoardHandler boardHandler)
    {
        writer.WriteInt32(boardHandler.robberCol);
        writer.WriteInt32(boardHandler.robberRow);
        BoardGridReaderWriter.WriteBoard(writer, boardHandler.GetBoardGrid());
    }

    public static BoardHandler ReadBoard(NetworkReader reader)
    {
        BoardHandler boardHandler = new BoardHandler();
        boardHandler.robberCol = reader.ReadInt32();
        boardHandler.robberRow = reader.ReadInt32();
        boardHandler.SetBoardGrid(BoardGridReaderWriter.ReadBoard(reader));
        return boardHandler;
    }
}