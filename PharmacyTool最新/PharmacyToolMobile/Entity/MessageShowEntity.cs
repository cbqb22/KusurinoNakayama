using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyToolMobile.Entity
{
    // シリアライズ化しないとSessionに放り込めない
    [Serializable]
    public class MessageShowEntity
    {
        /// <summary>
        /// 表示したいMessage
        /// </summary>
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
}