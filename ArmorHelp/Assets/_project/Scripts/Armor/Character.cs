namespace ArmorHelp
{
    public class Character
    {
        private int _wounds, _armorHead, _armorRightHand, _armorLeftHand, _armorBody, _armorRightLeg, _armorLeftLeg;
        private int _shelterArmorPoint, _bWillpower;
        private int _headTotal, _rightHandTotal, _leftHandTotal, _bodyTotal, _rightLegTotal, _leftLegTotal;
        private bool _isHeadSheltered, _isRightHandSheltered, _isLeftHandSheltered, _isBodySheltered, _isRightLegSheltered, _isLeftLegSheltered;

        public int Wounds { get => _wounds; set => _wounds = value; }
        public int ArmorHead { get => _armorHead; set => _armorHead = value; }
        public int ArmorRightHand { get => _armorRightHand; set => _armorRightHand = value; }
        public int ArmorLeftHand { get => _armorLeftHand; set => _armorLeftHand = value; }
        public int ArmorBody { get => _armorBody; set => _armorBody = value; }
        public int ArmorRightLeg { get => _armorRightLeg; set => _armorRightLeg = value; }
        public int ArmorLeftLeg { get => _armorLeftLeg; set => _armorLeftLeg = value; }
        public int ShelterArmorPoint { get => _shelterArmorPoint; set => _shelterArmorPoint = value; }
        public bool IsHeadSheltered { get => _isHeadSheltered; set => _isHeadSheltered = value; }
        public bool IsRightHandSheltered { get => _isRightHandSheltered; set => _isRightHandSheltered = value; }
        public bool IsLeftHandSheltered { get => _isLeftHandSheltered; set => _isLeftHandSheltered = value; }
        public bool IsBodySheltered { get => _isBodySheltered; set => _isBodySheltered = value; }
        public bool IsRightLegSheltered { get => _isRightLegSheltered; set => _isRightLegSheltered = value; }
        public bool IsLeftLegSheltered { get => _isLeftLegSheltered; set => _isLeftLegSheltered = value; }
        public int HeadTotal { get => _headTotal; set => _headTotal = value; }
        public int RightHandTotal { get => _rightHandTotal; set => _rightHandTotal = value; }
        public int LeftHandTotal { get => _leftHandTotal; set => _leftHandTotal = value; }
        public int BodyTotal { get => _bodyTotal; set => _bodyTotal = value; }
        public int RightLegTotal { get => _rightLegTotal; set => _rightLegTotal = value; }
        public int LeftLegTotal { get => _leftLegTotal; set => _leftLegTotal = value; }
        public int BWillpower { get => _bWillpower; set => _bWillpower = value; }
    }
}

