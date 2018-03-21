using UnityEngine;

public static class Kernels
{

    static private float h;
    static private float h2;
    static private float h9;
    static private float coef_value;
    static private float coef_gradient;
    static private float coef_laplacian;

    static public void Init_kernel(float threshold)
    {
        h = threshold;
        h2 = h * h;
        h9 = Mathf.Pow(h, 9);
        coef_value = 315 / (64 * Mathf.PI * h9);
        coef_gradient = -945 / (32 * Mathf.PI * h9);
        coef_laplacian = 945 / (64 * Mathf.PI * h9);
    }

    static public float Standard_kernel_value(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return 0f;

        return coef_value * Mathf.Pow((h2 - vd.sqrMagnitude), 3);
    }

    static public Vector3 Spiky_kernel_gradient(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return Vector3.zero;

        return coef_gradient * vd / vd.magnitude * Mathf.Pow((h * vd.magnitude), 2);
    }

    static public float Viscosity_kernel_laplacian(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return 0f;

        return coef_laplacian * (h - vd.magnitude);
    }
}
