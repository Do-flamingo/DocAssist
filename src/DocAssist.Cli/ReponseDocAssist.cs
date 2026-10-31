public enum Certitude { Elevee, Moyenne, Faible }

public record ReponseDocAssist(bool HorsSujet, string Reponse, string[] ObjetsKubernetes, Certitude Certitude);