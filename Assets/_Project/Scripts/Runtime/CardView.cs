using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Runtime
{
    public class CardView : MonoBehaviour
    {
        private CardData _cardData;
        private MeshRenderer _meshRenderer;
        
        private CardPool _cardPool;

        [Inject]
        private void Construct(CardPool cardPool)
        {
            _cardPool = cardPool;
        }

        public int CardValue => (int)_cardData.rank;

        void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
        
        public void SetCard(CardData newCard)
        {
            _cardData = newCard;
            if (_cardData)
            {
                ApplyCardTexture(_cardData);
            }
        }
        
        private void ApplyCardTexture(CardData card)
        {
            if (!_meshRenderer.material)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                _meshRenderer.material = mat;
            }

            _meshRenderer.material.mainTexture = card.sprite.texture;
        }
    }
}