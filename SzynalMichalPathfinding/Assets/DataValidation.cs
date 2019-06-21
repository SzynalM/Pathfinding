using System;

public static class DataValidation
{
    public static void Validate(int amountOfObstacles, int edgeLength, Action<bool> callback)
    {
        callback.Invoke(CanObstacleBePlaced(amountOfObstacles, edgeLength));
    }

    private static bool CanObstacleBePlaced(int amountOfObstacles, int edgeLength)
    {
        return ((edgeLength * edgeLength) - ((amountOfObstacles / 4) + 2)) >= 2;
    }
}
