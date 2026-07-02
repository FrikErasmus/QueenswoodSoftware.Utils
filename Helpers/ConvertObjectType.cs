namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Maps property values from one object instance to another when property names match.
    /// </summary>
    public static class ConvertObjectType
    {
        /// <summary>
        /// Convert one object to another where the field names are the same.
        /// </summary>
        /// <param name="oldObject">Object that needs to be converted</param>
        /// <param name="newType">New object type to convert to</param>
        /// <returns>New instance of <paramref name="newType"/> with matching properties copied from <paramref name="oldObject"/>.</returns>
        public static object ConvertObject(object oldObject, Type newType)
        {
            var object1 = Convert.ChangeType(oldObject, oldObject.GetType());
            var object2 = Activator.CreateInstance(newType);
            foreach (var item1 in object1.GetType().GetProperties())
            {
                foreach (var item2 in object2.GetType().GetProperties())
                {
                    if (item2.Name != item1.Name) continue;
                    item2.SetValue(object2, item1.GetValue(object1, null));
                    break;
                }
            }
            return object2!;
        }
    }
}