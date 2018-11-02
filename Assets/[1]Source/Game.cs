//   Project : Battlecruiser3.0
//  Contacts : Pixeye - info@pixeye.games 
//      Date : 8/18/2018


using UnityEngine;
namespace Homebrew
{
    public static partial class Game
    {
        public static int GetEntity(this RaycastHit hit)
        {
            return hit.transform.GetComponentInParent<Actor>().entity;
        }

    }
}

