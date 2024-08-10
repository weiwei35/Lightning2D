using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
public static class Global{
    public static bool GameBegain = false;
    public static bool continueGame = false;
    public static bool isEndLevel = false;
    public static bool isChangeLevel = false;
    public static bool isGameOver = false;

    public static float exp = 0;
    public static int exp_level = 0;
    public static float exp_max = 0;
    public static int heroId = 0;
    public static bool isSlowDown = false;
    public static bool isEndBoss = false;
    public static bool isReward = false;
    public static List<Vector3> papersPosList = new List<Vector3>();
    public static List<GameObject> playerCopyList = new List<GameObject>();
    public static List<SelectItem> item1Current = new List<SelectItem>();
    public static List<SelectItem> item2Current = new List<SelectItem>();
    public static List<SelectItem> item3Current = new List<SelectItem>();
}

public class SaveData
{
    //关卡
    public int levelId = 0;
    public bool isEndLevel = false;
    public List<int> selectItemId = new List<int>();
    //角色
    public int heroId = 0;
    public float speed = 5f;
    public float HP = 100f;
    public float protect = 100f;
    public float HPSpeed = 1f;
    public float protectSpeed = 1f;
    //雷电
    public float lightningTime = 3f;
    public float lightningTimeOriginal = 3f;
    public float lightningCount = 3;
    public float lightningHurt = 0;
    //道具
    public List<int> currentItemId = new List<int>();
    //经验
    public float exp = 0;
    public int exp_level = 0;
}

/*
存档时间：
1.关卡开始时
2.关卡结束时

存档数据：
1.所在关卡信息：关卡id；最后三选一道具（关卡结束保存时）
2.属性信息：角色属性（3个基础）（2个恢复速度）；雷电属性（3个）；所有道具（3类）；经验信息（经验等级；经验值）
*/
public class GameSaveManager : MonoBehaviour
{
    public SaveData data = new SaveData();
    //存放存档文件的那个文件夹的名字
    private const string SaveFolder = "Lightning";
    //存档文件名称（这里要全称）
    private const string SaveFileName = "test.txt";
    private DirectoryInfo directoryInfo;
    
    private void Awake()
    {
        string SavePath = Application.persistentDataPath;//存档地址：C:/Users/vivi/AppData/LocalLow/DefaultCompany/LightningEvent\Lightning
        DontDestroyOnLoad(gameObject);
        //这里的Path.Combine()用来链接路径，实际上用 SavePath + "/" + SaveFloder 表示也行
        directoryInfo = new DirectoryInfo(Path.Combine(SavePath, SaveFolder));
    }
    //保存
    public void Save()
    {        
        //如果该路径不存在就先将其创建出来
        if (!directoryInfo.Exists) 
        {
            directoryInfo.Create();
        }
        
        // string EncryptedData = SaveDataEncryption.EncryInNegation(JsonUtility.ToJson(data));
 
        // File.WriteAllText(Path.Combine(directoryInfo.FullName,SaveFileName),EncryptedData);
        File.WriteAllText(Path.Combine(directoryInfo.FullName,SaveFileName),JsonUtility.ToJson(data));
 
    }
    //加载
    public bool Load()
    {
        if(File.Exists(Application.persistentDataPath+"/"+SaveFolder+"/"+SaveFileName)){
            //与上述Save为逆过程
            // string DecryptedData 
            // = SaveDataEncryption.DecryInNegation(File.ReadAllText(Path.Combine(directoryInfo.FullName, SaveFileName)));
            data = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(directoryInfo.FullName, SaveFileName)));
            return true;
        }else{
            return false;
        }
    }
    //角色死亡删除存档
    public void Delet () {
        if (directoryInfo.Exists) 
        {
            File.Delete(Application.persistentDataPath+"/"+SaveFolder+"/"+SaveFileName);
        }
    }
}

public class SaveDataEncryption
{    
    #region 取反
    public static string EncryInNegation(string json)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(json);
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)(~bytes[i]);
        }
        return Encoding.Unicode.GetString(bytes);
 
    }
    //巧妙的是，取反加密解密正好就是完全相同的过程
    public static string DecryInNegation(string json)
    {
        return EncryInNegation(json);
    }
    #endregion
}