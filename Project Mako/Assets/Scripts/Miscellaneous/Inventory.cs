using Mako.Shooting;
using UnityEngine;

namespace Mako.Miscellaneous
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance;
        private void Awake()
        {
            //Singleton method
            if (instance == null)
            {
                //First run, set the instance
                instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else if (instance != this)
            {
                //Instance is not the same as the one we have, destroy old one, and reset to newest one
                Destroy(instance.gameObject);
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        [field: SerializeField] public Shooter[] PrimaryWeapons { get; private set; }
        [field: SerializeField] public Shooter[] SecondaryWeapons { get; private set; }
        public int primaryWeaponIndex = 0;
        public int secondaryWeaponIndex = 0;
        public Shooter GetPrimaryWeapon()
        {
            Shooter returnWeapon = null;
            if (PrimaryWeapons[primaryWeaponIndex] != null)
            {
                returnWeapon = PrimaryWeapons[primaryWeaponIndex];
            }
            else
            {
                returnWeapon = PrimaryWeapons[0];
            }

            return returnWeapon;
        }
        public Shooter GetSecondaryWeapon()
        {
            Shooter returnWeapon = null;
            if (SecondaryWeapons[secondaryWeaponIndex] != null)
            {
                returnWeapon = SecondaryWeapons[secondaryWeaponIndex];
            }
            else
            {
                returnWeapon = SecondaryWeapons[0];
            }
            return returnWeapon;
        }
    }
}