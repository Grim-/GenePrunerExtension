using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LichGenePatch
{
    [HarmonyPatch(typeof(Pawn_GeneTracker))]
    [HarmonyPatch("AddGene")]
    public static class Pawn_GeneTracker_AddGene_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn_GeneTracker __instance, ref Gene gene)
        {
            if (gene?.def == null) return true;

            // Check all genes for GeneRemovalModExtension
            foreach (Gene existingGene in __instance.GenesListForReading)
            {
                var modExtension = existingGene.def.GetModExtension<GeneRemovalModExtension>();
                if (modExtension != null && modExtension.IsGeneDisallowed(gene.def))
                {
                    //dont allow this gene to be added
                    return false;
                }
            }

            // If the gene being added has a GeneRemovalModExtension, remove the specified genes
            var newGeneModExtension = gene.def.GetModExtension<GeneRemovalModExtension>();
            if (newGeneModExtension != null)
            {
                List<Gene> genesToRemove = new List<Gene>();

                foreach (GeneDef geneDefToRemove in newGeneModExtension.genesToRemove)
                {
                    Gene geneToRemove = __instance.GetGene(geneDefToRemove);
                    if (geneToRemove != null)
                    {
                        genesToRemove.Add(geneToRemove);
                    }
                }

                foreach (Gene geneToRemove in genesToRemove)
                {
                    __instance.RemoveGene(geneToRemove);
                }
            }

            // otherwise allow the gene to be added by running the original method
            return true;
        }
    }
    //  <modExtensions>
    //  <li Class = "YourModNamespace.GeneRemovalModExtension">
    //      <keyGene> GeneDefName </keyGene>
    //      <genesToRemove>
    //          <li> GeneToRemove1 </li>
    //          <li> GeneToRemove2 </li>
    //          <li> GeneToRemove3 </li>
    //      </genesToRemove>
    //  </li>
    //</modExtensions>
}
