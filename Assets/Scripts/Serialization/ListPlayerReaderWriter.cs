using UnityEngine;
using System.Collections;
using Mirror;
using System.Collections.Generic;

public static class ListPlayerReaderWriter
{
    public static void WriteListPlayer(this NetworkWriter networkWriter, List<Player> playerList)
    {
        networkWriter.WriteInt32(playerList.Count);
        for (int i = 0; i < playerList.Count; i++)
        {
            PlayerReaderWriter.WritePlayer(networkWriter, playerList[i]);
        }
    }

    public static List<Player> ReadListPlayer(this NetworkReader networkReader)
    {
        List<Player> playerList = new List<Player>();
        int count = networkReader.ReadInt32();
        for(int i = 0; i < count; i++)
        {
            playerList.Add(PlayerReaderWriter.ReadPlayer(networkReader));
        }
        return playerList;
    }
}