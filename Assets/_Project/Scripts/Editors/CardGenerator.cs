using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Editors
{
    public class CardGenerator
    {
        [MenuItem("Tools/Generate Cards")]
        public static void GenerateCards()
        {
            string folderPath = "Assets/_Project/Data/Cards";
            
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/_Project/Art/Cards" });

            // Словник для мастей
            var suitDict = new Dictionary<string, CardSuit>()
            {
                { "Hearts", CardSuit.Hearts },
                { "Tiles", CardSuit.Tiles },
                { "Clovers", CardSuit.Clovers },
                { "Pikes", CardSuit.Pikes }
            };

            // Словник для рангу
            var rankDict = new Dictionary<string, CardRank>()
            {
                { "Ace", CardRank.Ace },
                { "King", CardRank.King },
                { "Queen", CardRank.Queen },
                { "Jack", CardRank.Jack },
                { "10", CardRank.Ten },
                { "9", CardRank.Nine },
                { "8", CardRank.Eight },
                { "7", CardRank.Seven },
                { "6", CardRank.Six },
                { "5", CardRank.Five },
                { "4", CardRank.Four },
                { "3", CardRank.Three },
                { "2", CardRank.Two }
            };
            
            foreach (string guid in spriteGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (!sprite) continue;
                
                var card = ScriptableObject.CreateInstance<ScriptableObjects.CardData>();
                card.sprite = sprite;
                
                string fileName = Path.GetFileNameWithoutExtension(path);
                
                foreach (var kvp in suitDict)
                {
                    if (fileName.Contains(kvp.Key))
                    {
                        card.suit = kvp.Value;
                        break;
                    }
                }

                foreach (var kvp in rankDict)
                {
                    if (fileName.Contains(kvp.Key))
                    {
                        card.rank = kvp.Value;
                        break;
                    }
                }
                
                string assetPath = $"{folderPath}/{fileName}.asset";

                CardData existingCard = AssetDatabase.LoadAssetAtPath<CardData>(assetPath);

                if (!existingCard)
                {
                    AssetDatabase.CreateAsset(card, assetPath);
                }
                else
                {
                    Debug.Log($"Карта {fileName} вже існує, пропускаємо створення.");
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Карти згенеровано!");
        }
    }
}