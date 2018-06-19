using System.Collections.Generic;
using UnityEngine;

public static class HashTable
{
    static List<int>[] base_array;

    const int prime1 = 73856093;
    const int prime2 = 19349663;
    const int prime3 = 83492791;

    static int h;
    static Vector3 bbMin, bbMax;
    static Vector3 v3h;
    static int size;
    static float l;

    public static FluidEnviroment fluidEnviroment;

    public static void Initialize()
    {
        v3h = new Vector3();
        l = FluidProperties.support_radius * 2;
        Debug.Log("asdad" + l);
        size = Next_prime(FluidProperties.n_particles * 2);

        bbMax = new Vector3();
        bbMin = new Vector3();

        base_array = new List<int>[size];

        fluidEnviroment = GameObject.FindGameObjectWithTag("GameController").GetComponent<FluidEnviroment>();

        for (int i = 0; i < size; i++)
            base_array[i] = new List<int>();
    }

    public static void Clean_table()
    {
        for (int i = 0; i < size; i++)
            base_array[i].Clear();
    }

    public static void Insert(List<Vector3> l_pos)
    {
        Vector3 auxPos = new Vector3();
        Clean_table();

        for (int i = 0; i < l_pos.Count; i++)
        {
            auxPos.Set(l_pos[i].x, l_pos[i].y, l_pos[i].z);

            Discretize(ref auxPos);
            uint index = Hash(ref auxPos);


            //fluidEnviroment.l_spheres[i].GetComponent<Renderer>().material.SetColor("_Color", Color.green);


            base_array[index].Add(i);
        }
    }

    private static void Discretize(ref Vector3 pos)
    {
        pos.x = Mathf.Floor(pos.x / l);
        pos.y = Mathf.Floor(pos.y / l);
        pos.z = Mathf.Floor(pos.z / l);
    }
    /*
        Bounding box calculation
    */
    #region 
    private static void Compute_bbMin(ref Vector3 pos)
    {
        Vector3 discretizedPos = pos;
        Discretize(ref discretizedPos);

        bbMin.x = discretizedPos.x * (pos.x - l);
        bbMin.y = discretizedPos.y * (pos.y - l);
        bbMin.z = discretizedPos.z * (pos.z - l);
    }

    private static void Compute_bbMax(ref Vector3 pos)
    {
        Vector3 discretizedPos = pos;
        Discretize(ref discretizedPos);

        bbMax.x = discretizedPos.x * (pos.x + l);
        bbMax.y = discretizedPos.y * (pos.y + l);
        bbMax.z = discretizedPos.z * (pos.z + l);
    }
    private static void Compute_bBox(ref Vector3 pos)
    {
        Vector3 auxPos = pos;
        //Discretize(ref auxPos);
        bbMax.x = (pos.x + l);
        bbMax.y = (pos.y + l);
        bbMax.z = (pos.z + l);
        Discretize(ref bbMax);

        bbMin.x = (pos.x - l);
        bbMin.y = (pos.y - l);
        bbMin.z = (pos.z - l);
        Discretize(ref bbMin);
    }
    #endregion

    private static void Compute_cell(ref Vector3 pos)
    {
        pos.x = Mathf.Floor((pos.x - bbMin.x) / l);
        pos.y = Mathf.Floor((pos.y - bbMin.y) / l);
        pos.z = Mathf.Floor((pos.z - bbMin.z) / l);
    }

    private static uint Hash(ref Vector3 pos)
    {
        uint h = ((uint)(pos.x * prime1)) ^ ((uint)(pos.y * prime2)) ^ ((uint)(pos.z * prime3));
        return (h % (uint)size);
    }


    public static void Search_neighbors(List<Vector3> l_pos, List<List<int>> l_neighbors)
    {
        Vector3 auxPos = new Vector3();
        float x, y, z;
        Clean_neighbors(l_neighbors);

        for (int i = 512; i < 513; i++)
        {
            /*
                Para cada punto computamos sus bounding box
            */

            auxPos.Set(l_pos[i].x, l_pos[i].y, l_pos[i].z);
            Compute_bBox(ref auxPos);

            /*
                Iteramos sobre ellas
            */

            for (x = bbMin.x; x <= bbMax.x; x += 1)
            {
                for (y = bbMin.y; y <= bbMax.y; y += 1)
                {
                    for (z = bbMin.z; z <= bbMax.z; z += 1)
                    {
                        v3h.Set(x, y, z);
                        uint index = Hash(ref v3h); //< BUG!?
                        Debug.Log(v3h);
                        foreach (int index2 in base_array[index])
                            fluidEnviroment.l_spheres[index2].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);//     h = index2;//l_neighbors[i].Add(index2);
                        #region
                        /* Esto permite borrar elementos para evitar la comprobacion del radio en los kernels
                        int lenght = l_neighbors[count].Count;
                        for (int position = 0; position < lenght; position++)
                        {
                            if (Vector3.Magnitude(l_pos[index] - l_pos[l_neighbors[count][position]]) > FluidProperties.support_radius)
                            {
                                l_neighbors[count].RemoveAt(position); //< RemoveAt es caro de narices una cola seria mejor probablemente
                                lenght--;
                                position--;
                            }
                        }
                        count++;
                        */
                        #endregion
                    }
                }
            }
            fluidEnviroment.l_spheres[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

    }

    private static void Clean_neighbors(List<List<int>> l_neighbors)
    {

        foreach (List<int> l in l_neighbors)
            l.Clear();
        //l_neighbors.Clear();

    }

    private static int Next_prime(int number)
    {
        bool found = false;
        int prime = number;

        while (!found)
        {
            bool searching = true;
            prime++;

            if (prime == 2 || prime == 3)
            {
                found = true;
                return prime;
            }

            if (prime % 2 == 0 || prime % 3 == 0)
            {
                found = false;
                searching = false;
            }


            int divisor = 6;

            while (searching && divisor * divisor - 2 * divisor + 1 <= prime)
            {
                if (prime % (divisor - 1) == 0 || prime % (divisor + 1) == 0)
                {
                    found = false;
                    searching = false;
                }


                divisor += 6;
            }

            if (searching)
            {
                found = true;
            }

        }

        return prime;
    }
}
