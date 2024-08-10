using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

public class Startup
{
[MenuItem("工具/编译道具配置")]
    public static void 编译道具配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName = "ItemData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        ItemDataSO itemData = (ItemDataSO)ScriptableObject.CreateInstance(typeof(ItemDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["道具"];
            //遍历每一行
            for (int i = worksheet.Dimension.Start.Row+3; i <= worksheet.Dimension.End.Row; i++)
            {
                SelectItem item = new SelectItem();
                //获取每列数据的数据类型
                Type itemType = typeof(SelectItem);
                //遍历每一列
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType.GetField(worksheet.GetValue(1, j).ToString());
                    string itemValue = "";
                    if (worksheet.GetValue(i, j) != null){
                        itemValue = worksheet.GetValue(i, j).ToString();
                    }
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                itemData.items.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName +".asset");
            if(File.Exists("Assets/Resources/" + assetName +".meta"))
                File.Delete("Assets/Resources/" + assetName +".meta");
            AssetDatabase.CreateAsset(itemData,"Assets/Resources/" + assetName +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(itemData,"Assets/Resources/" + assetName +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译经验配置")]
    public static void 编译经验配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "ExpLevelData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        ExpLevelDataSO expLevelData = (ExpLevelDataSO)ScriptableObject.CreateInstance(typeof(ExpLevelDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["经验"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                ExpLevel item = new ExpLevel();
                //获取每列数据的数据类型
                Type itemType1 = typeof(ExpLevel);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                expLevelData.items.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(expLevelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(expLevelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译波次配置")]
    public static void 编译波次配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "EnemyGroupData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        EnemyGroupDataSO enemyGroupData = (EnemyGroupDataSO)ScriptableObject.CreateInstance(typeof(EnemyGroupDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["波次"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                EnemyLevelGroup item = new EnemyLevelGroup();
                //获取每列数据的数据类型
                Type itemType1 = typeof(EnemyLevelGroup);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                enemyGroupData.enemyLevelGroups.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(enemyGroupData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(enemyGroupData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译道具概率配置")]
    public static void 编译道具概率配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "ItemLevelRateData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        ItemLevelRankSO itemLevelRankData = (ItemLevelRankSO)ScriptableObject.CreateInstance(typeof(ItemLevelRankSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["道具概率"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                ItemLevelRank item = new ItemLevelRank();
                //获取每列数据的数据类型
                Type itemType1 = typeof(ItemLevelRank);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                itemLevelRankData.itemLevelRanks.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(itemLevelRankData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(itemLevelRankData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
[MenuItem("工具/编译英雄配置")]
    public static void 编译英雄配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "HeroListData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        HeroListDataSO heroListData = (HeroListDataSO)ScriptableObject.CreateInstance(typeof(HeroListDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["英雄"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                HeroListData item = new HeroListData();
                //获取每列数据的数据类型
                Type itemType1 = typeof(HeroListData);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                heroListData.heros.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(heroListData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(heroListData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

[MenuItem("工具/编译怪物配置")]
    public static void 编译怪物配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "LevelEnemyData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        LevelEnemyDataSO levelEnemyData = (LevelEnemyDataSO)ScriptableObject.CreateInstance(typeof(LevelEnemyDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["怪物"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                LevelEnemy item = new LevelEnemy();
                //获取每列数据的数据类型
                Type itemType1 = typeof(LevelEnemy);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    if(typeInfo.FieldType.IsArray){
                        string[] numbersSplit = itemValue.Split(','); // 使用逗号分割字符串
                        int[] numbersArray = new int[numbersSplit.Length]; // 创建整型数组
                        for (int numberId = 0; numberId < numbersSplit.Length; numberId++)
                        {
                            if (int.TryParse(numbersSplit[numberId], out int number)) // 尝试转换字符串到整数
                            {
                                numbersArray[numberId] = number; // 成功转换，将整数放入数组
                            }
                        }
                        typeInfo.SetValue(item,Convert.ChangeType(numbersArray,typeInfo.FieldType));
                    }else{
                        typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                    }
                }
                //当前行赋值结束，添加到列表中
                levelEnemyData.enemys.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(levelEnemyData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(levelEnemyData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

[MenuItem("工具/编译关卡配置")]
    public static void 编译关卡配置()
    {
        string path = Application.dataPath + "/Editor/ItemData.xlsx";
        string assetName2 = "LevelData";
        FileInfo fileInfo = new FileInfo(path);
        //创建so类
        LevelDataSO levelData = (LevelDataSO)ScriptableObject.CreateInstance(typeof(LevelDataSO));
        //打开Excel文件，using会在使用完毕后关闭文件
        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //选取表单
            ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets["关卡"];
            //遍历每一行
            for (int i = worksheet2.Dimension.Start.Row+3; i <= worksheet2.Dimension.End.Row; i++)
            {
                LevelData item = new LevelData();
                //获取每列数据的数据类型
                Type itemType1 = typeof(LevelData);
                //遍历每一列
                for (int j = worksheet2.Dimension.Start.Column; j <= worksheet2.Dimension.End.Column; j++)
                {
                    //用反射对item赋值，将数据类型附加到赋值内容中
                    FieldInfo typeInfo = itemType1.GetField(worksheet2.GetValue(1, j).ToString());
                    string itemValue = worksheet2.GetValue(i, j).ToString();
                    typeInfo.SetValue(item,Convert.ChangeType(itemValue,typeInfo.FieldType));
                }
                //当前行赋值结束，添加到列表中
                levelData.levels.Add(item);
            }
        }
        //保存SO文件
        if(File.Exists("Assets/Resources/" + assetName2 +".asset"))
        {
            File.Delete("Assets/Resources/" + assetName2 +".asset");
            if(File.Exists("Assets/Resources/" + assetName2 +".meta"))
                File.Delete("Assets/Resources/" + assetName2 +".meta");
            AssetDatabase.CreateAsset(levelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }else{
            AssetDatabase.CreateAsset(levelData,"Assets/Resources/" + assetName2 +".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
