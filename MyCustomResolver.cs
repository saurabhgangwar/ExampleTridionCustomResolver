using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ContentManager;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Publishing;
using Tridion.ContentManager.Publishing.Resolving;

namespace ExampleTridionCustomResolver
{
    /// <summary>
    /// This custom resolver is just for demonstration purpose which shows how to override the resolved items during publishing.
    /// If a component of given type is published, it will only allow that component to be published and will remove all other resolved dependancies with it.
    /// </summary>
    public class MyCustomResolver : IResolver
    {
        /// <summary>
        /// Implement the resolver function to control the resolving
        /// </summary>
        /// <param name="item">the item published</param>
        /// <param name="instruction">Resolve Instructiuons</param>
        /// <param name="context">Publish Context</param>
        /// <param name="resolvedItems">Collection of Resolved Items</param>
        public void Resolve(IdentifiableObject item, ResolveInstruction instruction, PublishContext context, Tridion.Collections.ISet<ResolvedItem> resolvedItems)
        {
            
            // Array of schemas which are supposed to publish independently with out resolved items
            string[] lookupSchemas = { "Schema Title1", "Schema Title2" };

            // If published item is a component and it's type belongs to look up schemas array, start the process
            if (item is Component)
            {
                Component comp1 = (Component)item;
                if (lookupSchemas.Contains(comp1.Schema.Title))
                {
                    // Temp collection of resolved items
                    List<ResolvedItem> tempItems = new List<ResolvedItem>();

                    // Loop through resolved items
                    foreach (var resolvedItem in resolvedItems)
                    {
                        // If the resolved item is the published item
                        // Allow it to be published by adding to temp items collection
                        if (resolvedItem.Item is Component)
                        {
                            Component compResolved = (Component)resolvedItem.Item;
                            if (resolvedItem.Item.Id == item.Id)
                            {
                                tempItems.Add(resolvedItem);
                            }
                        }
                    }
                    
                    // Delete all resolved items
                    resolvedItems.Clear();

                    // Add temp items(needs to be published) to resolvedItems collection
                    foreach (ResolvedItem tempResolvedItem in tempItems)
                    {
                        resolvedItems.Add(tempResolvedItem);
                    }

                }
            }
        }
    }
}
