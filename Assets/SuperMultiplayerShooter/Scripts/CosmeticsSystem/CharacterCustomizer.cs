using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Visyde
{
    /// <summary>
    /// Character Customizer
    /// - Handles player customization in the main menu
    /// </summary>

    public class CharacterCustomizer : MonoBehaviour
    {
        public PlayerController playerPrefab;
        public InventorySlot slotPrefab;
        public Transform playerPreviewHandler;
        public Transform slotHandler;

        List<InventorySlot> curSlots = new List<InventorySlot>();   // the current instatiated slots
        PlayerController preview;                                   // the instantiated player prefab for preview

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform t in slotHandler){
                Destroy(t.gameObject);
            }

            //SampleInventory.instance.items.Length
            if (CharacterAndItem.Instance.noel == false && CharacterAndItem.Instance.pirate == false && CharacterAndItem.Instance.hat == false) {

            } else if ((CharacterAndItem.Instance.noel == true && CharacterAndItem.Instance.pirate == false && CharacterAndItem.Instance.hat == false) ||
                (CharacterAndItem.Instance.noel == false && CharacterAndItem.Instance.pirate == true && CharacterAndItem.Instance.hat == false) ||
                (CharacterAndItem.Instance.noel == false && CharacterAndItem.Instance.pirate == false && CharacterAndItem.Instance.hat == true)
                ) {
                for (int i = 0; i < 1; i++)
                {
                    InventorySlot s = Instantiate(slotPrefab, slotHandler);
                    s.cc = this;
                    curSlots.Add(s);
                }
            } else if ((CharacterAndItem.Instance.noel == true && CharacterAndItem.Instance.pirate == true && CharacterAndItem.Instance.hat == false) ||
                (CharacterAndItem.Instance.noel == true && CharacterAndItem.Instance.pirate == false && CharacterAndItem.Instance.hat == true) ||
                (CharacterAndItem.Instance.noel == false && CharacterAndItem.Instance.pirate == true && CharacterAndItem.Instance.hat == true)) {
                for (int i = 0; i < 2; i++)
                {
                    InventorySlot s = Instantiate(slotPrefab, slotHandler);
                    s.cc = this;
                    curSlots.Add(s);
                }
            } else if (CharacterAndItem.Instance.noel == true && CharacterAndItem.Instance.pirate == true && CharacterAndItem.Instance.hat == true) {
                for (int i = 0; i < 3; i++)
                {
                    InventorySlot s = Instantiate(slotPrefab, slotHandler);
                    s.cc = this;
                    curSlots.Add(s);
                }
            }            

            RefreshSlots();
        }

        /// <summary>
        /// Called in the MainMenu's "Customize" button.
        /// </summary>
        public void Open(){

            // Create a preview of the player:
            if (preview) Destroy(preview.gameObject);
            preview = Instantiate(playerPrefab, playerPreviewHandler);
            preview.forPreview = true;
            preview.SetAsPreview();

            RefreshSlots();

            // Refresh the player preview:
            Cosmetics c = new Cosmetics(DataCarrier.chosenHat);
            preview.cosmeticsManager.Refresh(c);
        }

        private List<CosmeticItemData> GetFilteredItems()
        {
            List<CosmeticItemData> filteredItems = new List<CosmeticItemData>();

            if (CharacterAndItem.Instance.noel)
            {
                filteredItems.Add(SampleInventory.instance.items[0]);
            }
            if (CharacterAndItem.Instance.pirate)
            {
                filteredItems.Add(SampleInventory.instance.items[1]);
            }
            if (CharacterAndItem.Instance.hat)
            {
                filteredItems.Add(SampleInventory.instance.items[2]);
            }

            return filteredItems;
        }

        public void RefreshSlots(){
            // Refresh all inventory slots:

            List<CosmeticItemData> newCosmeticItemData = GetFilteredItems();


            for (int i = 0; i < curSlots.Count; i++)
            {
                //curSlots[i].curItem = SampleInventory.instance.items[i];
                curSlots[i].curItem = newCosmeticItemData[i];
                curSlots[i].Refresh();
            }
        }

        /// <summary>
        /// Equips an item from the database:
        /// </summary>
        public void Equip(CosmeticItemData item){
            
            switch (item.itemType){
                case CosmeticType.Hat:
                    int hat = Array.IndexOf(ItemDatabase.instance.hats, item);
                    DataCarrier.chosenHat = DataCarrier.chosenHat == hat ? -1 : hat;    // equipping an already equipped item will unequip it (value of -1 means 'no item')
                    break;
                // add more here...
            }

            RefreshSlots();

            // Refresh the player preview:
            Cosmetics c = new Cosmetics(DataCarrier.chosenHat);
            preview.cosmeticsManager.Refresh(c);
        }
    }
}
