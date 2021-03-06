﻿using UnityEngine;

/*
    TODO: Comprobar los casos en los que se puedan dar divisiones por cero
*/
public static class Kernels
{

    static private float h;
    static private float h2;
    static private float h9;
    static private float h6;
    static private float _3h2;

    static private float coef_normal_value;
    static private float coef_normal_gradient;
    static private float coef_normal_laplacian;

    static private float coef_spiky_gradient;
    static private float coef_viscosity_laplacian;

    static public void Init_kernel(float threshold)
    {
        h = threshold;
        h2 = h * h;
        h9 = Mathf.Pow(h, 9);
        h6 = Mathf.Pow(h, 6);
        _3h2 = 3 * h2;

        coef_normal_value = 315f / (64f * Mathf.PI * h9);
        coef_normal_gradient = -945f / (32f * Mathf.PI * h9);
        coef_normal_laplacian = -945f / (32f * Mathf.PI * h9);

        coef_spiky_gradient = -45f / (Mathf.PI * h6);

        coef_viscosity_laplacian = 45f / (Mathf.PI * h6);
    }

    static public float Standard_kernel_value(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return 0f;

        return coef_normal_value * Mathf.Pow((h2 - vd.sqrMagnitude), 3);
    }

    static public Vector3 Standard_kernel_gradient(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return new Vector3();

        return coef_normal_gradient * vd * Mathf.Pow(h2 - vd.sqrMagnitude, 2);
    }

    static public float Standard_kernel_laplacian(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return 0f;

        return coef_normal_laplacian * (h2 - vd.sqrMagnitude) * (_3h2 - 7 * vd.sqrMagnitude);
    }

    static public Vector3 Spiky_kernel_gradient(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return new Vector3();

        return coef_spiky_gradient * vd / vd.magnitude * Mathf.Pow((h - vd.magnitude), 2);
    }

    static public float Viscosity_kernel_laplacian(ref Vector3 vd)
    {
        float d = vd.magnitude;
        if (d > h)
            return 0f;

        return coef_viscosity_laplacian * (h - vd.magnitude);
    }
}
