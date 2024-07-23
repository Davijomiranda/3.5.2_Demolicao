using System;
[Serializable]
public class BraceletModel
{
    public int id;
    public string parent_name;
    public string name;
    public int type;
    public int age;
    public string gender;
    public Accessibility accessibility;
    public string nationality;
    public string region;
    public string city;
    public string language;
    public AvatarModel avatar;
    public string group_hash;
    public string register_date;
    public string register_ok;
}
[Serializable]
public class Accessibility
{
    public bool libras;
    public bool low_vision;
    public bool pcd;
}
[Serializable]
public class AvatarModel
{
    public int keyBody;
    public int keyColorBody;
    public int keyOlho;
    public float keyBodyFat;
    public float keyBodyMuscle;
    public float keyBodyFTorso;
    public float keyBodyFLegs;
    public float keyBodyFFat;
    public float keyBodyFMuscle;
    public int keyHair;
    public int keyColorHair;
    public int keyChapeu;
    public int keyProtese;
    public int keyAcessorioRosto;
    public int keyAcessorioTorso;
    public int keyAcessorioBraco;
    public int keyCalca;
    public int keyColorCalca;
    public int keyCamiseta;
    public int keyColorCamiseta;
    public int keyTenis;
}
/*
 * {
    "id": "50",
    "parent_name": "",
    "name": "Visitante",
    "type": 31,
    "age": "",
    "gender": "",
    "accessibility": {
        "libras": false,
        "low_vision": false,
        "pcd": false
    },
    "nationality": "",
    "region": "",
    "city": "",
    "language": "pt",
    "avatar": "",
    "group_hash": "",
    "register_date": "2024-05-21 15:28",
    "register_ok": "0"
}
 */