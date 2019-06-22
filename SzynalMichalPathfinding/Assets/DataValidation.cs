public static class DataValidation
{
    public static bool Validate(int amountOfObstacles, int edgeLength)
    {
        return CanObstacleBePlaced(amountOfObstacles, edgeLength);
    }

    private static bool CanObstacleBePlaced(int amountOfObstacles, int edgeLength)
    {
        return ((edgeLength * edgeLength) - ((amountOfObstacles / 4) + 2)) >= 2;
    }
}