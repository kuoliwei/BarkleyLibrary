using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Unity.VisualScripting;

public class BookStory : MonoBehaviour
{
    [SerializeField] Sprite[] books;
    [SerializeField] Image book;
    //[SerializeField] VideoPlayer videoPlayer;
    //[SerializeField] VideoClip[] leeVideos;
    [SerializeField] Image storyBack;
    [SerializeField] Text[] storyText;
    [SerializeField] Image tv;
    bool isBookOnTable = false;
    int currentIndex;
    [SerializeField] Text logicConsole;
    [SerializeField] Text jsonConsole;
    [SerializeField] Text boolConsole;
    [SerializeField] Sprite[] firstBook;
    [SerializeField] Sprite[] secondBook;
    Coroutine init;
    Coroutine mainSequence;
    Coroutine storySequence;
    Coroutine welcomeSequence;
    Coroutine waitForBookSequence;
    bool isPersonSitDown = false;
    //[SerializeField] VideoClip welcomeVideo;
    //[SerializeField] VideoClip waitForBookVideo;
    bool isPersonInRoom = false;
    [SerializeField] string wellcomString;
    [SerializeField] string waitForBookString;
    [SerializeField] GameObject wellcomImage;
    Coroutine waitForPersonGetInTheRoomSequence;
    [SerializeField] Sprite welcomeSprite;
    [SerializeField] Sprite waitForBookSprite;
    Dictionary<int, Sprite[]> indexLinesPairs;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip welcomeAudio;
    [SerializeField] AudioClip waitForBookAudio;
    [SerializeField] AudioClip[] barkleyAudios;
    //[SerializeField] Sprite[] firstBookSprites;
    //[SerializeField] Sprite[] secondBookSprites;
    // Start is called before the first frame update
    void Start()
    {
        indexLinesPairs = new Dictionary<int, Sprite[]>();
        indexLinesPairs.Add(1, firstBook);
        indexLinesPairs.Add(2, secondBook);
        waitForPersonGetInTheRoomSequence = StartCoroutine(WaitForPersonGetInTheRoomSequence());
    }
    void Update()
    {
        boolConsole.text = $"isPersonSitDown:{isPersonSitDown}" + "\n" + $"isBookOnTable:{isBookOnTable}" + "\n" + $"isPersonInRoom:{isPersonInRoom}";
        if (ReceivedData.isReceiveData)
        {
            ReceivedData.isReceiveData = false;
            jsonConsole.text = $"json content:{ReceivedData.stringForTest}" + "\n" + $"book index:{ReceivedData.bookData.book.index}" + "\n" + $"person index:{ReceivedData.bookData.person.index}" + "\n" + $"room index:{ReceivedData.bookData.room.index}";
            if (ReceivedData.bookData.room.index == "people" && !isPersonInRoom)
            {
                isPersonInRoom = true;
                StopAllSwquence();
                welcomeSequence = StartCoroutine(WelcomeSequence());
                Debug.Log($"Call WelcomeSequence while person.index = {ReceivedData.bookData.room.index}");
            }
            else if (ReceivedData.bookData.room.index == "null" && isPersonInRoom)
            {
                isPersonInRoom = false;
                isPersonSitDown = false;
                isBookOnTable = false;
                StopAllSwquence();
                waitForPersonGetInTheRoomSequence = StartCoroutine(WaitForPersonGetInTheRoomSequence());
                Debug.Log($"Call WaitForPersonGetInTheRoomSequence while person.index = {ReceivedData.bookData.room.index}");
            }
            if (ReceivedData.bookData.person.index == "sit-down" && !isPersonSitDown && isPersonInRoom)
            {
                isPersonSitDown = true;
                StopAllSwquence();
                waitForBookSequence = StartCoroutine(WaitForBookSequence());
                Debug.Log($"Call WaitForBookSequence while person.index = {ReceivedData.bookData.person.index}");
            }
            else if (ReceivedData.bookData.person.index == "null" && isPersonSitDown && isPersonInRoom)
            {
                isPersonSitDown = false;
                isBookOnTable = false;
                StopAllSwquence();
                welcomeSequence = StartCoroutine(WelcomeSequence());
                Debug.Log($"Call WelcomeSequence while person.index = {ReceivedData.bookData.person.index}");
            }
            int bookIndex;
            if (ReceivedData.bookData.book.index != "null" && int.TryParse(ReceivedData.bookData.book.index, out bookIndex) && !isBookOnTable && isPersonSitDown && isPersonInRoom)
            {
                isBookOnTable = true;
                if (bookIndex <= books.Length && bookIndex > 0)
                {
                    StopAllSwquence();
                    mainSequence = StartCoroutine(MainSequence());
                    Debug.Log($"Call MainSequence while book.index != {ReceivedData.bookData.book.index}");
                    currentIndex = bookIndex - 1;
                }
                else
                {
                    isBookOnTable = false;
                }
            }
            else if (ReceivedData.bookData.book.index == "null" && isBookOnTable && isPersonSitDown && isPersonInRoom)
            {
                isBookOnTable = false;
                StopAllSwquence();
                waitForBookSequence = StartCoroutine(WaitForBookSequence());
                Debug.Log($"Call WaitForBookSequence while book.index = {ReceivedData.bookData.book.index}");
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            logicConsole.gameObject.SetActive(!logicConsole.gameObject.activeSelf);
            jsonConsole.gameObject.SetActive(!jsonConsole.gameObject.activeSelf);
            boolConsole.gameObject.SetActive(!boolConsole.gameObject.activeSelf);
        }
        ReceivedData.isReceiveData = true;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ReceivedData.bookData.book.index = "1";
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ReceivedData.bookData.book.index = "2";
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ReceivedData.bookData.person.index = "sit-down";
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            ReceivedData.bookData.book.index = "null";
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            ReceivedData.bookData.person.index = "null";
        }
        if (Input.GetKey(KeyCode.I))
        {
            ReceivedData.bookData.room.index = "people";
        }
        if (Input.GetKey(KeyCode.R))
        {
            ReceivedData.bookData.room.index = "null";
        }
        if (isBookOnTable && isPersonSitDown && isPersonInRoom)
        {
            int bookIndex;
            if(int.TryParse(ReceivedData.bookData.book.index, out bookIndex) && bookIndex <= books.Length && bookIndex > 0 && currentIndex != bookIndex - 1)
            {
                isBookOnTable = false;
                ReceivedData.isReceiveData = true;
            }
        }
    }
    void StopAllSwquence()
    {
        if (init != null)
        {
            StopCoroutine(init);
            Debug.Log("init stoped");
        }
        if (waitForPersonGetInTheRoomSequence != null)
        {
            StopCoroutine(waitForPersonGetInTheRoomSequence);
            Debug.Log("waitForPersonGetInTheRoomSequence stoped");
        }
        if (welcomeSequence != null)
        {
            StopCoroutine(welcomeSequence);
            Debug.Log("welcomeSequence stoped");
        }
        if (waitForBookSequence != null)
        {
            StopCoroutine(waitForBookSequence);
            Debug.Log("waitForBookSequence stoped");
        }
        if (mainSequence != null)
        {
            StopCoroutine(mainSequence);
            Debug.Log("mainSequence stoped");
        }
        if (storySequence != null)
        {
            StopCoroutine(storySequence);
            Debug.Log("storySequence stoped");
        }
    }
    IEnumerator Init()
    {
        //ReceivedData.bookData.person.index = "null";
        //ReceivedData.bookData.book.index = "null";
        //isPersonSitDown = false;
        //isBookOnTable = false;
        storyBack.gameObject.SetActive(false);
        book.gameObject.SetActive(false);
        tv.gameObject.SetActive(false);
        storyText[0].text = "";
        storyText[1].text = "";
        yield return null;
        //videoPlayer.clip = welcomeVideo;
        //videoPlayer.Stop();
        //yield return new WaitUntil(() => !videoPlayer.isPlaying);
        //videoPlayer.Play();
        //yield return new WaitUntil(() => videoPlayer.isPlaying);
        //videoPlayer.Stop();
        //yield return new WaitUntil(() => !videoPlayer.isPlaying);
    }
    IEnumerator WaitForPersonGetInTheRoomSequence()
    {
        Debug.Log("WaitForPersonGetInTheRoomSequence Start");
        yield return init = StartCoroutine(Init());
        logicConsole.text = "";
        wellcomImage.SetActive(true);
        logicConsole.text += "等待人進房間" + "\n";
        //等待人進房間
        yield return new WaitUntil(() => isPersonInRoom);
        isPersonInRoom = false;
        //偵測到有人進房
        wellcomImage.SetActive(false);
        welcomeSequence = StartCoroutine(WelcomeSequence());
    }
    IEnumerator WelcomeSequence()
    {
        Debug.Log("WelcomeSequence Start");
        yield return init = StartCoroutine(Init());
        wellcomImage.SetActive(false);
        float waitForWellcomSpeachTime = 1f;
        logicConsole.text += $"偵測到有人進房，{waitForWellcomSpeachTime}秒後開始說歡迎詞" + "\n";
        yield return new WaitForSeconds(waitForWellcomSpeachTime);
        logicConsole.text += "開始說歡迎詞" + "\n";
        //Debug.Log("play welcomeVideo");
        {
            audioSource.clip = welcomeAudio;
            audioSource.Play();
            yield return new WaitUntil(() => audioSource.isPlaying);
        }
        //videoPlayer.clip = welcomeVideo;
        //videoPlayer.Play();
        //yield return new WaitUntil(() => videoPlayer.isPlaying);
        //等李登輝說完歡迎詞
        tv.gameObject.SetActive(true);
        tv.sprite = welcomeSprite;
        storyText[0].text = wellcomString;
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        //yield return new WaitUntil(() => !videoPlayer.isPlaying);
        //tv.gameObject.SetActive(false);
        storyText[0].text = "";
        float repeatTime = 30f;
        logicConsole.text += $"歡迎完{repeatTime}秒後再歡迎一次" + "\n";
        yield return new WaitForSeconds(repeatTime);
        welcomeSequence = StartCoroutine(WelcomeSequence());
    }
    IEnumerator WaitForBookSequence()
    {
        Debug.Log("WaitForBookSequence Start");
        yield return init = StartCoroutine(Init());
        Debug.Log("Call WaitForBookSequence From WelcomeSequence");
        logicConsole.text += "等待人坐下" + "\n";
        //等待人坐下
        yield return new WaitUntil(() => isPersonSitDown);
        logicConsole.text += "開始介紹書" + "\n";
        //開始介紹書
        {
            audioSource.clip = waitForBookAudio;
            audioSource.Play();
            yield return new WaitUntil(() => audioSource.isPlaying);
        }
        //videoPlayer.clip = waitForBookVideo;
        //videoPlayer.Play();
        //yield return new WaitUntil(() => videoPlayer.isPlaying);
        //Debug.Log("play waitForBookVideo");
        //等李登輝介紹完書
        //tv.gameObject.SetActive(true);
        //tv.sprite = waitForBookSprite;
        storyText[0].text = waitForBookString;
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        //yield return new WaitUntil(() => !videoPlayer.isPlaying);
        //tv.gameObject.SetActive(false);
        storyText[0].text = "";
        float repeatTime = 30f;
        logicConsole.text += $"介紹完{repeatTime}秒後再介紹一次" + "\n";
        yield return new WaitForSeconds(repeatTime);
        Debug.Log("Call WaitForBookSequence From WaitForBookSequence");
        waitForBookSequence = StartCoroutine(WaitForBookSequence());
    }
    IEnumerator MainSequence()
    {
        Debug.Log("MainSequence Start");
        yield return init = StartCoroutine(Init());
        logicConsole.text += "等待人放書" + "\n";
        //等待人放書
        yield return new WaitUntil(() => isBookOnTable);
        logicConsole.text += $"已放書，在桌前顯示書本{currentIndex}" + "\n";
        //已放書，在桌前顯示書本
        //isBookOnTable = false;
        book.sprite = books[currentIndex];
        book.gameObject.SetActive(true);
        float waitStoryTime = 0f;
        logicConsole.text += $"已顯示書本，{waitStoryTime}秒後開始播影" + "\n";
        //已顯示書本，3秒後開始播影片
        yield return new WaitForSeconds(waitStoryTime);
        {
            audioSource.clip = barkleyAudios[currentIndex];
            audioSource.Play();
            yield return new WaitUntil(() => audioSource.isPlaying);
        }
        //videoPlayer.clip = leeVideos[currentIndex];
        //videoPlayer.Play();
        //yield return new WaitUntil(() => videoPlayer.isPlaying);
        //Debug.Log($"play leeVideos[{currentIndex}]");
        //開始播影片，放字幕
        yield return storySequence = StartCoroutine(StorySequence(currentIndex + 1));
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        //yield return new WaitUntil(() => !videoPlayer.isPlaying);
        float repeatIntroductionBookTime = 30f;
        logicConsole.text += $"影片播完，{repeatIntroductionBookTime}秒後重新說書" + "\n";
        //影片播完，返回開始畫面
        Debug.Log($"wait for {repeatIntroductionBookTime} seconds");
        yield return new WaitForSeconds(repeatIntroductionBookTime);
        Debug.Log("Call mainSequence From MainSequence");
        //waitForBookSequence = StartCoroutine(WaitForBookSequence());
        mainSequence = StartCoroutine(MainSequence());
    }
    IEnumerator StorySequence(int index)
    {
        Debug.Log("StorySequence Start");
        Debug.Log($"{index}.txt");
        Debug.Log(File.Exists(Path.Combine(Application.dataPath, $"{index}.txt")));
        Debug.Log(File.Exists(Path.Combine(Application.dataPath, $"{index}interval.txt")));
        storyBack.gameObject.SetActive(true);
        //{//一次全部顯示
        //    storyText[0].text = File.ReadAllText(Path.Combine(Application.dataPath, $"{index}.txt"));
        //}
        yield return null;
        tv.gameObject.SetActive(true);
        //{//逐句顯示，圖片版
        //    Sprite[] sprites = indexLinesPairs[index];
        //    string[] intervalStrings = File.ReadAllText(Path.Combine(Application.dataPath, $"{index}interval.txt")).Split('\n');
        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        //storyText[0].text = lines[i];
        //        tv.sprite = indexLinesPairs[index][i];
        //        yield return new WaitForSeconds(Convert.ToSingle(intervalStrings[i]));
        //    }
        //    storyText[0].text = "";
        //}
        //tv.gameObject.SetActive(false);
        {//逐句顯示
            string[] lines = File.ReadAllText(Path.Combine(Application.dataPath, $"{index}.txt")).Split('\n');
            string[] intervalStrings = File.ReadAllText(Path.Combine(Application.dataPath, $"{index}interval.txt")).Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                storyText[0].text = lines[i];
                //tv.sprite = firstBook[i - 1];
                yield return new WaitForSeconds(Convert.ToSingle(intervalStrings[i]));
            }
            storyText[0].text = "";
        }
        tv.gameObject.SetActive(true);
        //if (strings.Length % 2 == 0)
        //{
        //    for (int i = 1; i <= strings.Length / 2; i++)
        //    {
        //        int j = i * 2 - 1;
        //        storyText[0].text = strings[j - 1];
        //        storyText[1].text = strings[j];
        //        //tv.sprite = firstBook[i-1];
        //        yield return new WaitForSeconds(15f);
        //    }
        //}
        //else if (strings.Length % 2 != 0)
        //{
        //    for (int i = 1; i <= (strings.Length - 1) / 2; i++)
        //    {
        //        int j = i * 2 - 1;
        //        storyText[0].text = strings[j - 1];
        //        storyText[1].text = strings[j];
        //        //tv.sprite = firstBook[i-1];
        //        yield return new WaitForSeconds(15f);
        //    }
        //    storyText[0].text = strings[strings.Length - 1];
        //}
    }
}
