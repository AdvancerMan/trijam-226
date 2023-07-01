using UnityEngine;

public class ToddMath {

    public static float EPSILON = 1e-4f;

    public static int Sign(float x) {
        return Sign(x, EPSILON);
    }

    public static int Sign(float x, float epsilon) {
        if (Mathf.Abs(x) < epsilon) {
            return 0;
        }
        
        if (x > 0f) {
            return 1;
        }

        return -1;
    }

    public static Vector2 vector3ToVector2(Vector3 vector3) {
        return new Vector2(vector3.x, vector3.y);
    }

    public static Vector3 vector2ToVector3(Vector2 vector2, float z = 0f) {
        return new Vector3(vector2.x, vector2.y, z);
    }
}
