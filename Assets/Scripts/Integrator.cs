using System.Collections.Generic;
using UnityEngine;

public static class Integrator
{
    /*
        TODO: HACER UNA COMPROBACION SI SE CAMBIA EL TAMAÑO DEL ARRAY
    */
    //private static List<Vector3> l_finalForce = new List<Vector3>(FluidProperties.n_particles);
    private static Vector3[] l_finalForce;
    private static Vector3[] l_initialV;
    private static Vector3[] l_initialA;

    private static Vector3 aux_aceleration = new Vector3(); // Las acelearciones de este instante

    private static Vector3 aux_forwardV = new Vector3();
    private static Vector3 aux_backwardV = new Vector3();
    

    public static void Initialize_integrator()
    {
        l_finalForce = new Vector3[FluidProperties.n_particles];
        l_initialV = new Vector3[FluidProperties.n_particles];
        l_initialA = new Vector3[FluidProperties.n_particles];

        for (int i = 0; i < FluidProperties.n_particles; i++)
        {
            l_finalForce[i] = new Vector3(); //TODO: esto es 100% necesario? se inicializa abajo
            l_initialV[i] = new Vector3();
            l_initialA[i] = new Vector3();
        }
    }

    public static void Integrate(List<Vector3> l_pForce, List<Vector3> l_vForce, List<Vector3> l_gForce, List<Vector3> l_stForce, List<Vector3> l_pos, List<Vector3> l_vel, List<float> l_density)
    {

        Compute_forces(l_pForce, l_vForce, l_gForce, l_stForce); // Computa las fuerzas por referencia y las cambia

        for (int i = 0; i < l_pos.Count; i++)
        {
            aux_backwardV = l_initialV[i] - 0.5f * FluidProperties.time_delta * l_initialA[i];
            aux_aceleration = l_pForce[i] / l_density[i];
            aux_forwardV = aux_backwardV + FluidProperties.time_delta * aux_aceleration;
            l_pos[i] = l_pos[i] + FluidProperties.time_delta * aux_forwardV;
            l_vel[i] = (aux_forwardV + aux_backwardV) * 0.5f;

            /*
                Actualizamos las velocidades anteriores 
            */
            l_initialV[i] = l_vel[i];
            l_initialA[i] = aux_aceleration;
            
        }
    }

    private static void Compute_forces(List<Vector3> l_pForce, List<Vector3> l_vForce, List<Vector3> l_gForce, List<Vector3> l_stForce)
    {
        for (int i = 0; i < l_pForce.Count; i++)
        {
            //l_finalForce[i] = new Vector3(); //Esto no es necesario
            l_finalForce[i] = (l_pForce[i] + l_vForce[i]) + (l_gForce[i] + l_stForce[i]);
        }
    }



}
