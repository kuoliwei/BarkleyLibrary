[System.Serializable]
public struct BookData
{
    public Book book;
    public Person person;
    public Room room;
}
[System.Serializable]
public struct Book
{
    public string index;
}
[System.Serializable]
public struct Person
{
    public string index;
}
[System.Serializable]
public struct Room
{
    public string index;
}
public static class ReceivedData
{
    public static string stringForTest;
    public static bool isReceiveData = false;
    public static BookData bookData = new BookData();
}

