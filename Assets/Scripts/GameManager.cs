using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Fields
    static int soundLevel;
    static int vibration;
    static int level;
    static int randomLevel;
    static int previousLevel;
    static int totalCoin;
    #endregion
    public static int Level
    {
        get
        {
            if (!PlayerPrefs.HasKey("level"))
            {
                return 1;
            }
            return PlayerPrefs.GetInt("level");
        }
        set
        {
            level = value;
            PlayerPrefs.SetInt("level", level);
        }
    }

    public static int TotalCoin
    {
        get
        {
            if (!PlayerPrefs.HasKey("totalCoin"))
            {
                return 0;
            }
            return PlayerPrefs.GetInt("totalCoin");
        }
        set
        {
            totalCoin = value;
            PlayerPrefs.SetInt("totalCoin", totalCoin);
        }
    }

    public static int RandomLevel
    {
        get
        {
            if (!PlayerPrefs.HasKey("randomLevel"))
            {
                return 0;
            }
            return PlayerPrefs.GetInt("randomLevel");
        }
        set
        {
            randomLevel = value;
            PlayerPrefs.SetInt("randomLevel", randomLevel);
        }
    }

    public static int PreviousLevel
    {
        get
        {
            if (!PlayerPrefs.HasKey("previousLevel"))
            {
                return 1;
            }
            return PlayerPrefs.GetInt("previousLevel");
        }
        set
        {
            previousLevel = value;
            PlayerPrefs.SetInt("previousLevel", previousLevel);
        }
    }


    public static int Vibration
    {
        get
        {
            if (!PlayerPrefs.HasKey("vibration"))
            {
                return 1;
            }
            return PlayerPrefs.GetInt("vibration");
        }
        set
        {
            vibration = value;
            PlayerPrefs.SetInt("vibration", vibration);
        }
    }
    public static int Sound
    {
        get
        {
            return PlayerPrefs.GetInt("soundLevel");
        }
        set
        {
            soundLevel = value;
            PlayerPrefs.SetInt("soundLevel", soundLevel);
        }
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        if (!PlayerPrefs.HasKey("vibration"))
        {
            PlayerPrefs.SetInt("vibration", 1);
        }
        if (!PlayerPrefs.HasKey("soundLevel"))
        {
            PlayerPrefs.SetInt("soundLevel", 1);
        }
        if (!PlayerPrefs.HasKey("brickIndex"))
        {
            PlayerPrefs.SetInt("brickIndex", 0);
        }
        if (!PlayerPrefs.HasKey("randomLevel"))
        {
            PlayerPrefs.SetInt("randomLevel", 0);
        }
        if (!PlayerPrefs.HasKey("previousLevel"))
        {
            PlayerPrefs.SetInt("previousLevel", 0);
        }
    }
}
