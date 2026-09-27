using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform weaponPoint;


    [SerializeField] private GameObject[] weaponSlots = new GameObject[3];
    private int currentSlotIndex = 0;


    /*    [Header("Heal Charge")]
        [SerializeField] private int currentHealAmount = 0;
        [SerializeField] private TMP_Text healthDropHeldText;
        [SerializeField] private GameObject healthDropVisual;

        private Collider2D currentKnowledge = null;
        private float _pendingHealPower = 0f;

        public int CurrentHealAmount => currentHealAmount;

        */
    public int CurrentSlotIndex => currentSlotIndex;
    public GameObject[] WeaponSlots => weaponSlots;

    private void Awake()
    {
        if (weaponPoint == null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.name.Equals("WeaponPoint", System.StringComparison.OrdinalIgnoreCase))
                {
                    weaponPoint = child;
                    break;
                }
            }
        }


        if (weaponPoint == null)
        {
            Debug.LogError("[PlayerInventory] WeaponPoint not found. Please create a child object named 'WeaponPoint'.");
        }
    }

    /* private void Start()
    {
        UpdateVisuals();
    }
    */

    private void Update()
    {
        HandleSlotShootInput();
        // HandleHealChargeInput(); // dinonaktifkan sementara, butuh HealthSystem (nanti aja)
    }

    // TODO: aktifkan lagi setelah HealthSystem ditambahkan
    /*
    private void HandleHealChargeInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentHealAmount > 0 && currentKnowledge != null)
        {
            HealthSystem knowledgeHealth = currentKnowledge.GetComponent<HealthSystem>();

            if (knowledgeHealth != null)
            {
                float healPerCharge = _pendingHealPower / currentHealAmount;

                if (knowledgeHealth.Heal(healPerCharge))
                {
                    currentHealAmount--;
                    _pendingHealPower -= healPerCharge;
                    UpdateVisuals();
                    Debug.Log($"[PlayerInventory] Knowledge di-heal {healPerCharge} HP! Sisa charge: {currentHealAmount}");
                }
                else
                {
                    Debug.Log("[PlayerInventory] Nyawa Knowledge sudah penuh.");
                }
            }
        }
    }
    */

    private void HandleSlotShootInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToSlot(2);
    }

    public void PickupWeapon(BaseWeapon weapon)
    {
        int slotToUse = currentSlotIndex;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                slotToUse = i;
                break;
            }
        }

        if (weaponSlots[slotToUse] != null)
        {
            // Drop senjata lama (aktifkan physics & buang dari slot)
            DropWeapon(weaponSlots[slotToUse]);
        }

        Transform targetParent = weaponPoint != null ? weaponPoint : transform;
        weapon.EquipTo(targetParent);

        weaponSlots[slotToUse] = weapon.gameObject;
        SwitchToSlot(slotToUse);

        Debug.Log($"[PlayerInventory] Picked up {weapon.gameObject.name} in slot {slotToUse + 1}");
    }

    private void DropWeapon(GameObject weaponObj)
    {
        BaseWeapon baseWeapon = weaponObj.GetComponent<BaseWeapon>();
        if (baseWeapon != null)
        {
            baseWeapon.Unequip();
        }
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if (weaponData == null || weaponData.WeaponPrefab == null)
        {
            Debug.LogWarning("[PlayerInventory] Cannot add null weapon or weapon without prefab.");
            return;
        }

        int slotToUse = currentSlotIndex;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                slotToUse = i;
                break;
            }
        }

        if (weaponSlots[slotToUse] != null)
        {
            Destroy(weaponSlots[slotToUse]);
        }

        Transform targetParent = weaponPoint != null ? weaponPoint : transform;
        GameObject newWeapon = Instantiate(weaponData.WeaponPrefab, targetParent);

        BaseWeapon baseWeapon = newWeapon.GetComponent<BaseWeapon>();
        if (baseWeapon != null)
        {
            baseWeapon.EquipTo(targetParent);
            baseWeapon.Setup(weaponData);
        }

        weaponSlots[slotToUse] = newWeapon;
        SwitchToSlot(slotToUse);

        Debug.Log($"[PlayerInventory] Equipped {weaponData.WeaponName} in slot {slotToUse + 1}");
    }

    private void SwitchToSlot(int slotIndex)
    {
        currentSlotIndex = slotIndex;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlots[i].SetActive(i == currentSlotIndex);
            }
        }
    }

    // Untuk dipanggil dari UI jika perlu
    public string CurrentWeaponName
    {
        get

        {
            if (weaponSlots[currentSlotIndex] != null)
            {
                BaseWeapon bw = weaponSlots[currentSlotIndex].GetComponent<BaseWeapon>();
                // asumsi WeaponData dipass lewat Setup dan punya field weaponName
                // Sayangnya WeaponData itu private di BaseWeapon. 
                // Biar aman, return nama gameobject saja dulu (hapus (Clone))
                return weaponSlots[currentSlotIndex].name.Replace("(Clone)", "");

            }
            return "None";
        }
    }

    /* public void AddHealCharge(float healPower)
    {
        currentHealAmount++;
        _pendingHealPower += healPower;
        UpdateVisuals();
    }


    private void OnTriggerEnter2D(Collider2D knowledge)
    {
        if (knowledge.CompareTag("Knowledge"))
        {
            currentKnowledge = knowledge;
        }
    }

    private void OnTriggerExit2D(Collider2D knowledge)
    {
        if (knowledge.CompareTag("Knowledge") && currentKnowledge == knowledge)
        {
            currentKnowledge = null;
        }
    }

    /* private void UpdateVisuals()
    {
        if (healthDropHeldText != null)
        {
            healthDropHeldText.text = currentHealAmount.ToString();
        }

        if (healthDropVisual != null)
        {
            healthDropVisual.SetActive(currentHealAmount > 0);
        }
    }
    */
}
