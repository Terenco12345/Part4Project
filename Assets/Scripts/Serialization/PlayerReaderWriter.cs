using UnityEngine;
using System.Collections.Generic;
using Mirror;

public static class PlayerReaderWriter
{
    public static void WritePlayer(this NetworkWriter writer, Player player)
    {
        // Player id
        writer.WriteString(player.GetId());

        // Player state
        writer.WriteInt32((int)player.state);

        // Player free items
        writer.WriteInt32(player.freeRoads);
        writer.WriteInt32(player.freeSettlements);

        // Player store
        writer.WriteInt32(player.storeCityNum);
        writer.WriteInt32(player.storeRoadNum);
        writer.WriteInt32(player.storeSettlementNum);

        // Resources
        List<ResourceType> resources = player.GetResources();
        writer.WriteInt32(resources.Count);
        for (int j = 0; j < resources.Count; j++)
        {
            writer.WriteInt32((int)resources[j]);
        }

        // Development cards
        List<DevelopmentCardType> developmentCards = player.GetDevelopmentCards();
        writer.WriteInt32(developmentCards.Count);
        for (int j = 0; j < developmentCards.Count; j++)
        {
            writer.WriteInt32((int)developmentCards[j]);
        }
    }

    public static Player ReadPlayer(this NetworkReader reader)
    {
        Player player = new Player(reader.ReadString());

        player.state = (PlayerState) reader.ReadInt32();

        player.freeRoads = reader.ReadInt32();
        player.freeSettlements = reader.ReadInt32();

        player.storeCityNum = reader.ReadInt32();
        player.storeRoadNum = reader.ReadInt32();
        player.storeSettlementNum = reader.ReadInt32();

        int resourceCount = reader.ReadInt32();
        for (int i = 0; i < resourceCount; i++)
        {
            player.GetResources().Add((ResourceType)reader.ReadInt32());
        }

        int developmentCardCount = reader.ReadInt32();
        for (int i = 0; i < developmentCardCount; i++)
        {
            player.GetDevelopmentCards().Add((DevelopmentCardType)reader.ReadInt32());
        }

        return player;
    }
}
