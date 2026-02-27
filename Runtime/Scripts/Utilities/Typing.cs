using UnityEngine;
using System;

public class Typing
{
    // 指定された文字に対応する指のインデックスを取得する関数
    // 0:左親指、1:左人差し指、2:左中指、3:左薬指、4:左小指、5:右親指、6:右人差し指、7:右中指、8:右薬指、9:右小指
    static public int GetFingerIndex(char input)
    {
        return input switch
        {
            // 左人差し指（1）
            'r' or 'f' or 'v' or 't' or 'g' or 'b' => 1,            
            // 左中指（2）
            'e' or 'd' or 'c' => 2,
            // 左薬指（3）
            'w' or 's' or 'x' => 3,
            // 左小指（4）
            'q' or 'a' or 'z' => 4,
            // 右人差し指（6）
            'y' or 'h' or 'n' or 'u' or 'j' or 'm' => 6,
            // 右中指（7）
            'i' or 'k' => 7,
            // 右薬指（8）
            'o' or 'l' => 8,
            // 右小指（9）
            'p' => 9,
            _ => -1,// 非対応キー
        };
    }

    //  無視する文字を定義
    static public bool IsIgnoreChar(char input)
    {
        char[] englishIgnoreList = { ' ', '.', ',', '!', '?', ':', ';', '\'', '"', '(', ')', '[', ']', '{', '}', '-', '_', '=', '+', '*', '/', '\\', '|', '<', '>', '@', '#', '$', '%', '^', '&', '’', '…' };

        foreach (char ignore in englishIgnoreList)
        {
            if (input == ignore)
            {              
                return true;
            }
        }

        return false;
    }
}