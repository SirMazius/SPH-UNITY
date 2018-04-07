using System.Collections.Generic;
using UnityEngine;

public static class ExternalForces
{

    public static void Compute_gravity(List<Vector3> l_gForce, List<float> l_density, Vector3 gravity_factor)
    {
        for (int i = 0; i < l_density.Count; i++)
        {
            l_gForce[i] = l_density[i] * gravity_factor;
        }
    }

    public static void Compute_normal(List<Vector3> l_normal, List<Vector3> l_pos, List<float> l_mass, List<float> l_density)
    {
        for (int i = 0; i < l_pos.Count; i++)
        {
            l_normal[i].Set(0f, 0f, 0f);
            for (int j = 0; j < l_pos.Count; j++)
            {
                Vector3 vd = l_pos[i] - l_pos[j];
                l_normal[i] += l_mass[j] / l_density[j] * Kernels.Standard_kernel_gradient(ref vd);
            }
        }
    }

    public static void Compute_color_laplacian(List<Vector3> l_pos, List<float> l_mass, List<float> l_density, List<float> l_colorLaplacian)
    {
        for (int i = 0; i < l_pos.Count; i++)
        {
            l_colorLaplacian[i] = 0f;
            for (int j = 0; j < l_pos.Count; j++)
            {
                Vector3 vd = l_pos[i] - l_pos[j];
                l_colorLaplacian[i] += l_mass[j] / l_density[j] * Kernels.Standard_kernel_laplacian(ref vd);
            }
        }
    }

    public static void Compute_surfacte_tension(List<Vector3> l_normal, List<Vector3> l_stForce, List<float> l_color_laplacian)
    {
        for (int i = 0; i < l_normal.Count; i++)
        {
            if (FluidProperties.surfaceTension_threshold <= l_normal[i].magnitude)
                l_stForce[i] = -FluidProperties.surfaceTension_factor * l_color_laplacian[i] * l_normal[i] / l_normal[i].magnitude;
            
        }
    }
}
