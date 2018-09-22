using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OASystem.Model.Entity;
using OASystem.ViewModel.File;
namespace OASystem.Model
{
    public static class DI
    {

        /// <summary>
        /// Version名
        /// </summary>
        private static string _VersionName;

        public static string VersionName
        {
            get { return DI._VersionName; }
            set { DI._VersionName = value; }
        }


        /// <summary>
        /// 自店舗名
        /// </summary>
        private static string _自店舗名;

        public static  string 自店舗名
        {
            get { return _自店舗名; }
            set { _自店舗名 = value; }
        }

        /// <summary>
        /// 使用するプリンター名
        /// </summary>
        private static string _使用するプリンタ名;

        public static string 使用するプリンタ名
        {
            get { return _使用するプリンタ名; }
            set { _使用するプリンタ名 = value; }
        }

        /// <summary>
        /// 選択トレイ
        /// </summary>
        private static string _選択トレイ;

        public static string 選択トレイ
        {
            get { return DI._選択トレイ; }
            set { DI._選択トレイ = value; }
        }

        /// <summary>
        /// MEDICODE-WebSR FESTA.exeのファイルパス
        /// </summary>
        private static string _MEDICODEWebSRFIlePath;

        public static string MEDICODEWebSRFIlePath
        {
            get { return _MEDICODEWebSRFIlePath; }
            set { _MEDICODEWebSRFIlePath = value; }
        }

        /// <summary>
        /// MEDISから取得した全メーカー名リスト
        /// </summary>
        private static List<string> _メーカー名リスト;

        public static List<string> メーカー名リスト
        {
            get { return DI._メーカー名リスト; }
            set { DI._メーカー名リスト = value; }
        }

        private static List<BalancingAccountsEntity> _帳合先マスタ;

        public static List<BalancingAccountsEntity> 帳合先マスタ
        {
            get 
            {
                if (DI._帳合先マスタ == null)
                {
                    _帳合先マスタ = LoadCenter.Load帳合先マスタ();
                }

                List<BalancingAccountsEntity> ret = new List<BalancingAccountsEntity>();
                foreach (var data in DI._帳合先マスタ)
                {
                    BalancingAccountsEntity bae = new BalancingAccountsEntity();
                    bae.卸ＶＡＮコード = data.卸ＶＡＮコード;
                    bae.帳合先名称 = data.帳合先名称;
                    bae.表示順 = data.表示順;
                    ret.Add(bae);
                }

                return ret; 
            }
            set { DI._帳合先マスタ = value; }
        }


        private static List<IndividualBasedManagementMedicineEntity> _個別管理医薬品マスタ;

        public static List<IndividualBasedManagementMedicineEntity> 個別管理医薬品マスタ
        {
            get
            {
                if (DI._個別管理医薬品マスタ == null)
                {
                    _個別管理医薬品マスタ = LoadCenter.Load個別管理医薬品マスタ();
                }
                return DI._個別管理医薬品マスタ;
            }
            set { DI._個別管理医薬品マスタ = value; }
        }

    }
}
