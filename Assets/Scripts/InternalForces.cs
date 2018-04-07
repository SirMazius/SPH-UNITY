using System.Collections.Generic;
using UnityEngine;

public static class InternalForces
{

    public static void Compute_PressureForce(List<Vector3> l_pForce, List<float> l_density, List<float> l_mass, List<float> l_pressure, List<Vector3> l_pos)
    {
        for (int i = 0; i < l_pos.Count; i++)
        {

            Vector3 pForce = new Vector3();

            for (int j = 0; j < l_pos.Count; j++)
            {
                if (i != j)
                {
                    Vector3 vd = l_pos[i] - l_pos[j];
                    pForce += (l_pressure[i] + l_pressure[j]) / 2 * l_mass[j] / l_density[j] * Kernels.Spiky_kernel_gradient(ref vd);
                }
            }

            l_pForce[i] = -pForce;

        }

    }

    public static void Compute_ViscosityForce(List<Vector3> l_vForce, List<Vector3> l_pos, List<Vector3> l_velocity, List<float> l_mass, List<float> l_density, float viscosity_coeficient)
    {
        for (int i = 0; i < l_pos.Count; i++)
        {
            Vector3 vForce = new Vector3();
            float densityViscosity_factor = viscosity_coeficient / l_density[i];

            for (int j = 0; j < l_pos.Count; j++)
            {
                if (i != j)
                {
                    Vector3 vd = l_pos[i] - l_pos[j];
                    vForce += (l_velocity[j] - l_velocity[i]) * l_mass[j] * Kernels.Viscosity_kernel_laplacian(ref vd);
                }
            }

            l_vForce[i] = vForce * densityViscosity_factor;
        }
    }

}
