using System;

namespace CoffeeLibrary
{
    public enum BeanType
    {
        House,
        Breakfast,
        DarkRoast,
        LightRoast,
        Organic
    }

    public enum DrinkSize
    {
        Small,
        Medium,
        Large,
    }

    [Flags]
    public enum Flavorings
    {
        None        = 0,
        Caramel     = 1,
        Gingerbread = 2,
        Hazelnut    = 4,
        IrishCream  = 8,
        Pumpkin     = 16,
        Vanilla     = 32
    }

    public enum Temperature
    {
        Normal,
        ExtraHot,
        WithIce,
    }
}