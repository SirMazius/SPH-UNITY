using System.Collections.Generic;
using UnityEngine;

public static class HashTable
{
    static List<int>[] base_array;

    const int prime1 = 73856093;
    const int prime2 = 19349663;
    const int prime3 = 83492791;

    static Vector3 bbMin, bbMax;
    static Vector3 r;
    static Vector3 v3h;
    static int size;
    static float l;

    public static void Initialize()
    {
        v3h = new Vector3();
        r = new Vector3();
        l = FluidProperties.support_radius;
        size = Next_prime(FluidProperties.n_particles * 2);

        base_array = new List<int>[size];

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

        Clean_table();

        for (int i = 0; i < l_pos.Count; i++)
        {
            int index = Hash(l_pos[i]);
            base_array[index].Add(i);
        }
    }

    

    public static int Hash(Vector3 pos)
    {
        r.Set(Mathf.Floor(pos.x / l), Mathf.Floor(pos.y / l), Mathf.Floor(pos.z / l));
        return (int)(r.x * prime1) ^ (int)(r.y * prime2) ^ (int)(r.z * prime3) % size;
    }

    public static void Search_neighbors(List<Vector3> l_pos, List<List<int>> l_neighbors)
    {
        for (int i = 0; i < FluidProperties.n_particles; i++)
        {
            r.x = l_pos[i].x / l * prime1;
            r.y = l_pos[i].y / l * prime2;
            r.z = l_pos[i].z / l * prime3;

            bbMin.x = (r.x * (l_pos[i].x - l));
            bbMin.y = (r.y * (l_pos[i].y - l));
            bbMin.z = (r.z * (l_pos[i].z - l));

            bbMax.x = (r.x * (l_pos[i].x + l));
            bbMax.y = (r.y * (l_pos[i].y + l));
            bbMax.z = (r.z * (l_pos[i].z + l));

            //int count = 0;
            Clean_neighbors(l_neighbors);

            /*
                TODO: COMPROBAR EL FLOOR DEL BBMIN 
                Las R se podrian precalcular?
            */
            //for (v3h.x = (int)bbMin.x; v3h.x < (int)bbMax.x; v3h.x++)
            //{
            //    for (v3h.y = (int)bbMin.y; v3h.y < (int)bbMax.y; v3h.y++)
            //    {
            //        for (v3h.z = (int)bbMin.z; v3h.z < (int)bbMax.z; v3h.z++)
            //        {
            //            int index = Hash(v3h);
            //            foreach (int index2 in base_array[index])
            //                l_neighbors[i].Add(index2);
            //            /* Esto permite borrar elementos para evitar la comprobacion del radio en los kernels
            //            int lenght = l_neighbors[count].Count;
            //            for (int position = 0; position < lenght; position++)
            //            {
            //                if (Vector3.Magnitude(l_pos[index] - l_pos[l_neighbors[count][position]]) > FluidProperties.support_radius)
            //                {
            //                    l_neighbors[count].RemoveAt(position); //< RemoveAt es caro de narices una cola seria mejor probablemente
            //                    lenght--;
            //                    position--;
            //                }
            //            }
            //            count++;
            //            */
            //        }
            //    }
            //}
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
