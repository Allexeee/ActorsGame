using UnityEngine;
using Homebrew;

namespace MyProject
{
    public static class Phys
    {
        public static readonly RaycastHit[] hits = new RaycastHit[20];

        public static int Raycast(Ray ray)
        {
            return Physics.RaycastNonAlloc(ray, hits);
        }
    }

}