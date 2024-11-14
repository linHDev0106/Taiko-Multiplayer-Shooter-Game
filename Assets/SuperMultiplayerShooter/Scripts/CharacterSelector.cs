using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visyde
{
    /// <summary>
    /// Character Selector
    /// - displays a list of characters for the character selection screen
    /// </summary>

    public class CharacterSelector : MonoBehaviour
    {
        public CharacterSelectorItem itemPrefab;
        public CharacterData[] characters;
        public Transform content;
        public Connector connector;
        public SampleMainMenu mainMenu;
        public CharacterData[] newFilterCharacters;

        void Start()
        {
            DataCarrier.characters = characters;
            newFilterCharacters = GetFilteredCharacters();
    }

        private CharacterData[] GetFilteredCharacters()
        {
            // Kiểm tra điều kiện tough và agile để trả về dải phần tử phù hợp
            if (!CharacterAndItem.Instance.tough && !CharacterAndItem.Instance.agile)
            {
                // Chỉ lấy phần tử đầu tiên
                return new CharacterData[] { characters[0] };
            }
            else if (CharacterAndItem.Instance.tough && !CharacterAndItem.Instance.agile)
            {
                // Lấy 2 phần tử đầu tiên
                return new CharacterData[] { characters[0], characters[1] };
            }
            else if (!CharacterAndItem.Instance.tough && CharacterAndItem.Instance.agile)
            {
                // Lấy 2 phần tử đầu tiên
                return new CharacterData[] { characters[0], characters[2] };
            }
            else if (CharacterAndItem.Instance.tough && CharacterAndItem.Instance.agile)
            {
                // Lấy cả 3 phần tử
                return characters;
            }

            // Nếu không khớp điều kiện nào, trả về mảng rỗng (trường hợp không mong muốn)
            return new CharacterData[] { characters[0] };
        }

        /// <summary>
        /// Refresh the character selection window.
        /// </summary>
        public void Refresh()
        {
            // Clear items:
            foreach (Transform t in content)
            {
                Destroy(t.gameObject);
            }

            // Repopulate items:
            for (int i = 0; i < newFilterCharacters.Length; i++)
            {
                CharacterSelectorItem item = Instantiate(itemPrefab, content);
                item.data = newFilterCharacters[i];
                item.cs = this;
            }
        }

        // Character selection:
        public void SelectCharacter(CharacterData data)
        {
            // Close the character selection panel:
            mainMenu.characterSelectionPanel.SetActive(false);

            // ...then set the "character using" in the DataCarrier:
            for (int i = 0; i < characters.Length; i++)
            {
                if (data == characters[i])
                {
                    DataCarrier.chosenCharacter = i;
                }
            }

            mainMenu.characterIconPresenter.sprite = data.icon;
        }
    }
}
