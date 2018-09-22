using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using OASystem.Model.Enum;

namespace OASystem.ViewModel.Common.DataConvert
{
    public static class DataConvert
    {
        public static string 帳合先名称ToVANコードConvert(string 帳合先名称)
        {
            foreach (var bamaster in Model.DI.帳合先マスタ)
            {
                if (帳合先名称 == bamaster.帳合先名称)
                {
                    return bamaster.卸ＶＡＮコード;
                }
            }

            if (帳合先名称 == "-")
            {
                return 帳合先名称;
            }

            return "";
        }
        public static string VANコードTo帳合先名称Convert(string Vanコード)
        {
            foreach (var bamaster in Model.DI.帳合先マスタ)
            {
                if (Vanコード == bamaster.卸ＶＡＮコード)
                {
                    return bamaster.帳合先名称;
                }
            }

            return "";
        }


        public static string 包装単位ショート変換(string 単位)
        {
            if (単位 == "カプセル")
            {
                return "cap";
                //return "ｶﾌﾟｾﾙ";
            }
            else if (単位 == "バイアル")
            {
                return "ﾊﾞｲｱﾙ";
            }
            else if (単位 == "キット")
            {
                return "ｷｯﾄ";
            }
            else if (単位 == "セット")
            {
                return "ｾｯﾄ";
            }
            else if (単位 == "シリンジ")
            {
                return "シリンジ";
            }
            else if (単位 == "ブリスター")
            {
                return "ﾌﾞﾘｽﾀ";
            }
            else if (単位 == "Ｇ")
            {
                return "g";
            }
            else if (単位 == "ＭＬ")
            {
                return "mL";
            }
            else if (単位 == "Ｌ")
            {
                return "L";
            }
            else if (単位 == "ＣＭ")
            {
                return "cm";
            }
            else if (単位 == "ＫＧ")
            {
                return "kg";
            }
            else if (単位 == "ＭＢＱ")
            {
                return "mbq";
            }
            else
            {
                return 単位;
            }
            

           


        }

        public static 剤形区分Enum 漢字To剤形Enum(string 漢字)
        {
            if (漢字 == "内")
            {
                return 剤形区分Enum.内服;
            }
            else if (漢字 == "外")
            {
                return 剤形区分Enum.外用;
            }
            else if (漢字 == "注")
            {
                return 剤形区分Enum.注射;
            }
            else if (漢字 == "歯")
            {
                return 剤形区分Enum.歯科;
            }
            else
            {
                return 剤形区分Enum.その他;
            }
        }

        public static string 全角To半角変換(string source)
        {
            Regex re = new Regex("[０-９Ａ-Ｚａ-ｚ：－　]+");
            string output = re.Replace(source, myReplacer);

            return output;

        }

        public static string myReplacer(Match m)
        {
            return Strings.StrConv(m.Value, VbStrConv.Narrow, 0);
        }
    }
}
