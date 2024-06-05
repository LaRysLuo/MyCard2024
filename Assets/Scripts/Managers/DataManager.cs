
using System.Collections.Generic;
using UnityEngine;

namespace Larik.CardGame
{
    public class DataManager
    {
        public List<CardData> cardDataList = new();
        public Dictionary<string, CardType> dict = new();

        public DataManager()
        {
            dict.Add("攻击牌", CardType.Attack);
            dict.Add("防御牌", CardType.Defend);
            dict.Add("武器牌", CardType.Weapon);
            dict.Add("装备牌", CardType.Equips);
            dict.Add("姿态牌", CardType.Posture);
            dict.Add("技能牌", CardType.Skill);
            ReadData();


        }

        /// <summary>
        /// 载入Excel卡牌数据
        /// </summary>
        private void ReadData()
        {
            var textAsset = Resources.Load<TextAsset>("Data/CardSource");
            if (!textAsset || textAsset.text == "")
            {
                Debug.LogWarningFormat("没有读取到textAsset数据");
                return;
            }
            string[] rows = textAsset.text.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                if (i == 0 || i == 1) continue; //跳过前面2行
                string row = rows[i];
                string[] rowData = row.Split(',');
                if (rowData[0] == "") continue;

                int id = int.Parse(rowData[0]); //id
                string cardName = rowData[1]; // 卡牌名字
                Debug.Log("cardData" + cardName);
                CardType type = dict[rowData[2]]; //卡牌类型

                string effectTex = rowData[3]; //效果文本
                // string path = string.Format("Images/CardImages/{0}", id);

                cardDataList.Add(new(id, cardName, type, effectTex));
            }
        }

    }
}