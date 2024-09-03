using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.CommonStuff
{
    public class Enums
    {

    }

    public enum MailType
    {
        varyfication,
        changePassword,
        changePassConfirmation,
        changeEmail
    }

    public enum CodeType
    {
        other,
        emailVeryfication,
        changePassword,
        increaseCurrency,
        changeEmail
    }

    public enum UserType
    {
        person,
        company
    }

    public enum UserRole
    {
        user,
        admin,
        superAdmin,
        mrNobody
    }

    public enum Gender
    {
        [Description("Kobieta")]
        Female,
        [Description("Mężczyzna")]
        Male,
        [Description("Nie będę odpowiadała na pytanie")]
        FemaleLPG,
        [Description("Nie będę odpowiadał na pytanie")]
        MaleLPG
    }

    public enum Guns
    {
        [Description("pistolet B.Z.")]
        pistol,
        [Description("pistolet C.Z.")]
        pistolCentral,
        [Description("karabin B.Z.")]
        carabine,
        [Description("karabin C.Z.")]
        carabineCentral,
        [Description("strzelba")]
        shotgun
    }

    public enum FileStatus
    {
        temp,
        inUse,
        deleted
    }
    public enum ResponseCode
    {
        def,
        conflict
    }

    public enum Permissions
    {
        [Description("Sędzia")]
        Judge,
        [Description("Instruktor")]
        Instructor,
        [Description("Prowadzący")]
        Supervisor,
    }

    public enum Supervisor
    {
        [Description("A - Pneumatyczna")]
        Pneumatic,
        [Description("B - Boczny zapłon")]
        Small,
        [Description("C - Centralny zapłon")]
        Central,
        [Description("D - Maszynowa (pistolet)")]
        MachinePistol,
        [Description("E - Samoczynna")]
        Automatic,
        [Description("F - Gładkolufowa (powtarzalna)")]
        ShotgunRepeater,
        [Description("G - Gładkolufowa")]
        Shotgun,
        [Description("H - Czarnoprochowa")]
        Blackpowder,
    }

}
